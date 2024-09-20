using System.ComponentModel.DataAnnotations;

namespace christenProject.Authontication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name isrequired")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}