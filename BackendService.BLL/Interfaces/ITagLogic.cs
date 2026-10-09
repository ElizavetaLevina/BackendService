using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface ITagLogic
    {
        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <param name="page">Номер страницы (начиная с 1)</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="token"Токен отмены</param>
        /// <returns>список тегов</returns>
        Task<List<TagEditDTO>> GetTags(int page, int pageSize,CancellationToken token = default);

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <param name="token"Токен отмены</param>
        /// <returns></returns>
        Task<TagEditDTO?> GetTagById(int tagId, CancellationToken token = default);

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <returns>задача удаления</returns>
        /// <param name="token"Токен отмены</param>
        Task DeleteTag(int tagId, CancellationToken token = default);

        /// <summary>
        /// Сохранение тега
        /// </summary>
        /// <param name="tag">тег</param>
        /// <param name="token"Токен отмены</param>
        /// <returns>сохранённый тег</returns>
        Task<TagEditDTO> SaveTag(TagEditDTO tag, CancellationToken token = default);
    }
}
