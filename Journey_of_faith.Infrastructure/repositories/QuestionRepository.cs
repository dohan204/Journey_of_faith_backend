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
    public record InsertAnswerDto(int questionId,string Content, bool isCorrect, string? ImageUrl, string? Explanation);
    public sealed class QuestionRepository(
        IOptions<TableSchemaName> _options,
        IDbConnectionFactory factory
    ) : IQuestionRepository
    {
        private readonly IDbConnectionFactory _connection = factory;
        private readonly TableSchemaName _name = _options.Value;
        public async Task<bool> NameExistsAsync(string name, string table)
        {
            // tạo kết nối
            using var connection = _connection.CreateConnection();

            // CCheck null or empty
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
        public async Task<QuizLevel?> GetDetailQuizLevel(int Id)
        {
            var categories = new GetRequestData<QuizLevel>(factory: factory, _options);
            return await categories.GetEntityDetails<QuizLevel>(Id, "GetDetailsQuestionLevel");
        }
        public async Task<int> GetCountQuestionByLevel(string name)
        {
            var connection = _connection.CreateConnection();
            return await connection.ExecuteScalarAsync<int>
                ("GetCountQuestionLevel", new { Name = name }, commandType: System.Data.CommandType.StoredProcedure);
        }
        public async Task<bool> CreateQuestionType(QuestionType questionType)
        {
            return await InsertOnlyName(TableQuestion.QuestionType, new { Name = questionType.Name });
        }
        public async Task<QuestionType?> GetDetailsQuestionType(int Id)
        {
            var types = new GetRequestData<QuestionType>(factory: factory, _options);
            return await types.GetEntityDetails<QuestionType>(Id, "GetDetailsQuestionType");
        }
        public async Task<bool> CreateQuestionCategory(QuestionCategory questionCategory)
        {
            return await InsertOnlyName(TableQuestion.QuestionCategory, new { Name = questionCategory.Name });
        }
        public async Task<QuestionCategory?> GetDetailsQuestionCategory(int Id)
        {
            var categories = new GetRequestData<QuestionCategory>(factory: factory, _options);
            return await categories.GetEntityDetails<QuestionCategory>(Id, "GetDetailsQuestionCategory");
        }


        public async Task<bool> CreateQuestionAsync(Question question) {
            string command = 
                $"INSERT INTO [{_name.Schema}].[{TableQuestion.Question}] (LevelId, QuestionContent, TypeId, CategoryId, ImageUrl) " +
                $"VALUES(@LevelId, @QuestionContent, @TypeId, @CategoryId, @ImageUrl) ";
            using var connection = _connection.CreateConnection();
            connection.Open();
            var result = await connection.ExecuteAsync(command,
                new InsertQuestionDto(question.LevelId, question.QuestionContent, question.TypeId, question.CategoryId, question.ImageUrl));

            var questionId = await connection
                .ExecuteScalarAsync<int>($"select Id from [{_name.Schema}].[{TableQuestion.Question}] where QuestionContent = @QuestionContent"
                , new { QuestionContent = question.QuestionContent });

            foreach(var answer in question.Answers)
            {
                await connection
                    .ExecuteAsync($@"INSERT INTO [{_name.Schema}].[{TableQuestion.Answer}] 
                            (QuestionId, Content, IsCorrect, ImageUrl, Explanation)
                            VALUES(@QuestionId, @Content, @IsCorrect, @ImageUrl, @Explanation)",
                            new InsertAnswerDto(questionId,Content: answer.Content,
                            isCorrect: answer.IsCorrect, ImageUrl: answer.ImageUrl, Explanation: answer.Explanation));
            }
            return result > 0;
        }
        public async Task<bool> CheckUniqueName(string name)
        {
            using var connection = _connection.CreateConnection();

            string sql = $@"
            IF EXISTS (
                SELECT 1 
                FROM [{_name.Schema}].[{TableQuestion.Question}] 
                WHERE QuestionContent = @QuestionContent
            ) 
            SELECT 1 ELSE SELECT 0";

            return await connection.ExecuteScalarAsync<int>(sql, new { QuestionContent = name }) > 0;
        }
        public async Task<int> GetCountQuestion()
        {
            var connection = _connection.CreateConnection();
            return await connection.ExecuteScalarAsync<int>($"Select Count(*) from [{_name.Schema}].[{TableQuestion.Question}]");
        }

        public async Task<IEnumerable<QuizLevel>> GetLevelsAsync()
        {
            using var connection = _connection.CreateConnection();
            var levels = 
                await connection.QueryAsync<QuizLevel>($"SELECT * FROM [{_name.Schema}].[{TableQuestion.QuizLevel}] ");
            return levels;
        }

        public async Task<IEnumerable<QuestionType>> GetAllTypeQuestion()
        {
            var types = new GetRequestData<QuestionType>(factory: factory, _options);
            return await types.GetAll<QuestionType>(TableQuestion.QuestionType);
        }
        
        public async Task<IEnumerable<QuestionCategory>> GetAllCategoryQuestion()
        {
            var categories = new GetRequestData<QuestionCategory>(factory: factory, _options);
            return await categories.GetAll<QuestionCategory>(table: TableQuestion.QuestionCategory);
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

    public class GetRequestData<T>
    {
        private readonly IDbConnectionFactory _factory;
        private readonly TableSchemaName _schemaName;
        public GetRequestData(IDbConnectionFactory factory, IOptions<TableSchemaName> _options)
        {
            _factory = factory;
            _schemaName = _options.Value;
        }

        public async Task<IEnumerable<T>> GetAll<T>(string table)
        {
            var connection = _factory.CreateConnection();
            return await connection.QueryAsync<T>($"Select * from [{_schemaName.Schema}].[{table}]");
        }

        public async Task<T?> GetEntityDetails<T>(int Id, string StoredProcedure)
        {
            var connection = _factory.CreateConnection();
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", Id);

            var data = await connection.QuerySingleOrDefaultAsync<T>(
                    StoredProcedure, param, commandType: System.Data.CommandType.StoredProcedure
                );
            return data;
        }
    } 
}
