using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands;

public class CreateLiturgy : CreateLiturgyCommand, IRequest<bool>
{
    public DateTime DateActive {get; set;}
}


public class CreateLiturgyHandler : IRequestHandler<CreateLiturgy, bool>
{
    private readonly IChurchRepository churchRepository;
    public CreateLiturgyHandler(IChurchRepository churchRepository)
    {
        this.churchRepository = churchRepository;
    }

    public async Task<bool> Handle(CreateLiturgy command, CancellationToken cancellationToken)
    {
        var liturgy = new Liturgy
        {
          ReadingOne = command.Reading,
          ResponsorialPsalm = command.ResponsorialPsalm,
          GoodNew = command.Gospel,
          EndWord = command.EndWord,
          DateActive = command.DateActive,
        };
        return await churchRepository.CreateLiturgyAsync(liturgy);
    }
}