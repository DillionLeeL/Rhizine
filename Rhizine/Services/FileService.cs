using Rhizine.Services.Interfaces;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Rhizine.Services;

public class FileService : IFileService
{
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json);
        }

        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonSerializer.Serialize(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }
    public async Task<Stream> OpenForReadAsync(string path)
    {
        return await GetEmbeddedFileStreamAsync(GetType(), path);
    }


    private static async Task<Stream> GetEmbeddedFileStreamAsync(Type assemblyType, string fileName)
    {
        await Task.Yield();

        var manifestName = assemblyType.GetTypeInfo().Assembly
            .GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith(fileName.Replace(" ", "_").Replace("\\", ".").Replace("/", "."), StringComparison.OrdinalIgnoreCase));

        if (manifestName == null)
        {
            throw new InvalidOperationException($"Failed to find resource [{fileName}]");
        }

        return assemblyType.GetTypeInfo().Assembly.GetManifestResourceStream(manifestName);
    }
}
