using Microsoft.UI.Xaml;

namespace Lucy.Contracts.Services;

public interface IThemeSelectorService
{
    ElementTheme Theme
    {
        get;
    }

    string AvatarUrl
    {
        get; 
    }

    Task InitializeAsync();

    Task SetThemeAsync(ElementTheme theme);

    Task SetRequestedThemeAsync();

    Task SetAvatarUrlAsync(string avatarUrl);
}
