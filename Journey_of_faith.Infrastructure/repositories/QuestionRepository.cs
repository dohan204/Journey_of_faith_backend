using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.services;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Journey_of_faith.Infrastructure.repositories
{
    public record InsertQuestionDto(int LevelId, string QuestionContent, int? TypeId, int? CategoryId, string? ImageUrl);
    public record InsertAnswerDto(int questionId,string Content, bool isCorrect, string? ImageUrl, string? Explanation);
    public sealed class QuestionRepository :  DataHandlerRequest,IQuestionRepository
    {
        private readonly IDbConnectionFactory _connection;
        private readonly TableSchemaName _name;

        public QuestionRepository(IDbConnectionFactory factory, IOptions<TableSchemaName> _options) : base(factory, _options)
        {
            _connection = factory;
            _name = _options.Value;
        }
        #region another method with table relationship
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


         // tao moi tao level bai thi moi
        public async Task<bool> CreateQuizLevel(QuizLevel quizLevel)
        {
            return await InsertOnlyName(TableQuestion.QuizLevel, new { Name = quizLevel.Name, Code = quizLevel.Code, Score = quizLevel.Score });
        }
        public async Task<QuizLevel?> GetDetailQuizLevel(int Id)
        {
            var quizLevel = await GetEntityDetailsAsync<QuizLevel>(Id, "GetDetailsQuestionLevel");
            return quizLevel;
        }
        public async Task<int> GetCountQuestionByLevel(string name)
        {
            var connection = _connection.CreateConnection();
            return await connection.ExecuteScalarAsync<int>
                ("GetCountQuestionLevel", new { Name = name }, commandType: System.Data.CommandType.StoredProcedure);
        }

        // tao kieu cau hoi moi
        public async Task<bool> CreateQuestionType(QuestionType questionType)
        {
            return await InsertOnlyName(TableQuestion.QuestionType, new { Name = questionType.Name, Code = questionType.Code, Description = questionType.Description });
        }
        public async Task<QuestionType?> GetDetailsQuestionType(int Id)
        {
            return await  GetEntityDetailsAsync<QuestionType>(Id, "GetDetailsQuestionType");
        }


        // tao danh muc cau hoi moi
        public async Task<bool> CreateQuestionCategory(QuestionCategory questionCategory)
        {
            return await InsertOnlyName(TableQuestion.QuestionCategory, new { Name = questionCategory.Name, Code = questionCategory.Code, Description = questionCategory.Description });
        }
        public async Task<QuestionCategory?> GetDetailsQuestionCategory(int Id)
        {
            return await GetEntityDetailsAsync<QuestionCategory>(Id, "GetDetailsQuestionCategory");
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
            return await GetAllEntityAsync<QuestionType>(TableQuestion.QuestionType);
        }

        public async Task<IEnumerable<QuestionCategory>> GetAllCategoryQuestion()
        {
            return await GetAllEntityAsync<QuestionCategory>(TableQuestion.QuestionCategory);
        }

        public async Task<bool> CheckValidId(int id, string table)
        {
            var command = $@"IF EXISTS (SELECT 1 FROM [{_name.Schema}].[{table}] where Id = @Id)
                                SELECT 1 ELSE SELECT 0";
            using var connection = _connection.CreateConnection();
            var result = await connection.ExecuteScalarAsync<int>(command, new { Id = id });
            return result == 1;
        }
        
        #endregion
        #region questions

        public async Task<bool> CreateQuestionAsync(Question question) {
            try
            {
                using var connection = _connection.CreateConnection();
                DataTable answers = new DataTable();
                answers.Columns.Add("Content", typeof(string));
                answers.Columns.Add("IsCorrect", typeof(bool));
                answers.Columns.Add("ImageUrl", typeof(string));
                answers.Columns.Add("Explanation", typeof(string));


                foreach (var answer in question.Answers)
                {
                    answers.Rows.Add(answer.Content, answer.IsCorrect, answer.ImageUrl, answer.Explanation);
                }


                var parameters = new DynamicParameters();
                parameters.Add("@LevelId", question.LevelId);
                parameters.Add("@QuestionContent", question.QuestionContent);
                parameters.Add("@TypeId", question.TypeId);
                parameters.Add("@CategoryId", question.CategoryId);
                parameters.Add("@ImageUrl", question.ImageUrl);
                parameters.Add("@Answers", answers.AsTableValuedParameter("[dbo].[QuestionAnswerType]"));


                var result = await connection.ExecuteAsync("spCreateQuestionWithAnswert", parameters, commandType: CommandType.StoredProcedure);
                return result > 0;
            } catch (SqlException ex)
            {
                throw new InvalidOperationException("An error occurred while creating the question.", ex);
            }
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
        public async Task<QuestionView?> GetDetailsQuestion(int id)
        {
            using var connection = _connection.CreateConnection();
            var sql = @"
                        SELECT * FROM [jcodepro_journey_of_faith].Question WHERE Id = @Id;
                        SELECT * FROM Answer WHERE QuestionId = @Id";
            using (var multiLine = await connection.QueryMultipleAsync(sql, new { Id = id }))
            {
                var question = await multiLine.ReadSingleOrDefaultAsync<QuestionView>();
                if(question is not null)
                {
                    var answer = (await multiLine.ReadAsync<AnswerView>()).ToList();

                    question?.Answers = answer.ToList();
                }

                return question;
            }
        }

        public async Task<bool> UpdateQuestion(Question question)
        {
            using var connection = _connection.CreateConnection();
            using var transaction = connection.BeginTransaction();

            try
            {

                var affectedRows = await connection.ExecuteAsync(
                    $@"Update [jcodepro_journey_of_faith].Question Set 
                                    LevelId = Coalesce(@LevelId, LevelId),  
                                    QuestionContent = Coalesce(@QuestionContent, QuestionContent),
                                    TypeId = Coalesce(@TypeId, TypeId),
                                    CategoryId = Coalesce(@CategoryId, CategoryId),
                                    ImageUrl = Coalesce(@ImageUrl, ImageUrl)
                                    where Id = @Id",
                    new { 
                        LevelId = question.LevelId, 
                        QuestionContent = question.QuestionContent,
                        TypeId = question.TypeId,
                        CategoryId = question.CategoryId,
                        ImageUrl = question.ImageUrl,
                        Id = question.Id },
                    transaction
                );

                if (affectedRows == 0)
                    throw new NotFoundException($"Question {question.Id} not found.");
                foreach (var answer in question.Answers)
                {
                    await connection.ExecuteAsync($@"
                        UPdate [{_name.Schema}].[{TableQuestion.Answer}] SET
                            Content = Coalesce(@Content, Content),
                            IsCorrect = Coalesce(@IsCorrect, IsCorrect), 
                            ImageUrl = CoaLesce(@ImageUrl, ImageUrl),
                            Explanation = Coalesce(@Explanation, Explanation)
                        where Id = @AnswerId
                    ", new
                    {
                        Content = answer.Content,
                        IsCorrect = answer.IsCorrect,
                        ImageUrl = answer.ImageUrl,
                        Explanation = answer.Explanation,
                        Id = answer.Id,
                    }, transaction);
                }

                transaction.Commit();
                return true;
            } catch
            {
                transaction.Rollback();
                throw;
            }

        }

        public async Task<bool> DeleteQuestion(int id)
        {
            using var connection = _connection.CreateConnection();

            var rowsAffected = await connection.ExecuteAsync($@"
                UPDATE [{_name.Schema}].[{TableQuestion.Question}]
                SET IsDeleted = @IsDeleted,
                    DeletedAt = @DeletedAt
                WHERE Id = @Id and IsDeleted = 0",
                        new
                        {
                            IsDeleted = true,
                            DeletedAt = DateTime.Now,
                            Id = id
                        });

            return rowsAffected > 0;
        }

        #endregion
    }
    #region Constants
    public static class TableQuestion
    {
        public const string Answer = "Answer";
        public const string Question = "Question";
        public const string QuizLevel = "QuizLevel";
        public const string QuestionType = "QuestionType";
        public const string QuestionCategory = "QuestionCategory";
    }
    #endregion
    #region Get data generic
    public abstract class DataHandlerRequest
    {
        private readonly IDbConnectionFactory _factory;
        private readonly TableSchemaName _schemaName;
        public DataHandlerRequest(IDbConnectionFactory factory, IOptions<TableSchemaName> _options)
        {
            _factory = factory;
            _schemaName = _options.Value;
        }

        public async Task<IEnumerable<T>> GetAllEntityAsync<T>(string table)
        {
            var connection = _factory.CreateConnection();
            return await connection.QueryAsync<T>($"Select * from [{_schemaName.Schema}].[{table}]");
        }

        public async Task<T?> GetEntityDetailsAsync<T>(int Id, string StoredProcedure)
        {
            var connection = _factory.CreateConnection();
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", Id);

            var data = await connection.QueryFirstOrDefaultAsync<T>(
                    StoredProcedure, param, commandType: System.Data.CommandType.StoredProcedure
                );
            return data;
        }

        public async Task<bool> InsertOnlyName(string table, object param)
        {
            string command = $"INSERT INTO [{_schemaName.Schema}].[{table}] (Name, Code, Description) VALUES(@Name, @Code, @Description)";
            if(table == TableQuestion.QuizLevel)
            {
                command = $"INSERT INTO [{_schemaName.Schema}].[{table}] (Name, Code, Score) VALUES(@Name, @Code, @Score)";
            }
            using var connection = _factory.CreateConnection();
            var result = await connection.ExecuteAsync(command, param);

            return result > 0;
        }
    }
    #endregion
}
