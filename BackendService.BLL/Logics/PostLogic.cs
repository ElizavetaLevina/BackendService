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

        public async Task<PostDTO?> GetPostById(int postId, CancellationToken token = default)
        {
            if (postId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            var post = await _postRepository.GetPostById(postId, token);

            return post is null ? throw new NotFoundException($"Пост с ID {postId} не найден") : post;
        }

        public async Task DeletePost(int postId, Guid userId, CancellationToken token = default)
        {
            if (postId <= 0) throw new ValidationException("ID должен быть положительным целым числом");

            if (await IsPostOwner(postId, userId, token) == false) throw new ForbiddenException("Недостаточно прав для удаления поста");

            try
            {
                await _postRepository.DeletePost(postId, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Пост с ID {postId} не найден и не может быть удалён");
            }
        }

        public async Task<PostEditDTO> SavePost(PostEditDTO post, Guid userId, CancellationToken token = default)
        {
            if (post.Id < 0) throw new ValidationException("ID должен быть положительным целым числом");

            if (post.Id > 0 && await IsPostOwner(post.Id, userId, token) == false) throw new ForbiddenException("Недостаточно прав для редактирования поста");

            try
            {
                return await _postRepository.SavePost(post, userId, token);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Пост с ID {post.Id} не найден и не может быть отредактирован");
            }
        }

        public async Task<bool> IsPostOwner(int postId, Guid userId, CancellationToken token = default)
        {
            var userIdInPost = await _postRepository.GetUserIdByPostId(postId, token);
            return userIdInPost is null ? throw new NotFoundException($"Пост с ID {postId} не найден") : userId == userIdInPost;
        }
    }
}
