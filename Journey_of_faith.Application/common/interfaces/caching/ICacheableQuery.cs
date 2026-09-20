namespace Journey_of_faith.Application.common.interfaces;

public interface ICacheableQuery
{
    string CacheKey {get; }
    bool BypassCache {get; }
}