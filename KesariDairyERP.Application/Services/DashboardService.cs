using KesariDairyERP.Application.DTOs.Dashboard;
using KesariDairyERP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            return new DashboardStatsDto
            {
                ActiveUsers = await _repo.CountActiveUsersAsync(),
                ActiveRoles = await _repo.CountActiveRolesAsync(),
                ActivePermissions = await _repo.CountActivePermissionsAsync(),
                ProductTypes = await _repo.CountProductTypesAsync(),
                IngredientTypes = await _repo.CountIngredientTypesAsync(),
                ProductionBatches = await _repo.CountProductionBatchesAsync()
            };
        }
    }
}
