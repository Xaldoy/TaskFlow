using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Mapping;
using System.Threading.RateLimiting;
using TaskFlow.DAL.Repositories.Authorization;
using TaskFlow.DAL.Repositories.Tasks;
using TaskFlow.Service.Services;
using TaskFlow.Service.Services.Authentication;
using TaskFlow.Service.Services.Authorization;
using TaskFlow.Service.Services.Tasks;

namespace Service.Services
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddCors(opt =>
            {
                opt.AddPolicy("DevelopmentPolicy", policy =>
                {
                    policy
                       .WithOrigins(configuration.GetValue<string>("AllowedHosts") ?? "*")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
                opt.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins("https://taskflowapp.duckdns.org")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddScoped<TokenService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();


            services.AddAutoMapper(typeof(TaskProfile));
            services.AddHttpContextAccessor();

            services.AddRateLimiter(opt =>
            {
                opt.AddFixedWindowLimiter(policyName: "fixedPolicy", options =>
                {
                    options.PermitLimit = 10;
                    options.Window = TimeSpan.FromSeconds(1);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 5;
                });
                opt.OnRejected = (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    return new ValueTask();
                };
            });

            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
