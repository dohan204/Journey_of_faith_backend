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
                    UserId = quizAttempt.UserId,
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
    }
    public static class QuizTalbe
    {
        public const string Quiz = "Quiz";
        public const string QuizQuestion = "QuizQuestion";
        public const string QuizAttempt = "QuizAttempt";
        public const string AttemptAnswer = "AttemptAnswer";
    }

    

}


