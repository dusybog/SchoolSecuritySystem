using Microsoft.Extensions.DependencyInjection;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Interfaces.Services;
using SchoolSecuritySystem.Infrastructure.Repositories;
using SchoolSecuritySystem.Infrastructure.Services;

namespace SchoolSecuritySystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<ISequenceRepository, SequenceRepository>();
            services.AddScoped<IPdfPasswordRepository, PdfPasswordRepository>();
            services.AddScoped<IReportService, ReportService>();
            services.AddSingleton<IEmailTaskQueue, EmailTaskQueue>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISystemAuditLogRepository, SystemAuditLogRepository>();
            return services;
        }
    }
}