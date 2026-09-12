using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.quizs.queries;


public class GetAllTopicHandler : IRequestHandler<GetAllTopicQuery, IEnumerable<Topic>>
{
    private readonly IExamRepository examRepository;
    public GetAllTopicHandler(IExamRepository examRepository)
    {
        this.examRepository = examRepository;
    }
    public async Task<IEnumerable<Topic>> Handle(GetAllTopicQuery query, CancellationToken cancellationToken)
    {
        return await examRepository.GetTopicsAsync();
    }
}