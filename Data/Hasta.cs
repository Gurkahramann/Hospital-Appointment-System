using System.ComponentModel.DataAnnotations;
namespace ASPWebProgramming.Data
{
    public class Hasta
    {
        [Key]
        public int HastaId { get; set; }
        public int HastaTc { get; set; }
        public string? HastaAd { get; set; }
        public string? HastaSoyad { get; set; }
        public string? HastaTel { get; set; }
        public string? HastaEposta { get; set; }
        public string? HastaCinsiyet { get; set; }
    }
}