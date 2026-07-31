using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands;


public class UpdateChurchHandler : IRequestHandler<UpdateChurchCommand, int>
{
    private readonly IChurchRepository churchRepository;
    private readonly ICurrentUserService currentUserService;
    public UpdateChurchHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService)
    {
        this.churchRepository = churchRepository;
        this.currentUserService = currentUserService;
    }


    public async Task<int> Handle(UpdateChurchCommand command, CancellationToken cancellationToken)
    {
        if(!Guid.TryParse(currentUserService.UserId, out var userId))
        {
            throw new UnauthorizationException("Người dùng không hợp lệ");
        }

        var listMassSche = command.MassSchedules.Select(e => new MassSchedule
        {
            Id = e.Id,
            Name = e.Name,
            Time = e.Time,
            MassTypeId = e.MassTypeId
        }).ToList();


        var church = new Church(command.Id, command.Name, command.Email, command.Email, command.DioceseId, command.Boss, command.Description, userId, listMassSche);
        church.SetLocation((double)command.Latitude, (double)command.Longitude);

        return await churchRepository.UpdateAsync(church, userId);
    }
}