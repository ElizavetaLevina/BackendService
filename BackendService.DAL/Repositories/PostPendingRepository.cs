using AutoMapper;
using BackendService.BLL.Interfaces;
using BackendService.Common.DTO;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace BackendService.DAL.Repositories
{
    public class PostPendingRepository(ApplicationDbContext dbContext, IMapper mapper) : IPostPendingRepository
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;
        private const int BatchSize = 50;

        public async Task<List<PostPendingViewDTO>> GetPostsPending(CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostPendingViewDTO>(_dbContext.PostsPending.AsNoTracking()).ToListAsync(token);
        }

		public async Task<List<PostPendingEditDTO>> GetPostsPendingNotPublishedBatch(CancellationToken token = default)
		{
			var query = _dbContext.PostsPending.AsNoTracking().Where(c => c.Status == StatusModerationEnum.Pending).Take(BatchSize);

			return await _mapper.ProjectTo<PostPendingEditDTO>(query).ToListAsync(token);
		}

		public async Task<PostPendingViewDTO?> GetPostPendingById(int postPendingId, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostPendingViewDTO>(_dbContext.PostsPending).AsNoTracking().FirstOrDefaultAsync(c => c.Id == postPendingId, token);
        }

        public async Task<StatusModerationEnum?> GetPostPendingStatus(int postPendingId, CancellationToken token = default)
        {
            return (await _dbContext.PostsPending.AsNoTracking().Where(c => c.Id == postPendingId).Select(c => c.Status).FirstOrDefaultAsync(token));
        }

        public async Task<Guid?> GetUserIdByPostPendingId(int postPendingId, CancellationToken token = default)
        {
            return (await _dbContext.PostsPending.AsNoTracking().Where(c => c.Id == postPendingId).Select(c => c.UserId).FirstOrDefaultAsync(token));
        }

        public async Task<bool> DeletePostPending(int postPendingId, CancellationToken token = default)
        {
			var postPending = await _dbContext.PostsPending.FirstOrDefaultAsync(c => c.Id == postPendingId, token);

			if (postPending == null) return false;

			_dbContext.PostsPending.Remove(postPending);
			return true;
        }

        public async Task<PostPendingEditDTO?> SavePostPending(PostPendingEditDTO postPending, Guid userId, CancellationToken token = default)
        {
            PostPendingEntity? postPendingEntity;

            if (postPending.Id == 0)
            {
                postPendingEntity = _mapper.Map<PostPendingEntity>(postPending);
                postPendingEntity.UserId = userId;
                _dbContext.PostsPending.Add(postPendingEntity);
            }
            else
            {
                postPendingEntity = await _dbContext.PostsPending.FirstOrDefaultAsync(c => c.Id == postPending.Id, token);
                if (postPendingEntity is null) return null;

                _mapper.Map(postPending, postPendingEntity);
                postPendingEntity.Status = StatusModerationEnum.Pending;
                postPendingEntity.RejectionReason = null;
            }

            await _dbContext.SaveChangesAsync(token);
            return _mapper.Map<PostPendingEditDTO>(postPendingEntity);
        }

        public async Task<bool> UpdateModerationResult(PostModeratedEvent postModeratedEvent, CancellationToken token = default)
        {
            PostPendingEntity? postPendingEntity = await _dbContext.PostsPending.FirstOrDefaultAsync(c => c.Id == postModeratedEvent.PendingId, token);

			if (postPendingEntity == null) return false;

            postPendingEntity.Status = postModeratedEvent.Status;
            postPendingEntity.RejectionReason = postModeratedEvent.RejectionReason;

            await _dbContext.SaveChangesAsync(token);
			return true;
        }

        public async Task<bool> UpdateStatusPublishedPost(int postPendingId, CancellationToken token = default)
        {
            PostPendingEntity? postPendingEntity = await _dbContext.PostsPending.FirstOrDefaultAsync(c => c.Id == postPendingId, token);

			if (postPendingEntity == null) return false;

            postPendingEntity.Status = StatusModerationEnum.SentForModeration;
			return true;
		}
	}
}
