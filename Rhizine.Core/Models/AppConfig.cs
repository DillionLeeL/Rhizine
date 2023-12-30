namespace Rhizine.Models;

// TODO
public class AppConfig
{
    public string ConfigurationsFolder { get; set; }

    public string AppPropertiesFileName { get; set; }

    public string PrivacyStatement { get; set; }

    public bool SingleFilePublish { get; set; } = true;

    public string UserFileName { get; set; }

    public string IdentityClientId { get; set; }

    public string IdentityCacheFileName { get; set; }

    public string IdentityCacheDirectoryName { get; set; }
}