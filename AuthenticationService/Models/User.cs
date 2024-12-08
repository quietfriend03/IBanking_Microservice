using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public double Sodu { get; set; }
    }
}
