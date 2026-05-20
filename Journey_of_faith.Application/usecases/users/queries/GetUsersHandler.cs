using Journey_of_faith.Domain.entities;
using MediatR;
using Journey_of_faith.Domain.interfaces;
namespace Journey_of_faith.Application.usecases.users.queries;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserRepository _userRepository;
    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<IEnumerable<User>> Handle(GetUsersQuery query, CancellationToken token)
    {
        return await _userRepository.GetUsersAsync();
    }
}