using KesariDairyERP.Application.DTOs.RawMaterial;
using KesariDairyERP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class RawMaterialService : IRawMaterialService
    {
        private readonly IRawMaterialRepository _repo;

        public RawMaterialService(IRawMaterialRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RawMaterialDto>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
