using APEX.Client.Windows.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace APEX.Client.Windows.Views.Windows
{
    public partial class LoadingWindow : Window
    {
        private readonly ISettingsService _settingsService;

        public LoadingWindow(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            InitializeComponent();
        }

        public async Task LoadAsync()
        {
            var loadSettingsResult = await _settingsService.LoadSettingsAsync();
            if (loadSettingsResult.IsFaulted)
            {
                MessageBox.Show(loadSettingsResult.FailReason, "Failed to load application settings.");
                Application.Current.Shutdown();
            }

            await _settingsService.ApplySettingsAsync();
        }
    }
}
