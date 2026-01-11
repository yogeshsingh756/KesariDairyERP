using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class EmployeeProductAssignmentRepository
    : IEmployeeProductAssignmentRepository
    {
        private readonly AppDbContext _db;

        public EmployeeProductAssignmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(EmployeeProductAssignment assignment)
        {
            _db.EmployeeProductAssignments.Add(assignment);
            await _db.SaveChangesAsync();
        }

        public IQueryable<EmployeeProductAssignment> Query()
        {
            return _db.EmployeeProductAssignments
                .Include(x => x.Employee)
                .Include(x => x.ProductType)
                .Where(x => !x.IsDeleted);
        }

        public IQueryable<EmployeeProductAssignment>
            GetDetailsQuery(long employeeId, long productTypeId)
        {
            return _db.EmployeeProductAssignments
                .Where(x =>
                    x.EmployeeId == employeeId &&
                    x.ProductTypeId == productTypeId &&
                    !x.IsDeleted);
        }
    }
}
