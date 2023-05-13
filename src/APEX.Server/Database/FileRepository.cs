namespace APEX.Server.Database
{
    public class FileRepository
    {
        private readonly ApexDbContext _ctx;

        public FileRepository(ApexDbContext context)
        {
            _ctx = context;
        }
    }
}
