using Journey_of_faith.Application.common.interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.auth.commands
{
    public class CreateRefreshTokenCommand : IRequest<LoginUserResponse>
    {
        public string Token { get; set; }
    }
}
