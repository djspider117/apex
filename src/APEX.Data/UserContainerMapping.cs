namespace APEX.Data
{
    public class UserContainerMapping
    {
        public long Id { get; set; }
        public ApexUser User { get; set; }
        public long UserId { get; set; }
        public FileContainer Container { get; set; }
        public long ContainerId { get; set; }
    }
}