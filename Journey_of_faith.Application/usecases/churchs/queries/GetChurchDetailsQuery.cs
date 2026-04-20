using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.queries
{
    public class GetChurchDetailsQuery : IRequest<Church?>
    {
        public int Id { get; set; }
    }

    public class GetChurchDetailsHandler : IRequestHandler<GetChurchDetailsQuery, Church?>
    {
        private readonly IChurchRepository _churchRepository;

        public GetChurchDetailsHandler(IChurchRepository churchRepository)
        {
            _churchRepository = churchRepository;
        }

        public async Task<Church?> Handle(GetChurchDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _churchRepository.GetByIdAsync(request.Id);
        }
    }
}
