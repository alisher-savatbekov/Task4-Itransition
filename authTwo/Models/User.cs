using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace authTwo.Models
{
    [Table("user",Schema = "AuthUser")]
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public string? DeletedAt { get; set; }
        public string? LastLoginTime { get; set; } 
        public string RegistrationDate { get; set; }
        public string Role { get; set; } = "User";
    }

}
