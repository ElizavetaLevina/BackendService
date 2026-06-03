using AutoMapper;
using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace BackendService.BLL.Logics
{
    public class PostPendingLogic(IPostPendingRepository postPendingRepository, IPostRepository postRepository, IUnitOfWork unitOfWork, IMapper mapper) : IPostPendingLogic
    {
        private readonly IPostPendingRepository _postPendingRepository = postPendingRepository;
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<List<PostPendingViewDTO>> GetPostsPending(CancellationToken token = default)
        {
            return await _postPendingRepository.GetPostsPending(token);
        }

        public async Task<PostPendingViewDTO> GetPostPendingById(int postPendingId, CancellationToken token = default)
        {
            if (postPendingId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            var postPending = await _postPendingRepository.GetPostPendingById(postPendingId, token);

            return postPending is null ? throw new NotFoundException($"Пост с ID {postPendingId} не найден") : postPending;
        }

        public async Task DeletePostPending(int postPendingId, CancellationToken token = default)
        {
            if (postPendingId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            try
            {
                await _postPendingRepository.DeletePostPending(postPendingId, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Пост с ID {postPendingId} не найден и не может быть удалён");
            }
        }

        public async Task<PostPendingEditDTO> SavePostPending(PostPendingEditDTO postPending, Guid userId, CancellationToken token = default)
        {
            if (postPending.Id < 0 || postPending.PostId < 0) throw new ValidationException("ID должен быть положительным целым числом");

            if (postPending.Id > 0 && await IsPostModeration(postPending.Id, token)) throw new ValidationException($"Пост с ID {postPending.PostId} находится на модерации, его нельзя отредактировать");

            if (postPending.PostId is not null && postPending.Id > 0 && await IsPostPendingOwner(postPending.Id, userId, token) == false) throw new ForbiddenException("Недостаточно прав для редактирования поста");

            if (postPending.Id == 0 && postPending.PostId is not null && await IsPostOwner((int)postPending.PostId, userId, token) == false) throw new ForbiddenException("Недостаточно прав для редактирования поста");

            try
            {
                var result = await _postPendingRepository.SavePostPending(postPending, userId, token);
                return result ?? throw new NotFoundException($"Пост с ID {postPending.Id} не найден и не может быть отредактирован");
			}
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при сохранении поста", ex);
            }
        }

        public async Task ApprovePost(int postPendingId, CancellationToken token = default)
        {
            try
            {
                var postPending = await GetPostPendingById(postPendingId, token);
                var post = _mapper.Map<PostEditDTO>(postPending);
                await _postRepository.SavePost(post, postPending.UserId, token);
                await DeletePostPending(postPending.Id, token);

                await _unitOfWork.SaveChangesAsync(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при одобрении поста {postPendingId}", ex);
            }
        }

        public async Task RejectPost(PostModeratedEvent postModeratedEvent, CancellationToken token = default)
        {
            try
            {
                await _postPendingRepository.UpdateModerationResult(postModeratedEvent, token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при отклонении поста {postModeratedEvent.PendingId}", ex);
            }
        }

        /// <summary>
        /// Проверяет, является ли указанный пользователь владельцем поста
        /// </summary>
        /// <param name="postId">Идентификатор поста</param>
        /// <param name="userId">Идентификатор пользователя для проверки</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Результат проверки</returns>
        private async Task<bool> IsPostOwner(int postId, Guid userId, CancellationToken token = default)
        {
            var userIdInPost = await _postRepository.GetUserIdByPostId(postId, token);
            return userIdInPost is null ? throw new NotFoundException($"Пост с ID {postId} не найден") : userId == userIdInPost;
        }

        /// <summary>
        /// Проверяет, является ли указанный пользователь владельцем поста, находящегося на модерации
        /// </summary>
        /// <param name="postPendingId">Идентификатор поста на модерации</param>
        /// <param name="userId">Идентификатор пользователя для проверки</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Результат проверки</returns>
        private async Task<bool> IsPostPendingOwner(int postPendingId, Guid userId, CancellationToken token = default)
        {
            var userIdInPost = await _postPendingRepository.GetUserIdByPostPendingId(postPendingId, token);
            return userIdInPost is null ? throw new NotFoundException($"Пост с ID {postPendingId} не найден") : userId == userIdInPost;
        }

        /// <summary>
        /// Проверяет, находится ли данный пост на модерации
        /// </summary>
        /// <param name="postPendingId">Идентификатор поста</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Результат проверки</returns>
        private async Task<bool> IsPostModeration(int postPendingId, CancellationToken token = default)
        {
            var status = await _postPendingRepository.GetPostPendingStatus(postPendingId, token);
            return status is not null && (status == StatusModerationEnum.Pending || status == StatusModerationEnum.SentForModeration);
        }
    }
}