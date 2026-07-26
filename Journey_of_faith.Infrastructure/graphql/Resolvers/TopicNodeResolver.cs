using AutoMapper;
using AutoMapper.QueryableExtensions;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Infrastructure.context;
using System;
using System.Collections.Generic;   
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.Resolvers
{
    [ExtendObjectType(typeof(Query))]
    public static partial class TopicNodeResolver
    {
        [UsePaging]
        [UseFiltering]
        public static IQueryable<Topic> Topics(ApplicationDbContext context, IMapper mapper)
        {
            return context.Topics.ProjectTo<Topic>(mapper.ConfigurationProvider);
        }


    }
}
