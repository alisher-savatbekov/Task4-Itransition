using System.ComponentModel.DataAnnotations;

namespace authTwo.ModelDTO
{
    public class LoginDTO
    {   
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email{ get; set; }
        [Required]
        public string Password { get; set; }
    }
}
