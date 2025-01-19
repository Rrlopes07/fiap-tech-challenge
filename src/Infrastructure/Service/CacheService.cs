namespace Infrastructure.Service;

public class CacheService(IMemoryCache cache) : ICacheService
{
	private readonly IMemoryCache _cacheService = cache;

	public void Delete(string key)
		=> _cacheService.Remove(key);

	public object? Get(string key)
		=> _cacheService.TryGetValue(key, out var value) ? value : null;

	public void Set(string key, object value)
		=> _cacheService.Set(key, value);
}
