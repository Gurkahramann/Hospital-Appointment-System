using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspWebProgram.Models
{
    public class RandevuEditViewModel
    {
        public int RandevuId { get; set; }

        [Display(Name = "Hasta")]
        public int HastaId { get; set; }

        [Display(Name = "Doktor")]
        public int DoktorId { get; set; }

        [Display(Name = "Randevu Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime RandevuTarih { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan RandevuSaati { get; set; }
    }
}