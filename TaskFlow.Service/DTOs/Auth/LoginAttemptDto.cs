using System.ComponentModel.DataAnnotations;
using TaskFlow.Service.DTOs;

namespace Service.DTOs.Auth
{
    public class LoginAttemptDto : BaseDto
    {
        [Required(ErrorMessage = "Credentials can't be empty")]
        public string Credentials { get; set; } = string.Empty;
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$", ErrorMessage = "Password must be complex.")]
        public string Password { get; set; } = string.Empty;
    }
}
