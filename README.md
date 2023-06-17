# ASP.NET Core database audit logs with Entity Framework Core 6

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample&metric=bugs)](https://sonarcloud.io/summary/new_code?id=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=PasinduUmayanga_EntityFrameworkCoreAuditLogsSample)

[![Build status](https://ci.appveyor.com/api/projects/status/puaa910410b37479?svg=true)](https://ci.appveyor.com/project/Mahadenamuththa/entityframeworkcoreauditlogssample)

[![Build history](https://buildstats.info/appveyor/chart/Mahadenamuththa/entityframeworkcoreauditlogssample)](https://ci.appveyor.com/project/Mahadenamuththa/entityframeworkcoreauditlogssample/history)

![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/77f377f3-e6e7-4aea-b315-3536cb397238)

## Create Project

01. First Create Project File->New->Project
02. Select `ASP.NET Core Web Api` and click on Next
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/9b649899-61d2-4c19-b824-b29de972fd47)

03.  Project name as `Al.Api` and Soluation as `EntityFrameworkCoreAuditLogsSample`
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/e4096757-d8d2-433f-a8f1-5820725a8653)

04. Select .Net 6 
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/b0edf8ab-3ec7-4f10-a5b4-77227df28201)

05. Now you will see your created project under soluation explorer
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/84833136-0382-4817-ab29-c4a224fce7ec)

06. Now Create New project for business layer Right click on Soluation->Add->New Project as below
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/66c00edb-888f-4622-995d-12e2c239f093)

07. Then select Class library That support .NET or .Net Standard and click on Next
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/0f7dffad-e301-42d5-af03-db215ff9fce9)

08. Create Project name as `AL.Application` and next
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/7fb529ba-937e-49c4-b369-ab846f168045)

09. Select framework as .NET 6 and next
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/e7226d10-a6ae-44ef-8544-b622dd50ef9f)

10. By Using 6,7,8,9 steps Create project name as `AL.Infrastructure`
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/24671d52-180e-4c86-ae06-fc436331d55f)

11. Expand Al.Infrastructure project and right click on Dependencices as below you can Manage Nuget packages
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/7f042170-b49a-4be4-9514-2b740dfaac95)

12. Install Following Nuget packages
- `Microsoft.AspNetCore.Http.Abstractions`
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Newtonsoft.Json`
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/b520dd79-5505-4f98-8e69-e776f2644165)

13. By using 11 step on `AL.Application` project Install Following Nuget packages
-  `Microsoft.Extensions.Configuration.Abstractions`
-  `Microsoft.Extensions.DependencyInjection.Abstractions`
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/e5edb205-084f-4ed5-8f44-3bca578f26b1)

14. Inside `AL.Infrastructure` create 3 folders Names of `Audi,Helpers,Persistance`
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/22024466-2ce5-4439-b6c1-e61ea919f863)

15. Create  database name as `[AuditLogDB]` in SQL Microsoft Sql Server and create following tables(`AuditType`,`AuditTrail`) as belows
```sql
USE [AuditLogDB]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditType](
	[Id] [int] NOT NULL,
	[AuditType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NULL,
 CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[AuditTrail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](50) NULL,
	[AuditType] [int] NOT NULL,
	[TableName] [nvarchar](50) NULL,
	[PrimaryKey] [nvarchar](max) NULL,
	[OldValues] [nvarchar](max) NULL,
	[NewValues] [nvarchar](max) NULL,
	[AffectedColumns] [nvarchar](max) NULL,
	[DateTime] [datetime] NULL,
 CONSTRAINT [PK_AuditTrail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuditTrail]  WITH CHECK ADD  CONSTRAINT [FK_AuditTrail_AuditType] FOREIGN KEY([AuditType])
REFERENCES [dbo].[AuditType] ([Id])
GO

ALTER TABLE [dbo].[AuditTrail] CHECK CONSTRAINT [FK_AuditTrail_AuditType]
GO
```
16. Now in project go to `Tools`-> `Nuget Package Manager` -> Package Manager Console
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/2fc37ab7-2bbc-49d9-920f-5e077ba8bb2e)

17. In package manager console select default project as AL.Infrastructure And run `Scaffold-DbContext Name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -Context AuditLogDbContext -OutputDir Persistance/Models -f`
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/4158e2bd-2a61-4575-9476-e36bb545909d)

18. After you run above command, In side `AL.Infrastructure` -> `Persistance` -> `Models` DbContext is created.
![image](https://github.com/PasinduUmayanga/EntityFrameworkCoreAuditLogsSample/assets/21302583/063efd8d-2d09-4709-80a1-7c15d42f5f7f)

19. In side Helpers Create folder Interfaces and Create `ISerializerService.cs` 
```csharp
namespace AL.Infrastructure.Helpers.Interfaces
{
    public interface ISerializerService
    {
        string Serialize<T>(T obj);
        string Serialize<T>(T obj, Type type);
        T Deserialize<T>(string text);
    }
}
```

20. In side AL.Infrastructure -> Helpers Create `SerializerService.cs` 

```csharp
using AL.Infrastructure.Helpers.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace AL.Infrastructure.Helpers
{
    public class SerializerService : ISerializerService
    {
        public T Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>
            {
                new StringEnumConverter() { CamelCaseText = true }
            }
            });
        }

        public string Serialize<T>(T obj, Type type)
        {
            return JsonConvert.SerializeObject(obj, type, new());
        }
    }
}
```

21. In side AL.Infrastructure -> Audit  Create `AuditLogSaveChangesInterceptor`
```csharp
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
```

