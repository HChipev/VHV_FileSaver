using System.ComponentModel.DataAnnotations;

namespace VHV_FileSaver.ViewModels.UserModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required!")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}