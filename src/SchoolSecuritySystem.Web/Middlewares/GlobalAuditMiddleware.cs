using SchoolSecuritySystem.Core.Entities;
using SchoolSecuritySystem.Infrastructure.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolSecuritySystem.Web.Middlewares
{
    public class GlobalAuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalAuditMiddleware> _logger;

        // 存放當前啟用的版本與已解碼的 Byte 金鑰 (提升效能)
        private readonly int _activeKeyVersion;
        private readonly byte[] _activeKeyBytes;

        public GlobalAuditMiddleware(
            RequestDelegate next,
            ILogger<GlobalAuditMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;

            // 1. 讀取目前啟用的金鑰版本
            var versionStr = configuration["AuditSettings:ActiveKeyVersion"]
                ?? throw new InvalidOperationException("啟動失敗：找不到 AuditSettings:ActiveKeyVersion");

            if (!int.TryParse(versionStr, out _activeKeyVersion))
            {
                throw new InvalidOperationException("啟動失敗：ActiveKeyVersion 格式錯誤，必須為整數。");
            }

            // 2. 根據版本號，讀取對應的 Base64 金鑰字串
            string keyPath = $"AuditSettings:HmacKeys:{_activeKeyVersion}";
            var base64Key = configuration[keyPath]
                ?? throw new InvalidOperationException($"啟動失敗：找不到版本 {_activeKeyVersion} 的金鑰 ({keyPath})");

            // 3. 將 Base64 字串解碼為 Byte 陣列並暫存，避免每次 Request 都重新解碼
            try
            {
                _activeKeyBytes = Convert.FromBase64String(base64Key);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException($"啟動失敗：版本 {_activeKeyVersion} 的金鑰不是有效的 Base64 格式。");
            }
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            string? errorMessage = null;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                throw;
            }
            finally
            {
                try
                {
                    var userEmail = context.User?.FindFirst(ClaimTypes.Email)?.Value
                                 ?? context.User?.FindFirst(ClaimTypes.Upn)?.Value
                                 ?? context.User?.FindFirst("preferred_username")?.Value
                                 ?? "Anonymous";

                    var ipAddress = context.Connection.RemoteIpAddress?.ToString();

                    var log = new system_audit_log
                    {
                        user_email = userEmail,
                        ip_address = ipAddress,
                        http_method = context.Request.Method,
                        request_path = context.Request.Path + context.Request.QueryString,
                        status_code = errorMessage != null ? 500 : context.Response.StatusCode,
                        error_message = errorMessage,
                        created_at = DateTime.UtcNow,
                        key_version = _activeKeyVersion
                    };

                    string rawData = $"{log.user_email}|{log.http_method}|{log.request_path}|{log.status_code}|{log.created_at:O}";

                    log.record_hash = ComputeHmacSha256(rawData);

                    dbContext.Set<system_audit_log>().Add(log);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "稽核紀錄寫入失敗！");
                }
            }
        }

        private string ComputeHmacSha256(string rawData)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(rawData);

            using (var hmac = new HMACSHA256(_activeKeyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(messageBytes);

                StringBuilder builder = new StringBuilder(hashBytes.Length * 2);
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}