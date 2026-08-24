
using Google.Apis.Auth;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Journey_of_faith.Application.usecases.auth.commands
{
    public class GoogleLoginCommand : IRequest<LoginResponse>
    {
        public string IdToken { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
    }

    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _configuration;

        public GoogleLoginCommandHandler(IUserRepository userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Xác thực token từ Google
            var clientId = _configuration["Authentication:Google:ClientId"];
            Console.WriteLine($"🟢 ClientId from appsettings: {clientId}");

            var payload = await VerifyGoogleToken(request.IdToken, clientId);

            if (payload == null)
            {
                Console.WriteLine("❌ Token verification failed");
                throw new UnauthorizedAccessException("Invalid Google token");
            }

            Console.WriteLine($"✅ Token verified! Email: {payload.Email}, Name: {payload.Name}");

            // 2. Tìm user theo email
            var existingUser = await _userRepo.GetByEmailAsync(payload.Email);

            if (existingUser == null)
            {
                Console.WriteLine($"🟢 User not found, creating new user: {payload.Email}");

                var user = new User(
                    payload.Name,
                    payload.Email,
                    payload.Email.Split('@')[0],
                    Guid.NewGuid().ToString(),
                    payload.Picture
                );

                user.Id = Guid.NewGuid();
                user.Role = "User";
                user.IsDeleted = false;
                user.CreationTime = DateTime.UtcNow;
                user.CreatorUserId = user.Id;
                user.LastModifierUserId = user.Id;
                user.LastModificationTime = DateTime.UtcNow;
                user.DeleterUserId = Guid.Empty;
                user.DeletionTime = null;
                user.AccessFailedCount = 0;
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.TwoFactorEnabled = false;
                user.PhoneNumber = null;
                user.PhoneNumberConfirmed = false;

                await _userRepo.CreateAsync(user);
                Console.WriteLine($"✅ User created: {user.Id}");

                existingUser = user;
            }
            else
            {
                Console.WriteLine($"🟢 User found: {existingUser.Email}, IsDeleted={existingUser.IsDeleted}");

                if (existingUser.IsDeleted == true)
                {
                    Console.WriteLine("🟢 Reactivating deleted user");
                    existingUser.IsDeleted = false;
                    existingUser.DeletionTime = null;
                    await _userRepo.UpdateAsync(existingUser);
                }
            }

            // 3. Tạo JWT token
            var token = GenerateJwtToken(existingUser);
            var refreshToken = GenerateRefreshToken();

            Console.WriteLine($"✅ Login successful for: {existingUser.Email}");

            return new LoginResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                UserId = existingUser.Id.ToString(),
                UserName = existingUser.Username,
                Email = existingUser.Email,
                Role = existingUser.Role ?? "User",
                Avatar = existingUser.Avatar
            };
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken, string clientId)
        {
            try
            {
                Console.WriteLine($"🟢 Verifying token with ClientId: {clientId}");
                Console.WriteLine($"🟢 Token preview: {idToken?.Substring(0, Math.Min(80, idToken?.Length ?? 0))}...");

                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                Console.WriteLine($"✅ Token valid! Email: {payload.Email}");
                return payload;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Token verification error: {ex.Message}");
                Console.WriteLine($"❌ Stack: {ex.StackTrace}");
                return null;
            }
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
    }
}