using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.quizs.queries
{
    public class GetDetailsQuizQuery : IRequest<QuizView>
    {
        public int Id { get; set; }
    }
}
