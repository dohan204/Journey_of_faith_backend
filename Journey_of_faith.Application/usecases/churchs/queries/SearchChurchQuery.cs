using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries
{
    public class SearchChurchQuery : IRequest<IEnumerable<ChurchListItemView>>
    {
        public string? Keyword { get; set; }
        public int? DioceseId { get; set; }
    }

    public class SearchChurchHandler : IRequestHandler<SearchChurchQuery, IEnumerable<ChurchListItemView>>
    {
        private readonly IChurchRepository _churchRepository;
        private readonly ICurrentUserService _currentUserService;

        public SearchChurchHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService)
        {
            _churchRepository = churchRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ChurchListItemView>> Handle(SearchChurchQuery request, CancellationToken cancellationToken)
        {
            Guid? userId = null;
            if (Guid.TryParse(_currentUserService.UserId, out var parsed))
            {
                userId = parsed;
            }

            return await _churchRepository.SearchChurchesAsync(request.Keyword, request.DioceseId, userId);
        }
    }
}
