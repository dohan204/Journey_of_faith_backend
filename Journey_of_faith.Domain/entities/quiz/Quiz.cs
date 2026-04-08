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
        public DateTime? CreatedTime { get; set; }

        private readonly List<QuizQuestion> _quizQuestions = new();
        private readonly List<QuizAttempt> _quizAttempts = new();

        public IReadOnlyCollection<QuizQuestion> QuizQuestions => _quizQuestions.AsReadOnly();
        public IReadOnlyCollection<QuizAttempt> QuizAttempts => _quizAttempts.AsReadOnly();
    }
}
