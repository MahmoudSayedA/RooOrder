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
        var option = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
        };
        await Task.Run(() => _cache.Set(key, data, option));

    }

    public async Task SetDataAsync<T>(string key, T data, TimeSpan time, CancellationToken cancellationToken)
    {
        var option = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = time / 2,
            AbsoluteExpiration = DateTimeOffset.Now.Add(time)
        };
        await Task.Run(() => _cache.Set(key, data, option));
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


/*
 * cache invalidation

Scenario                            | Sliding       | Absolute      | Why this ratio?
------------------------------------|---------------|---------------|----------------
User profile / session-like data    | 3–10 minutes  | 20–60 minutes | "Frequent reads during a session, but force refresh after ~30–60 min"
Product details / catalog items     | 5–15 minutes  | 1–4 hours     | "Less volatile, but still refresh periodically"
Configuration / settings            | 10–30 minutes | 12–24 hours   | "Rarely changes,  but absolute prevents eternal cache"
Very volatile data(..stock price)   | 10–60 seconds | 5–30 minutes  | Sliding almost never triggers long life
*/