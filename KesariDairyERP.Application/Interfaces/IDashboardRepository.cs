using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> CountActiveUsersAsync();
        Task<int> CountActiveRolesAsync();
        Task<int> CountActivePermissionsAsync();

        Task<int> CountProductTypesAsync();
        Task<int> CountIngredientTypesAsync();
        Task<int> CountProductionBatchesAsync();
    }
}
