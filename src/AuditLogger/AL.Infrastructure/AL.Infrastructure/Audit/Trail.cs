
using AL.Infrastructure.Helpers.Interfaces;
using AL.Infrastructure.Persistance.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AL.Infrastructure.Audit
{
    public class Trail
    {
        private readonly ISerializerService _serializer;
        public Trail(EntityEntry entry, ISerializerService serializer)
        {
            Entry = entry;
            _serializer = serializer;
        }
        public EntityEntry Entry { get; }
        public string UserId { get; set; }
        public string? TableName { get; set; }
        public Dictionary<string, object?> KeyValues { get; } = new();
        public Dictionary<string, object?> OldValues { get; } = new();
        public Dictionary<string, object?> NewValues { get; } = new();
        public List<PropertyEntry> TemporaryProperties { get; } = new();
        public int TrailType { get; set; }
        public List<string> ChangedColumns { get; } = new();
        public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

        public AuditTrail ToAuditTrail() =>
            new()
            {
                UserId = UserId,
                AuditType = TrailType,
                TableName = TableName,
                DateTime = DateTime.UtcNow,
                PrimaryKey = _serializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : _serializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : _serializer.Serialize(NewValues),
                AffectedColumns = ChangedColumns.Count == 0 ? null : _serializer.Serialize(ChangedColumns)
            };
    }
}
