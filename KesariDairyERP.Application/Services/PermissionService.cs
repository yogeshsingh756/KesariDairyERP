using KesariDairyERP.Application.DTOs.Permissions;
using KesariDairyERP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repo;

        public PermissionService(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PermissionGroupDto>> GetAllGroupedAsync()
        {
            var permissions = await _repo.GetAllAsync();

            return permissions
                .GroupBy(p => p.ModuleName)
                .Select(g => new PermissionGroupDto
                {
                    ModuleName = g.Key,
                    Permissions = g.Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        PermissionKey = p.PermissionKey,
                        PermissionName = p.PermissionName
                    }).ToList()
                })
                .ToList();
        }
    }
}
