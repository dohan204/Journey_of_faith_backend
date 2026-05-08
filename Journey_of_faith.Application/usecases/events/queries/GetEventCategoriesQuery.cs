using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.events.queries
{
    public class GetEventCategoriesQuery : IRequest<IEnumerable<EventCategoryView>>
    {
    }

    public class GetEventCategoriesHandler : IRequestHandler<GetEventCategoriesQuery, IEnumerable<EventCategoryView>>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventCategoriesHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IEnumerable<EventCategoryView>> Handle(GetEventCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _eventRepository.GetCategoriesAsync();
        }
    }
}
