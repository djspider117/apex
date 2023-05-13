namespace APEX.Data
{
    public class FileManifestEntry : IEquatable<FileManifestEntry>
    {
        public string RelativePath { get; set; }
        //public DateTime LastModifiedUtc { get; set; }
        public string Checksum { get; set; }
        public long Id { get; set; }

        public override string ToString() => $"{RelativePath}";

        public bool Equals(FileManifestEntry other)
        {
            return other.Checksum == Checksum;
        }
    }
}

