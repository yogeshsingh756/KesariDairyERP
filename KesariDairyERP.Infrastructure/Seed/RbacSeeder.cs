using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Infrastructure.Seed
{
    public static class RbacSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // 1️⃣ Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<Role>
            {
                new Role { RoleName = "SuperAdmin", Description = "System Owner" },
                new Role { RoleName = "Admin", Description = "System Administrator" }
            };

                context.Roles.AddRange(roles);
                await context.SaveChangesAsync();
            }

            // 2️⃣ Seed Permissions
            if (!await context.Permissions.AnyAsync())
            {
                var permissions = new List<Permission>
            {
                // USER MANAGEMENT
                new Permission { PermissionKey = "USER_VIEW", PermissionName = "View Users", ModuleName = "User" },
                new Permission { PermissionKey = "USER_CREATE", PermissionName = "Create User", ModuleName = "User" },
                new Permission { PermissionKey = "USER_EDIT", PermissionName = "Edit User", ModuleName = "User" },
                new Permission { PermissionKey = "USER_DELETE", PermissionName = "Delete User", ModuleName = "User" },

                // ROLE MANAGEMENT
                new Permission { PermissionKey = "ROLE_VIEW", PermissionName = "View Roles", ModuleName = "Role" },
                new Permission { PermissionKey = "ROLE_CREATE", PermissionName = "Create Role", ModuleName = "Role" },
                new Permission { PermissionKey = "ROLE_EDIT", PermissionName = "Edit Role", ModuleName = "Role" },
                new Permission { PermissionKey = "ROLE_DELETE", PermissionName = "Delete Role", ModuleName = "Role" },

                // PERMISSION MANAGEMENT
                new Permission { PermissionKey = "PERMISSION_VIEW", PermissionName = "View Permissions", ModuleName = "Permission" }
            };

                context.Permissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }

            // 3️⃣ Assign ALL permissions to SuperAdmin
            var superAdminRole = await context.Roles.FirstAsync(r => r.RoleName == "SuperAdmin");

            var allPermissions = await context.Permissions.ToListAsync();

            foreach (var permission in allPermissions)
            {
                var exists = await context.RolePermissions.AnyAsync(rp =>
                    rp.RoleId == superAdminRole.Id &&
                    rp.PermissionId == permission.Id);

                if (!exists)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = superAdminRole.Id,
                        PermissionId = permission.Id
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
