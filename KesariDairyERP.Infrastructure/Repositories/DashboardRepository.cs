using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _db;

        public DashboardRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<int> CountActiveUsersAsync() =>
            _db.Users.CountAsync(x => x.IsActive && !x.IsDeleted);

        public Task<int> CountActiveRolesAsync() =>
            _db.Roles.CountAsync(x => x.IsActive && !x.IsDeleted);

        public Task<int> CountActivePermissionsAsync() =>
            _db.Permissions.CountAsync(x => x.IsActive && !x.IsDeleted);

        public Task<int> CountProductTypesAsync() =>
            _db.ProductType.CountAsync(x => !x.IsDeleted);

        public Task<int> CountIngredientTypesAsync() =>
            _db.IngredientType.CountAsync(x => !x.IsDeleted);

        public Task<int> CountProductionBatchesAsync() =>
            _db.ProductionBatch.CountAsync(x => !x.IsDeleted);
    }
}
