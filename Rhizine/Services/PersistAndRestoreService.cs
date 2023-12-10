using Microsoft.Extensions.Options;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Collections;
using System.IO;

namespace Rhizine.Services;

public class PersistAndRestoreService : IPersistAndRestoreService
{
    private readonly IFileService _fileService;
    private readonly ILoggingService _loggingService;
    private readonly AppConfig _appConfig;

    private const string _defaultAppPropertiesFileName = "AppProperties.json";
    private readonly string _folderPath;
    private readonly string _fileName;

    public PersistAndRestoreService(IFileService fileService, IOptions<AppConfig> appConfigOptions, ILoggingService loggingService)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        _appConfig = appConfigOptions?.Value ?? new AppConfig()
        {
            ConfigurationsFolder = Directory.GetCurrentDirectory(),
            AppPropertiesFileName = _defaultAppPropertiesFileName,
            PrivacyStatement = ""
        };
        _folderPath = _appConfig.SingleFilePublish
            ? Directory.GetCurrentDirectory()
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _appConfig.ConfigurationsFolder);

        _fileName = _appConfig.AppPropertiesFileName ?? _defaultAppPropertiesFileName;
    }

    public void PersistData()
    {
        try
        {
            var properties = App.Current?.Properties;
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

    public void RestoreData()
    {
        try
        {
            var properties = _fileService.Read<IDictionary>(_folderPath, _fileName);
            if (properties != null)
            {
                foreach (DictionaryEntry property in properties)
                {
                    App.Current.Properties[property.Key] = property.Value;
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