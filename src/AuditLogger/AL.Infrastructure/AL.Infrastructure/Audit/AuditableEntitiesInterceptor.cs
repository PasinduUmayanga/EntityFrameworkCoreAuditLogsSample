using AL.Infrastructure.Helpers;
using AL.Infrastructure.Helpers.Interfaces;
using AL.Infrastructure.Persistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

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
            DbContext? dbContext = eventData.Context;
            List<Trail> auditEntries = new List<Trail>();
            InterceptionResult<int> result1 = result;
         
            try
            {

                List<AuditTrail> auditTrails = new List<AuditTrail>();
                if (dbContext is null)
                {
                    return base.SavingChanges(eventData, result);
                }

                auditEntries = HandleAuditingBeforeSaveChanges((AuditLogDbContext)dbContext);
                var interceptor = new CustomSaveChangesInterceptor();
                result = base.SavingChanges(eventData, result);
                interceptor.AfterSaveChanges(dbContext);
                //var originalResult = result.Result;



                return result1;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                //HandleAuditingAfterSaveChangesAsync(eventData, result, auditEntries);
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
                        //     UserId = entry.OriginalValues["ModifiedUser"] != null ? entry.OriginalValues["ModifiedUser"]?.ToString() : string.Empty
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
                                {
                                    trailEntry.TrailType = (int)EnumAuditTrailType.Create;
                                    trailEntry.NewValues[propertyName] = property.CurrentValue;
                                    break;
                                }

                            case EntityState.Deleted:
                                {
                                    trailEntry.TrailType = (int)EnumAuditTrailType.Delete;
                                    trailEntry.OldValues[propertyName] = property.OriginalValue;
                                    break;
                                }

                            case EntityState.Modified:
                                {
                                    if (property.IsModified && property.OriginalValue == null && property.CurrentValue != null)
                                    {
                                        trailEntry.TrailType = (int)EnumAuditTrailType.Update;
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
                    }


                }
                List<AuditTrail> auditTrails = new List<AuditTrail>();
                foreach (var auditEntry in trailEntries)
                {
                    auditTrails.Add(auditEntry.ToAuditTrail());

                }
                dbContext.AuditTrails.AddRange(auditTrails);

                return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void HandleAuditingAfterSaveChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, List<Trail> trailEntries, CancellationToken cancellationToken = new())
        {
            try
            {
                eventData.Context.ChangeTracker.DetectChanges();
                if (result.HasResult)
                {
                    var originalResult = result.Result;
                }
                var insertedEntities = eventData.Context.ChangeTracker.Entries()
         .Where(e => e.State == EntityState.Added)
    .Select(e => e.Entity)
            .ToList();
                foreach (var entity in insertedEntities)
                {
                    var entry = eventData.Context.Entry(entity);
                    if (entry != null && entry.Metadata.FindPrimaryKey() is IConventionKey primaryKey)
                    {
                        var temporaryId = entry.Property(primaryKey.Properties[0].Name).CurrentValue;
                        var realTimeId = entry.Property(primaryKey.Properties[0].Name).OriginalValue;

                    }
                }
                //if (trailEntries == null || trailEntries.Count == 0)
                //{
                //    //return Task.CompletedTask;
                //}
                //else
                //{

                //    foreach (var entry in trailEntries)
                //    {
                //        foreach (var prop in entry.TemporaryProperties)
                //        {
                //            if (prop.Metadata.IsPrimaryKey())
                //            {
                //                entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                //            }
                //            else
                //            {
                //                entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                //            }
                //        }
                //        AuditTrail auditTrail = entry.ToAuditTrail();
                //        HPRDbContext dbContext = (HPRDbContext)eventData.Context;
                //        dbContext.AuditTrails.Add(entry.ToAuditTrail());
                //    }

                //    SavingChanges(eventData, result);
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private List<Trail> OnBeforeSaveChanges(AuditLogDbContext dbContext)
        {
            dbContext.ChangeTracker.DetectChanges();
            var auditEntries = new List<Trail>();
            foreach (var entry in dbContext.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new Trail(entry, _serializer);
                auditEntry.TableName = entry.Metadata.GetTableName();
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                dbContext.AuditTrails.Add(auditEntry.ToAuditTrail());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<Trail> auditEntries, AuditLogDbContext dbContext)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                dbContext.AuditTrails.Add(auditEntry.ToAuditTrail());
            }

            return dbContext.SaveChangesAsync();
        }
    }
}
