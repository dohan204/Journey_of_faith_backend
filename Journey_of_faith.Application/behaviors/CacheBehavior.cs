using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Journey_of_faith.Application.common.interfaces;
namespace Journey_of_faith.Application.behaviors;

public class CacheBehavior<TRequest, TResponse>
    (IMemoryCache memoryCache, IConfiguration configuration) 
    : IPipelineBehavior<TRequest, TResponse> 
    where TRequest: IRequest<TResponse>, ICacheableQuery
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if(request.BypassCache)
        {
            return await next();
        }

        // lấy ra dữ liệu từ cache 
        if(memoryCache.TryGetValue(request.CacheKey, out TResponse? data) && data is not null)
        {
            return data;
        }

        // cache miss: goi toi middleware tiep theo de lay du lieu
        var response = await next();
        // đọc cau hinh tu appsettings;
        double slidingMinutes = configuration.GetValue<double>(configuration["CacheSettings:SlidingExpiration"]!, 10);
        double absoluteExpirationRelativeToNow = configuration.GetValue<double>(configuration["CacheSettings:AbsoluteExpirationRelativeToNow"]!, 1);
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(slidingMinutes),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(absoluteExpirationRelativeToNow)
        };
        // set cache
        memoryCache.Set(request.CacheKey, response, options);

        return response;

    }
}