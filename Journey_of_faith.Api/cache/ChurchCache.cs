using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;

namespace Journey_of_faith.Api.cache;


public class ChurchCacheDataLoader : CacheDataLoader<int, Church>
{
    private readonly IServiceProvider _serviceProvider;
    public ChurchCacheDataLoader(IServiceProvider serviceProvider, DataLoaderOptions opitons) : base(opitons)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task<Church> LoadSingleAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var repo = _serviceProvider.GetRequiredService<IChurchRepository>();
        return await repo.GetChurchByIdAsync(id, cancellationToken);
    }
}
