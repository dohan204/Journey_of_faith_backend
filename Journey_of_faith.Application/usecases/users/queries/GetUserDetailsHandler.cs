using System.Data.Common;
using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Journey_of_faith.Application.usecases.users.queries;

public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsQuery, User?>
{
    private readonly ILogger<GetUserDetailsHandler> _logger;
    private readonly IUserRepository _userRepo;
    public GetUserDetailsHandler(ILogger<GetUserDetailsHandler> logger, IUserRepository userRepo)
    {
        _userRepo = userRepo;
        _logger = logger;
    }

    public async Task<User?> Handle(GetUserDetailsQuery query, CancellationToken token)
    {
        try
        {
            return await _userRepo.GetUserByIdAsync(query.Id, token);
        } catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Request timeout to server.");
            throw;
        } catch (DbException ex)
        {
            _logger.LogError(ex, "Error connect db"); throw;
        }
    }
}