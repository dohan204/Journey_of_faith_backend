using Journey_of_faith.Application.common.dtos.church;
using Journey_of_faith.Application.usecases.churchs;
using Journey_of_faith.Domain.dtos;
using MediatR;

public class GetChurchWithCondition : IRequest<PagedResult<ChurchViewDto>>
{
    public string? NameChurch { get; set; }
    public string? Province { get; set; }
    public string? Ward { get; set; }
    public string? Time { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
public class GetChurchWithConditionHandler : IRequestHandler<GetChurchWithCondition, PagedResult<ChurchViewDto>>
{
    private readonly IChurchQueries churchQueries;

    public GetChurchWithConditionHandler(IChurchQueries churchQueries)
    {
        this.churchQueries = churchQueries;
    }

    public async Task<PagedResult<ChurchViewDto>> Handle(
        GetChurchWithCondition query,
        CancellationToken cancellationToken)
    {
        return await churchQueries.GetListAsync(new QueryFilter
        {
            ChurchName = query.NameChurch,
            Province = query.Province,
            Ward = query.Ward,
            Time = query.Time,
            Page = query.Page,
            PageSize = query.PageSize,
        }
            );

    }
}
