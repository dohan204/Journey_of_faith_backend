using Journey_of_faith.Domain.exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class Quiz
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? QuestionCount { get; set; }
        public int? TimeLimit { get; set; }
        public bool? IsDailyQuiz { get; set; }
        public int TopicId { get; set; }
        public DateTime? CreatedTime { get; set; }

        private readonly List<QuizQuestion> _quizQuestions = new();
        private readonly List<QuizAttempt> _quizAttempts = new();

        public IReadOnlyCollection<QuizQuestion> QuizQuestions => _quizQuestions.AsReadOnly();
        public IReadOnlyCollection<QuizAttempt> QuizAttempts => _quizAttempts.AsReadOnly();

        private Quiz () { }
        public Quiz(string title,int topicId, string description, int timeLimit, int questionCount)
        {
            if(title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }
            if(description is null)
            {
                throw new ArgumentNullException(nameof(description));
            }

            if(timeLimit < 0)
            {
                throw new DomainException("thời gian Làm bài không hợp lệ");
            } 
            Title = title;
            TopicId = topicId;
            Description = description;
            TimeLimit = timeLimit;
            CreatedTime = DateTime.UtcNow;
            QuestionCount = questionCount;
        }
        public static Quiz Create(string title, int topicid, string description, int timeLimit, int questionCount)
            => new Quiz(title,topicid, description, timeLimit, questionCount);
        public void SetDailyQuiz()
        {
            IsDailyQuiz = true;
        }

        public void AddQuizQuestion(int quizId, int qusstionId, int orderIndex)
        {
            _quizQuestions.Add(new QuizQuestion(quizId, qusstionId, orderIndex));
        }

    }

}
