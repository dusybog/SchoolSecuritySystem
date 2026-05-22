using SchoolSecuritySystem.Core.DTOs.Email;
using System.Threading.Channels;

namespace SchoolSecuritySystem.Infrastructure.Services
{
    public interface IEmailTaskQueue
    {
        ValueTask QueueEmailAsync(EmailMessagePayload payload);
        ValueTask<EmailMessagePayload> DequeueAsync(CancellationToken cancellationToken);
    }

    public class EmailTaskQueue : IEmailTaskQueue
    {
        private readonly Channel<EmailMessagePayload> _queue;

        public EmailTaskQueue()
        {
            // 建立一個無上限的 Channel，並設定為單一讀取者 (SingleReader) 以優化效能
            var options = new UnboundedChannelOptions { SingleReader = true };
            _queue = Channel.CreateUnbounded<EmailMessagePayload>(options);
        }

        public async ValueTask QueueEmailAsync(EmailMessagePayload payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            await _queue.Writer.WriteAsync(payload);
        }

        public async ValueTask<EmailMessagePayload> DequeueAsync(CancellationToken cancellationToken)
        {
            // 這裡會非同步等待，直到有信件進入佇列才會繼續執行
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}