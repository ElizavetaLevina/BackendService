using BackendService.DAL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(ITagLogic tagLogic) : ControllerBase
    {
        private readonly ITagLogic _tagLogic = tagLogic;

        [HttpGet("list")]
        public async Task<ActionResult<ICollection<TagEntity>>> GetPosts()
        {
            return await _tagLogic.GetTags();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TagEntity?>> GetPostById(int id)
        {
            return await _tagLogic.GetTagById(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTag(int id)
        {
            await _tagLogic.DeleteTag(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> CreatePost(TagEntity tagEntity)
        {
            var result = await _tagLogic.SaveTag(tagEntity);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePost(TagEntity tagEntity)
        {
            var result = await _tagLogic.SaveTag(tagEntity);
            return Ok(result);
        }
    }
}
