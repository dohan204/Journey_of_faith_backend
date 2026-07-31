using Journey_of_faith.Application.common.interfaces;
using Journey_of_faith.Infrastructure.context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationTesting
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<ICurrentUserService> CurrentUserServiceMock { get; } = new Mock<ICurrentUserService>();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Override Firebase config to empty so it won't try to init
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "Firebase:CredentialFilePath", "" },
                    { "ConnectionStrings:Connection", "" },
                    { "Token:Key", "ThisIsATestSecretKeyForIntegrationTesting1234567890" },
                    { "Token:Issuer", "TestIssuer" },
                    { "Token:Audience", "TestAudience" },
                    { "Db:SchemaName", "dbo" }
                });
            });

            builder.ConfigureServices(services =>
            {
                // Remove ALL EF Core / DbContext related services to avoid dual-provider conflict
                var efCoreServiceTypes = new[]
                {
                    typeof(DbContextOptions<ApplicationDbContext>),
                    typeof(DbContextOptions),
                };

                // Remove descriptors that match these types
                foreach (var serviceType in efCoreServiceTypes)
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == serviceType);
                    if (descriptor != null)
                        services.Remove(descriptor);
                }

                // Remove ALL descriptors whose ServiceType lives in the EF Core SqlServer assembly
                // This catches SqlServerOptionsExtension, relational services, etc.
                var descriptorsToRemove = services
                    .Where(d =>
                        d.ServiceType.FullName != null &&
                        (d.ServiceType.FullName.Contains("SqlServer") ||
                         d.ImplementationType?.FullName?.Contains("SqlServer") == true))
                    .ToList();

                foreach (var d in descriptorsToRemove)
                {
                    services.Remove(d);
                }

                // Also remove the ApplicationDbContext registration itself
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ApplicationDbContext));
                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                // Re-add with InMemory
                var dbName = "IntegrationTestDb_" + Guid.NewGuid().ToString();
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });

                // Mock ICurrentUserService
                CurrentUserServiceMock.Setup(s => s.UserId).Returns(Guid.NewGuid().ToString());
                CurrentUserServiceMock.Setup(s => s.GetRoleUserName).Returns("admin");

                var currentUserServiceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICurrentUserService));
                if (currentUserServiceDescriptor != null)
                {
                    services.Remove(currentUserServiceDescriptor);
                }
                services.AddScoped(_ => CurrentUserServiceMock.Object);
            });
        }
    }
}
