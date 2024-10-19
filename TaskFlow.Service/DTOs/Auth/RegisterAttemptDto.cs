using System.ComponentModel.DataAnnotations;
using TaskFlow.Service.DTOs;

namespace Service.DTOs.Auth
{
    public class RegisterAttemptDto : BaseDto
    {
        [Required(ErrorMessage = "Email can't be empty.")]
        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username can't be empty.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$", ErrorMessage = "Password must be complex.")]
        public string Password { get; set; } = string.Empty;
    }
}
