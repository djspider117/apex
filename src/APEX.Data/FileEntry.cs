namespace APEX.Data
{
    public record FileEntry(long Id, 
                            string Name, 
                            byte IsFolder, 
                            string MimeType, 
                            long Size, 
                            string Hash, 
                            List<FileEntry> Children, 

                            FileEntry ParentFile, 
                            long ParentFileId, 

                            DateTimeOffset DateCreated, 
                            ApexUser CreatedBy, 
                            long CreatedById, 

                            DateTimeOffset DateModified, 
                            ApexUser ModifiedBy, 
                            long ModifiedById, 

                            FileContainer ParentContainer, 
                            long? ParentContainerId);
}