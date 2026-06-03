using AutoMapper;
using BackendService.BLL.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Contracts.DTO;

namespace BackendService.BLL.Logics
{
	public class PostPendingPublisherLogic(IPostPendingRepository postPendingRepository, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork, ILogger<IPostPendingPublisherLogic> logger, IMapper mapper) : IPostPendingPublisherLogic
	{
		private readonly IPostPendingRepository _postPendingRepository = postPendingRepository;
		private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly ILogger<IPostPendingPublisherLogic> _logger = logger;
		private readonly IMapper _mapper = mapper;

		public async Task PublishMessage(CancellationToken token = default)
		{
			var posts = await _postPendingRepository.GetPostsPendingNotPublishedBatch(token);

			foreach (var post in posts)
			{
				try
				{
					await _publishEndpoint.Publish(_mapper.Map<PostSubmittedForModeration>(post), token);
					await _postPendingRepository.UpdateStatusPublishedPost(post.Id, token);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Не удалось отправить на модерацию пост {PostId}", post.Id);
				}
			}

			await _unitOfWork.SaveChangesAsync(token);
		}
	}
}
