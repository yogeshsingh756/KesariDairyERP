using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class EmployeeProductStockRepository
    : IEmployeeProductStockRepository
    {
        private readonly AppDbContext _db;

        public EmployeeProductStockRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<EmployeeProductStock?> GetAsync(
            long employeeId, long productTypeId)
        {
            return await _db.EmployeeProductStocks.FirstOrDefaultAsync(x =>
                x.EmployeeId == employeeId &&
                x.ProductTypeId == productTypeId &&
                !x.IsDeleted);
        }

        public async Task AddAsync(EmployeeProductStock stock)
        {
            _db.EmployeeProductStocks.Add(stock);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeProductStock stock)
        {
            _db.EmployeeProductStocks.Update(stock);
            await _db.SaveChangesAsync();
        }
    }
}
