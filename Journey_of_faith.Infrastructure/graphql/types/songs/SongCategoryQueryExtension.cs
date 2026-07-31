// using Journey_of_faith.Domain.entities.musics;
// using Journey_of_faith.Infrastructure.graphql.DataLoaders;
// using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;

// namespace Journey_of_faith.Infrastructure.graphql.types;

// [ExtendObjectType(typeof(Song))]
// public static partial class SongCategoryExtension
// {
//     public static async Task<SongCategory>GetSongCategoryAsync(
//         [Parent] Song song, 
//         ICategoryByIdDataLoader categoryByIdDataLoader,
//         CancellationToken cancellationToken)
//     {
//         return await categoryByIdDataLoader.LoadAsync(song.Id, cancellationToken);
//     }
// }