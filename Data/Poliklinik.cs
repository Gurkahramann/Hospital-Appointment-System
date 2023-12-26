using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspWebProgramming.Data
{
    public class Poliklinik
    {
        [Key]
        public int? PoliklinikId { get; set; }
        public string? PoliklinikAd { get; set; }

    }
}
