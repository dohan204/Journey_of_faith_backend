using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes
{
    public static partial class AnswerDataLoader
    {
        [DataLoader]
        public static async Task<Dictionary<int, Answer[]>> AnswerByQuestionDataLoader(
            IReadOnlyList<int> questionIds,
            IDbConnectionFactory dbConnectionFactory, 
            CancellationToken cancellation)
        {
            using var connection = dbConnectionFactory.CreateConnection();
            var sql = @"Select * From [jcodepro_journey_of_faith].[Answer] WHERE QuestionId IN @QuestionIds";

            var result = await connection.QueryAsync<Answer>(sql, new { QuestionIds = questionIds });

            return result.GroupBy(e => e.QuestionId)
                    .ToDictionary(e => e.Key, e => e.ToArray());
        }
    }
}
