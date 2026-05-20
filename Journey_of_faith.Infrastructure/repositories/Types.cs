using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Journey_of_faith.Infrastructure.repositories;


public class GetDataRepository<TData, TEntity> : BaseRepository,  IGetOneToManyData<TData, TEntity>, IGetOneToOneData<TData, TEntity>
where TData : notnull
where TEntity : class
{
    public GetDataRepository(
        IDbConnectionFactory dbConnection, IOptions<TableSchemaName> options) : base(dbConnection, options
    )
    {
      
    }

    public async Task<Dictionary<TData, TEntity>> GetOneToOneDataAsync(string sql, IReadOnlyList<TData> ids, Func<TEntity, TData> keySelector, CancellationToken cancellationToken = default)
    {
        if(ids == null || !ids.Any())
        {
            return new Dictionary<TData, TEntity>();
        }


        using var connection = _dbConnection.CreateConnection();
        var command = new CommandDefinition(sql, new { Ids = ids}, cancellationToken: cancellationToken);
        var data = await connection.QueryAsync<TEntity>(command);
        return data.ToDictionary(keySelector);
    }

    public async Task<Dictionary<TData, TEntity[]>> GetDataByIdsAsync(string sql,IReadOnlyList<TData> ids, Func<TEntity, TData> keySelector, CancellationToken cancellationToken = default)
    {
        if(ids.Count == 0 || !ids.Any())
        {
            return new Dictionary<TData, TEntity[]>();
        }
        using var connection = _dbConnection.CreateConnection();
        var command = new CommandDefinition(sql, new {Ids = ids}, cancellationToken: cancellationToken);
        var data = await connection.QueryAsync<TEntity>(command);

        var lookup = data.ToLookup(keySelector: keySelector);

        return ids.Distinct()
            .ToDictionary(
                id => id,
                id => lookup[id].ToArray()
            );
    }
}
