using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.events.queries;

public class GetCommentForEventQuery : IRequest<List<EventCommentView>>
{
    public int EventId {get; set;}
}

public class GetCommentForEventHandler : IRequestHandler<GetCommentForEventQuery, List<EventCommentView>>
{
    private readonly IEventRepository eventRepository;
    public GetCommentForEventHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task<List<EventCommentView>> Handle(GetCommentForEventQuery query, CancellationToken cancellationToken = default)
    {
        return await eventRepository.GetCommmentForEventAsync(query.EventId);
    }
}