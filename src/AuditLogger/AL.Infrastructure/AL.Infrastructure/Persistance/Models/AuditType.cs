using System;
using System.Collections.Generic;

namespace AL.Infrastructure.Persistance.Models
{
    public partial class AuditType
    {
        public AuditType()
        {
            AuditTrails = new HashSet<AuditTrail>();
        }

        public int Id { get; set; }
        public string AuditType1 { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
    }
}
