using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.graphql.DataLoaders.quizes
{
    public static partial class QuestionDataloader
    {
        [DataLoader]
        public static async Task<Dictionary<int, Question[]>> QuestionByQuizDataLoader(
            IReadOnlyList<int> quizIds, 
            IDbConnectionFactory dbConnectionFactory,
            CancellationToken cancellationToken)
        {
            using var connection = dbConnectionFactory.CreateConnection();
            var sql = @"SELECT qq.QuizId, q.* FROM [jcodepro_journey_of_faith].[QuizQuestion] qq
                        INNER JOIN [jcodepro_journey_of_faith].[Question] q
                        ON qq.QuestionId = q.Id WHERE qq.QuizId in @QuizIds";

            var result = await connection.QueryAsync<int, Question, (int QuizId, Question question)>(
                sql,
                (quizId, question) => (quizId, question),
                new { QuizIds = quizIds.Distinct().ToArray() },
                splitOn: "Id"
            );


            return result
                    .GroupBy(q => q.QuizId)
                    .ToDictionary(q => q.Key, q => q.Select(e => e.question).ToArray());
        }
    }
}
