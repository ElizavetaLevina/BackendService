using BackendService.DAL.Models;

namespace BackendService.DAL.Interfaces
{
    public interface ITagLogic
    {
        /// <summary>
        /// Получение списка тегов
        /// </summary>
        /// <returns>список тегов</returns>
        Task<List<TagEntity>> GetTags();

        /// <summary>
        /// Получение тега по идентификатору
        /// </summary>
        /// <param name="id">идентификатор тега</param>
        /// <returns></returns>
        Task<TagEntity?> GetTagById(int id);

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
        Task<TagEntity> SaveTag(TagEntity tagEntity);
    }
}
