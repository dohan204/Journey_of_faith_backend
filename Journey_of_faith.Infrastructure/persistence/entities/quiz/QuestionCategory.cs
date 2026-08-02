using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
{
    public class QuestionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code {get; set; } = string.Empty;
        public string Description {get; set; } = string.Empty;

        public ICollection<Question> Questions { get; set; } = [];
    }
}
