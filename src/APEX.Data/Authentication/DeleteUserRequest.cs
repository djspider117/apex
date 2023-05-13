using System.ComponentModel.DataAnnotations;

namespace APEX.Data.Authentication
{
    public class DeleteUserRequest
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }
    }
}
