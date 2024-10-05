using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Auth
{
    public class RegisterAttemptDto
    {
        [Required(ErrorMessage = "Email can't be empty.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username can't be empty.")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$", ErrorMessage = "Password must be complex.")]
        public string Password { get; set; } = string.Empty;
    }
}
