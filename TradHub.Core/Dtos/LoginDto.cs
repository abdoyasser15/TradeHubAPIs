using System.ComponentModel.DataAnnotations;

namespace TradeHub.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,32}$",
            ErrorMessage = "Password must be 8-32 chars, include upper, lower, number, and symbol.")]
        public string Password { get; set; }
    }
}
