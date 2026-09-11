using Dapper;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Infrastructure.common;
using Journey_of_faith.Infrastructure.dtos.quiz;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.repositories
{
    public class ExamRepository(IDbConnectionFactory _factory, IOptions<TableSchemaName> _name) : IExamRepository
    {
        private readonly TableSchemaName name = _name.Value;
        public async Task<int> CreateQuiz(Quiz quiz, int HardQuestion, int MediumQuestion, int EasyQuestion)
        {
            using var connection = _factory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            try
            {
                var Id = await connection.ExecuteScalarAsync<int>("CreateQuiz", new {
                    quiz.Title, 
                    quiz.TopicId,
                    quiz.Description,
                    quiz.TimeLimit,
                    quiz.QuestionCount,
                    HardQuestion,
                    MediumQuestion,
                    EasyQuestion
                }, 
                transaction, 
                commandType: System.Data.CommandType.StoredProcedure);

                transaction.Commit();
                return Id;
            } catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<QuizView>> GetAllQuizzesAsync()
        {
            using var connection = _factory.CreateConnection();
            using var multi = await connection.QueryMultipleAsync($@"
                SELECT Id, Title, Description, TimeLimit, QuestionCount, IsDailyQuiz, TopicId, CreatedTime
                FROM [{name.Schema}].[{QuizTalbe.Quiz}]
                WHERE ISNULL(IsDeleted, 0) = 0
                ORDER BY CreatedTime DESC, Id DESC;

                SELECT qq.QuizId, q.Id, q.QuestionContent, q.ImageUrl
                FROM [{name.Schema}].[{QuizTalbe.QuizQuestion}] qq
                INNER JOIN [{name.Schema}].[{TableQuestion.Question}] q ON q.Id = qq.QuestionId
                INNER JOIN [{name.Schema}].[{QuizTalbe.Quiz}] quiz ON quiz.Id = qq.QuizId
                WHERE ISNULL(quiz.IsDeleted, 0) = 0
                  AND ISNULL(q.IsDeleted, 0) = 0
                ORDER BY qq.QuizId, qq.OrderIndex, qq.Id;

                SELECT a.QuestionId, a.Id, a.Content, a.IsCorrect
                FROM [{name.Schema}].[{TableQuestion.Answer}] a
                WHERE EXISTS (
                    SELECT 1
                    FROM [{name.Schema}].[{QuizTalbe.QuizQuestion}] qq
                    INNER JOIN [{name.Schema}].[{QuizTalbe.Quiz}] quiz ON quiz.Id = qq.QuizId
                    WHERE qq.QuestionId = a.QuestionId
                      AND ISNULL(quiz.IsDeleted, 0) = 0
                )
                ORDER BY a.QuestionId, a.Id;
            ");

            var quizzes = (await multi.ReadAsync<QuizView>()).ToList();
            var questionRows = (await multi.ReadAsync<QuizQuestionRow>()).ToList();
            var answers = (await multi.ReadAsync<AnsewrQuestion>()).ToList();

            var answerLookup = answers.ToLookup(answer => answer.QuestionId);
            var questionLookup = questionRows
                .Select(row => new
                {
                    row.QuizId,
                    Question = new QuestionQuiz
                    {
                        Id = row.Id,
                        QuestionContent = row.QuestionContent,
                        ImageUrl = row.ImageUrl,
                        Ansewrs = answerLookup[row.Id].ToList()
                    }
                })
                .ToLookup(item => item.QuizId, item => item.Question);

            foreach (var quiz in quizzes)
            {
                quiz.Questions = questionLookup[quiz.Id].ToList();
            }

            return quizzes;
        }


        public async Task<QuizView?> GetDetailsQuiz(int Id)
        {
            using var connection = _factory.CreateConnection();
            using (var multi = await connection.QueryMultipleAsync("GetDetails", new { Id = Id }, commandType: System.Data.CommandType.StoredProcedure))
            {
                var quiz = await multi.ReadSingleOrDefaultAsync<QuizView>();
                if (quiz is null) return null;
                var questions = (await multi.ReadAsync<QuestionQuiz>()).ToList();

                var answer = (await multi.ReadAsync<AnsewrQuestion>()).ToList();
                Console.WriteLine(answer);
                var answerLookUp = answer.ToLookup(a => a.QuestionId);
                foreach(var p in questions)
                {
                    p.Ansewrs = answerLookUp[p.Id].ToList();
                }
                quiz.Questions = questions;
                return quiz;
            }
            
        }


        public async Task<int> SaveScoreTest(QuizAttempt quizAttempt)
        {
            using var connection = _factory.CreateConnection();
            using var transaction = connection.BeginTransaction();
            try
            {
                var quizAttemp = await connection
                .ExecuteScalarAsync<int>($@"
                    Insert into [{name.Schema}].[{QuizTalbe.QuizAttempt}] (QuizId, UserId, StartTime, EndTime, Score)
                    Output inserted.Id
                    VALUES(@QuizId, @UserId, @StartTime, @EndTime, @Score)
                ", new CreateQuizAttempt
                {
                    QuizId = quizAttempt.QuizId,
                    UserId = quizAttempt.UserId.ToString(),
                    StartTime = quizAttempt.StartTime,
                    EndTime = quizAttempt.EndTime,
                    Score = quizAttempt.Score,
                }, transaction: transaction);
                
                foreach(var attemptAnswer in quizAttempt.AttemptAnswers)
                {
                    await connection.ExecuteAsync($@"
                        Insert into [{name.Schema}].[{QuizTalbe.AttemptAnswer}] (AttemptId, QuestionId, AnswerId, IsCorrect)
                        Values(@AttemptId, @QuestionId, @AnswerId, @IsCorrect)
                    ", new CreateAttemptAnswer
                    {
                        AttemptId = quizAttemp,
                        QuestionId = attemptAnswer.QuestionId,
                        AnswerId = attemptAnswer.AnswerId,
                        IsCorrect = attemptAnswer.IsCorrect
                    }, transaction);
                }

                transaction.Commit();
                return quizAttemp;
            } catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteQuiz(int Id)
        {
            using var connection = _factory.CreateConnection();
            var isDelete = await connection.ExecuteAsync($@"
                UPDATE [{name.Schema}].[{QuizTalbe.Quiz}] SET
                IsDeleted = @IsDeleted, DeletedAt = @DeletedAt
                Where Id = @Id and IsDeleted = 0
            ", new { IsDeleted = true, DeletedAt = DateTime.Now, Id = Id});

            return isDelete > 0;
        }

        public async Task<IEnumerable<Topic>> GetTopicsAsync()
        {
            using var connection = _factory.CreateConnection();
            var topics = await connection.QueryAsync<Topic>("Select * From [jcodepro_journey_of_faith].[Topic]");
            return topics;
        }
        #region Topic
        public async Task<int> CreateTopicAsync(Topic topic)
        {
            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(@$"
                insert into [{name.Schema}].[{QuizTalbe.Topic}] (TopicName, QuizCount, CreationTime)
                values (@TopicName, @QuizCount, Getdate())
            ", new { TopicName = topic.TopicName, QuizCount = topic.QuizCount });
        }


        public async Task<int> DeleteTopicAsync(int id)
        {
            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>($@"
                UPDATE [{name.Schema}].[{QuizTalbe.Topic}] Set
                    DeletedAt = GETDATE(),
                    IsDeleted = 1
                Where Id = @id
            ", new { id = id });
        }

        public async Task<bool> ExistsNameAsync(string nameTop)
        {
            using var connection = _factory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>($@"
                 IF EXISTS (Select 1 from [{name.Schema}].[{QuizTalbe.Topic}] where TopicName = @name)
                     Select 1
                 ELSE 
                    Select 0
            ", new { name = nameTop }) > 0;
        }
        #endregion
    }
    public static class QuizTalbe
    {
        public const string Quiz = "Quiz";
        public const string QuizQuestion = "QuizQuestion";
        public const string QuizAttempt = "QuizAttempt";
        public const string AttemptAnswer = "AttemptAnswer";
        public const string Topic = "Topic";
    }

    internal sealed class QuizQuestionRow
    {
        public int QuizId { get; set; }
        public int Id { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }

    

}


