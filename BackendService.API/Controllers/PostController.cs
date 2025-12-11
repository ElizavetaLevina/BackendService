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
        public async Task<ActionResult<ICollection<PostEntity>>> GetPosts()
        {
            return await _postLogic.GetPosts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostEntity?>> GetPostById(int id)
        {
            return await _postLogic.GetPostById(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePost(int id)
        {
            await _postLogic.DeletePost(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(PostEntity postEntity)
        {
            var result = await _postLogic.SavePost(postEntity);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePost(PostEntity postEntity)
        {
            var result = await _postLogic.SavePost(postEntity);
            return Ok(result);
        }
    }
}
