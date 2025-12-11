using BackendService.DAL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DAL.Repositories
{
    public class TagRepository(ApplicationDbContext dbContext) : ITagRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<TagEntity>> GetTags(CancellationToken token = default)
        {
            return await _dbContext.Tags.ToListAsync(token);
        }

        public async Task<TagEntity?> GetTagById(int id, CancellationToken token = default)
        {
            return await _dbContext.Tags.FirstOrDefaultAsync(c => c.Id == id, token);
        }

        public async Task DeleteTag(int id, CancellationToken token = default)
        {
            await _dbContext.Tags.Where(c => c.Id == id).ExecuteUpdateAsync(c => c.SetProperty(p => p.Deleted, false), token);
        }

        public async Task<TagEntity> SaveTag(TagEntity tagEntity, CancellationToken token = default)
        {
            if (tagEntity.Id == 0)
                _dbContext.Tags.Add(tagEntity);
            else
                _dbContext.Tags.Update(tagEntity);

            await _dbContext.SaveChangesAsync(token);
            return tagEntity;
        }
    }
}
