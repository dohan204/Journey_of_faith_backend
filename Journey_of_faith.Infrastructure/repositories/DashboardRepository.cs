using Dapper;
using Journey_of_faith.Application.common.dtos;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.repositories;

public class DashboardRepository : BaseRepository, IDashboardRepository
{
    public DashboardRepository(IDbConnectionFactory dbConnectionFactory, IOptions<TableSchemaName> options): base(dbConnectionFactory, options) {}


    public async Task<DashboardInfoDto> GetDashboardInfoAsync()
    {
        return await QueryAsync<DashboardInfoDto>(async connection =>
        {
            var queryMultiple = await connection.QueryMultipleAsync("spGetDashboardInfo", commandType: System.Data.CommandType.StoredProcedure);


            var totalUser = await queryMultiple.ReadFirstOrDefaultAsync<int>();
            var totalChurch = await queryMultiple.ReadFirstOrDefaultAsync<int>();
            var totalQuestion = await queryMultiple.ReadFirstOrDefaultAsync<int>();
            var totalEvent = await queryMultiple.ReadFirstOrDefaultAsync<int>();
            var totalRole = await queryMultiple.ReadFirstOrDefaultAsync<int>();

            return new DashboardInfoDto
            {
                UserCount = totalUser,
                ChurchCount = totalChurch,
                QuestionCount = totalQuestion,
                EventCount = totalEvent,
                RoleCount = totalRole,
                AccessCount = 1000
            };
        });
    }
}