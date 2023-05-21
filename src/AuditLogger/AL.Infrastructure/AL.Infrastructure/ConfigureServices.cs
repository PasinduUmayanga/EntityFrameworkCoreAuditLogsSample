﻿//using AL.Infrastructure.Models;
using AL.Infrastructure.Helpers.Interfaces;
using AL.Infrastructure.Helpers;
using AL.Infrastructure.Persistance.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AL.Infrastructure.Audit;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AL.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<ISerializerService, SerializerService>();
            services.AddSingleton<AuditableEntitiesInterceptor>();

            services.AddDbContextFactory<AuditLogDbContext>((sp,options) =>
            {
                var auditableIntetceptor = sp.GetService<AuditableEntitiesInterceptor>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(AuditLogDbContext).Assembly.FullName));//.AddInterceptors(auditableIntetceptor); ;

                options.ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.SaveChangesStarting));
             
            });
            return services;
        }
    }
}