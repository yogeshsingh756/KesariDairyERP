using KesariDairyERP.Application.DTOs.Vendor;
using KesariDairyERP.Application.Interfaces;

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
    }
}
