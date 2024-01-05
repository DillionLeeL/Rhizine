using Microsoft.Extensions.Options;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Rhizine.Core.Services;

/// <summary>
/// Service for managing local settings in a JSON file.
/// </summary>
/// <remarks>
/// This class provides functionalities to read from and save settings to a local JSON file.
/// It uses <see cref="IFileService"/> for file operations and <see cref="ILoggingService"/> for logging errors.
/// Settings are stored in a concurrent dictionary and are lazily loaded upon first use.
/// </remarks>
public class LocalSettingsService : ILocalSettingsService
{
    // Constants for default file paths and names.
    private const string _defaultApplicationDataFolder = "Rhizine/ApplicationData";
    private const string _defaultLocalSettingsFile = "LocalSettings.json";

    // Dependencies for file operations and logging.
    private readonly IFileService _fileService;
    private readonly ILoggingService _loggingService;
    private readonly LocalSettingsOptions _options;

    // Paths for application data and settings file.
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

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalSettingsService"/> class.
    /// </summary>
    /// <param name="fileService">File service for file operations.</param>
    /// <param name="loggingService">Logging service for error logging.</param>
    /// <param name="options">Options to customize file paths.</param>
    public LocalSettingsService(IFileService fileService, ILoggingService loggingService, IOptions<LocalSettingsOptions> options)
    {
        _fileService = fileService;
        _loggingService = loggingService;
        _options = options.Value;

        _applicationDataFolder = Path.Combine(_localApplicationData, _options.ApplicationDataFolder ?? _defaultApplicationDataFolder);
        _localsettingsFile = _options.LocalSettingsFile ?? _defaultLocalSettingsFile;
    }

    /// <summary>
    /// Initializes the service asynchronously by loading settings from file.
    /// </summary>
    /// <remarks>
    /// This method checks if the service is already initialized to avoid redundant operations.
    /// It reads the settings file, deserializes it, and populates the settings dictionary.
    /// </remarks>
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
                    _loggingService.LogError($"Failed to deserialize local settings file: {_localsettingsFile}");
                }
            }

            _isInitialized = true;
        }
    }

    /// <summary>
    /// Reads a setting asynchronously and deserializes it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the setting to.</typeparam>
    /// <param name="key">The key of the setting to read.</param>
    /// <returns>The deserialized setting value or default if not found or deserialization fails.</returns>
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
                _loggingService.LogError($"Failed to deserialize local setting: {key}");
                return default;
            }
        }

        return default;
    }

    /// <summary>
    /// Saves a setting asynchronously by serializing and writing it to the settings file.
    /// </summary>
    /// <typeparam name="T">The type of the setting to save.</typeparam>
    /// <param name="key">The key of the setting to save.</param>
    /// <param name="value">The setting value to save.</param>
    public async Task SaveSettingAsync<T>(string key, T value)
    {
        await InitializeAsync();

        var json = JsonSerializer.Serialize(value, _jsonOptions);
        _settings[key] = json;

        var serializedSettings = JsonSerializer.Serialize(_settings, _jsonOptions);
        await _fileService.SaveAsync(_applicationDataFolder, _localsettingsFile, serializedSettings);
    }
}