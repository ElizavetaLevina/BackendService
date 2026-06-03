using Microsoft.EntityFrameworkCore.Storage;

namespace BackendService.BLL.Interfaces
{
    /// <summary>
    /// Unit of Work для управления транзакциями и репозиториями
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Репозиторий постов на модерации
        /// </summary>
        IPostPendingRepository PostPendingRepository { get; }

        /// <summary>
        /// Репозиторий опубликованных постов
        /// </summary>
        IPostRepository PostRepository { get; }

        /// <summary>
        /// Сохраняет все изменения в рамках одной транзакции
        /// </summary>
        /// <param name="token">Токен отмены</param>
        Task SaveChangesAsync(CancellationToken token = default);
    }

}
