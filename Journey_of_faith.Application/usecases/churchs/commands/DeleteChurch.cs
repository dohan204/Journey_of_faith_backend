using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.churchs.commands;


public class DeleteChurchCommand : IRequest<bool>
{
    public int Id {get; set;}
    public bool? Force {get; set;}
}


public class DeleteChurchHandler : IRequestHandler<DeleteChurchCommand, bool>
{
    private readonly IChurchRepository churchRepository;
    public DeleteChurchHandler(IChurchRepository churchRepository)
    {
        this.churchRepository = churchRepository;
    }

    public async Task<bool> Handle(DeleteChurchCommand command, CancellationToken cancellationToken)
    {
        bool isDeleted = await churchRepository.DeleteChurchAsync(command.Id, command.Force);
        if(!isDeleted)
        {
            throw new NotFoundException($"Nhà thờ với mã: {command.Id} không hợp lệ");
        }

        return true;
    }
}