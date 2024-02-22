using Microsoft.AspNetCore.Identity;

namespace Question1.Models
{
    public class Person : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
