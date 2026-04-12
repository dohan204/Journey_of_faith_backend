using Journey_of_faith.Domain.exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.entities.quiz
{
    public class AttemptAnswer
    {
        public long Id { get; set; }
        public int AttempId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public bool IsCorrect { get; set; }

        public AttemptAnswer(int attemptId, int questionId, int answerId, bool isCorrct)
        {
            if(attemptId < 0)
            {
                throw new DomainException("Mã Lần chọn kh hợp lệ");
            }

            if(questionId < 0)
            {
                throw new DomainException("Mã câu hỏi không hợp lệ");
            }

            if(answerId < 0)
            {
                throw new DomainException("Mã đáp án không hợp lệ");
            }
            AttempId = attemptId;
            QuestionId = questionId;
            AnswerId = answerId;
            IsCorrect = isCorrct;
        }

        public static AttemptAnswer Create(int attemptId, int questionId, int answerId, bool isCorrct)
            => new AttemptAnswer(attemptId, questionId, answerId, isCorrct);
    }
}
