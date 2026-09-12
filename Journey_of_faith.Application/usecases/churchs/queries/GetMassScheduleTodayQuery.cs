using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries;


public class GetMassScheduleTodayQuery : IRequest<IReadOnlyList<MassScheduleTodayView>> {}


public class GetMassScheduleTodayHandler : IRequestHandler<GetMassScheduleTodayQuery, IReadOnlyList<MassScheduleTodayView>>
{
    private readonly IChurchRepository churchRepository;
    public GetMassScheduleTodayHandler(IChurchRepository churchRepository)
    {
        this.churchRepository = churchRepository;
    }

    public async Task<IReadOnlyList<MassScheduleTodayView>> Handle(GetMassScheduleTodayQuery request, CancellationToken cancellationToken)
    {
        return await churchRepository.GetMassScheduleTodayViewsAsync();
    }
}