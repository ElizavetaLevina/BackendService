using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;

namespace BackendService.BLL.Logics
{
    public class PostLogic(IPostRepository postRepository) : IPostLogic
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<List<PostDTO>> GetPosts(CancellationToken token = default)
        {
            return await _postRepository.GetPosts(token);
        }

        public async Task<PostDTO?> GetPostById(int id, CancellationToken token = default)
        {
            return await _postRepository.GetPostById(id, token);
        }

        public async Task DeletePost(int id, CancellationToken token = default)
        {
            await _postRepository.DeletePost(id, token);
        }

        public async Task<PostEditDTO> SavePost(PostEditDTO post, CancellationToken token = default)
        {
            return await _postRepository.SavePost(post, token);
        }
    }
}
