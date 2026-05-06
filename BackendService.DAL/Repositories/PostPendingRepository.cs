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

        public async Task<List<PostPendingViewDTO>> GetPostsPending(CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostPendingViewDTO>(_dbContext.PostsPending).ToListAsync(token);

        }

        public async Task<PostPendingViewDTO?> GetPostPendingById(int postPendingId, CancellationToken token = default)
        {
            return await _mapper.ProjectTo<PostPendingViewDTO>(_dbContext.PostsPending).FirstOrDefaultAsync(c => c.Id == postPendingId, token);
        }

        public async Task<StatusModerationEnum?> GetPostPendingStatus(int postPendingId, CancellationToken token = default)
        {
            return (await _dbContext.PostsPending.FirstOrDefaultAsync(c => c.Id == postPendingId, token))?.Status;
        }

        public async Task<Guid?> GetUserIdByPostPendingId(int postPendingId, CancellationToken token = default)
        {
            return (await _dbContext.PostsPending.FirstOrDefaultAsync(c => c.Id == postPendingId, token))?.UserId;
        }

        public async Task DeletePostPending(int postPendingId, CancellationToken token = default)
        {
            _dbContext.PostsPending.Remove(await _dbContext.PostsPending.FirstAsync(c => c.Id == postPendingId, token));
        }

        public async Task<PostPendingEditDTO> SavePostPending(PostPendingEditDTO postPending, Guid userId, CancellationToken token = default)
        {
            PostPendingEntity postPendingEntity;

            if (postPending.Id == 0)
            {
                postPendingEntity = _mapper.Map<PostPendingEntity>(postPending);
                postPendingEntity.UserId = userId;
                _dbContext.PostsPending.Add(postPendingEntity);
            }
            else
            {
                postPendingEntity = await _dbContext.PostsPending.FirstAsync(c => c.Id == postPending.Id, token);
                _mapper.Map(postPending, postPendingEntity);
                postPendingEntity.Status = StatusModerationEnum.Pending;
                postPendingEntity.RejectionReason = null;
            }
            
            await _dbContext.SaveChangesAsync(token);
            return _mapper.Map<PostPendingEditDTO>(postPendingEntity);
        }

        public async Task UpdateModerationResult(PostModeratedEvent postModeratedEvent, CancellationToken token = default)
        {
            PostPendingEntity postPendingEntity = await _dbContext.PostsPending.FirstAsync(c => c.Id == postModeratedEvent.PendingId, token);
            postPendingEntity.Status = postModeratedEvent.Status;
            postPendingEntity.RejectionReason = postModeratedEvent.RejectionReason;

            _dbContext.PostsPending.Update(postPendingEntity);
            await _dbContext.SaveChangesAsync(token);
        }
    }
}
