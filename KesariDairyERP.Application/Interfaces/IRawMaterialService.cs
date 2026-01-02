using KesariDairyERP.Application.DTOs.RawMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IRawMaterialService
    {
        Task<List<RawMaterialDto>> GetAllAsync();

    }
}
