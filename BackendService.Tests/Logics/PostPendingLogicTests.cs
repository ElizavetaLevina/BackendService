using AutoMapper;
using BackendService.BLL.Interfaces;
using BackendService.BLL.Logics;
using BackendService.Common.DTO;
using BackendService.Common.Exceptions;
using MassTransit;
using Moq;
using Shared.Contracts.DTO;
using Shared.Contracts.Enum;

namespace BackendService.Tests.Logics
{
    public class PostPendingLogicTests
    {
        private readonly Mock<IPostPendingRepository> _postPendingRepository;
        private readonly Mock<IPostRepository> _postRepository;
        private readonly Mock<IPublishEndpoint> _publishEndpoint;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IMapper> _mapper;
        private readonly IPostPendingLogic _postPendingLogic;

        public PostPendingLogicTests()
        {
            _postPendingRepository = new Mock<IPostPendingRepository>();
            _postRepository = new Mock<IPostRepository>();
            _publishEndpoint = new Mock<IPublishEndpoint>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _postPendingLogic = new PostPendingLogic(_postPendingRepository.Object, _postRepository.Object, _unitOfWork.Object, _mapper.Object);
        }

        [Theory]
        [InlineData(2, 10, "11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222")]
        [InlineData(0, 11, "11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222")]
        public async Task SavePostPending_UserIsNotOwner_ThrowsForbiddenException(int id, int postId, string userIdString, string ownerIdString)
        {
            var userId = Guid.Parse(userIdString);
            var ownerId = Guid.Parse(ownerIdString);

            var fakeDTO = new PostPendingEditDTO { Id = id, PostId = postId };

            if (id > 0)
                _postPendingRepository.Setup(c => c.GetUserIdByPostPendingId(id, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);
            else
                _postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);

            await Assert.ThrowsAsync<ForbiddenException>(async () => await _postPendingLogic.SavePostPending(fakeDTO, userId));

            if (id > 0)
                _postPendingRepository.Verify(c => c.GetUserIdByPostPendingId(id, It.IsAny<CancellationToken>()), Times.Once);
            else
                _postRepository.Verify(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>()), Times.Once);

