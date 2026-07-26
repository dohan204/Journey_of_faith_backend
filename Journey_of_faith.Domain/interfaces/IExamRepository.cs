using Journey_of_faith.Domain.entities.quiz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.interfaces
{
    public class QuizView
    {
        public QuizView() { }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public int QuestionCount { get; set; }
        public bool IsDailyQuiz { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<QuestionQuiz> Questions { get; set; } =  new List<QuestionQuiz>();
        
    }

    public class QuestionQuiz
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public string ImageUrl { get; set; }
        public List<AnsewrQuestion> Ansewrs { get; set; } = new List<AnsewrQuestion>();

    }

    public class AnsewrQuestion
    {
        public int QuestionId { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class SubmitResult
    {
        public double Score { get; set; }
        public int FailQuestionCount { get; set; }
        public int CorrectQuestionCount { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    public interface IExamRepository
    {
        Task<int> CreateQuiz(Quiz quiz, int HardQuestion, int MediumQuestion, int EasyQuestion);
        Task<QuizView?> GetDetailsQuiz(int Id);

        Task<int> SaveScoreTest(QuizAttempt quiz);
        Task<bool> DeleteQuiz(int Id);


        Task<int> CreateTopicAsync(Topic topic);
        Task<int> DeleteTopicAsync(int id);
        Task<bool> ExistsNameAsync(string name);
    }



    public class QuizAttemptDto
    {
        public int QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Score { get; set; }
        public List<AttemptAnswerDto> Ansewrs { get; set; } = new();
    }

    public class AttemptAnswerDto
    {
        public int AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
