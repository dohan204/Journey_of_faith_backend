using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Infrastructure.graphql.DataLoaders;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;

namespace Journey_of_faith.Infrastructure.graphql.types;

public static partial class SongCategoryExtension
{
    public static async Task<Song[]> GetSongsByCategoryAsync(
        [Parent] SongCategory songCategory,
        ISongByCategoryDataLoader songByCategoryData,
        CancellationToken cancellationToken
    )
    {
        return await songByCategoryData.LoadAsync(songCategory.Id, cancellationToken);
    }
}