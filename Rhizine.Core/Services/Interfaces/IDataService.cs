namespace Rhizine.Core.Services.Interfaces;

public interface IDataService
{
    public Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default);

    public Task<T> PostAsync<T>(string url, T item, CancellationToken cancellationToken = default);

    public Task<T> PutAsync<T>(string url, T item, CancellationToken cancellationToken = default);

    public Task<T> DeleteAsync<T>(string url, CancellationToken cancellationToken = default);

    public Task<T> SendRequestAsync<T>(HttpMethod method, string url, CancellationToken cancellationToken);

    public Task<T> SendRequestAsync<T>(HttpMethod method, string url, T item, CancellationToken cancellationToken);

    public Task<T> ProcessResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken);

    public string EncryptString(string text);

    public string DecryptString(string cipherText);
}