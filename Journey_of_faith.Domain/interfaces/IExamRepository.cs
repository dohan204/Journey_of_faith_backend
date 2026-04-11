using Journey_of_faith.Domain.entities.quiz;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Domain.interfaces
{
    public interface IExamRepository
    {
        Task<int> CreateQuiz(Quiz quiz);
    }
}
