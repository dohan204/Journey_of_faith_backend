using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes
{
    public static partial class QuizDataLoader
    {
        [DataLoader]
        public static async Task<Dictionary<int, Quiz[]>> GetQuizByTopicDataLoader(
            IReadOnlyList<int> topicIds,
            IDbConnectionFactory dbConnectionFactory,
            CancellationToken cancellationToken)
        {
            using var connection = dbConnectionFactory.CreateConnection();
            var sql = @"Select * from [jcodepro_journey_of_faith].[Quiz] where TopicId in @TopicIds";

            var result = await connection.QueryAsync<Quiz>(sql, new { TopicIds = topicIds.Distinct().ToArray() });
            return result.GroupBy(e => e.TopicId)
                .ToDictionary(e => e.Key, e => e.ToArray());
        }
    }
}
