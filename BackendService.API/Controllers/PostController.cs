using BackendService.DAL.DTO;
using BackendService.DAL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostLogic postLogic) : ControllerBase
    {
        private readonly IPostLogic _postLogic = postLogic;

        [HttpGet("list")]
        public async Task<ActionResult<ICollection<PostDTO>>> GetPosts(CancellationToken token = default)
        {
            return await _postLogic.GetPosts(token);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO?>> GetPostById(int id, CancellationToken token = default)
        {
            return await _postLogic.GetPostById(id, token);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id, CancellationToken token = default)
        {
            await _postLogic.DeletePost(id, token);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(PostEditDTO postEntity, CancellationToken token = default)
        {
            var result = await _postLogic.SavePost(postEntity, token);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePost(PostEditDTO postEntity, CancellationToken token = default)
        {
            var result = await _postLogic.SavePost(postEntity, token);
            return Ok(result);
        }
    }
}
