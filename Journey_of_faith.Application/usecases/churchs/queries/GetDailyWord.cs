using MediatR;
using Journey_of_faith.Domain.entities.catholic;
using Journey_of_faith.Domain.interfaces;
namespace Journey_of_faith.Application.usecases.churchs.queries;

public class GetDailyWordCommand : IRequest<DailyWord?>
{
    public DateTime Date {get; set;}
}


public class GetDailyWordHandler : IRequestHandler<GetDailyWordCommand, DailyWord?>
{
    private readonly IChurchRepository churchRepository;
    public GetDailyWordHandler(IChurchRepository churchRepository)
    {
        this.churchRepository = churchRepository;
    }


    public async Task<DailyWord?> Handle(GetDailyWordCommand command, CancellationToken cancellationToken)
    {
        return await churchRepository.GetDailyWorldAsync(command.Date);
    }
}