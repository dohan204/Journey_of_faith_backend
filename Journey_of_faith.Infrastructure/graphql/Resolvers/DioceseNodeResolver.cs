using AutoMapper;
using AutoMapper.QueryableExtensions;
using GreenDonut.Data;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Infrastructure.context;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.Resolvers
{
    [ExtendObjectType(typeof(Query))]
    public static partial class DioceseNodeResolver
    {
        public static async Task<Page<Diocese>> dioceses(
            PagingArguments pagingArguments,
            [Service] ApplicationDbContext context,
            [Service] IMapper mapper,
            CancellationToken cancellationToken
        )
        {
            return await context.Dioceses
                .OrderBy(e => e.Id)
                .ProjectTo<Diocese>(mapper.ConfigurationProvider)
                .ToPageAsync(pagingArguments, cancellationToken);
        }
    }
}
