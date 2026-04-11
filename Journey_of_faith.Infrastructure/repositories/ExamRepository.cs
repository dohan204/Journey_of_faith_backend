using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.repositories
{
    public class ExamRepository(IDbConnectionFactory _factory, IOptions<TableSchemaName> _name) : IExamRepository
    {
        private readonly TableSchemaName name = _name.Value;
        public async Task<int> CreateQuiz(Quiz quiz)
        {
            using var connection = _factory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                var ins = await connection.ExecuteScalarAsync<int>(
                $@"INSERT INTO [{name.Schema}].[{QuizTalbe.Quiz}] (Title, Description, TimeLimit)
                Output inserted.Id
                VALUES(@Title, @Description, @TimeLimit)", new
                {
                    Title = quiz.Title,
                    Description = quiz.Description,
                    TimeLimit = quiz.TimeLimit,
                }, transaction);

                await connection.ExecuteAsync($@"
                INSERT INTO [{name.Schema}].[{QuizTalbe.QuizQuestion}] (QuizId, QuestionId, OrderIndex)
                Select @QuizId, Id, ROW_NUMBER() OVER (Order by NewId())
                From (
                    Select Top (@Count) Id
                    from [{name.Schema}].[Question]
                    where IsActive = 1
                    Order by NewId()
                ) as RandomQuestion
            ", new
                {
                    QuizId = ins,
                    Count = quiz.QuestionCount
                }, transaction);
                transaction.Commit();
                return ins;
            } catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
    public static class QuizTalbe
    {
        public const string Quiz = "Quiz";
        public const string QuizQuestion = "QuizQuestion";
    }
}


