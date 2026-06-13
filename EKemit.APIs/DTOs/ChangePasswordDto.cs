using System.ComponentModel.DataAnnotations;

namespace EKemit.APIs.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string oldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword),ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
