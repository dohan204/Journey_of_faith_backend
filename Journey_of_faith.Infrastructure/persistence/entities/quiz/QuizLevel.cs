using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
{
    public class QuizLevel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code {get; set; } = string.Empty;
        public int Score { get; set; } = 0;

        public ICollection<Question> Questions { get; set; } = [];
    }
}
