using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
{
    public class Topic
    {
        public int Id { get; set; }
        public string TopicName { get; set; } = string.Empty;
        public int? QuizCount { get; set; }
        public DateTime? CreationTime { get; set; } = DateTime.Now;
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; } = string.Empty;
        public bool? IsDeleted { get; set; }
        public ICollection<Quiz> Quizs { get; set; } = new List<Quiz>();
    }
}
