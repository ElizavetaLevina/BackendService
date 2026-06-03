using BackendService.BLL.Interfaces;
using BackendService.DAL.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace BackendService.DAL.UnitOfWork
{
    public class UnitOfWork(ApplicationDbContext dbContext, IPostPendingRepository postPendingRepository, IPostRepository postRepository) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public IPostPendingRepository PostPendingRepository => postPendingRepository;

        public IPostRepository PostRepository => postRepository;

        public async Task SaveChangesAsync(CancellationToken token = default) => await _dbContext.SaveChangesAsync(token);

        public void Dispose() => _dbContext.Dispose();
    }
}
