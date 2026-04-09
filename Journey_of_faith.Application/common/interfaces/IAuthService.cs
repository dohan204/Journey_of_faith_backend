using Journey_of_faith.Application.usecases.auth.queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.common.interfaces
{
    public record LoginUserRequest(string username, string password);
    public record LoginUserResponse(bool status, string token);
    public interface IAuthService
    {
        Task<LoginUserResponse> Login(string username, string passwrod);
    }
}
