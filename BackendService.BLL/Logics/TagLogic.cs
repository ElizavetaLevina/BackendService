using BackendService.Common.DTO;
using BackendService.BLL.Interfaces;
using BackendService.Common.Exceptions;

namespace BackendService.BLL.Logics
{
    public class TagLogic(ITagRepository tagRepository) : ITagLogic
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<List<TagEditDTO>> GetTags(CancellationToken token = default)
        {
            return await _tagRepository.GetTags(token);
        }

        public async Task<TagEditDTO?> GetTagById(int id, CancellationToken token = default)
        {
            if (id <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            var tag = await _tagRepository.GetTagById(id, token);

            return tag is null ? throw new NotFoundException($"Тег с ID {id} не найден") : tag;
        }

        public async Task DeleteTag(int id, CancellationToken token = default)
        {
            if (id <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            try
            {
                await _tagRepository.DeleteTag(id, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Тег с ID {id} не найден и не может быть удалён");
            }
        }

        public async Task<TagEditDTO> SaveTag(TagEditDTO tag, CancellationToken token = default)
        {
            if (tag.Id < 0) throw new ValidationException("ID должен быть положительным целым числом");

            try
            {
                return await _tagRepository.SaveTag(tag, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Тег с ID {tag.Id} не найден и не может быть отредактирован");
            }
        }
    }
}
