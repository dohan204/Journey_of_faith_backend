using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Application.usecases.users.commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, bool>
    {
        private readonly IIdentityService _services;
        public CreateUserHandler(IIdentityService services)
        {
            _services = services;
        }

        public async Task<bool> Handle(CreateUserCommand command, CancellationToken token)
        {
            if(await _services.ExistsEmail(command.Email))
            {
                Console.WriteLine($"email: ${command.Email} trùng với email trong cơ sở dữ liệu");
                throw new ConfictException($"Email {command.Email} đã đươc sử dụng, vui lòng nhập email khác");
            }

            Console.WriteLine("ok!, tạo người dùng");
            var user = User.Create(command.Username, command.Password, command.Name, command.Email);

            await _services.CreateAsync(user);
            Console.WriteLine("Tạo người dùng thành công.");
            return true;
        }
    }
}
