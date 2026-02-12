using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface ITagLogic
    {
        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <returns>список тегов</returns>
        Task<List<TagEditDTO>> GetTags(CancellationToken token = default);

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <returns></returns>
        Task<TagEditDTO?> GetTagById(int tagId, CancellationToken token = default);

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="tagId">идентификатор тега</param>
        /// <returns>задача удаления</returns>
        Task DeleteTag(int tagId, CancellationToken token = default);

        /// <summary>
        /// Сохранение тега
        /// </summary>
        /// <param name="tag">тег</param>
        /// <returns>сохранённый тег</returns>
        Task<TagEditDTO> SaveTag(TagEditDTO tag, CancellationToken token = default);
    }
}
