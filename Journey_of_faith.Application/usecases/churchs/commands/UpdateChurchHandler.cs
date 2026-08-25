using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities.location;
using Journey_of_faith.Domain.entities.masslive;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Journey_of_faith.Application.usecases.churchs.commands;


public class UpdateChurchHandler : IRequestHandler<UpdateChurchCommand, int>
{
    private readonly IChurchRepository churchRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly ILogger<UpdateChurchHandler> _logger;
    public UpdateChurchHandler(IChurchRepository churchRepository, ICurrentUserService currentUserService, ILogger<UpdateChurchHandler> logger)
    {
        this.churchRepository = churchRepository;
        this.currentUserService = currentUserService;
        this._logger = logger;
    }


    public async Task<int> Handle(UpdateChurchCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (!Guid.TryParse(currentUserService.UserId, out var userId))
            {
                throw new UnauthorizationException("Người dùng không hợp lệ");
            }
            foreach(var mas in command.MassSchedules)
            {
                _logger.LogError("Id: {0}, name: {1}, time: {2}", mas.Id, mas.Name, mas.Time);
            }
            var listMassSche = command.MassSchedules.Select(e => new MassSchedule
            {
                Id = (int)e.Id,
                Name = e?.Name ?? string.Empty,
                Time = e?.Time ?? string.Empty,
                MassTypeId = 1
            }).ToList();

            _logger.LogError("data update: {0}", command);
            _logger.LogWarning("MassSchedule: {0}", listMassSche);
            var church = new Church(command.Id, command.Name, command.Email, command.Address, command.DioceseId, command.Boss, command.Description, userId, listMassSche);
            church.SetLocation(command?.Latitude ?? 0, command?.Longitude ?? 0);

            return await churchRepository.UpdateAsync(church, userId);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            _logger.LogError("error log: {0}", ex.ToString());
            return 0;
        }
    }
}