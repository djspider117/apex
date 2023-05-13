using APEX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;

namespace APEX.Client.Windows.Data
{
    public class AppSettings
    {
        public List<SyncedFolder> SyncedFolders { get; set; }
        public ThemeType Theme { get; set; }
    }

    public class SyncedFolder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FileSyncMode SyncMode { get; set; }
        public long? RemoteContainerId { get; set; }
    }



}
