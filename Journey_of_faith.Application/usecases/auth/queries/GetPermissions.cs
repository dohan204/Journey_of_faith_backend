using System.Reflection.Metadata.Ecma335;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.auth.queries;

public class GetPermissionsQuery : IRequest<List<object>>
{

}


public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, List<object>>
{
    private readonly IRoleRepository roleRepository;
    public GetPermissionsHandler(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    public async Task<List<object>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        return await roleRepository.GetPermissionForRole();
    }
}