using Journey_of_faith.Domain.exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class QuizAttempt
    {
        public long Id { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Score { get; set; }

        private readonly List<AttemptAnswer> _attemptAnswers = new();

        public IReadOnlyCollection<AttemptAnswer> AttemptAnswers => _attemptAnswers.AsReadOnly();
        private QuizAttempt() { }
        public QuizAttempt(int quizId, string userId, DateTime startTime, DateTime endTime, int score)
        {
            if(quizId < 0)
            {
                throw new DomainException("Mã bài thi không hợp lệ");
            }

            if(string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException("Mã người dùng không được phép null");
            }
            if(score < 0)
            {
                throw new DomainException("Điểm kh được nhỏ hơn 0.");
            }
            StartTime = startTime;
            EndTime = endTime;
            Score = score;
        }

        public static QuizAttempt Create(int quizId, string userId, DateTime startTime, DateTime endTime, int score)
            => new QuizAttempt(quizId, userId, startTime, endTime, score);


        public void AddAttemptAnswer(int attemptId, int questionid, int answerid,  bool isCorrect)
        {
            var attemptAnswers = AttemptAnswer.Create(attemptId, questionid, answerid, isCorrect);
            _attemptAnswers.Add(attemptAnswers);
        }
    }
}
