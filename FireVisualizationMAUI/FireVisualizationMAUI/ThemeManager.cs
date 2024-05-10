using Microsoft.Maui.Hosting;

namespace FireVisualizationMAUI;

public static class ThemeManager
{
    private static readonly IDictionary<string, ResourceDictionary> _themes = new Dictionary<string, ResourceDictionary>()
    {
        [nameof(Resources.Themes.Light)] = new Resources.Themes.Light(),
        [nameof(Resources.Themes.Dark)] = new Resources.Themes.Dark(),
    };

    public static string? SelectedTheme { get; set; } = nameof(Resources.Themes.Light);

    public static async Task SetTheme(string themeName)
    {
        if (SelectedTheme == themeName)
        {
            return;
        }

        var themeToBeApplied = _themes[themeName];

        await Task.Run(() =>
        {
            Application.Current.Dispatcher.Dispatch(() =>
            {
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(themeToBeApplied);
                SelectedTheme = themeName;
            });
        });
    }
}