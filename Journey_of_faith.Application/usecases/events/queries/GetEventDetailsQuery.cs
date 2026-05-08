using FluentValidation;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.events.queries
{
    public class GetEventDetailsQuery : IRequest<EventDetailsView?>
    {
        public int EventId { get; set; }
    }

    public class GetEventDetailsQueryValidator : AbstractValidator<GetEventDetailsQuery>
    {
        public GetEventDetailsQueryValidator()
        {
            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("Mã sự kiện không hợp lệ.");
        }
    }

    public class GetEventDetailsHandler : IRequestHandler<GetEventDetailsQuery, EventDetailsView?>
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetEventDetailsHandler(IEventRepository eventRepository, ICurrentUserService currentUserService)
        {
            _eventRepository = eventRepository;
            _currentUserService = currentUserService;
        }

        public async Task<EventDetailsView?> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            if (Guid.TryParse(_currentUserService.UserId, out var parsedUserId))
            {
                userId = parsedUserId;
            }

            return await _eventRepository.GetEventDetailsAsync(request.EventId, userId);
        }
    }
}
