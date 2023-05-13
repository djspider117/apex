namespace APEX.Data
{
    public class FileContainer
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public FileEntry RootFolder { get; set; }
        public long RootFolderId { get; set; }

        public List<FileEntry> FileEntries { get; set; }
    }
}