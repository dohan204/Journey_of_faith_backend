using FluentValidation;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands
{
    public class UnfollowChurchCommand : IRequest<bool>
    {
        public int ChurchId { get; set; }
    }

    public class UnfollowChurchCommandValidator : AbstractValidator<UnfollowChurchCommand>
    {
        public UnfollowChurchCommandValidator()
        {
            RuleFor(x => x.ChurchId)
                .GreaterThan(0)
                .WithMessage("Mã nhà thờ không hợp lệ.");
        }
    }

    public class UnfollowChurchHandler : IRequestHandler<UnfollowChurchCommand, bool>
    {
        private readonly IChurchRepository _churchRepository;
        private readonly ICurrentUserService _currentUserService;

        public UnfollowChurchHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService)
        {
            _churchRepository = churchRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UnfollowChurchCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                throw new UnauthorizationException("Không xác định được người dùng hiện tại.");
            }

            if (!await _churchRepository.ChurchExistsAsync(request.ChurchId))
            {
                throw new NotFoundException("Không tìm thấy nhà thờ.");
            }

            if (!await _churchRepository.IsFollowingChurchAsync(userId, request.ChurchId))
            {
                throw new NotFoundException("Nhà thờ chưa có trong danh sách theo dõi.");
            }

            return await _churchRepository.UnfollowChurchAsync(userId, request.ChurchId);
        }
    }
}
