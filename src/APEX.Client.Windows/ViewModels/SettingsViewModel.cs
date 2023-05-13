using APEX.Client.Windows.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Reflection;
using System.Windows.Input;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common.Interfaces;

namespace APEX.Client.Windows.ViewModels
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        private readonly ISettingsService _settingsService;

        [ObservableProperty]
        private string _appVersion = string.Empty;

        [ObservableProperty]
        private ThemeType _currentTheme = ThemeType.Unknown;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private void InitializeViewModel()
        {
            CurrentTheme = _settingsService.Settings.Theme;
            AppVersion = $"APEXSync - {GetAssemblyVersion()}";

            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ThemeType.Light)
                        break;

                    Theme.Apply(ThemeType.Light);
                    CurrentTheme = ThemeType.Light;

                    break;

                default:
                    if (CurrentTheme == ThemeType.Dark)
                        break;

                    Theme.Apply(ThemeType.Dark);
                    CurrentTheme = ThemeType.Dark;

                    break;
            }
        }
    }
}
