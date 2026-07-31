using Journey_of_faith.Application.common.dtos;
using Journey_of_faith.Application.common.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.dashboard.queries;


public class GetDashboardQuery : IRequest<DashboardInfoDto>
{
    
}


public class GetDashboardHandler : IRequestHandler<GetDashboardQuery, DashboardInfoDto>
{
    private readonly IDashboardRepository dashboardRepository;
    public GetDashboardHandler(IDashboardRepository dashboardRepository)
    {
        this.dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardInfoDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        return await dashboardRepository.GetDashboardInfoAsync();
    }
}