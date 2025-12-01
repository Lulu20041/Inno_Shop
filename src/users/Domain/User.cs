using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string HashPassword { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public int Role {  get; set; }

        public string PasswordResetToken { get; set; } = string.Empty;

        public DateTime? ResetTokenExpires { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsEmailConfirmed { get; set; } = false;
    }
}
