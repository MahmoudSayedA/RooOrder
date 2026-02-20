using Application.Common.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Infrastructure.Services.Caching;
public class InMemoryCacheService(IMemoryCache cache) : ICacheService
{
    private readonly IMemoryCache _cache = cache;

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        _cache.Remove(key);
        return Task.CompletedTask;
    }

    public async Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken)
    {
        T? value = default;
        await Task.Run(() => _cache.TryGetValue(key, out T? value));

        return value;
    }
    public async Task SetDataAsync<T>(string key, T data, CancellationToken cancellationToken)
    {

        await Task.Run(() => _cache.Set(key, data, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20),
            SlidingExpiration = TimeSpan.FromMinutes(5)
        }));

    }

    public async Task<long> GetVersionAsync(string masterKey, CancellationToken cancellationToken)
    {
        return await GetDataAsync<long>($"{masterKey}:version", cancellationToken);
    }
    public async Task IncrementVersionAsync(string masterKey, CancellationToken cancellationToken)
    {
        long version = await GetVersionAsync(masterKey, cancellationToken);
        version++;

        var key = $"{masterKey}:version";

        await SetDataAsync(key, version, cancellationToken);
    }
}
