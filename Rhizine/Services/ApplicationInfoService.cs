using System.Diagnostics;
using System.Reflection;
using Rhizine.Services.Interfaces;

namespace Rhizine.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public ApplicationInfoService()
    {
    }

    public Version GetVersion()
    {
        // Set the app version in Rhizine > Properties > Package > PackageVersion
        string assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        return new Version(version);
    }
}
