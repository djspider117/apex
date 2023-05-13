namespace APEX.Data
{
    public class SyncedFolder
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public FileSyncMode SyncMode { get; set; }
        public long? RemoteContainerId { get; set; }
    }



}
