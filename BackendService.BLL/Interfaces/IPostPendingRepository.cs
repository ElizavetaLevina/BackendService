using BackendService.Common.DTO;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace BackendService.BLL.Interfaces
{
    public interface IPostPendingRepository
    {
        /// <summary>
        /// Получение постов на модерации
        /// </summary>
        /// <param name="token">Токен отмены</param>
        /// <returns>Список постов</returns>
        Task<List<PostPendingEditDTO>> GetPostsPending(CancellationToken token = default);

        /// <summary>
        /// Получение поста на модерации по идентификатору
        /// </summary>
        /// <param name="postId">Идентификатор поста на модерации</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Пост</returns>
        Task<PostPendingEditDTO?> GetPostPendingById(int postPendingId, CancellationToken token = default);

        /// <summary>
        /// Получение статуса поста на модерации
        /// </summary>
        /// <param name="postPendingId">Идентификатор поста</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Статус</returns>
        Task<StatusModerationEnum?> GetPostPendingStatus(int postPendingId, CancellationToken token = default);

        /// <summary>
        /// Возвращает идентификатор владельца поста, находящегося на модерации
        /// </summary>
        /// <param name="postPendingId">идентификатор поста на модерации</param>
        /// <param name="token">токен отмены</param>
        /// <returns>Идентификатор владельца поста</returns>
        Task<Guid?> GetUserIdByPostPendingId(int postPendingId, CancellationToken token = default);

        /// <summary>
        /// Удаления поста
        /// </summary>
        /// <param name="postPendingId">Идентификатор поста для удаления</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача удаления</returns>
        Task DeletePostPending(int postPendingId, CancellationToken token = default);

        /// <summary>
        /// Добавление поста на модерацию
        /// </summary>
        /// <param name="postPending">Пост для добавления</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Добавленный пост</returns>
        Task<PostPendingEditDTO> SavePostPending(PostPendingEditDTO postPending, CancellationToken token = default);

        /// <summary>
        /// Обновляет результат модерации поста на основе полученного события
        /// </summary>
        /// <param name="postModeratedEvent">Событие с результатом модерации</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Задача обновления результата модерации</returns>
        Task UpdateModerationResult(PostModeratedEvent postModeratedEvent, CancellationToken token = default);
    }
}
