using Journey_of_faith.Infrastructure.identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.persistence.entities.quiz
{
    public class QuizAttempt
    {
        public long Id { get; set; }
        public int QuizId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }

        public Quiz Quiz { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = [];
    }
}
