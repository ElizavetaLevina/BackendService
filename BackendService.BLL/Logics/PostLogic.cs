using BackendService.Common.Exceptions;
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
            if (id <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            var post = await _postRepository.GetPostById(id, token);

            return post is null ? throw new NotFoundException($"Пост с ID {id} не найден") : post;
        }

        public async Task DeletePost(int id, CancellationToken token = default)
        {
            if (id <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            try
            {
                await _postRepository.DeletePost(id, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Пост с ID {id} не найден и не может быть удалён");
            }
        }

        public async Task<PostEditDTO> SavePost(PostEditDTO post, CancellationToken token = default)
        {
            if (post.Id < 0) throw new ValidationException("ID должен быть положительным целым числом");

            try
            {
                return await _postRepository.SavePost(post, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Пост с ID {post.Id} не найден и не может быть отредактирован");
            }
        }
    }
}
