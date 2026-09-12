using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands;

public class CreateMassAndLiturgyHandler : IRequestHandler<CreateMassAndLiturgyCommand, bool>
{
    private readonly IChurchRepository _churchRepository;

    public CreateMassAndLiturgyHandler(IChurchRepository churchRepository)
    {
        _churchRepository = churchRepository;
    }

    public async Task<bool> Handle(CreateMassAndLiturgyCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var dataInsert = command.Items.Select(item => new MassAndLiturgyInsert
        {
            MassSchedules = new MassSchedule
            {
                ChurchId = item.CreateMassSchedule.ChurchId,
                Name = item.CreateMassSchedule.Name,
                Date = item.CreateMassSchedule.Date,
                Time = item.CreateMassSchedule.Time
            },
            Liturgies = new Liturgy
            {
                ReadingOne = item.CreateLiturgy.Reading,
                ResponsorialPsalm = item.CreateLiturgy.ResponsorialPsalm,
                GoodNew = item.CreateLiturgy.Gospel,
                EndWord = item.CreateLiturgy.EndWord
            }
        }).ToList();

        return await _churchRepository.CreateMassAndLiturgyAsync(dataInsert);
    }
}
