using KesariDairyERP.Application.Interfaces;
using KesariDairyERP.Domain.Entities;
using KesariDairyERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Infrastructure.Repositories
{
    public class BatchPackagingRepository : IBatchPackagingRepository
    {
        private readonly AppDbContext _db;

        public BatchPackagingRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<bool> ExistsAsync(long batchId)
            => await _db.BatchPackaging.AnyAsync(x =>
                x.ProductionBatchId == batchId && !x.IsDeleted);

        public async Task AddAsync(BatchPackaging packaging)
        {
            _db.BatchPackaging.Add(packaging);
            await _db.SaveChangesAsync();
        }
    }
}
