using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.usecases.songs;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.repositories;
using Microsoft.Extensions.Options;

namespace Journey_of_faith.Infrastructure.persistence.queries;

public sealed class SongQueries : BaseRepository, ISongQueries
{
    public SongQueries(IDbConnectionFactory connectionFactory, IOptions<TableSchemaName> options)
        : base(connectionFactory, options) { }

    public Task<bool> SongCategoryExistsAsync(string name, CancellationToken cancellationToken = default)
        => ExistsAsync(SongRelationShip.SongCategory, "Name = @Name", new { Name = name }, cancellationToken);

    public Task<bool> ArtistExistsAsync(int id, CancellationToken cancellationToken = default)
        => ExistsAsync(SongRelationShip.Artist, "Id = @Id", new { Id = id }, cancellationToken);

    public Task<bool> ArtistNameExistsAsync(string name, CancellationToken cancellationToken = default)
        => ExistsAsync(SongRelationShip.Artist, "Name = @Name", new { Name = name }, cancellationToken);

    public Task<bool> AlbumExistsAsync(int id, CancellationToken cancellationToken = default)
        => ExistsAsync(SongRelationShip.Album, "Id = @Id", new { Id = id }, cancellationToken);

    private Task<bool> ExistsAsync(string table, string predicate, object parameters, CancellationToken cancellationToken)
        => QueryAsync(async connection =>
            await connection.ExecuteScalarAsync<int>(new CommandDefinition(
                $"SELECT CASE WHEN EXISTS (SELECT 1 FROM [{_schemaName.Schema}].[{table}] WHERE {predicate}) THEN 1 ELSE 0 END",
                parameters,
                cancellationToken: cancellationToken)) > 0);
}
