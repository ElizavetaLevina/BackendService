using AutoMapper;
using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DAL.Repositories
{
    public class ImageRepository(ApplicationDbContext dbContext, IMapper mapper) : IImageRepository
    {
        readonly ApplicationDbContext _dbContext = dbContext;
        readonly IMapper _mapper = mapper;

        public async Task<List<ImageViewDTO>> GetPostImages(int postId, CancellationToken token = default)
        {
			var query = _dbContext.Images.AsNoTracking().Where(c => c.PostId == postId && c.Deleted == false).OrderBy(c => c.Id);

            return await _mapper.ProjectTo<ImageViewDTO>(query).ToListAsync(token);
        }

        public async Task DeleteImage(int imageId, CancellationToken token = default)
        {
            var image = await _dbContext.Images.FirstOrDefaultAsync(c => c.Id == imageId, token);

			if (image == null) return;

            image.Deleted = true;
            await _dbContext.SaveChangesAsync(token);
        }        

        public async Task<int> SaveImage(byte[] data, int postId, CancellationToken token = default)
        {
            var imageEntity  = new ImageEntity() { Id = 0, Data = data, PostId = postId };

            _dbContext.Add(imageEntity);
            await _dbContext.SaveChangesAsync(token);
            return imageEntity.Id;
        }

        public async Task<int?> GetPostIdByImageId(int imageId, CancellationToken token = default)
        {
			return (await _dbContext.Images.AsNoTracking().Where(c => c.Id == imageId).Select(c => c.Id).FirstOrDefaultAsync(token));
        }
    }
}
