using Microsoft.Extensions.DependencyInjection;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Core.Services;

namespace SchoolSecuritySystem.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISerialNumberService, SerialNumberService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddScoped<IPdfSettingService, PdfSettingService>();
            services.AddScoped<ISystemAuditLogService, SystemAuditLogService>();
            return services;
        }
    }
}