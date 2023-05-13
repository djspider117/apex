using APEX.Client.Windows.Data;
using APEX.Data;
using GhostCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;

namespace APEX.Client.Windows.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SETTINGS_PATH = "appsettings.json";

        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private AppSettings _defaultSettings;

        public AppSettings Settings { get; private set; }

        public SettingsService()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                AllowTrailingCommas = true
            };
        }

        public async Task<ISafeTaskResult> LoadSettingsAsync()
        {
            try
            {
                if (!File.Exists(SETTINGS_PATH))
                {
                    CreateDefaults();

                    var json = JsonSerializer.Serialize(_defaultSettings, _jsonSerializerOptions);
                    await File.WriteAllTextAsync(SETTINGS_PATH, json);

                    Settings = _defaultSettings;
                }

                var settingsJson = await File.ReadAllTextAsync(SETTINGS_PATH);
                Settings = JsonSerializer.Deserialize<AppSettings>(settingsJson);

                return SafeTaskResult.Ok;
            }
            catch (Exception ex)
            {
                // TODO: logging
                Debug.WriteLine(ex);
                return new SafeTaskResult(ex.Message, ex);
            }
        }

        private void CreateDefaults()
        {
            _defaultSettings = new()
            {
                Theme = ThemeType.Light,
                SyncedFolders = new List<SyncedFolder>
                {
                    new SyncedFolder{ Name = "Defauly Sync Folder", Path = @"D:\APEXSync", SyncMode = FileSyncMode.Bidirectional }
                }
            };
        }

        public Task ApplySettingsAsync()
        {
            Theme.Apply(Settings.Theme);
            return Task.CompletedTask;
        }
    }
}
