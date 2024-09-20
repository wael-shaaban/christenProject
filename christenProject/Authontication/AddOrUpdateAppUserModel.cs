using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace christenProject.Authontication
{
    public class AddOrUpdateAppUserModel
    {
        [Required(ErrorMessage = "First name is required"), MinLength(3)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required"), MinLength(3)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Photo is required"), MinLength(5)]
        public string ProfilePicture { get; set; }
        [Required(ErrorMessage = "User name is required"), MinLength(3)]
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required"),MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}