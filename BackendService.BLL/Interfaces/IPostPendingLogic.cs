using BackendService.Common.DTO;
using Shared.Contracts.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface IPostPendingLogic
    {
        /// <summary>
        /// Получение постов на модерации
        /// </summary>
        /// <param name="token">Токен отмены</param>
        /// <returns>Список постов</returns>
        Task<List<PostPendingViewDTO>> GetPostsPending(CancellationToken token = default);

        /// <summary>
        /// Получение поста на модерации по идентификатору
        /// </summary>
        /// <param name="postId">Идентификатор поста на модерации</param>
        /// <param name="token">Токен отмены</param>
        /// <returns><Пост/returns>
        Task<PostPendingViewDTO> GetPostPendingById(int postId, CancellationToken token = default);

        /// <summary>
        /// Добавление поста на модерацию
        /// </summary>
        /// <param name="postPending">Пост для добавления</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Добавленный пост</returns>
        Task<PostPendingEditDTO> SavePostPending(PostPendingEditDTO postPending, Guid userId, CancellationToken token = default);

        /// <summary>
        /// Удаления поста
        /// </summary>
        /// <param name="postPendingId">Идентификатор поста для удаления</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача удаления</returns>
        Task DeletePostPending(int postPendingId, CancellationToken token = default);

        /// <summary>
        ///  Одобряет пост после проверки сервисом модерации
        /// </summary>
        /// <param name="postPendingId">Идентификатор поста для удаления</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача одобрения</returns>
        Task ApprovePost(int postPendingId, CancellationToken token = default);

        /// <summary>
        /// Отклоняет пост после проверки сервисом модерации
        /// </summary>
        /// <param name="postModeratedEvent">Событие с результатом модерации</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача отклонения</returns>
        Task RejectPost(PostModeratedEvent postModeratedEvent, CancellationToken token = default);
    }
}
