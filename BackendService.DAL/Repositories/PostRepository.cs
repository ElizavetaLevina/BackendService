using BackendService.DAL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DAL.Repositories
{
    public class PostRepository(ApplicationDbContext dbContext) : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public async Task<List<PostEntity>> GetPosts(CancellationToken token = default)
        {
            return await _dbContext.Posts.ToListAsync(token);
        }

        public async Task<PostEntity?> GetPostById(int id, CancellationToken token = default)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(c => c.Id == id, token);
        }

        public async Task DeletePost(int id, CancellationToken token = default)
        {
            await _dbContext.Posts.Where(c => c.Id == id).ExecuteUpdateAsync(c => c.SetProperty(p => p.Deleted, true), token);
        }

        public async Task<PostEntity> SavePost(PostEntity postEntity, CancellationToken token = default)
        {
            if (postEntity.Id == 0)
                _dbContext.Posts.Add(postEntity);
            else
                _dbContext.Posts.Update(postEntity);

            await _dbContext.SaveChangesAsync(token);  
            return postEntity;
        } 
    }
}
