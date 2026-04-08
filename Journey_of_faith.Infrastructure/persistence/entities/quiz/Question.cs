using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
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

        public QuizLevel Level { get; set; } = null!;
        public QuestionType? Type { get; set; }
        public QuestionCategory? Category { get; set; }
        public ICollection<Answer> Answers { get; set; } = [];
        public ICollection<QuizQuestion> QuizQuestions { get; set; } = [];
    }
}
