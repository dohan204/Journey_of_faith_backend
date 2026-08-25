using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using MediatR;

namespace Journey_of_faith.Application.usecases.users.queries;

public class GetMeQuery : IRequest<User>
{
    
}

public class GetMeHandler : IRequestHandler<GetMeQuery, User>
{
    private readonly IAuthService authService;
    public GetMeHandler(IAuthService authService)
    {
        this.authService = authService;
    }

    public async Task<User> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        return await authService.GetMe();
    }
}