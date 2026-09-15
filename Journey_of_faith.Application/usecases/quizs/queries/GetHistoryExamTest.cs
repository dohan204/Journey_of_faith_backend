using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.quizs.queries;

public class GetHistoryExamTestQuery : IRequest<IReadOnlyList<HistoryExamTest>>
{
}

public class GetHistoryExamTestHandler : IRequestHandler<GetHistoryExamTestQuery, IReadOnlyList<HistoryExamTest>>
{
    private readonly ICurrentUserService currentUserService;
    private readonly IExamRepository examRepository;
    public GetHistoryExamTestHandler(IExamRepository examRepository, 
        ICurrentUserService currentUserService)
    {
        this.examRepository = examRepository;
        this.currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<HistoryExamTest>> Handle(GetHistoryExamTestQuery query, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(currentUserService.UserId, out Guid userId))
        {
            throw new UnauthorizationException("Thoong tin tai khoan khong hojp le");
        }
        return await examRepository.GetHistoryExamTestsAsync(userId);
    }
}