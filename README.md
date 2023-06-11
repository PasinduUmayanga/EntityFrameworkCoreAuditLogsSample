# ASP.NET Core database audit logs with Entity Framework Core 6

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

15. Create  database name as `[AuditLogDB]` in SQL Microsoft Sql Server and create following tables as belows
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

20. Inside `AL.Infrastructure`->`Persistance`->`Models` create 












 
