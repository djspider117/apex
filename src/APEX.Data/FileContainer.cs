namespace APEX.Data
{
    public record FileContainer(long Id, 
                                string Name, 
                                FileEntry RootFolder, 
                                long RootFolderId, 
                                List<FileEntry> FileEntries);
}