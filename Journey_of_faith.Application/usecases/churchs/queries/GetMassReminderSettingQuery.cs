using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries
{
    public class GetMassReminderSettingQuery : IRequest<ReminderSettingView>
    {
    }

    public class GetMassReminderSettingHandler : IRequestHandler<GetMassReminderSettingQuery, ReminderSettingView>
    {
        private readonly IChurchRepository _churchRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMassReminderSettingHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService)
        {
            _churchRepository = churchRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ReminderSettingView> Handle(GetMassReminderSettingQuery request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
            {
                throw new UnauthorizationException("Không xác định được người dùng hiện tại.");
            }

            return await _churchRepository.GetReminderSettingAsync(userId);
        }
    }
}
