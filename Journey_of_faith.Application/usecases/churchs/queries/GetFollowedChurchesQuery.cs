using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries
{
    public class GetFollowedChurchesQuery : IRequest<IEnumerable<Church>>
    {
    }

    public class GetFollowedChurchesHandler : IRequestHandler<GetFollowedChurchesQuery, IEnumerable<Church>>
    {
        private readonly IChurchRepository _churchRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetFollowedChurchesHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService)
        {
            _churchRepository = churchRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<Church>> Handle(GetFollowedChurchesQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                throw new UnauthorizationException("Không xác định được người dùng hiện tại.");
            }

            return await _churchRepository.GetFollowedChurchesAsync(userId);
        }
    }
}
