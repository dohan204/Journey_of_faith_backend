using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.quizs.queries
{
    public class GetAllQuizzesQuery : IRequest<IEnumerable<QuizView>>
    {
    }
}
