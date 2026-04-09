using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Infrastructure.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure.identity.services
{
    public record User (string username, string PasswordHash, string Name);
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext _context;
        
        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager; 
            this.roleManager = roleManager;
            _context = applicationDbContext;
        }

        public async Task<bool> CreateAsync(Domain.entities.User input)
        {
            var user = new ApplicationUser
            {
                Name = input.Name,
                UserName = input.Username,
                Email = input.Email,
            };

            var result = await _userManager.CreateAsync(user, input.Password);

            if(!result.Succeeded)
            {
                var error = result.Errors.Select(e => e.Description);
                throw new UnprocessableEntityException(error.ToString()!);
            }

            string role = "user";
            if(!await roleManager.RoleExistsAsync(role))
            {
                var newRole = new ApplicationRole
                {
                    Name = role
                };
                await roleManager.CreateAsync(newRole);
            }


            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<bool> ExistsEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return false;
            return true;
        }
    }
}
