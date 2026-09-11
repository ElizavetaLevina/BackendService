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

			var deleted = await _postRepository.DeletePost(postId, token);

			if (deleted == false) throw new NotFoundException($"Пост с ID {postId} не найден и не может быть удалён");
        }

        /// <summary>
        /// Проверяет, является ли указанный пользователь владельцем поста
        /// </summary>
        /// <param name="postId">Идентификатор поста</param>
        /// <param name="userId">Идентификатор пользователя для проверки</param>
        /// <param name="token">Токен отмены</param>
        /// <returns>Результат проверки</returns>
        private async Task<bool> IsPostOwner(int postId, Guid userId, CancellationToken token = default)
        {
            var userIdInPost = await _postRepository.GetUserIdByPostId(postId, token);
            return userIdInPost is null ? throw new NotFoundException($"Пост с ID {postId} не найден") : userId == userIdInPost;
        }
    }
}
