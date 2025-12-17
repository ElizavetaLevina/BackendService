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
            return await _mapper.ProjectTo<PostDTO>(_dbContext.Set<PostEntity>().OrderBy(p => p.Id)).ToListAsync(token);
        }

        public async Task<PostDTO?> GetPostById(int id, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostDTO>(_dbContext.Posts).FirstOrDefaultAsync(c => c.Id == id, token);
        }

        public async Task DeletePost(int id, CancellationToken token = default)
        {
            await _dbContext.Posts.Where(c => c.Id == id).ExecuteUpdateAsync(c => c.SetProperty(p => p.Deleted, true), token);
        }

        public async Task<PostEditDTO> SavePost(PostEditDTO postEntity, CancellationToken token = default)
        {
            var post = _mapper.Map<PostEditDTO, PostEntity>(postEntity);

            if (post.Id == 0)
            {
                post.DateCreate = DateTime.Now;
                post.DateUpdate = DateTime.Now;

                _dbContext.Posts.Add(post);
            }
            else
            {
                post.DateUpdate = DateTime.Now;

                _dbContext.Posts.Update(post);
            }

            await _dbContext.SaveChangesAsync(token);  
            return _mapper.Map<PostEditDTO>(post);
        } 
    }
}
