using KesariDairyERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Application.Interfaces
{
    public interface IBatchPackagingRepository
    {
        Task<bool> ExistsAsync(long batchId);
        Task AddAsync(BatchPackaging packaging);
    }
}
