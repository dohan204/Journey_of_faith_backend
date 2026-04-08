using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class Question
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public string QuestionContent { get; set; } = string.Empty;
        public int? TypeId { get; set; }
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedTime { get; set; }

        private readonly List<Answer> _answers = new();
        private readonly List<QuizQuestion> _quizQuestions = new();
        private readonly List<AttemptAnswer> _attemptAnswers = new();

        public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();
        public IReadOnlyCollection<QuizQuestion> QuizQuestions => _quizQuestions.AsReadOnly();
        public IReadOnlyCollection<AttemptAnswer> AttemptAnswers => _attemptAnswers.AsReadOnly();
    }
}
