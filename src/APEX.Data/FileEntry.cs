namespace APEX.Data
{
    public class FileEntry
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
        public string Hash { get; set; }
        public List<FileEntry> Children { get; set; }

        public FileEntry ParentFile { get; set; }
        public long? ParentFileId { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        public ApexUser CreatedBy { get; set; }
        public long CreatedById { get; set; }

        public DateTimeOffset DateModified { get; set; }
        public ApexUser ModifiedBy { get; set; }
        public long ModifiedById { get; set; }
    }
}