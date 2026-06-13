using System.ComponentModel.DataAnnotations;

namespace EKemit.APIs.DTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword),ErrorMessage ="The Password do not matched")]
        public string ConfirmNewPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
