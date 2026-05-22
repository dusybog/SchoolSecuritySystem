using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Web;
using System.Security.Claims;
using SchoolSecuritySystem.Core.Interfaces.Repositories;
using SchoolSecuritySystem.Core.Constants;

namespace SchoolSecuritySystem.Web.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddO365Authentication(this IServiceCollection services, IConfiguration configuration)
        {
            // ==========================================
            // 1. 讀取 appsettings.json 綁定 Entra ID 基本設定
            // ==========================================
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));

            // ==========================================
            // 2. 獨立設定 OpenIdConnect 攔截器
            // ==========================================
            services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, (OpenIdConnectOptions options) =>
            {
                options.Events ??= new OpenIdConnectEvents();

                options.Events.OnTokenValidated = async context =>
                {
                    var principal = context.Principal;
                    if (principal == null) return;
                    var identity = (ClaimsIdentity)principal.Identity!;

                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                    logger.LogInformation("=== O365 登入成功，開始印出所有 Claims ===");
                    foreach (var claim in identity.Claims)
                    {
                        logger.LogInformation("[標籤類型]: {Type} | [標籤內容]: {Value}", claim.Type, claim.Value);
                    }
                    logger.LogInformation("=============================================");

                    var email = identity.FindFirst("preferred_username")?.Value ?? identity.FindFirst(ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(email)) return;

                    var userRoleRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRoleRepository>();
                    var userRole = await userRoleRepo.GetByEmailWithDetailsAsync(email);

                    if (userRole != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, userRole.role.name));
                        if (userRole.department_id.HasValue)
                        {
                            identity.AddClaim(new Claim("DepartmentId", userRole.department_id.Value.ToString()));
                        }

                        var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                        var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
                        memoryCache.Set($"UserStatus_{email}", true, cacheOptions);
                    }
                    else
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, AppRoles.GeneralUser));
                    }

                    logger.LogInformation("=== 寫入本地 DB 權限後的 Claims ===");
                    foreach (var claim in identity.Claims)
                    {
                        logger.LogInformation("Type: {Type}, Value: {Value}", claim.Type, claim.Value);
                    }
                };
            });

            // ==========================================
            // 3. 防禦過期權限攔截器 (維持您原本的寫法即可)
            // ==========================================
            services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, (CookieAuthenticationOptions options) =>
            {
                options.Events.OnValidatePrincipal = async context =>
                {
                    var principal = context.Principal;
                    if (principal == null) return;

                    // 使用 FindFirst 語法更簡潔安全
                    var email = principal.FindFirst("preferred_username")?.Value
                             ?? principal.FindFirst(ClaimTypes.Email)?.Value
                             ?? principal.FindFirst(ClaimTypes.Upn)?.Value;

                    if (string.IsNullOrEmpty(email)) return;

                    var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                    string cacheKey = $"UserStatus_{email}";

                    // 如果快取還在，代表 1 小時內驗證過，直接放行 (效能最優化)
                    if (memoryCache.TryGetValue(cacheKey, out bool _)) return;

                    // 快取過期，去資料庫重新確認最新權限
                    var userRoleRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRoleRepository>();
                    var currentUserRole = await userRoleRepo.GetByEmailWithDetailsAsync(email);

                    var cookieDept = principal.FindFirst("DepartmentId")?.Value;

                    if (currentUserRole == null)
                    {
                        // DB 找不到人，但 Cookie 的角色不是 GeneralUser -> 強制登出
                        if (!principal.IsInRole(AppRoles.GeneralUser))
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return;
                        }
                    }
                    else
                    {
                        // 🛡️ 修正 Role 檢查：使用 IsInRole 來避免多重 Role 造成的誤判
                        string expectedRole = currentUserRole.role?.name ?? AppRoles.GeneralUser;
                        bool hasCorrectRole = principal.IsInRole(expectedRole);

                        // 🛡️ 修正 Dept 檢查：安全處理 nullable 轉型
                        string expectedDept = currentUserRole.department_id?.ToString() ?? string.Empty;
                        string actualDept = cookieDept ?? string.Empty;
                        bool isDeptChanged = expectedDept != actualDept;

                        if (!hasCorrectRole || isDeptChanged)
                        {
                            context.RejectPrincipal();
                            await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            return;
                        }
                    }

                    // 檢查通過，重新核發 1 小時的快取免死金牌
                    var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
                    memoryCache.Set(cacheKey, true, cacheOptions);
                };
            });

            return services;
        }
    }
}