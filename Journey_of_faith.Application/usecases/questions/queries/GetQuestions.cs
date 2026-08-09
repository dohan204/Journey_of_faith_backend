using Journey_of_faith.Application.common.dtos;
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.questions.queries;

public class GetQuestionsQuery : IRequest<PagedResult<QuestionView>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
}

public class GetQuestionsHandler : IRequestHandler<GetQuestionsQuery, PagedResult<QuestionView>>
{
    private readonly IQuestionRepository questionRepository;
    public GetQuestionsHandler(IQuestionRepository questionRepository)
    {
        this.questionRepository = questionRepository;
    }

    public async Task<PagedResult<QuestionView>> Handle(GetQuestionsQuery query, CancellationToken cancellationToken)
    {
        var result = await questionRepository.GetQuestionsAsync(query.Page, query.PageSize, query.Search);

        // Ép Data về IEnumerable<dynamic>
        var rawList = (IEnumerable<dynamic>)result.Data;

        return new PagedResult<QuestionView>
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            Data = rawList.Select(e => new QuestionView
            {
                Id = e.Id ?? 0,
                QuestionContent = e.QuestionContent,
                ImageUrl = e.ImageUrl,
                CategoryId = e.CategoryId ?? 0,
                CategoryName = e.CategoryName,
                LevelId = e.LevelId ?? 0,
                LevelName = e.LevelName,
                TypeId = e.TypeId ?? 0,
                TypeName = e.TypeName,
                IsDeleted = e.IsDeleted ?? false,

                // Chỉ giữ ép kiểu duy nhất cho e.Answers để chạy Lambda
                Answers = e.Answers != null 
                    ? ((IEnumerable<dynamic>)e.Answers).Select(a => new AnswerView
                      {
                          Id = a.Id ?? 0,
                          QuestionId = a.QuestionId ?? 0,
                          Content = a.Content,
                          IsCorrect = a.IsCorrect ?? false,
                      }).ToList()
                    : new List<AnswerView>()
            }).ToList()
        };
    }
}