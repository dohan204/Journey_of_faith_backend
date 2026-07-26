using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Application.usecases.auth.queries;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.persistence.entities.location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Journey_of_faith.Infrastructure.identity.services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IConfiguration configuration;
        public AuthService(TokenService tokenService, UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
            _currentUser = currentUserService;
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
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
            UserActive active = new UserActive
            {
                ApplicationUserId = user.Id,
                Status = true,
                ActiveLocation = "ha",
                Timespan = DateTime.UtcNow,
            };
            await _context.UserActive.AddAsync(active);
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


        public async Task<bool> ChangePassword(string currentPassword, string newPassword)
        {
            if(string.IsNullOrEmpty(currentPassword))
            {
                throw new ArgumentException(nameof(currentPassword));
            }

            if(string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                throw new UnauthorizationException("Vui long dang nhap");
            }

            var isValid = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if(!isValid.Succeeded)
            {
                return false;
            }


            return true;
        }
    }
}
