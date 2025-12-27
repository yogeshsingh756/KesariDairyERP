using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.IngredientType;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class IngredientTypeService : IIngredientTypeService
    {
        private readonly IIngredientTypeRepository _repo;

        public IngredientTypeService(IIngredientTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<IngredientTypeListDto>> GetPagedAsync(
            int pageNumber, int pageSize, string? search)
        {
            var result = await _repo.GetPagedAsync(pageNumber, pageSize, search);

            return new PagedResult<IngredientTypeListDto>
            {
                Items = result.Items.Select(i => new IngredientTypeListDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Unit = i.Unit,
                    CostPerUnit = i.CostPerUnit,
                    Description = i.Description
                }).ToList(),
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<IngredientTypeListDto> GetByIdAsync(int id)
        {
            var i = await _repo.GetByIdAsync(id);
            return new IngredientTypeListDto
            {
                Id = i.Id,
                Name = i.Name,
                Unit = i.Unit,
                CostPerUnit = i.CostPerUnit,
                Description = i.Description
            };
        }

        public async Task<long> CreateAsync(CreateIngredientTypeRequest request)
        {
            var entity = new IngredientType
            {
                Name = request.Name,
                Unit = request.Unit,
                CostPerUnit = request.CostPerUnit,
                Description = request.Description
            };

            await _repo.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateIngredientTypeRequest request)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            entity.Name = request.Name;
            entity.Unit = request.Unit;
            entity.CostPerUnit = request.CostPerUnit;
            entity.Description = request.Description;

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
        public async Task<List<IngredientTypeDropdownDto>> GetDropdownAsync()
        {
            var ingredients = await _repo.GetDropdownAsync();

            return ingredients.Select(i => new IngredientTypeDropdownDto
            {
                Id = i.Id,
                Name = i.Name,
                Unit = i.Unit,
                CostPerUnit = i.CostPerUnit
            }).ToList();
        }
    }
}
