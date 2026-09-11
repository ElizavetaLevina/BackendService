using AutoMapper;
using BackendService.Common.DTO;
using BackendService.BLL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DAL.Repositories
{
    public class TagRepository(ApplicationDbContext dbContext, IMapper mapper) : ITagRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TagEditDTO>> GetTags(CancellationToken token = default)
        {
            return await _mapper.ProjectTo<TagEditDTO>(_dbContext.Tags.AsNoTracking()).ToListAsync(token);
        }

        public async Task<TagEditDTO?> GetTagById(int tagId, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<TagEditDTO>(_dbContext.Tags.AsNoTracking().Where(c => c.Id == tagId)).FirstOrDefaultAsync(token);
        }

        public async Task<bool> DeleteTag(int tagId, CancellationToken token = default)
        {
            var postEntity = await _dbContext.Tags.Include(p => p.Posts).FirstOrDefaultAsync(p => p.Id == tagId, token);

			if (postEntity == null) return false;

            postEntity.Posts.Clear();
            postEntity.Deleted = true;
            await _dbContext.SaveChangesAsync(token);
			return true;
        }

        public async Task<TagEditDTO?> SaveTag(TagEditDTO tag, CancellationToken token = default)
        {
            TagEntity? tagEntity;

            if (tag.Id != 0)
            {
                tagEntity = await _dbContext.Tags.FirstOrDefaultAsync(c => c.Id == tag.Id, token);

				if (tagEntity == null) return null;

                _mapper.Map(tag, tagEntity);
            }
            else
            {
                tagEntity = _mapper.Map<TagEntity>(tag);
                _dbContext.Tags.Add(tagEntity);
            }

            await _dbContext.SaveChangesAsync(token);
            return _mapper.Map<TagEditDTO>(tagEntity);
        }
    }
}
