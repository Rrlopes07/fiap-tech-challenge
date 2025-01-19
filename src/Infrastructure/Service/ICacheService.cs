namespace Infrastructure.Service;

public interface ICacheService
{
	object? Get(string key);
	void Set(string key, object value);
	void Delete(string key);
}
