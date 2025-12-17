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
        public Task<List<TagEditDTO>> GetTags(CancellationToken token = default);

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="id">идентификатор тега</param>
        /// <param name="token">токен отмены</param>
        /// <returns>тег</returns>
        public Task<TagEditDTO?> GetTagById(int id, CancellationToken token = default);

        /// <summary>
        /// Удаление поста
        /// </summary>
        /// <param name="id">идентификатор тега</param>
        /// <param name="token">токен отмены</param>
        /// <returns>задача удаления</returns>
        public Task DeleteTag(int id, CancellationToken token = default);

        /// <summary>
        /// Сохранение тега
        /// </summary>
        /// <param name="tagEntity">тег для сохранения</param>
        /// <param name="token">токен отмены</param>
        /// <returns>сохранённый тег</returns>
        public Task<TagEditDTO> SaveTag(TagEditDTO tagEntity, CancellationToken token = default);
    }
}
