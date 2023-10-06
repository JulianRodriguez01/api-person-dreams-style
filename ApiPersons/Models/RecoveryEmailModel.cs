using System.ComponentModel.DataAnnotations;

namespace ApiPersons.Models
{
    public class RecoveryEmailModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
