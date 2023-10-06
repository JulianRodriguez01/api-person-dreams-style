using System.ComponentModel.DataAnnotations;

namespace ApiPersons.Models
{
    public class UpdatePasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
