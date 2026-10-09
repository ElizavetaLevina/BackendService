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
			var publishedIds = new List<int>();

			foreach (var post in posts)
			{
				try
				{
					await _publishEndpoint.Publish(_mapper.Map<PostSubmittedForModeration>(post), token);
					publishedIds.Add(post.Id);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Не удалось отправить на модерацию пост {PostId}", post.Id);
				}
			}

			var updatedIds = await _postPendingRepository.UpdateStatusPublishedPosts(publishedIds, token);

			foreach (var postId in publishedIds)
			{
				if (!updatedIds.Contains(postId))
					_logger.LogWarning("Пост {PostId} не найден при обновлении статуса", postId);
			}

			await _unitOfWork.SaveChangesAsync(token);
		}
	}
}
