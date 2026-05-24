using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.musics;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;


public static partial class SongCategoryDataLoader
{
    [DataLoader]
    public static async Task<Dictionary<int, SongCategory>> CategoryByIdAsync(
        IReadOnlyList<int> songIds,
        [Service] IDbConnectionFactory dbConnectionFactory
    )
    {

        var uniqueSongIds = songIds.Distinct().ToArray(); // lọc các key trùng lặp và chuyển nó thành mảng
        using var connection = dbConnectionFactory.CreateConnection();

        var sql = @"
            Select scm.SongId, sc.Id, sc.Name  from [jcodepro_journey_of_faith].[SongCategoryMapping] scm
            left join [jcodepro_journey_of_faith].[SongCategory] sc on sc.Id = scm.CategoryId
            where scm.SongId in @SongIds
        ";

        var result = await connection.QueryAsync<int, SongCategory, (int songId, SongCategory songCategory)>(sql,
            (songId, songCategory) => (songId, songCategory),
            new { SongIds = uniqueSongIds },
            splitOn: "Id"
        );
        return result.
            GroupBy(e => e.songId).
            ToDictionary(g => g.Key, g => g.First().songCategory);
    }
}