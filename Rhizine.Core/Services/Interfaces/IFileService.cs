namespace Rhizine.Core.Services.Interfaces;

public interface IFileService
{
    T Read<T>(string folderPath, string fileName);

    Task<T> ReadAsync<T>(string folderPath, string fileName);

    void Save<T>(string folderPath, string fileName, T content);

    Task SaveAsync<T>(string folderPath, string fileName, T content);

    void Delete(string folderPath, string fileName);

    Task<Stream> OpenForReadAsync(string path);

    Task<Stream> OpenForReadAsync(string path, Type assemblyType);

    Task<string> ReadAllTextAsync(string path);

    Task<string> ReadAllTextAsync(string path, Type assemblyType);

    Task<byte[]> ReadAllBytesAsync(string path);

    Task<byte[]> ReadAllBytesAsync(string path, Type assemblyType);

    Task<Stream> OpenForWriteAsync(string path);

    Task<Stream> OpenForWriteAsync(string path, Type assemblyType);

    Task WriteAllTextAsync(string path, string contents);

    Task WriteAllTextAsync(string path, string contents, Type assemblyType);

    Task<Stream?> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName);
}