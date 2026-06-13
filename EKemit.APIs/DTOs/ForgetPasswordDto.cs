using System.ComponentModel.DataAnnotations;

namespace EKemit.APIs.DTOs
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
