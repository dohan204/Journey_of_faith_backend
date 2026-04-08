using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Infrastructure.context;
using Journey_of_faith.Infrastructure.identity;
using Journey_of_faith.Infrastructure.identity.services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            });

            service.AddIdentityCore<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            service.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;

                // sign in 
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetValue<string>("Token:Issuer"),
                        ValidAudience = configuration.GetValue<string>("Token:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Token:Key") ?? string.Empty))
                    };
                });
            service.AddScoped<TokenService>();
            service.AddScoped<IIdentityService, IdentityService>();
            return service;
        } 
    }


    public static class DpAutomapper
    {
        public static IServiceCollection AddAutomapper(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
