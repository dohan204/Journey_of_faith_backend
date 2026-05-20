using System.Text;
using Dapper;
using HotChocolate.Types.Pagination;
using Journey_of_faith.Api.cache;
using Journey_of_faith.Api.types.data;
using Journey_of_faith.Api.types.resolvers;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.interfaces;

namespace Journey_of_faith.Api.types.extensions;

[ExtendObjectType(typeof(Query))]
public class ChurchQueryResolver
{
    public async Task<Church?> GetChurchByIdAsync(
        int id, 
        ChurchCacheDataLoader dataLoader,
        CancellationToken token)
    {
      return await dataLoader.LoadAsync(id, cancellationToken: token);  
    }
    [UsePaging]
    public async Task<IEnumerable<Church>> GetChurchesAsync(
        string sortBy,
        [Service] IChurchRepository churchRepository,
        CancellationToken token
    )
    {
        return await churchRepository.GetAllAsync(sortBy: sortBy, cancellationToken: token);        
    }

    // public async Task<Connection<Church>> GetChurchesAsync(
    //     string sortBy,
    //     string? after,
    //     int? first,
    //     [Service] IDbConnectionFactory db,
    //     CancellationToken token)
    // {
    //     var pageSize = first ?? 10;
    //     var connection = db.CreateConnection();

    //     int offset = 0;
    //     if (!string.IsNullOrEmpty(after))
    //     {
    //         var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(after));
    //         offset = int.Parse(decoded.Split(':')[1]);
    //     }

    //     var validSortColumns = new HashSet<string> { "name", "address" };
    //     var orderBy = validSortColumns.Contains(sortBy) ? sortBy : "name";

    //     var sql = $@"
    //         SELECT * FROM Church
    //         ORDER BY {orderBy}
    //         OFFSET @Offset ROWS
    //         FETCH NEXT @First ROWS ONLY";

    //     var churches = (await connection.QueryAsync<Church>(sql, new
    //     {
    //         Offset = offset,
    //         First = pageSize + 1
    //     })).ToList();

    //     var hasNextPage = churches.Count > pageSize;
    //     if (hasNextPage) churches.RemoveAt(churches.Count - 1);

    //     var edges = churches.Select((church, index) =>
    //     {
    //         var cursorOffset = offset + index + 1;
    //         var cursor = Convert.ToBase64String(
    //             Encoding.UTF8.GetBytes($"offset:{cursorOffset}")
    //         );
    //         return new Edge<Church>(church, cursor);
    //     }).ToList();

    //     var pageInfo = new ConnectionPageInfo(
    //         hasNextPage: hasNextPage,
    //         hasPreviousPage: offset > 0,
    //         startCursor: edges.FirstOrDefault()?.Cursor,
    //         endCursor: edges.LastOrDefault()?.Cursor
    //     );

    //     return new Connection<Church>(edges, pageInfo, churches.Count);
    // }
}


[ExtendObjectType(typeof(Church))]
public partial class ChurchNode
{
    public async Task<Diocese> GetDioceseAsync(
        [Parent] Church church,
        GetDioceseByIdDataLoader dataLoader,
        CancellationToken token
    )
    {
        return await dataLoader.LoadAsync(church.DioceseId,cancellationToken: token);
    }


    public async Task<MassScheduleQueryResult[]> MassSchedulesAsync(
        [Parent] Church ch,
        MassduleByChurchIdDataLoaderAsync dataLoaderAsync,
        CancellationToken token
    )
    {
        return await dataLoaderAsync.LoadAsync(ch.Id, cancellationToken: token) ?? [];
    }
}


public class MassduleByChurchIdDataLoaderAsync : MappingOneToManyBatchDataLoader<int, MassScheduleQueryResult>
{
    public MassduleByChurchIdDataLoaderAsync(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ): base(
        serviceProvider,
        sql: @"select 
                    ms.Id,
                    ms.ChurchId,
                    ms.IsFixed,
                    ms.FromDate, 
                     ms.ToDate, 
                     ms.Date, 
                     ms.Time, 
                     mt.Name as MassTypeName
                from [jcodepro_journey_of_faith].MassSchedule ms 
                left join [jcodepro_journey_of_faith].MassType 
                mt on mt.Id = ms.MassTypeId 
                where ms.ChurchId in @Ids",
        selector: (MassScheduleQueryResult c) => c.ChurchId,
        batchScheduler,
        options
    ) {}
}

public sealed class MassScheduleQueryResult
{
    public int Id { get; set; }
    public int ChurchId { get; set; }
    public bool IsFixed { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public DateOnly? Date { get; set; }
    public TimeSpan? Time { get; set; }
    public string MassTypeName { get; set; } = string.Empty;

}