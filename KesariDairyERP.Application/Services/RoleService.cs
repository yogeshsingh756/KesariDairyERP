using KesariDairyERP.Application.DTOs.Roles;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;

namespace KesariDairyERP.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repo;

        public RoleService(IRoleRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RoleListDto>> GetAllAsync()
        {
            var roles = await _repo.GetAllAsync();
            return roles.Select(r => new RoleListDto
            {
                Id = r.Id,
                RoleName = r.RoleName,
                Description = r.Description
            }).ToList();
        }

        public async Task<RoleDetailDto> GetByIdAsync(long id)
        {
            var role = await _repo.GetByIdWithPermissionsAsync(id)
                       ?? throw new Exception("Role not found");

            return new RoleDetailDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
                Description = role.Description,
                PermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList()
            };
        }

        public async Task CreateAsync(CreateRoleDto dto)
        {
            var role = new Role
            {
                RoleName = dto.RoleName,
                Description = dto.Description
            };

            await _repo.CreateAsync(role, dto.PermissionIds);
        }

        public async Task UpdateAsync(UpdateRoleDto dto)
        {
            var role = new Role
            {
                Id = dto.Id,
                RoleName = dto.RoleName,
                Description = dto.Description
            };

            await _repo.UpdateAsync(role, dto.PermissionIds);
        }

        public async Task DeleteAsync(long id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
