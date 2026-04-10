using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.repositories
{
    public record InsertQuestionDto(int LevelId, string QuestionContent, int? TypeId, int? CategoryId, string? ImageUrl);
    public record InsertAnswerDto(int questionId, string Content, bool isCorrect, string? ImageUrl, string? Expla);
    public sealed class QuestionRepository(
        IOptions<TableSchemaName> _options,
        IDbConnectionFactory factory
    ) : IQuestionRepository
    {
        private readonly IDbConnectionFactory _connection = factory;
        private readonly TableSchemaName _name = _options.Value;
        public async Task<bool> NameExistsAsync(string name, string table)
        {
            using var connection = _connection.CreateConnection();

            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            var sql = $@"IF EXISTS (SELECT 1 FROM [{_name.Schema}].[{table}] WHERE Name = @Name)
                                        SELECT 1 ELSE SELECT 0";

            var result = await connection.ExecuteScalarAsync<int>(sql, new { Name = name });
            return result == 1;
        }
        public async Task<bool> CreateQuizLevel(QuizLevel quizLevel)
        {
            return await InsertOnlyName(TableQuestion.QuizLevel, new { Name = quizLevel.Name });
        }

        public async Task<bool> CreateQuestionType(QuestionType questionType)
        {
            return await InsertOnlyName(TableQuestion.QuestionType, new { Name = questionType.Name });
        }
        public async Task<bool> CreateQuestionCategory(QuestionCategory questionCategory)
        {
            return await InsertOnlyName(TableQuestion.QuestionCategory, new { Name = questionCategory.Name });
        }

        public async Task<bool> CreateQuestionAsync(Question question) {
            string command = 
                $"INSERT INTO [{_name.Schema}].[{TableQuestion.Question}] (LevelId, QuestionContent, TypeId, CategoryId, ImageUrl) " +
                $"VALUES(@LevelId, @QuestionContent, @TypeId, @CategoryId, @ImageUrl) ";
            using var connection = _connection.CreateConnection();
            var result = await connection.ExecuteAsync(command,
                new InsertQuestionDto(question.LevelId, question.QuestionContent, question.TypeId, question.CategoryId, question.ImageUrl));
            foreach(var answer in question.Answers)
            {
                await connection
                    .ExecuteAsync($@"INSERT INTO [{_name.Schema}].[{TableQuestion.Answer}] 
                            (QuestionId, Content, IsCorrect, ImageUrl, Explanation)
                            VALUES(@QuestionId, @Content, @IsCorrect, @ImageUrl, @Explantion)",
                            new InsertAnswerDto(questionId: answer.QuestionId, Content: answer.Content,
                            isCorrect: answer.IsCorrect, ImageUrl: answer.ImageUrl, Expla: answer.Explanation));
            }
            return result > 0;
        }

        public async Task<bool> CreateAnswerAsync(Answer answer)
        {
            string command = $@"INSERT INTO [{_name.Schema}].[{TableQuestion.Answer}] (QuestionId, Content, IsCorrect, ImageUrl, Explanation)
                            VALUES(@QuestionId, @Content, @IsCorrect, @ImageUrl, @Explantion)";
            using var connection = _connection.CreateConnection();
            var result = await connection.ExecuteAsync(command,
                new InsertAnswerDto(questionId: answer.QuestionId, Content: answer.Content,
                isCorrect: answer.IsCorrect, ImageUrl: answer.ImageUrl, Expla: answer.Explanation));
            return result > 1;
        }
        public async Task<bool> CheckValidId(int id, string table)
        {
            var command = $@"IF EXISTS (SELECT 1 FROM [{_name.Schema}].[{table}] where Id = @Id)
                                SELECT 1 ELSE SELECT 0";
            using var connection = _connection.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(command, new { Id = id });
            return result == 1;
        }
        private async Task<bool> InsertOnlyName(string table, object param)
        {
            string command = $"INSERT INTO [{_name.Schema}].[{table}] (Name) VALUES(@Name)";

            using var connection = _connection.CreateConnection();
            var result = await connection.ExecuteAsync(command, param);

            return result > 0;
        }
    }

    public static class TableQuestion
    {
        public const string Answer = "Answer";
        public const string Question = "Question";
        public const string QuizLevel = "QuizLevel";
        public const string QuestionType = "QuestionType";
        public const string QuestionCategory = "QuestionCategory";
    }
}
