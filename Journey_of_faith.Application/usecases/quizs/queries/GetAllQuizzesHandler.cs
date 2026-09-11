using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.quizs.queries
{
    public class GetAllQuizzesHandler : IRequestHandler<GetAllQuizzesQuery, IEnumerable<QuizView>>
    {
        private readonly IExamRepository _repository;

        public GetAllQuizzesHandler(IExamRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<QuizView>> Handle(
            GetAllQuizzesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllQuizzesAsync();
        }
    }
}
