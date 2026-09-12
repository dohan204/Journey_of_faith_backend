using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries;


public class GetLiturgyTodayQuery : IRequest<Liturgy?>
{
    
}

public class GetLiturgyTodayHandler : IRequestHandler<GetLiturgyTodayQuery, Liturgy?>
{
    private readonly IChurchRepository churchRepository;
    public GetLiturgyTodayHandler(IChurchRepository repository)
    {
        this.churchRepository = repository;
    }

    public async Task<Liturgy?> Handle(GetLiturgyTodayQuery query, CancellationToken cancellationToken)
    {
        return await churchRepository.GetLiturgyTodayAsync();
    }
}