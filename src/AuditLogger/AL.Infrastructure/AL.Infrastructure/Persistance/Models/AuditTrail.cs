using System;
using System.Collections.Generic;

namespace AL.Infrastructure.Persistance.Models
{
    public partial class AuditTrail
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int AuditType { get; set; }
        public string? TableName { get; set; }
        public string? PrimaryKey { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
        public DateTime? DateTime { get; set; }

        public virtual AuditType AuditTypeNavigation { get; set; } = null!;
    }
}
