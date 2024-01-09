﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Core.Models;
using Rhizine.Core.Models.Interfaces;
using Rhizine.WinUI.Helpers;
using System.Reflection;
using Windows.ApplicationModel;

namespace Rhizine.WinUI.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private AppTheme _appTheme;

    [ObservableProperty]
    private string _versionDescription;

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _appTheme = _themeSelectorService.GetCurrentTheme();
        _versionDescription = GetVersionDescription();
    }

    [RelayCommand]
    public void SwitchTheme(AppTheme theme)
    {
        _themeSelectorService.SetThemeAsync(theme);
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}