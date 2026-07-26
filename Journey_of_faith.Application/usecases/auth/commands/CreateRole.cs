using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.auth.commands;

public class CreateRoleCommand : IRequest<string>
{
    public string Name {get; set;}
    public string Description {get; set;}
}

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, string>
{
    private readonly IRoleRepository roleRepository;
    public CreateRoleHandler(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    public async Task<string> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        if(await roleRepository.NameExists(command.Name))
        {
            throw new ConfictException("Tên vai trò đã tồn tại");
        }
        var role = new Role
        {
            Name = command.Name,
            Descriptions = command.Description,
        };
        return await roleRepository.CreateAsync(role, cancellationToken);
    }
}