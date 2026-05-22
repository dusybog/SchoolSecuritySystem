using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;
using SchoolSecuritySystem.Core.DTOs.Email;
using SchoolSecuritySystem.Core.Interfaces.Repositories;

namespace SchoolSecuritySystem.Infrastructure.Services
{
    public class GraphEmailBackgroundService : BackgroundService
    {
        private readonly IEmailTaskQueue _taskQueue;
        private readonly ILogger<GraphEmailBackgroundService> _logger;
        private readonly GraphServiceClient _graphClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _senderEmail;

        public GraphEmailBackgroundService(
            IEmailTaskQueue taskQueue,
            ILogger<GraphEmailBackgroundService> logger,
            IServiceScopeFactory scopeFactory,
            IConfiguration configuration)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _scopeFactory = scopeFactory;

            var tenantId = configuration["GraphApi:TenantId"];
            var clientId = configuration["GraphApi:ClientId"];
            var clientSecret = configuration["GraphApi:ClientSecret"];
            _senderEmail = configuration["GraphApi:SenderEmail"];

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            _graphClient = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var payload = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    // 1. 執行 Graph API 寄信
                    await SendViaGraphApiAsync(payload);

                    // 2. 寄信成功，委派給 Repository 更新
                    await ReportStatusAsync(payload, isSuccess: true, "API 發送成功");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "背景寄信失敗");

                    // 3. 發生例外，委派給 Repository 更新
                    await ReportStatusAsync(payload, isSuccess: false, $"發送失敗: {ex.Message}");
                }
            }
        }

        private async Task ReportStatusAsync(EmailMessagePayload payload, bool isSuccess, string messageMsg)
        {
            if (payload.DispatchId == 0 || payload.DepartmentId == 0) return;

            // 🌟 建立 Scope 並解析 ISubmissionRepository，而非 DbContext
            using (var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ISubmissionRepository>();

                await repository.UpdateDispatchStatusAfterEmailSentAsync(
                    payload.DispatchId,
                    payload.DepartmentId,
                    payload.ToEmail,
                    isSuccess,
                    messageMsg
                );
            }
        }

        private async Task SendViaGraphApiAsync(EmailMessagePayload payload)
        {
            var message = new Message
            {
                Subject = payload.Subject,
                Body = new ItemBody { ContentType = BodyType.Html, Content = payload.Body },
                ToRecipients = new List<Recipient>
                {
                    new Recipient { EmailAddress = new EmailAddress { Address = payload.ToEmail } }
                },
                HasAttachments = payload.Attachments != null && payload.Attachments.Count > 0
            };

            if (message.HasAttachments == true)
            {
                message.Attachments = new List<Attachment>();

                foreach (var att in payload.Attachments)
                {
                    message.Attachments.Add(new FileAttachment
                    {
                        OdataType = "#microsoft.graph.fileAttachment",
                        Name = att.FileName,
                        ContentType = att.ContentType,
                        ContentBytes = att.Content
                    });
                }
            }

            var requestBody = new SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            };

            await _graphClient.Users[_senderEmail].SendMail.PostAsync(requestBody);
        }
    }
}