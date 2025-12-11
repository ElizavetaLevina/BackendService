using BackendService.DAL.Interfaces;
using BackendService.DAL.Models;

namespace BackendService.DAL.Logics
{
    public class PostLogic(IPostRepository postRepository) : IPostLogic
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<List<PostEntity>> GetPosts()
        {
            return await _postRepository.GetPosts();
        }

        public async Task<PostEntity?> GetPostById(int id)
        {
            return await _postRepository.GetPostById(id);
        }

        public async Task DeletePost(int id)
        {
            await _postRepository.DeletePost(id);
        }

        public async Task<PostEntity> SavePost(PostEntity post)
        {
            return await _postRepository.SavePost(post);
        }
    }
}
