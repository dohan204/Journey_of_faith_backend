using Journey_of_faith.Domain.interfaces;

namespace Journey_of_faith.Api.types;


public class MappingOneToOneBatchDataLoader<TData, TEntity> : BatchDataLoader<TData, TEntity>
where TData : notnull
where TEntity : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _sql;
    private readonly Func<TEntity, TData> _keySelector;
    public MappingOneToOneBatchDataLoader(
        IServiceProvider serviceProvider,
        string sql,
        Func<TEntity, TData> selector,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options) : base(batchScheduler, options
    )
    {
        _serviceProvider = serviceProvider;
        _sql = sql;
        _keySelector = selector;
    }



    protected override async Task<IReadOnlyDictionary<TData, TEntity>> LoadBatchAsync(
        IReadOnlyList<TData> keys,
        CancellationToken cancellationToken
    )
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var repo = scope.ServiceProvider.GetRequiredService<IGetOneToOneData<TData, TEntity>>();

        var item = await repo.GetOneToOneDataAsync(_sql, keys, _keySelector, cancellationToken);
        return item;
    }
}



public class MappingOneToManyBatchDataLoader<TData, TEntity> : BatchDataLoader<TData, TEntity[]> 
where TData: notnull
where TEntity : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _sql;
    private readonly Func<TEntity, TData> _keySelector;
    public MappingOneToManyBatchDataLoader(
        IServiceProvider serviceProvider,
        string sql,
        Func<TEntity, TData> selector,
        IBatchScheduler batchScheduler,
        DataLoaderOptions options) : base(batchScheduler, options
    )
    {
        _serviceProvider = serviceProvider;
        _sql = sql;
        _keySelector = selector;
    }

    protected override async Task<IReadOnlyDictionary<TData, TEntity[]>> LoadBatchAsync(
        IReadOnlyList<TData> keys,
        CancellationToken cancellationToken
    )
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var repo = scope.ServiceProvider.GetRequiredService<IGetOneToManyData<TData, TEntity>>();

        var item = await repo.GetDataByIdsAsync(_sql, keys, _keySelector, cancellationToken);
        return item;
    }
}