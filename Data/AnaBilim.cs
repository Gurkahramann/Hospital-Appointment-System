using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspWebProgramming.Data
{
    public class AnaBilim
    {
        [Key]
        public int? AnaBilimId { get; set; }
        public string? AnaBilimAd { get; set; }

    }
}
