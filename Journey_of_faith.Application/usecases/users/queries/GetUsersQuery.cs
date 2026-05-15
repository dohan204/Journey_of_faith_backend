using Journey_of_faith.Domain.entities;
using MediatR;

namespace Journey_of_faith.Application.usecases.users.queries;

public class GetUsersQuery : IRequest<IEnumerable<User>>
{
    
}