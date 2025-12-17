using BackendService.Common.DTO;
using BackendService.BLL.Interfaces;

namespace BackendService.BLL.Logics
{
    public class TagLogic(ITagRepository tagRepository) : ITagLogic
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<List<TagEditDTO>> GetTags()
        {
            return await _tagRepository.GetTags();
        }

        public async Task<TagEditDTO?> GetTagById(int id)
        {
            return await _tagRepository.GetTagById(id);
        }

        public async Task DeleteTag(int id)
        {
            await _tagRepository.DeleteTag(id);
        }

        public async Task<TagEditDTO> SaveTag(TagEditDTO tagEntity)
        {
            return await _tagRepository.SaveTag(tagEntity);
        }
    }
}
