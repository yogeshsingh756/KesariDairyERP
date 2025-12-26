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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly AppDbContext _db;

        public PermissionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<string>> GetAllPermissionKeysAsync()
        {
            return await _db.Permissions
                .Select(p => p.PermissionKey)
                .ToListAsync();
        }
        public async Task<List<Permission>> GetAllAsync()
        {
            return await _db.Permissions
                .Where(p => !p.IsDeleted)
                .OrderBy(p => p.ModuleName)
                .ThenBy(p => p.PermissionName)
                .ToListAsync();
        }
    }
}