            _postPendingRepository.Verify(c => c.SavePostPending(fakeDTO, userId, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(-1, -1, null)]
        [InlineData(1, -1, StatusModerationEnum.Pending)]
        [InlineData(1, 11, StatusModerationEnum.Pending)]
        public async Task SavePostPending_InvalidId_ThrowsValidationException(int id, int postId, StatusModerationEnum? status)
        {
            var userId = Guid.NewGuid();
            var fakeDTO = new PostPendingEditDTO { Id = id, PostId = postId };

            if (id > 0 && postId > 0)
                _postPendingRepository.Setup(c => c.GetPostPendingStatus(id, It.IsAny<CancellationToken>())).ReturnsAsync(status);

            await Assert.ThrowsAsync<ValidationException>(async () => await _postPendingLogic.SavePostPending(fakeDTO, userId));

            if (id > 0 && postId > 0)
                _postPendingRepository.Verify(c => c.GetPostPendingStatus(id, It.IsAny<CancellationToken>()), Times.Once);

            _postPendingRepository.Verify(c => c.SavePostPending(fakeDTO, userId, It.IsAny<CancellationToken>()), Times.Never);
        }

		[Theory]
		[InlineData(0, 1, "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111", null)]
		[InlineData(1, 1, "11111111-1111-1111-1111-111111111111", "11111111-1111-1111-1111-111111111111", StatusModerationEnum.Rejected)]
		public async Task SavePostPending_SavesPost(int id, int postId, string userIdString, string ownerIdString, StatusModerationEnum? status)
		{
			var userId = Guid.Parse(userIdString);
			var ownerId = Guid.Parse(ownerIdString);
			var fakeDTO = new PostPendingEditDTO { Id = id, PostId = postId };
			var expectedEvent = new PostSubmittedForModeration { Id = fakeDTO.Id, PostId = fakeDTO.PostId };

			if (id > 0)
			{
				_postPendingRepository.Setup(c => c.GetUserIdByPostPendingId(id, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);
				_postPendingRepository.Setup(c => c.GetPostPendingStatus(id, It.IsAny<CancellationToken>())).ReturnsAsync(status);
			}
			else
				_postRepository.Setup(c => c.GetUserIdByPostId(postId, It.IsAny<CancellationToken>())).ReturnsAsync(ownerId);

			_postPendingRepository.Setup(c => c.SavePostPending(fakeDTO, userId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);

			var result = await _postPendingLogic.SavePostPending(fakeDTO, userId);

			Assert.NotNull(result);
			Assert.Equal(fakeDTO.Id, result.Id);
			Assert.Equal(fakeDTO.PostId, result.PostId);

			_postPendingRepository.Verify(c => c.SavePostPending(fakeDTO, userId, It.IsAny<CancellationToken>()), Times.Once);
		}

		[Theory]
        [InlineData(2, 0)]
        [InlineData(2, 3)]
        public async Task ApprovePost_Success_CallsSaveAndDeleteAndSaveChanges(int postPendingId, int postId)
        {
            var userId = Guid.NewGuid();
            var fakeDTO = new PostPendingViewDTO { Id = postPendingId, PostId = postId, UserId = userId };
            var expectedEvent = new PostEditDTO { Id = (int)fakeDTO.PostId };

            _postPendingRepository.Setup(c => c.GetPostPendingById(postPendingId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);
            _mapper.Setup(c => c.Map<PostEditDTO>(fakeDTO)).Returns(expectedEvent);
            _postRepository.Setup(c => c.SavePost(expectedEvent, fakeDTO.UserId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _postPendingRepository.Setup(c => c.DeletePostPending(postPendingId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _unitOfWork.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()));

            await _postPendingLogic.ApprovePost(postPendingId);

            _postPendingRepository.Verify(c => c.GetPostPendingById(postPendingId, It.IsAny<CancellationToken>()), Times.Once);
            _mapper.Verify(c => c.Map<PostEditDTO>(fakeDTO), Times.Once);
            _postRepository.Verify(c => c.SavePost(expectedEvent, fakeDTO.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _postPendingRepository.Verify(c => c.DeletePostPending(postPendingId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(2, 0)]
        public async Task ApprovePost_SavePostThrows_ThrowsInvalidOperationException(int postPendingId, int postId)
        {
            var userId = Guid.NewGuid();
            var fakeDTO = new PostPendingViewDTO { Id = postPendingId, PostId = postId, UserId = userId };
            var expectedEvent = new PostEditDTO { Id = (int)fakeDTO.PostId };

            _postPendingRepository.Setup(c => c.GetPostPendingById(postPendingId, It.IsAny<CancellationToken>())).ReturnsAsync(fakeDTO);
            _mapper.Setup(c => c.Map<PostEditDTO>(fakeDTO)).Returns(expectedEvent);
            _postRepository.Setup(c => c.SavePost(expectedEvent, fakeDTO.UserId, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _postPendingRepository.Setup(c => c.DeletePostPending(postPendingId, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<InvalidOperationException>(async() => await _postPendingLogic.ApprovePost(postPendingId));

            _postPendingRepository.Verify(c => c.GetPostPendingById(postPendingId, It.IsAny<CancellationToken>()), Times.Once);
            _mapper.Verify(c => c.Map<PostEditDTO>(fakeDTO), Times.Once);
            _postRepository.Verify(c => c.SavePost(expectedEvent, fakeDTO.UserId, It.IsAny<CancellationToken>()), Times.Once);
            _postPendingRepository.Verify(c => c.DeletePostPending(postPendingId, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWork.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Theory]
        [InlineData(2)]
        public async Task RejectPost_Success_CallsUpdateModerationResult(int postPendingId)
        {
            var fakeDTO = new PostModeratedEvent { PendingId = postPendingId };
            _postPendingRepository.Setup(c => c.UpdateModerationResult(fakeDTO, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            await _postPendingLogic.RejectPost(fakeDTO);

            _postPendingRepository.Verify(c => c.UpdateModerationResult(fakeDTO, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(2)]
        public async Task RejectPost_UpdateModerationResultThrows_ThrowsInvalidOperationException(int postPendingId)
        {
            var fakeDTO = new PostModeratedEvent { PendingId = postPendingId };
            _postPendingRepository.Setup(c => c.UpdateModerationResult(fakeDTO, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<InvalidOperationException>(async() => await _postPendingLogic.RejectPost(fakeDTO));

            _postPendingRepository.Verify(c => c.UpdateModerationResult(fakeDTO, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
