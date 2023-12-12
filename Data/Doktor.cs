using System.ComponentModel.DataAnnotations;
namespace ASPWebProgramming.Data
{
    public class Doktor
    {
        [Key]
        public int DoktorId { get; set; }
        public string? DoktorAd {get; set;}
        public string? DoktorSoyad {get; set;}
        public int DoktorTc {get; set;}
        public string? DoktorCinsiyet {get; set;}
    }
}