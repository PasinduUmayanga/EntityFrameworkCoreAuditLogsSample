using AL.Infrastructure.Helpers.Interfaces;
using AL.Infrastructure.Persistance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AL.Infrastructure.Audit
{
    public class AuditLogSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ISerializerService _serializer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private string userName = "SYSTEM_USER";
        private List<AuditTrail> auditTrailSaveList = new List<AuditTrail>();

        public AuditLogSaveChangesInterceptor(ISerializerService serializer, IServiceScopeFactory serviceScopeFactory)
        {
            _serializer = serializer;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private void SetUserName()
        {
            try
            {
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    var myScopedService = scope.ServiceProvider.GetService<IHttpContextAccessor>();
                    string? userName = myScopedService?.HttpContext?.User.Claims?.FirstOrDefault(c => c.Type == "UserName")?.Value;
                    this.userName = string.IsNullOrEmpty(userName) ? "SYSTEM_USER" : userName;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            try
            {
                SetUserName();
                AuditLogDbContext dbContext = (AuditLogDbContext)eventData.Context!;

                if (dbContext is null)
                {
                    return base.SavingChanges(eventData, result);
                }

                // Get changes before save database
                BeforeSaveChanges(dbContext);
                result = base.SavingChanges(eventData, result);

                // Subscribe from the StateChanged event
                eventData.Context.ChangeTracker.StateChanged += (s, e) =>
                {
                    if (e.OldState == EntityState.Added && e.Entry.Entity is not AuditTrail)
                    {
                        var trailEntry = AfterSaveChanges(e.Entry);
                        if (trailEntry != null)
                        {
                            AuditTrail auditTrailSave = trailEntry.ToAuditTrail();
                            auditTrailSaveList.Add(auditTrailSave);
                        }
                    }
                };
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // Unsubscribe from the StateChanged event
                eventData.Context.ChangeTracker.StateChanged -= (s, e) =>
                {
                };
            }
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            try
            {
                AuditLogDbContext dbContext = (AuditLogDbContext)eventData.Context!;
                if (auditTrailSaveList.Count > 0)
                {
                    var auditTrailSaveDistinctList = auditTrailSaveList.DistinctBy(i => new { i.UserId, i.AuditType, i.TableName, i.PrimaryKey, i.OldValues, i.NewValues, i.AffectedColumns }).ToList();
                    dbContext.AuditTrails.AddRange(auditTrailSaveDistinctList);
                    auditTrailSaveList.Clear();
                    dbContext.SaveChanges();

                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void BeforeSaveChanges(AuditLogDbContext dbContext)
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
                dbContext.ChangeTracker.DetectChanges();

                List<EntityEntry> entries = dbContext.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified) && e.Entity is not AuditTrail).DistinctBy(x => x.Properties)
                .ToList();

                for (int i = 0; i < entries.Count; i++)
                {
                    EntityEntry entry = entries[i];

                    var trailEntry = new Trail(entry, _serializer)
                    {
                        TableName = entry.Entity.GetType().Name,
                        UserId = userName
                    };


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
                                    trailEntry.ChangedColumns.Add(propertyName);
                                    trailEntry.TrailType = (int)EnumAuditTrailType.Delete;
                                }
                                else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                                {
                                    trailEntry.ChangedColumns.Add(propertyName);
                                    trailEntry.TrailType = (int)EnumAuditTrailType.Update;
                                    trailEntry.OldValues[propertyName] = property.OriginalValue;
                                    trailEntry.NewValues[propertyName] = property.CurrentValue;
                                }
                                else
                                {
                                    trailEntry.TrailType = (int)EnumAuditTrailType.Update;
                                }
                                break;
                            default:
                                trailEntry.TrailType = (int)EnumAuditTrailType.None;

                                break;
                        }
                    }

                    AuditTrail auditTrailSave = trailEntry.ToAuditTrail();
                    if (!string.IsNullOrEmpty(auditTrailSave.PrimaryKey))
                    {
                        auditTrailSaveList.Add(auditTrailSave);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Trail AfterSaveChanges(EntityEntry entityEntry)
        {
            Trail trailEntry = new Trail(entityEntry, _serializer);
            try
            {

                trailEntry.TableName = entityEntry.Entity.GetType().Name;
                trailEntry.UserId = userName;


                foreach (var property in entityEntry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        trailEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entityEntry.State)
                    {
                        case EntityState.Detached:
                            break;
                        case EntityState.Unchanged:
                            break;
                        case EntityState.Deleted:
                            break;
                        case EntityState.Modified:
                            break;
                        case EntityState.Added:
                            trailEntry.TrailType = (int)EnumAuditTrailType.Create;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                    }
                    trailEntry.TrailType = (int)EnumAuditTrailType.Create;
                    trailEntry.NewValues[propertyName] = property.CurrentValue;

                }
                return trailEntry;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
