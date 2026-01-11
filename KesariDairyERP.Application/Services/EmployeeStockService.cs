using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.EmployeeStockAssignment;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class EmployeeStockService : IEmployeeStockService
    {
        private readonly IEmployeeProductAssignmentRepository _assignmentRepo;
        private readonly IEmployeeProductStockRepository _employeeStockRepo;
        private readonly IFinishedProductStockRepository _finishedStockRepo;

        public EmployeeStockService(
            IEmployeeProductAssignmentRepository assignmentRepo,
            IEmployeeProductStockRepository employeeStockRepo,
            IFinishedProductStockRepository finishedStockRepo)
        {
            _assignmentRepo = assignmentRepo;
            _employeeStockRepo = employeeStockRepo;
            _finishedStockRepo = finishedStockRepo;
        }

        public async Task AssignAsync(AssignProductToEmployeeRequest request)
        {
            // 1️⃣ Get finished stock
            var finishedStock =
                await _finishedStockRepo.GetByProductTypeAsync(request.ProductTypeId)
                ?? throw new Exception("Finished stock not found");

            // 2️⃣ Block negative stock
            if (finishedStock.QuantityAvailable < request.Quantity)
                throw new Exception("Insufficient finished product stock");

            // 3️⃣ Deduct finished stock
            finishedStock.QuantityAvailable -= request.Quantity;
            await _finishedStockRepo.SaveAsync(finishedStock);

            // 4️⃣ Save assignment history
            await _assignmentRepo.AddAsync(new EmployeeProductAssignment
            {
                EmployeeId = request.EmployeeId,
                ProductTypeId = request.ProductTypeId,

                QuantityAssigned = request.Quantity,

                SellingPricePerUnit = request.SellingPricePerUnit,
                TotalAmount = request.TotalAmount > 0
         ? request.TotalAmount
         : request.Quantity * request.SellingPricePerUnit,

                AssignmentDate = request.AssignmentDate,
                Remarks = request.Remarks
            });

            // 5️⃣ Upsert employee stock
            var employeeStock =
                await _employeeStockRepo.GetAsync(
                    request.EmployeeId, request.ProductTypeId);

            if (employeeStock == null)
            {
                await _employeeStockRepo.AddAsync(new EmployeeProductStock
                {
                    EmployeeId = request.EmployeeId,
                    ProductTypeId = request.ProductTypeId,
                    QuantityAvailable = request.Quantity
                });
            }
            else
            {
                employeeStock.QuantityAvailable += request.Quantity;
                await _employeeStockRepo.UpdateAsync(employeeStock);
            }
        }

        // =========================
        // 🔹 SUMMARY API
        // =========================
        public async Task<PagedResult<EmployeeAssignmentSummaryDto>>
            GetAssignmentSummaryAsync(
                int pageNumber,
                int pageSize,
                long? employeeId,
                DateTime? fromDate,
                DateTime? toDate)
        {
            var query = _assignmentRepo.Query();

            if (employeeId.HasValue)
                query = query.Where(x => x.EmployeeId == employeeId);

            if (fromDate.HasValue)
                query = query.Where(x =>
                    x.AssignmentDate.Date >= fromDate.Value.Date);

            if (toDate.HasValue)
                query = query.Where(x =>
                    x.AssignmentDate.Date <= toDate.Value.Date);

            var groupedQuery = query
                .GroupBy(x => new
                {
                    x.EmployeeId,
                    x.Employee.FullName,
                    x.ProductTypeId,
                    x.ProductType.Name,
                    x.ProductType.Variant
                })
                .Select(g => new
                {
                    g.Key.EmployeeId,
                    g.Key.FullName,
                    g.Key.ProductTypeId,
                    g.Key.Name,
                    g.Key.Variant,
                    TotalQty = g.Sum(x => x.QuantityAssigned),
                    LastDate = g.Max(x => x.AssignmentDate),
                    LastId = g.Max(x => x.Id)
                });

            var total = await groupedQuery.CountAsync();

            var items = await groupedQuery
                .OrderByDescending(x => x.LastId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EmployeeAssignmentSummaryDto
                {
                    EmployeeId = x.EmployeeId,
                    EmployeeName = x.FullName,
                    ProductTypeId = x.ProductTypeId,
                    ProductName = x.Name + " " + x.Variant,
                    TotalQuantityAssigned = x.TotalQty,
                    LastAssignedDate = x.LastDate
                })
                .ToListAsync();

            return new PagedResult<EmployeeAssignmentSummaryDto>
            {
                Items = items,
                TotalRecords = total
            };
        }

        // =========================
        // 🔹 DETAILS API
        // =========================
        public async Task<PagedResult<EmployeeAssignmentDetailDto>>
            GetAssignmentDetailsAsync(
                long employeeId,
                long productTypeId,
                int pageNumber,
                int pageSize)
        {
            var query = _assignmentRepo.GetDetailsQuery(
                employeeId, productTypeId);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new EmployeeAssignmentDetailDto
                {
                    AssignmentDate = x.AssignmentDate,
                    QuantityAssigned = x.QuantityAssigned,
                    SellingPricePerUnit = x.SellingPricePerUnit,
                    TotalAmount = x.TotalAmount,
                    Remarks = x.Remarks
                })
                .ToListAsync();

            return new PagedResult<EmployeeAssignmentDetailDto>
            {
                Items = items,
                TotalRecords = total
            };
        }
    }
}
