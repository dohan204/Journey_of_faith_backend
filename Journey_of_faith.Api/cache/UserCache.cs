using Journey_of_faith.Application.exceptions;
using Journey_of_faith.Domain.entities;
using Journey_of_faith.Domain.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Journey_of_faith.Api.cache;

public class UserCacheDataLoader : CacheDataLoader<Guid, User>
{
    private readonly IServiceProvider _serviceProvider;
    public UserCacheDataLoader(IServiceProvider serviceProvider, 
    DataLoaderOptions options) : base(options)
    {
        _serviceProvider = serviceProvider;
    }


    protected override async Task<User> LoadSingleAsync(
        Guid userId, 
        CancellationToken cancellationToken
    )
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        return await repo.GetUserByIdAsync(userId, cancellationToken);
    }
}