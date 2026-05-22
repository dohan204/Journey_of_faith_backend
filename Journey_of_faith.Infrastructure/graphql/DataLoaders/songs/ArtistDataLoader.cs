using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.musics;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;

public static partial class ArtistByDataLoader
{
    [DataLoader]
    public static async Task<Dictionary<int, Artist>> GetArtistAsync(
        IReadOnlyList<int> artistIds,
        [Service] IDbConnectionFactory connectionFactory,
        CancellationToken cancellationToken
    )
    {
        using var connection = connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<Artist>(
            "SELECT * FROM [jcodepro_journey_of_faith].[Artist] WHERE Id IN @Ids",
            new { Ids = artistIds.Distinct().ToArray() }
        );
        return result.ToDictionary(a => a.Id);
    }
}