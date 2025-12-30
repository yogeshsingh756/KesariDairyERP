using KesariDairyERP.Application.DTOs.Common;
using KesariDairyERP.Application.DTOs.Vendor;
using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;

namespace KesariDairyERP.Application.Services
{
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _repo;

        public VendorService(IVendorRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<VendorDto>> GetAllAsync(string? vendorType = null)
        {
            var vendors = await _repo.GetAllAsync(vendorType);

            return vendors.Select(x => new VendorDto
            {
                Id = x.Id,
                Name = x.Name,
                ContactNumber = x.ContactNumber,
                VendorType = x.VendorType
            }).ToList();
        }
        public async Task<PagedResult<VendorDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? search,
        string? vendorType)
        {
            var result = await _repo.GetPagedAsync(
                pageNumber, pageSize, search, vendorType);

            return new PagedResult<VendorDto>
            {
                TotalRecords = result.TotalRecords,
                Items = result.Items.Select(v => new VendorDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    ContactNumber = v.ContactNumber,
                    VendorType = v.VendorType
                }).ToList()
            };
        }

        public async Task<VendorDto> GetByIdAsync(long id)
        {
            var vendor = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Vendor not found");

            return new VendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactNumber = vendor.ContactNumber,
                VendorType = vendor.VendorType
            };
        }

        public async Task CreateAsync(VendorRequest request)
        {
            await _repo.AddAsync(new Vendor
            {
                Name = request.Name,
                ContactNumber = request.ContactNumber,
                VendorType = request.VendorType
            });
        }

        public async Task UpdateAsync(long id, VendorRequest request)
        {
            var vendor = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Vendor not found");

            vendor.Name = request.Name;
            vendor.ContactNumber = request.ContactNumber;
            vendor.VendorType = request.VendorType;

            await _repo.UpdateAsync(vendor);
        }

        public async Task DeleteAsync(long id)
        {
            var vendor = await _repo.GetByIdAsync(id)
                ?? throw new Exception("Vendor not found");

            await _repo.DeleteAsync(vendor);
        }
    }
}
