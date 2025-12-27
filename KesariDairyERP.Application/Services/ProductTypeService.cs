using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.ProductType;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IProductTypeRepository _repo;

        public ProductTypeService(IProductTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<PagedResult<ProductType>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search)
        {
            return await _repo.GetPagedAsync(
                pageNumber,
                pageSize,
                search
            );
        }

        public async Task<ProductType> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("Product Type not found");

            return entity;
        }

        public async Task<long> CreateAsync(CreateProductTypeRequest request)
        {
            var entity = new ProductType
            {
                Name = request.Name,
                Variant = request.Variant,
                Unit = request.Unit,
                Quantity = request.Quantity
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateAsync(UpdateProductTypeRequest request)
        {
            var entity = await GetByIdAsync(request.Id);

            entity.Name = request.Name;
            entity.Variant = request.Variant;
            entity.Unit = request.Unit;
            entity.Quantity = request.Quantity;
            entity.IsActive = request.IsActive;

            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            entity.IsDeleted = true;

            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
        }
    }
}
