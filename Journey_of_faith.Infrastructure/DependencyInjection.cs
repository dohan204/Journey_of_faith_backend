using Journey_of_faith.Infrastructure.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Journey_of_faith.Infrastructure
{
    public class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(IServiceCollection service, IConfiguration configuration)
        {

            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            });
            return service;
        } 
    }
}
