using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Lucy.Contracts.Services;
using Lucy.Helpers;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel;

namespace Lucy.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }

    [ObservableProperty]
    private bool isBackEnabled;

    public ICommand GoPageMainCommand
    {
        get;
    }

    public INavigationService NavigationService
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, INavigationService navigationService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });


        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;

        GoPageMainCommand = new RelayCommand(OnGoPageMain);
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

        return $"🌕{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    private void OnNavigated(object sender, NavigationEventArgs e) => IsBackEnabled = NavigationService.CanGoBack;

    private void OnGoPageMain() => NavigationService.NavigateTo(typeof(MainViewModel).FullName!);
}
