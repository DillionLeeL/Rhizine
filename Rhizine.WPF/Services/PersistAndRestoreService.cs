using Microsoft.Extensions.Options;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WPF.Models;
using System.Collections;
using System.IO;

namespace Rhizine.WPF.Services;

/// <summary>
/// Provides services for persisting and restoring application data.
/// </summary>
public class PersistAndRestoreService : IPersistAndRestoreService
{
    private readonly IFileService _fileService;
    private readonly ILoggingService _loggingService;

    private const string _defaultAppPropertiesFileName = "AppProperties.json";
    private readonly string _folderPath;
    private readonly string _fileName;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistAndRestoreService"/> class.
    /// </summary>
    /// <param name="fileService">Service for handling file operations.</param>
    /// <param name="appConfigOptions">Configuration options for the application.</param>
    /// <param name="loggingService">Service for logging errors and information.</param>
    /// <exception cref="ArgumentNullException">Thrown if fileService or loggingService is null.</exception>
    public PersistAndRestoreService(IFileService fileService, IOptions<AppConfig> appConfigOptions, ILoggingService loggingService)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        var appConfig = appConfigOptions?.Value ?? new AppConfig()
        {
            ConfigurationsFolder = Directory.GetCurrentDirectory(),
            AppPropertiesFileName = _defaultAppPropertiesFileName,
            PrivacyStatement = ""
        };
        _folderPath = appConfig.SingleFilePublish
            ? Directory.GetCurrentDirectory()
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appConfig.ConfigurationsFolder);

        _fileName = appConfig.AppPropertiesFileName ?? _defaultAppPropertiesFileName;
    }

    /// <summary>
    /// Persists application data to a file.
    /// </summary>
    /// <remarks>
    /// The method saves the current state of the application properties to a file.
    /// In case of any exceptions, logs the error and rethrows it.
    /// </remarks>
    public void PersistData()
    {
        try
        {
            var properties = System.Windows.Application.Current?.Properties;
            if (properties != null)
            {
                _fileService.Save(_folderPath, _fileName, properties);
            }
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error persisting data.");
            throw;
        }
    }

    /// <summary>
    /// Restores application data from a file.
    /// </summary>
    /// <remarks>
    /// The method reads application properties from a file and restores them.
    /// In case of any exceptions, logs the error and rethrows it.
    /// </remarks>
    public void RestoreData()
    {
        try
        {
            var properties = _fileService.Read<IDictionary>(_folderPath, _fileName);
            if (properties != null)
            {
                foreach (DictionaryEntry property in properties)
                {
                    System.Windows.Application.Current.Properties[property.Key] = property.Value;
                }
            }
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error restoring data.");
            throw;
        }
    }
}