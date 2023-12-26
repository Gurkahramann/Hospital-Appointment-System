using System.ComponentModel.DataAnnotations;
namespace AspWebProgramming.Data
{
    public class Hasta
    {
        [Key]
        public int HastaId { get; set; }
        public string? HastaTc { get; set; }
        public string? HastaAd { get; set; }
        public string? HastaSoyad { get; set; }
        public string? HastaTel { get; set; }
        public string? HastaEposta { get; set; }
        public string? HastaCinsiyet { get; set; }
        public string? HastaSifre { get; set; }
        public string? Rol { get; set; }

    }
}