using Journey_of_faith.Application.common.dtos;

namespace Journey_of_faith.Application.common.interfaces;


public interface IDashboardRepository
{
    Task<DashboardInfoDto> GetDashboardInfoAsync();
}