using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Infrastructure.graphql.DataLoaders;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;

namespace Journey_of_faith.Infrastructure.graphql.types.songs;

[ExtendObjectType(typeof(Song))]
public static partial class AlbumQueryExtension
{
    public static async Task<Album> GetAlbumAsync(
        [Parent] Song song,
        IAlbumDataLoader albumDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await albumDataLoader.LoadAsync(song.Id, cancellationToken);
    }
}