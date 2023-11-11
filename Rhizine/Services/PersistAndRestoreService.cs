using Microsoft.Extensions.Options;
using Rhizine.Contracts.Services;
using Rhizine.Models;
using System.Collections;
using System.IO;

namespace Rhizine.Services;

public class PersistAndRestoreService : IPersistAndRestoreService
{
    private readonly IFileService _fileService;
    private readonly AppConfig _appConfig;
    private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    private bool _singleFilePublish = true;
    protected readonly string _defaultAppPropertiesFileName = "AppProperties.json";
    protected string FolderPath => _singleFilePublish ? Directory.GetCurrentDirectory()
                                                        : Path.Combine(_localAppData, _appConfig.ConfigurationsFolder);
    protected string FileName => _appConfig?.AppPropertiesFileName ?? _defaultAppPropertiesFileName;
    public PersistAndRestoreService(IFileService fileService, IOptions<AppConfig> appConfig)
    {
        _fileService = fileService;
        _appConfig = appConfig?.Value ?? new AppConfig()
        {
            ConfigurationsFolder = Directory.GetCurrentDirectory(),
            AppPropertiesFileName = _defaultAppPropertiesFileName,
            PrivacyStatement = ""
        };
    }

    public void PersistData()
    {
        if (App.Current.Properties != null)
        {
            _fileService.Save(FolderPath, FileName, App.Current.Properties);
        }
    }

    public void RestoreData()
    {
        var properties = _fileService.Read<IDictionary>(FolderPath, FileName);
        if (properties != null)
        {
            foreach (DictionaryEntry property in properties)
            {
                App.Current.Properties.Add(property.Key, property.Value);
            }
        }
    }
}
