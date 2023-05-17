using AL.Application.Repositories.Interfaces;
using AL.Application.Repositories;
using AL.Application.Services;
using AL.Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AL.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                services.AddScoped<IStudentService, StudentService>();
                services.AddScoped<IStudentRepository, StudentRepository>();
                return services;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}