using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;

public class GetChurchWithCondition : IRequest<PagedResult<Church>>
{
    public string? NameChurch {get; set;}
    public string? Province {get; set;}
    public string? Ward {get; set;}
    public string? Time {get; set;}
    public int Page {get; set;}
    public int PageSize {get; set;}
}

public class GetChurchWithConditionHandler : IRequestHandler<GetChurchWithCondition, PagedResult<Church>>
{
    private readonly IChurchRepository churchRepository;
    public GetChurchWithConditionHandler(IChurchRepository churchRepository)
    {
        this.churchRepository = churchRepository;
    }

    public async Task<PagedResult<Church>> Handle(GetChurchWithCondition query, CancellationToken cancellationToken)
    {
        return await churchRepository.GetChurchWithCondition(query.NameChurch, query.Province, query.Ward, query.Time, query.Page, query.PageSize);
    }
}