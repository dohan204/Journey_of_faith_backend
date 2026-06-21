using Journey_of_faith.Application.behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Journey_of_faith.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssembly).Assembly);
                cfg.AddOpenBehavior(typeof(LoggingRequestBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehaviors<,>));
                cfg.AddOpenBehavior(typeof(CacheBehavior<,>));
                cfg.AddOpenBehavior(typeof(CacheInvalidBehavior<,>));
            });

            services.AddMemoryCache();
            return services;
        }
    }
}
