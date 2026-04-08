using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
{
    public class AttemptAnswer
    {
        public long Id { get; set; }
        public long AttemptId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public bool IsCorrect { get; set; }

        public QuizAttempt Attempt { get; set; } = null!;
    }
}
