using System.ComponentModel.DataAnnotations;

namespace NinjaApp.Persistence.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}