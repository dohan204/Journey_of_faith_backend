using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Application.usecases.auth.queries;
using Journey_of_faith.Infrastructure.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IConfiguration configuration;
        public AuthService(TokenService tokenService, UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
            _currentUser = currentUserService;
            this.configuration = configuration;
        }

        public async Task<LoginUserResponse> Login(string email, string passwrod)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user is null)
            {
                throw new NotFoundException("Tài khoản hoặc mật khẩu không chính xác");
            }

            var result = await _userManager.CheckPasswordAsync(user, passwrod);
            if(!result)
            {
                throw new UnauthorizationException("Tài khoản hoặc mật khẩu không chính xác");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.GenerateToken(user, roles.ToList());
            var refreshToken = _tokenService.CreateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();
            return new LoginUserResponse(status: true, token: token, refreshToken: refreshToken.Token, expiry: configuration.GetValue<int>("Token:Expiry"));
        }

        public async Task<LoginUserResponse> RefreshToken(string refreshToken)
        {
            var refresh = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if(refresh is null || refresh.ExpiresOnUtc < DateTime.UtcNow)
            {
                throw new UnauthorizationException("Refresh token đã hết hạn hoặc không hợp lệ.");
            }

            var user = await _userManager.FindByIdAsync(refresh.UserId.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var expiry = configuration.GetValue<int>("Token:Expiry");

            var newToken = _tokenService.GenerateToken(user, roles.ToList());
            var newRefreshToken = _tokenService.CreateRefreshToken(user.Id);

            _context.RefreshTokens.Remove(refresh);
            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();

            return new LoginUserResponse(true, token: newToken, refreshToken: newRefreshToken.Token, expiry);
        }
    }
}
