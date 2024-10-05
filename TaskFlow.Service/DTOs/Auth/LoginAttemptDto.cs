using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Auth
{
    public class LoginAttemptDto
    {
        [Required(ErrorMessage = "Credentials can't be empty")]
        public string Credentials { get; set; } = string.Empty;
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$", ErrorMessage = "Password must be complex.")]
        public string Password { get; set; } = string.Empty;
    }
}
