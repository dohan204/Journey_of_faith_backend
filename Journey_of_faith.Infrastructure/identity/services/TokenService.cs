using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Journey_of_faith.Infrastructure.identity.services
{
    public class TokenService(IConfiguration config)
    {
        public async Task<string> GenerateToken(ApplicationUser user, List<string> roles)
        {
            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if(roles.Any())
            {
                foreach(var role in roles)
                {
                    claim.Add(new Claim("role", role.ToString()));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("Token:Key") ?? string.Empty));

            var signa = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config.GetValue<string>("Token:Issuer"),
                audience: config.GetValue<string>("Token:Audence"),
                claims: claim,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signa
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
