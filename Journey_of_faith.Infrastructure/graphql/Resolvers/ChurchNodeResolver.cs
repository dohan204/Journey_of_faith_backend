using AutoMapper;
using AutoMapper.QueryableExtensions;
using GreenDonut.Data;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.Resolvers
{
    [ExtendObjectType(typeof(Query))]
    public static partial class ChurchNodeResolver
    {
        public static async Task<Page<Church>> GetChurchesAsync(
            PagingArguments pagingArguments,
            [Service] ApplicationDbContext context,
            [Service] IMapper mapper,
            CancellationToken cancellationToken
        )
        {
            return await context.Churches
                .OrderBy(x => x.Id)
                .ProjectTo<Church>(mapper.ConfigurationProvider)
                .ToPageAsync(arguments: pagingArguments, cancellationToken: cancellationToken);
        }
    }
}
