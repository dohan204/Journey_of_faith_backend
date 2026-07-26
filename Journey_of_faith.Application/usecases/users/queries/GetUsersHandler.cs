using Journey_of_faith.Domain.entities;
using MediatR;
using Journey_of_faith.Domain.interfaces;
using Journey_of_faith.Domain.dtos;
using Journey_of_faith.Application.common.dtos;
namespace Journey_of_faith.Application.usecases.users.queries;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PagedResult<UserResponseDto>>
{
    private readonly IUserRepository _userRepository;
    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<PagedResult<UserResponseDto>> Handle(GetUsersQuery query, CancellationToken token)
    {
        var user = await _userRepository.GetUsersAsync(query.Page, query.PageSize, query.Search);
        return new PagedResult<UserResponseDto>
        {
            TotalCount = user.TotalCount,
            Data = user.Data.Select(e => new UserResponseDto
            {
                Id = e.Id,
                UserName = e.Username, 
                Email = e.Email,
                Role = e.Role,
                Avatar = e.Avatar,
                IsDeleted = e.IsDeleted
            }).ToList(),
            Page = query.Page,  
            PageSize = query.PageSize,
        };
    }
}