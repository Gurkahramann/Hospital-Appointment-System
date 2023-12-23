using Microsoft.AspNetCore.Identity;

namespace AspWebProgram.Models
{
    public class AppUser:IdentityUser
    {
        public string? HastaTc { get; set; }
        public string? HastaAd { get; set; }
        public string? HastaSoyad { get; set; }
        public string? HastaTel { get; set; }
        public string? HastaCinsiyet { get; set; }
        public string? Password{get; set;}
    }
}