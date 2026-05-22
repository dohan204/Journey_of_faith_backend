using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Infrastructure.graphql.DataLoaders;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;

namespace Journey_of_faith.Infrastructure.graphql.types.songs;


[ExtendObjectType(typeof(Song))]
public static partial class ArtistQueryExtension
{
    public static async Task<Artist> GetArtistByIsAsync(
        [Parent] Song song,
        IArtistDataLoader artistDataLoader,
        CancellationToken cancellationToken
    )
    {
        return await artistDataLoader.LoadAsync(song.Id, cancellationToken);
    }
}