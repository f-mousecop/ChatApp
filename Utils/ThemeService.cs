using ChatApp.ViewModels;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Media;

namespace ChatApp.Utils
{
    public class ThemeService : BaseViewModel
    {
        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (SetProperty(ref _isDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? BaseTheme.Dark : BaseTheme.Light));
                }
            }
        }



        public ThemeService()
        {
            var palette = new PaletteHelper();
            Theme theme = palette.GetTheme();

            IsDarkTheme = theme.GetBaseTheme() == BaseTheme.Dark;

            if (palette.GetThemeManager() is { } themeManager)
            {
                themeManager.ThemeChanged += (_, e) =>
                {
                    IsDarkTheme = e.NewTheme?.GetBaseTheme() == BaseTheme.Dark;
                };
            }
        }

        private static void ModifyTheme(Action<Theme> modificationAction)
        {
            var palette = new PaletteHelper();
            Theme theme = palette.GetTheme();

            modificationAction?.Invoke(theme);

            palette.SetTheme(theme);
        }
    }
}
