using System.ComponentModel.DataAnnotations;
namespace ASPWebProgramming.Data
{
    public class Admin
    {
        [Key]
        public int AdminId {get; set;}
        public string? KullaniciAdi { get; set; }
        public string? Sifre { get; set; }
    }
}