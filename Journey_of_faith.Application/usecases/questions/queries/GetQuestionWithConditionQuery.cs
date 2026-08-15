using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities.quiz;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.questions.queries;


public class GetQuestionWithConditionQuery : IRequest<IEnumerable<Question>>
{
    public int CategoryId {get; set;}
    public int LevelId {get; set;}
    public int QuestionCount {get; set;}
}


public class GetQuestionWithConditionHandler : IRequestHandler<GetQuestionWithConditionQuery, IEnumerable<Question>>
{
    private readonly IQuestionRepository questionRepository;
    private readonly IDataHandler dataHandler;
    public GetQuestionWithConditionHandler(IQuestionRepository questionRepository, IDataHandler dataHandler)
    {
        this.questionRepository = questionRepository;
        this.dataHandler = dataHandler;
    }
    public async Task<IEnumerable<Question>> Handle(GetQuestionWithConditionQuery query, CancellationToken cancellationToken)
    {
        return await questionRepository.GetQuestionsWithCondition(query.CategoryId, query.LevelId, query.QuestionCount);
    }
}