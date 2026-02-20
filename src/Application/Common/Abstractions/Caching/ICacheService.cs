namespace Application.Common.Abstractions.Caching;
public interface ICacheService
{
    Task RemoveAsync(string key ,CancellationToken cancellationToken);
    Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken);
    Task SetDataAsync<T>(string key, T data, CancellationToken cancellationToken);
    Task SetDataAsync<T>(string key, T data, TimeSpan time, CancellationToken cancellationToken);
    Task<long> GetVersionAsync(string masterKey, CancellationToken cancellationToken);
    Task IncrementVersionAsync(string masterKey, CancellationToken cancellationToken);
}
