using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AspWebProgramming.Data;

namespace AspWebProgramming.Data
{
    public class Randevu
    {
        [Key]
        public int RandevuId { get; set; }
        public int HastaId { get; set; }

        [ForeignKey("HastaId")]
        public Hasta Hasta { get; set; } // Eğer bu alan için Required anotasyonu varsa kaldırın

        public int DoktorId { get; set; }

        [ForeignKey("DoktorId")]
        public Doktor Doktor { get; set; } 
        
        [DataType(DataType.Date)]
        public DateTime RandevuTarih { get; set; }
        
        [DataType(DataType.Time)]
        public TimeSpan RandevuSaati { get; set; }

    }

}