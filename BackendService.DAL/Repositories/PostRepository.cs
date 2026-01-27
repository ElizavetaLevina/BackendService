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
            return await _mapper.ProjectTo<PostDTO>(_dbContext.Set<PostEntity>().Where(c => c.Deleted == false).OrderBy(p => p.Id)).ToListAsync(token);
        }

        public async Task<PostDTO?> GetPostById(int id, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostDTO>(_dbContext.Set<PostEntity>()).FirstOrDefaultAsync(c => c.Id == id, token);
        }

        public async Task DeletePost(int id, CancellationToken token = default)
        {
            var postEntity = await _dbContext.Posts.Include(p => p.Tags).Include(p => p.Images).FirstAsync(p => p.Id == id, token);
            postEntity.Tags.Clear();
            postEntity.Images?.Clear();
            postEntity.Deleted = true;
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<PostEditDTO> SavePost(PostEditDTO post, CancellationToken token = default)
        {
            PostEntity postEntity;

            if (post.Id != 0)
            {
                postEntity = await _dbContext.Posts.Include(p => p.Tags).Include(p => p.Images).FirstAsync(p => p.Id == post.Id, token);
                _mapper.Map(post, postEntity);
                postEntity.Tags = await _dbContext.Tags.Where(c => post.Tags.Contains(c.Id)).ToListAsync(token);
                postEntity.Images = await _dbContext.Images.Where(c => post.Images.Contains(c.Id)).ToListAsync(token);
                postEntity.DateUpdate = DateTime.Now;
            }
            else
            {
                postEntity = _mapper.Map<PostEntity>(post);
                postEntity.DateCreate = DateTime.Now;
                postEntity.Tags = await _dbContext.Tags.Where(c => post.Tags.Contains(c.Id)).ToListAsync(token);

                _dbContext.Posts.Add(postEntity);
            }

            await _dbContext.SaveChangesAsync(token);
            return _mapper.Map<PostEditDTO>(postEntity);
        } 
    }
}
