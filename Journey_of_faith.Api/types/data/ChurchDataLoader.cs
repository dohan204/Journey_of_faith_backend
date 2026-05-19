using Journey_of_faith.Api.types;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.repositories;

namespace Journey_of_faith.Api.types.data;

public class ChurchDataLoader
{
    private const string scheme = "jcodepro_journey_of_faith";
    [DataLoader]
    public async Task<Dictionary<int, Diocese>> GetDioceseByChurchId(
        IReadOnlyList<int> ids,
        GetDataRepository<int, Diocese> getData,
        CancellationToken cancellation)
    {
        var sql = $"Select * from [{scheme}].[Diocese] where Id = @Ids";
        return await getData.GetOneToOneDataAsync(sql, ids,
            (Diocese c) => c.Id, cancellation);
    }
}


public class GetDioceseByIdDataLoader : MappingOneToOneBatchDataLoader<int, Diocese>
{
    private static readonly string scheme = "jcodepro_journey_of_faith";
    public GetDioceseByIdDataLoader(
        IServiceProvider serviceProvider,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options
    ) : base(
        serviceProvider,
        sql: $"Select * from [{scheme}].[Diocese] where Id = @Ids",
        selector: (Diocese d) => d.Id,
        batchScheduler,
        options
    ){}
}