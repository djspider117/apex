using Microsoft.AspNetCore.Identity;

namespace APEX.Data
{
    public class ApexUser : IdentityUser<long>
    {
        public ApexUser()
        {
        }

        public ApexUser(string userName) : base(userName)
        {
        }
    }

    public class ApexRole : IdentityRole<long>
    {
        public ApexRole()
        {
        }

        public ApexRole(string roleName) : base(roleName)
        {
        }
    }
}