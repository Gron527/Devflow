using DevFlowAPI.Models;
using DevFlowAPI.Services;
using DevFlowAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DevFlowAPI.DependenyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
