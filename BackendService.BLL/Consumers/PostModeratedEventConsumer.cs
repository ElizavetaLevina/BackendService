using BackendService.BLL.Interfaces;
using MassTransit;
using Shared.Contracts.DTO;

namespace BackendService.BLL.Consumers
{
    public class PostModeratedEventConsumer(IPostPendingLogic postPendingLogic) : IConsumer<PostModeratedEvent>
    {
        private readonly IPostPendingLogic _postPendingLogic = postPendingLogic;

        public async Task Consume(ConsumeContext<PostModeratedEvent> context)
        {
            PostModeratedEvent message = context.Message ?? throw new InvalidOperationException("ConsumeContext.Message is null");

            if (message.RejectionReason is null)
            {
                await _postPendingLogic.ApprovePost(message.PendingId);
            }
            else
            {
                await _postPendingLogic.RejectPost(message);
            }
        }
    }
}
