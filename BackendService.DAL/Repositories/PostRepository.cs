using AutoMapper;
using BackendService.Common.DTO;
using BackendService.BLL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DAL.Repositories
{
    public class PostRepository(ApplicationDbContext dbContext, IMapper mapper) : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        public async Task<List<PostDTO>> GetPosts(CancellationToken token = default)
        {
			
            return await _mapper.ProjectTo<PostDTO>(_dbContext.Posts.AsNoTracking().Where(c => c.Deleted == false).OrderBy(p => p.Id)).ToListAsync(token);
        }

        public async Task<PostDTO?> GetPostById(int postId, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostDTO>(_dbContext.Posts.AsNoTracking().Where(c => c.Id == postId)).FirstOrDefaultAsync(token);
        }

        public async Task<bool> DeletePost(int postId, CancellationToken token = default)
        {
            var postEntity = await _dbContext.Posts.Include(p => p.Tags).Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == postId, token);

			if (postEntity == null) return false;

            postEntity.Tags.Clear();
            postEntity.Images?.Clear();
            postEntity.Deleted = true;
            await _dbContext.SaveChangesAsync(token);
			return true;
        }

        public async Task<bool> SavePost(PostEditDTO post, Guid userId, CancellationToken token = default)
        {
            PostEntity? postEntity;

            if (post.Id != 0)
            {
                postEntity = await _dbContext.Posts.Include(p => p.Tags).Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == post.Id, token);

				if (postEntity == null) return false;

                _mapper.Map(post, postEntity);
                postEntity.Tags = await _dbContext.Tags.Where(c => post.Tags.Contains(c.Id)).ToListAsync(token);
                postEntity.Images = await _dbContext.Images.Where(c => post.Images.Contains(c.Id)).ToListAsync(token);
                postEntity.DateUpdate = DateTime.Now;
            }
            else
            {
                postEntity = _mapper.Map<PostEntity>(post);
                postEntity.DateCreate = DateTime.Now;
                postEntity.UserId = userId;
                postEntity.Tags = await _dbContext.Tags.Where(c => post.Tags.Contains(c.Id)).ToListAsync(token);

                _dbContext.Posts.Add(postEntity);
            }
			return true;
        }

        public async Task<Guid?> GetUserIdByPostId(int postId, CancellationToken token = default)
        {
            return (await _dbContext.Posts.AsNoTracking().Where(c => c.Id == postId).Select(c => c.UserId).FirstOrDefaultAsync(token));
        }
    }
}
