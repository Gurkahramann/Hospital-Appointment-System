using System.ComponentModel.DataAnnotations;
using AspWebProgramming.Data;

namespace AspWebProgramming.Data
{
    public class Randevu
    {
        [Key]
        public int RandevuId { get; set; }
        public int HastaId { get; set; }
        public Hasta Hasta { get; set; } = null!;
        public int DoktorId { get; set; }
        public Doktor Doktor { get; set; } = null!;
        public DateTime RandevuTarih { get; set; }
    }
}