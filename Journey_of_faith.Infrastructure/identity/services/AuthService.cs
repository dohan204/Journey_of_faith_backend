using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Journey_of_faith.Infrastructure.identity.services
{
    public class AuthService : IAuthService
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthService(TokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<LoginUserResponse> Login(string username, string passwrod)
        {
            var user = await _userManager.FindByNameAsync(request.username);
            if(user is null)
            {
                throw new NotFoundException("Tài khoản này chưa được tạo");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.password);
            if(!result)
            {
                throw new UnauthorizationException("Tên đăng nhập hoặc mật khẩu không đúng");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.GenerateToken(user, roles.ToList());

            return new LoginUserResponse(status: true, token: token.ToString());
        }
    }
}
