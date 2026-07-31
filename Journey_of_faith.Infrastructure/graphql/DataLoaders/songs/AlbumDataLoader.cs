// using Dapper;
// using Journey_of_faith.Application.common.interfaces;
// using Journey_of_faith.Domain.entities.musics;

// namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;


// public static partial class AlbumByDataLoader
// {
//     [DataLoader]
//     public static async Task<Dictionary<int, Album>> GetAlbumAsync(
//         IReadOnlyList<int> songIds,
//         [Service] IDbConnectionFactory factory,
//         CancellationToken cancellationToken
//     )
//     {
//         var uniqueId = songIds.Distinct().ToArray();

//         using var connection = factory.CreateConnection();
//         var sql = @"
//             SELECT Id,Title FROM [jcodepro_journey_of_faith].[Album] where Id in @Ids
//         ";

//         var result = await connection.QueryAsync<Album>(
//     "SELECT Id, Title FROM [jcodepro_journey_of_faith].[Album] WHERE Id IN @Ids",
//     new { Ids = uniqueId }
// );
//         return result.ToDictionary(a => a.Id);
//     }
// }