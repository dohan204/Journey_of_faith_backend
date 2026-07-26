using AutoMapper;
using AutoMapper.QueryableExtensions;
using GreenDonut.Data;
using HotChocolate.Caching;
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
        [UsePaging(IncludeTotalCount = true)]
        [CacheControl(300, Scope = CacheControlScope.Public, SharedMaxAge = 900)]
        [UseFiltering]
        [UseSorting]
        public static IQueryable<Church>  GetChurches(
            [Service] ApplicationDbContext context,
            [Service] IMapper mapper
        )
        {
            return context.Churches
                .OrderBy(x => x.Id)
                .ProjectTo<Church>(mapper.ConfigurationProvider);
        }
    }
}
