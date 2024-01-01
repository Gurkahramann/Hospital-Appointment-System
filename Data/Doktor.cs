using System.ComponentModel.DataAnnotations;
namespace AspWebProgramming.Data
{
    public class Doktor
    {
        [Key]
        public int DoktorId { get; set; }
        public string? DoktorAd { get; set; }
        public string? DoktorSoyad { get; set; }
        public string? DoktorTc { get; set; }
         public string DoktorAdSoyad
        {
            get
            {
                return this.DoktorAd + " " + this.DoktorSoyad;
            }
        }
        public string? DoktorCinsiyet { get; set; }
        public string? DoktorSifre { get; set; }
        public string? DoktorBrans { get; set; }
        public string? DoktorAnaBilim { get; set; }
        public string? DoktorPoliklinik { get; set; }
        public string? Rol { get; set; }

    }
}