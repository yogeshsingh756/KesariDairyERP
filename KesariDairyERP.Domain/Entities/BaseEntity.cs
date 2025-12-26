using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KesariDairyERP.Domain.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public long? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public long? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
