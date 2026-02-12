using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface ITagRepository
    {
        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <param name="token">токен отмены</param>
        /// <returns>список тегов</returns>
        Task<List<TagEditDTO>> GetTags(CancellationToken token = default);

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <param name="token">токен отмены</param>
        /// <returns>тег</returns>
        Task<TagEditDTO?> GetTagById(int tagId, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <param name="token">токен отмены</param>
        /// <returns>задача удаления</returns>
        Task DeleteTag(int tagId, CancellationToken token = default);

        /// <summary>
        /// Сохранение тега
        /// </summary>
        /// <param name="tag">тег для сохранения</param>
        /// <param name="token">токен отмены</param>
        /// <returns>сохранённый тег</returns>
        Task<TagEditDTO> SaveTag(TagEditDTO tag, CancellationToken token = default);
    }
}
