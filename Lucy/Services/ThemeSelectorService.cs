using Lucy.Contracts.Services;
using Lucy.Helpers;

using Microsoft.UI.Xaml;
namespace Lucy.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    public ElementTheme Theme { get; set; } = ElementTheme.Default;

    private readonly ILocalSettingsService _localSettingsService;

    public ThemeSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();

        // Load avatar setting
        AvatarUrl = await LoadAvatarUrlFromSettingsAsync();

        await Task.CompletedTask;
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = Theme;

            TitleBarHelper.UpdateTitleBar(Theme);
        }

        await Task.CompletedTask;
    }

    private async Task<ElementTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
        {
            return cacheTheme;
        }

        return ElementTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(ElementTheme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }

    public string AvatarUrl
    {
        get;
        set;
    } = "";

    private const string SettingsKeyAvatarUrl = "AppAvatarUrl";

    public async Task SetAvatarUrlAsync(string avatarUrl)
    {
        AvatarUrl = avatarUrl;
        await _localSettingsService.SaveSettingAsync(SettingsKeyAvatarUrl, avatarUrl);
    }

    private async Task<string> LoadAvatarUrlFromSettingsAsync()
    {
        var url = await _localSettingsService.ReadSettingAsync<string>(SettingsKeyAvatarUrl);

        //Console.WriteLine($"load url get: {url}");

        url ??= "AvatarDefaultUrl".GetLocalized();

        return url;
    }
}
