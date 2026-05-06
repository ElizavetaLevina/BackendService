using BackendService.BLL.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.DTO;

namespace BackendService.BLL.Consumers
{
    public class PostModeratedEventConsumer(IPostPendingLogic postPendingLogic, ILogger<PostModeratedEventConsumer> logger) : IConsumer<PostModeratedEvent>
    {
        private readonly IPostPendingLogic _postPendingLogic = postPendingLogic;
        private readonly ILogger<PostModeratedEventConsumer> _logger = logger;

        public async Task Consume(ConsumeContext<PostModeratedEvent> context)
        {
            PostModeratedEvent message = context.Message ?? throw new InvalidOperationException("ConsumeContext.Message is null");

            if (message.RejectionReason is null)
            {
                _logger.LogInformation("Пост {Id} успешно прошел модерацию", message.PendingId);
                await _postPendingLogic.ApprovePost(message.PendingId);
            }
            else
            {
                _logger.LogInformation("Пост {Id} не прошел модерацию, причина: {RejectionReason}", message.PendingId, message.RejectionReason);
                await _postPendingLogic.RejectPost(message);
            }
        }
    }
}
