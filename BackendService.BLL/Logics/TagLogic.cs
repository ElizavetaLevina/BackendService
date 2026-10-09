using BackendService.Common.DTO;
using BackendService.BLL.Interfaces;
using BackendService.Common.Exceptions;

namespace BackendService.BLL.Logics
{
    public class TagLogic(ITagRepository tagRepository) : ITagLogic
    {
        private readonly ITagRepository _tagRepository = tagRepository;

        private const int MaxPageSize = 100;

        public async Task<List<TagEditDTO>> GetTags(int page, int pageSize, CancellationToken token = default)
        {
            if (page < 1) throw new ValidationException("Номер страницы должен быть положительным целым числом");
            if (pageSize < 1) throw new ValidationException("Размер страницы должен быть положительным целым числом");
            if (pageSize > MaxPageSize) throw new ValidationException($"Размер страницы не должен превышать {MaxPageSize}");

            return await _tagRepository.GetTags(page, pageSize, token);
        }

        public async Task<TagEditDTO?> GetTagById(int tagId, CancellationToken token = default)
        {
            if (tagId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            var tag = await _tagRepository.GetTagById(tagId, token);

            return tag is null ? throw new NotFoundException($"Тег с ID {tagId} не найден") : tag;
        }

        public async Task DeleteTag(int tagId, CancellationToken token = default)
        {
            if (tagId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

			var deleted = await _tagRepository.DeleteTag(tagId, token);

			if (deleted == false) throw new NotFoundException($"Тег с ID {tagId} не найден и не может быть удалён");
        }

        public async Task<TagEditDTO> SaveTag(TagEditDTO tag, CancellationToken token = default)
        {
            if (tag.Id < 0) throw new ValidationException("ID должен быть положительным целым числом");

			return await _tagRepository.SaveTag(tag, token) ?? throw new NotFoundException($"Тег с ID {tag.Id} не найден и не может быть отредактирован");
        }
    }
}
