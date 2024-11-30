using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace authTwo.ModelDTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }  
        public string Password { get; set; }  
    }

}
