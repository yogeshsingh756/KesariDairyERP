using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.DTOs.RawMaterial
{
    public class RawMaterialDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
    }
}
