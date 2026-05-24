using AutoMapper;
using AutoMapper.QueryableExtensions;
using GreenDonut.Data;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.musics;
using Journey_of_faith.Infrastructure.context;

namespace Journey_of_faith.Infrastructure.graphql.Resolvers;

[ExtendObjectType(typeof(Query))]
public static partial class ArtistNodeResolver
{
    [UsePaging]
    public static async Task<Page<Artist>> GetArtistsAsync(
        PagingArguments pagingArguments,
        [Service] ApplicationDbContext context,
        [Service] IMapper mapper,
        CancellationToken cancellationToken
    )
    {
        return await context.Artists
            .OrderBy(e => e.Id)
            .ProjectTo<Artist>(mapper.ConfigurationProvider)
            .ToPageAsync(pagingArguments, cancellationToken);
    }
}