using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
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
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; }
        public int? TopicId { get; set; }
        public Topic Topic { get; set; }
        public ICollection<QuizQuestion> QuizQuestions { get; set; } = [];
        public ICollection<QuizAttempt> Attempts { get; set; } = [];
    }
}
