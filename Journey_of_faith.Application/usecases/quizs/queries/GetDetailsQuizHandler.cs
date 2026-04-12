using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.quizs.queries
{
    public class GetDetailsQuizHandler : IRequestHandler<GetDetailsQuizQuery, QuizView?>
    {
        private readonly IExamRepository _repo;
        public GetDetailsQuizHandler(IExamRepository repo)
        {
            _repo = repo; 
        }

        public async Task<QuizView?> Handle(GetDetailsQuizQuery query, CancellationToken token)
        {
            return await _repo.GetDetailsQuiz(query.Id);
        }
    }

    //public class QuizViewComparer : IEqualityComparer<QuizView>
    //{
    //    public bool Equals(QuizView? z, QuizView? y)
    //    {
    //        if (ReferenceEquals(z, y)) return true;
    //        if (z is null || y is null) return false;
    //        return z.QuizId == y.QuizId &&
    //            z.Title == y.Title && z.Description == y.Description &&
    //            z.TimeLimit == y.TimeLimit && z.QuestionCount == y.QuestionCount &&
    //            z.IsDailyQuiz == y.IsDailyQuiz && z.CreatedTime == y.CreatedTime &&
    //            z.QuestionId == y.QuestionId && z.QuestionContent == y.QuestionContent &&
    //            z.ImageUrl == y.ImageUrl;
    //    }

    //    public int GetHashCode(QuizView obj)
    //    {
    //        if (obj is null) return 0;
    //        return HashCode.Combine(obj.QuizId, obj.QuestionId);
    //    }
    //}
}
