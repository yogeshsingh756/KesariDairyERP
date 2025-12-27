using KesariDairyERP.Application.DTOs.Common;
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
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _db;

        public RoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<Role>> GetPagedAsync(
     int pageNumber,
     int pageSize,
     string? search)
        {
            var query = _db.Roles
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(r =>
                    r.RoleName.ToLower().Contains(search) ||
                    (r.Description != null && r.Description.ToLower().Contains(search))
                );
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(r => r.RoleName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Role>
            {
                Items = items,
                TotalRecords = total
            };
        }

        public async Task<Role?> GetByIdWithPermissionsAsync(long id)
        {
            return await _db.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task CreateAsync(Role role, List<long> permissionIds)
        {
            _db.Roles.Add(role);
            await _db.SaveChangesAsync();

            foreach (var pid in permissionIds)
            {
                _db.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = pid
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role, List<long> permissionIds)
        {
            var existing = await _db.RolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .ToListAsync();

            _db.RolePermissions.RemoveRange(existing);

            foreach (var pid in permissionIds)
            {
                _db.RolePermissions.Add(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = pid
                });
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var role = await _db.Roles.FindAsync(id);
            if (role == null) throw new Exception("Role not found");

            role.IsDeleted = true;
            role.IsActive = false;

            await _db.SaveChangesAsync();
        }
    }
}
