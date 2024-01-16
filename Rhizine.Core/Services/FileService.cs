using Rhizine.Core.Services.Interfaces;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Rhizine.Core.Services;

public class FileService(ILoggingService loggingService) : IFileService
{
    private readonly ILoggingService _loggingService = loggingService;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    /// <summary>
    /// Synchronously reads and deserializes the JSON content from a file into an object of type T.
    /// </summary>
    /// <param name="folderPath">The folder path where the file is located.</param>
    /// <param name="fileName">The name of the file to read.</param>
    /// <returns>The deserialized object of type T, or default(T) if the file does not exist.</returns>
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (!File.Exists(path))
        {
            return default;
        }

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
    }

    /// <summary>
    /// Asynchronously reads and deserializes the JSON content from a file into an object of type T.
    /// </summary>
    /// <param name="folderPath">The folder path where the file is located.</param>
    /// <param name="fileName">The name of the file to read.</param>
    /// <returns>A task representing the asynchronous operation, with the deserialized object of type T as the result.</returns>
    public async Task<T> ReadAsync<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (!File.Exists(path))
        {
            return default;
        }

        try
        {
            var json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Failed to read file {path}");
            throw;
        }
    }

    /// <summary>
    /// Synchronously serializes an object and writes it as JSON to a file.
    /// </summary>
    /// <param name="folderPath">The folder path where to save the file.</param>
    /// <param name="fileName">The name of the file to be saved.</param>
    /// <param name="content">The object to serialize and save.</param>
    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonSerializer.Serialize(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    /// <summary>
    /// Asynchronously serializes an object and writes it as JSON to a file.
    /// </summary>
    /// <param name="folderPath">The folder path where to save the file.</param>
    /// <param name="fileName">The name of the file to be saved.</param>
    /// <param name="content">The object to serialize and save.</param>
    /// <returns>A task representing the asynchronous save operation.</returns>
    public async Task SaveAsync<T>(string folderPath, string fileName, T content)
    {
        if (string.IsNullOrWhiteSpace(folderPath) || string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Folder path and file name must be provided.");
        }

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonSerializer.Serialize(content, _jsonSerializerOptions);
        var path = Path.Combine(folderPath, fileName);
        try
        {
            await File.WriteAllTextAsync(path, fileContent, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Failed to save file {path}");
            throw;
        }
    }

    /// <summary>
    /// Deletes a specified file from a folder.
    /// </summary>
    /// <param name="folderPath">The folder path where the file is located.</param>
    /// <param name="fileName">The name of the file to be deleted.</param>
    public void Delete(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Opens a file for reading asynchronously, assuming it's an embedded resource in the current assembly.
    /// </summary>
    /// <param name="path">The relative path to the embedded resource.</param>
    /// <returns>A Stream of the opened file.</returns>
    public async Task<Stream> OpenForReadAsync(string path)
    {
        return await GetEmbeddedFileStreamAsync(GetType(), path);
    }

    /// <summary>
    /// Opens a file for reading asynchronously, assuming it's an embedded resource in a specified assembly.
    /// </summary>
    /// <param name="path">The relative path to the embedded resource.</param>
    /// <param name="assemblyType">The type from which to infer the assembly.</param>
    /// <returns>A Stream of the opened file.</returns>
    public async Task<Stream> OpenForReadAsync(string path, Type assemblyType)
    {
        return await GetEmbeddedFileStreamAsync(assemblyType, path);
    }

    /// <summary>
    /// Reads all text from an embedded resource file asynchronously.
    /// </summary>
    /// <param name="path">The relative path to the embedded resource.</param>
    /// <returns>A string containing all text from the file.</returns>
    public async Task<string> ReadAllTextAsync(string path)
    {
        await using var stream = await OpenForReadAsync(path);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Reads all text from an embedded resource file asynchronously, from a specified assembly.
    /// </summary>
    /// <param name="path">The relative path to the embedded resource.</param>
    /// <param name="assemblyType">The type from which to infer the assembly.</param>
    /// <returns>A string containing all text from the file.</returns>
    public async Task<string> ReadAllTextAsync(string path, Type assemblyType)
    {
        await using var stream = await OpenForReadAsync(path, assemblyType);
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Reads all bytes from an embedded resource file asynchronously.
    /// </summary>
    /// <param name="path">The relative path to the embedded resource.</param>
    /// <returns>A byte array containing the file's contents.</returns>
    public async Task<byte[]> ReadAllBytesAsync(string path)
    {
        await using var stream = await OpenForReadAsync(path);
        await using var reader = new MemoryStream();
        await stream.CopyToAsync(reader);
        return reader.ToArray();
    }

    /// <summary>
    /// Reads all bytes from an embedded resource file asynchronously, from a specified assembly.
    /// </summary>
    /// <param name="path">The relative path to the embedded resource.</param>
    /// <param name="assemblyType">The type from which to infer the assembly.</param>
    /// <returns>A byte array containing the file's contents.</returns>
    public async Task<byte[]> ReadAllBytesAsync(string path, Type assemblyType)
    {
        await using var stream = await OpenForReadAsync(path, assemblyType);
        await using var reader = new MemoryStream();
        await stream.CopyToAsync(reader);
        return reader.ToArray();
    }

    /// <summary>
    /// Opens a file for writing asynchronously, creating or overwriting the file.
    /// </summary>
    /// <param name="path">The path of the file to be opened.</param>
    /// <returns>A Stream to the opened file for writing.</returns>
    public async Task<Stream> OpenForWriteAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));
        }

        await Task.Yield();
        var directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            // FileMode.Create will create the file if it doesn't exist or overwrite it if it does
            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Failed to open file {path}");
            throw;
        }
    }

    /// <summary>
    /// Opens a file for writing asynchronously in a specified assembly's directory, creating or overwriting the file.
    /// </summary>
    /// <param name="path">The relative path of the file.</param>
    /// <param name="assemblyType">The type from which to infer the assembly's directory.</param>
    /// <returns>A Stream to the opened file for writing.</returns>
    public async Task<Stream> OpenForWriteAsync(string path, Type assemblyType)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or whitespace.", nameof(path));
        }
        ArgumentNullException.ThrowIfNull(assemblyType);

        // Get the directory of the assembly and combine it with the provided path
        string assemblyLocation = Path.GetDirectoryName(assemblyType.Assembly.Location);
        string fullPath = Path.Combine(assemblyLocation, path);

        var directory = Path.GetDirectoryName(fullPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            return new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Failed to open file {fullPath}");
            throw;
        }
    }

    /// <summary>
    /// Writes all text to a file asynchronously, creating or overwriting the file.
    /// </summary>
    /// <param name="path">The path of the file where the text will be written.</param>
    /// <param name="contents">The text to write to the file.</param>
    public async Task WriteAllTextAsync(string path, string contents)
    {
        await using var stream = await OpenForWriteAsync(path);
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(contents);
    }

    /// <summary>
    /// Writes all text to a file asynchronously in a specified assembly's directory, creating or overwriting the file.
    /// </summary>
    /// <param name="path">The relative path of the file.</param>
    /// <param name="contents">The text to write to the file.</param>
    /// <param name="assemblyType">The type from which to infer the assembly's directory.</param>
    public async Task WriteAllTextAsync(string path, string contents, Type assemblyType)
    {
        await using var stream = await OpenForWriteAsync(path, assemblyType);
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(contents);
    }

    /// <summary>
    /// Retrieves an embedded resource file stream asynchronously.
    /// </summary>
    /// <param name="assemblyType">The type from which to infer the assembly.</param>
    /// <param name="fileName">The name of the file to retrieve.</param>
    /// <returns>A Stream of the embedded resource file.</returns>
    public async Task<Stream?> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
    {
        await Task.Yield();

        var manifestName = Array.Find(assemblyType.GetTypeInfo().Assembly
            .GetManifestResourceNames(), n => n.EndsWith(fileName.Replace(" ", "_").Replace("\\", ".").Replace("/", "."), StringComparison.OrdinalIgnoreCase));

        return manifestName == null
            ? throw new InvalidOperationException($"Failed to find resource [{fileName}]")
            : assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName);
    }
}