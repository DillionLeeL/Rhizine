using Microsoft.Extensions.Options;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Rhizine.Core.Services;

public class LocalSettingsService : ILocalSettingsService
{
    private const string _defaultApplicationDataFolder = "Rhizine/ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";

    private readonly IFileService _fileService;
    private readonly LocalSettingsOptions _options;

    private readonly string _localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private readonly string _applicationDataFolder;
    private readonly string _localsettingsFile;

    private readonly ConcurrentDictionary<string, object> _settings = new ConcurrentDictionary<string, object>();
    private bool _isInitialized;

    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public LocalSettingsService(IFileService fileService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            var fileContents = await _fileService.ReadAsync<string>(_applicationDataFolder, _localsettingsFile);
            if (!string.IsNullOrEmpty(fileContents))
            {
                try
                {
                    var settings = JsonSerializer.Deserialize<IDictionary<string, object>>(fileContents, _jsonOptions);
                    if (settings != null)
                    {
                        foreach (var setting in settings)
                        {
                            _settings[setting.Key] = setting.Value;
                        }
                    }
                }
                catch (JsonException)
                {
                    // Handle or log the exception as necessary
                }
            }

            _isInitialized = true;
        }
    }

    public async Task<T?> ReadSettingAsync<T>(string key)
    {
        await InitializeAsync();

        if (_settings.TryGetValue(key, out var obj) && obj is JsonElement element)
        {
            try
            {
                return element.Deserialize<T>(_jsonOptions);
            }
            catch (JsonException)
            {
                // Handle or log the exception as necessary
                return default;
            }
        }

        return default;
    }

    public async Task SaveSettingAsync<T>(string key, T value)
    {
        await InitializeAsync();

        var json = JsonSerializer.Serialize(value, _jsonOptions);
        _settings[key] = json;

        var serializedSettings = JsonSerializer.Serialize(_settings, _jsonOptions);
        await _fileService.SaveAsync(_applicationDataFolder, _localsettingsFile, serializedSettings);
    }
}