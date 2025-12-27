using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int ActiveUsers { get; set; }
        public int ActiveRoles { get; set; }
        public int ActivePermissions { get; set; }

        public int ProductTypes { get; set; }
        public int IngredientTypes { get; set; }
        public int ProductionBatches { get; set; }
    }
}
