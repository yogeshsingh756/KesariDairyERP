using KesariDairyERP.Application.DTOs.Packaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IBatchPackagingService
    {
        Task<PackagingCalculateResponse> CalculateAsync(long batchId, PackagingCalculateRequest request);
        Task ConfirmAsync(long batchId, PackagingConfirmRequest request);
    }
}
