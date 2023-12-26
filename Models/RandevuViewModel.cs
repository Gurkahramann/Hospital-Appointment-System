using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspWebProgram.Models
{
    public class RandevuViewModel
    {
        public SelectList Doktorlar { get; set; }
        public SelectList Hastalar { get; set; }
        [Display(Name = "Doktor")]
        public int DoktorId { get; set; } // Assuming the ID is of type int

        [Display(Name = "Hasta")]
        public int HastaId { get; set; } // Assuming the ID is of type int
    }
}