using Journey_of_faith.Domain.entities.quiz;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.questions.queries
{
    public class GetCategoriesQuery : IRequest<IEnumerable<QuestionCategory>>
    {
    }
}
