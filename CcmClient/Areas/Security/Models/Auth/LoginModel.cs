using System.ComponentModel.DataAnnotations;

namespace CcmClient.Areas.Security.Models.Auth
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
