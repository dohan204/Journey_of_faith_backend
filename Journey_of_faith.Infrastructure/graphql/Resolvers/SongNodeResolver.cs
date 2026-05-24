using AutoMapper;
using AutoMapper.QueryableExtensions;
using GreenDonut.Data;
using HotChocolate.Caching;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Infrastructure.context;

namespace Journey_of_faith.Infrastructure.graphql.Resolvers
{
    [ExtendObjectType(typeof(Query))]
    public static partial class SongNodeResolver
    {
        [UsePaging(IncludeTotalCount = true)]
        [CacheControl(300, Scope = CacheControlScope.Public, SharedMaxAge = 900)]
        [UseFiltering]
        [UseSorting]
        public static async Task<Page<Song>> GetSongsAsync(
            PagingArguments pagingArguments,
            [Service] ApplicationDbContext context,
            [Service] IMapper mapper,
            CancellationToken cancellation
            )
        {
            return await context.Songs
                .OrderByDescending(t => t.Title)
                .ThenBy(t => t.Id)
                .ProjectTo<Song>(mapper.ConfigurationProvider)
                .ToPageAsync(pagingArguments, cancellation);
        }
    }
}
