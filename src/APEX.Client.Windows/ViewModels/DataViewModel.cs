using APEX.Client.Windows.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Wpf.Ui.Common.Interfaces;

namespace APEX.Client.Windows.ViewModels
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly ISettingsService _settingsService;

        public DataViewModel(ISettingsService settingsService)
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
            _isInitialized = true;
        }
    }
}
