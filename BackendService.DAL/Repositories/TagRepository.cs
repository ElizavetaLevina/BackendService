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
            return await _mapper.ProjectTo<TagEditDTO>(_dbContext.Set<TagEntity>()).ToListAsync(token);
        }

        public async Task<TagEditDTO?> GetTagById(int tagId, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<TagEditDTO>(_dbContext.Set<TagEntity>()).FirstOrDefaultAsync(c => c.Id == tagId, token);
        }

        public async Task DeleteTag(int tagId, CancellationToken token = default)
        {
            var postEntity = await _dbContext.Tags.Include(p => p.Posts).FirstAsync(p => p.Id == tagId, token);
            postEntity.Posts.Clear();
            postEntity.Deleted = true;
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<TagEditDTO> SaveTag(TagEditDTO tag, CancellationToken token = default)
        {
            TagEntity tagEntity;

            if (tag.Id != 0)
            {
                tagEntity = await _dbContext.Tags.FirstAsync(c => c.Id == tag.Id, token);
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
