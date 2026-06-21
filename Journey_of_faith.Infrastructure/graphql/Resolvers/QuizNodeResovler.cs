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
    public static partial class QuizNodeResovler
    {
        [UsePaging]
        [UseFiltering]
        public static IQueryable<Quiz> GetQuizs(
            [Service] ApplicationDbContext context,
            [Service] IMapper mapper)
        {
            return context.Quizzes
                .OrderBy(q => q.Id)
                .ProjectTo<Quiz>(mapper.ConfigurationProvider);
        }
    }
}
