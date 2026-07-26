using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.auth.queries;

public class GetTotalUserPerRoleQuery: IRequest<Dictionary<string, int>>
{
    
}

public class GetTotalUserPerRoleHandler : IRequestHandler<GetTotalUserPerRoleQuery, Dictionary<string, int>>
{
    private readonly IRoleRepository roleRepository;
    public GetTotalUserPerRoleHandler(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }


    public async Task<Dictionary<string, int>> Handle(GetTotalUserPerRoleQuery query, CancellationToken cancellationToken)
    {
        return await roleRepository.GetTotalUserRole();
    }
}