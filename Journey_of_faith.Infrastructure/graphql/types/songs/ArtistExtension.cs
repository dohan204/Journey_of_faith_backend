// using Journey_of_faith.Domain.entities.musics;
// using Journey_of_faith.Infrastructure.graphql.DataLoaders;
// using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;

// namespace Journey_of_faith.Infrastructure.graphql.types.songs;


// [ExtendObjectType(typeof(Artist))]
// public static partial class ArtistExtension
// {
//     public static async Task<Song[]> GetSongsByArtistAsync(
//         [Parent] Artist artist,
//         ISongByArtistDataLoader songByArtistData,
//         CancellationToken cancellationToken
//     )
//     {
//         return await songByArtistData.LoadAsync(artist.Id, cancellationToken);
//     }


    
// }
