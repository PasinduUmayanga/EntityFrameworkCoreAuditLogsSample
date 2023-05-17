using AL.Infrastructure.Helpers;
using AL.Infrastructure.Helpers.Interfaces;
using AL.Infrastructure.Persistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AL.Infrastructure.Audit
{
    public sealed class AuditableEntitiesInterceptor : SaveChangesInterceptor
    {
        private readonly ISerializerService _serializer;

        public AuditableEntitiesInterceptor()
        {
            _serializer = new SerializerService();
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            try
            {
                DbContext? dbContext = eventData.Context;
                List<AuditTrail> auditTrails = new List<AuditTrail>();
                if (dbContext is null)
                {
                    return base.SavingChanges(eventData, result);
                }

                var auditEntries = HandleAuditingBeforeSaveChanges((AuditLogDbContext)dbContext);
                var result1 = base.SavingChanges(eventData, result);


                return result1;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private List<Trail> HandleAuditingBeforeSaveChanges(AuditLogDbContext dbContext)
        {
            try
            {
                //foreach (var entry in dbContext.ChangeTracker.Entries().ToList())
                //{
                //    switch (entry.State)
                //    {
                //        case EntityState.Added:
                //            entry.Entity.ModifiedDate = DateTime.UtcNow;
                //            entry.Entity.ModifiedBy = userId;

                //            break;

                //        case EntityState.Modified:
                //            entry.Entity.ModifiedDate = DateTime.UtcNow;
                //            entry.Entity.ModifiedBy = userId;
                //            break;

                //        //case EntityState.Deleted:
                //        //    if (entry.Entity is ISoftDelete softDelete)
                //        //    {
                //        //        softDelete.DeletedBy = userId;
                //        //        softDelete.DeletedOn = DateTime.UtcNow;
                //        //        entry.State = EntityState.Modified;
                //        //    }

                //        //    break;
                //    }
                //}
                //dbContext.ChangeTracker.DetectChanges();

                List<Trail> trailEntries = new List<Trail>();
                foreach (var entry in dbContext.ChangeTracker.Entries()
                .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
                .ToList())
                {
                    var trailEntry = new Trail(entry, _serializer)
                    {
                        TableName = entry.Entity.GetType().Name,
                        UserId = entry.OriginalValues["ModifiedUser"] != null ? entry.OriginalValues["ModifiedUser"]?.ToString() : string.Empty
                    };
                    trailEntries.Add(trailEntry);
                    foreach (var property in entry.Properties)
                    {
                        if (property.IsTemporary)
                        {
                            trailEntry.TemporaryProperties.Add(property);
                            continue;
                        }

                        string propertyName = property.Metadata.Name;
                        if (property.Metadata.IsPrimaryKey())
                        {
                            trailEntry.KeyValues[propertyName] = property.CurrentValue;
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                trailEntry.TrailType = (int)EnumAuditTrailType.Create;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                                break;

                            case EntityState.Deleted:
                                trailEntry.TrailType = (int)EnumAuditTrailType.Delete;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                break;

                            case EntityState.Modified:
                                if (property.IsModified && property.OriginalValue == null && property.CurrentValue != null)
                                {
                                    //trailEntry.ChangedColumns.Add(propertyName);
                                    //trailEntry.TrailType = (int)EnumAuditTrailType.Delete;
                                    //trailEntry.OldValues[propertyName] = property.OriginalValue;
                                    //trailEntry.NewValues[propertyName] = property.CurrentValue;
                                }
                                else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                                {
                                    trailEntry.ChangedColumns.Add(propertyName);
                                    trailEntry.TrailType = (int)EnumAuditTrailType.Update;
                                    trailEntry.OldValues[propertyName] = property.OriginalValue;
                                    trailEntry.NewValues[propertyName] = property.CurrentValue;
                                }

                                break;
                        }
                    }

                    foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
                    {
                        dbContext.AuditTrails.Add(auditEntry.ToAuditTrail());
                    }
                }

                return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
