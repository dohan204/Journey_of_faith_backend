using System;
using System.Collections.Generic;
using System.Text;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Infrastructure.graphql.DataLoaders;
using Journey_of_faith.Infrastructure.graphql.DataLoaders.songs;
namespace Journey_of_faith.Infrastructure.graphql.types
{
    [ExtendObjectType(typeof(User))]
    public static partial class UserSongAysnc
    {
        public static async Task<Song[]> GetSongsAsync(
            [Parent] User user,
            ISongsByUserIdDataLoader songsByUserIdDataLoader,
            CancellationToken cancellationToken
        )
        {
            return await songsByUserIdDataLoader.LoadAsync(user.Id, cancellationToken);
        }
    }
}
