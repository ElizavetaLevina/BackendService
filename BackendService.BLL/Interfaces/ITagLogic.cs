using BackendService.Common.DTO;

namespace BackendService.BLL.Interfaces
{
    public interface ITagLogic
    {
        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <returns>список тегов</returns>
        Task<List<TagEditDTO>> GetTags();

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="id">идентификатор тега</param>
        /// <returns></returns>
        Task<TagEditDTO?> GetTagById(int id);

        /// <summary>
        /// Удаление тега
        /// </summary>
        /// <param name="id">идентификатор тега</param>
        /// <returns>задача удаления</returns>
        Task DeleteTag(int id);

        /// <summary>
        /// Сохранение тега
        /// </summary>
        /// <param name="tagEntity">тег</param>
        /// <returns>сохранённый тег</returns>
        Task<TagEditDTO> SaveTag(TagEditDTO tagEntity);
    }
}
