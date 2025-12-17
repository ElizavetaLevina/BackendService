using AutoMapper;
using BackendService.DAL.DTO;
using BackendService.DAL.Interfaces;
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
            return await _mapper.ProjectTo<TagEditDTO>(_dbContext.Tags).ToListAsync(token);
        }

        public async Task<TagEditDTO?> GetTagById(int id, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<TagEditDTO>(_dbContext.Posts).FirstOrDefaultAsync(c => c.Id == id, token);
        }

        public async Task DeleteTag(int id, CancellationToken token = default)
        {
            await _dbContext.Tags.Where(c => c.Id == id).ExecuteUpdateAsync(c => c.SetProperty(p => p.Deleted, false), token);
        }

        public async Task<TagEditDTO> SaveTag(TagEditDTO tagEntity, CancellationToken token = default)
        {
            var tag = _mapper.Map<TagEditDTO, TagEntity>(tagEntity);

            if (tag.Id == 0)
                _dbContext.Tags.Add(tag);
            else
                _dbContext.Tags.Update(tag);

            await _dbContext.SaveChangesAsync(token);
            return _mapper.Map<TagEditDTO>(tagEntity);
        }
    }
}
