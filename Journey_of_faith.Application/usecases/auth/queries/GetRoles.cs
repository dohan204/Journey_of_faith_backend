using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using MediatR;

namespace Journey_of_faith.Application.usecases.auth.queries;

public class GetRolesQuery : IRequest<PagedResult<Role>>
{
    public int Page {get; set;}
    public int PageSize {get; set;}
    public string? Search {get; set;}
}



public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, PagedResult<Role>>
{
    private readonly IRoleRepository roleRepository;
    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }

    public async Task<PagedResult<Role>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
    {
        return await roleRepository.GetRolesAsync(query.Page, query.PageSize, query.Search);
    }
}