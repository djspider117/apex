using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace APEX.Data
{
    public class FileManifest
    {
        public string RootPath { get; set; }
        public DateTime GeneratedAt { get; set; }
        public TimeSpan GeneratedIn { get; set; }

        [JsonIgnore]
        public ConcurrentBag<FileManifestEntry> Entries { get; set; }
    }

    public class FileManifestReq
    {
        public string RootPath { get; set; }
        public DateTime GeneratedAt { get; set; }
        public TimeSpan GeneratedIn { get; set; }

        public List<FileManifestEntry> Entries { get; set; }
    }
}

