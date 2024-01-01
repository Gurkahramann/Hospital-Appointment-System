using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspWebProgram.Models
{
    public class RegisterDoktorModel
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        public string? DoktorAd { get; set; }
        
        [Required(ErrorMessage = "Soyad gereklidir.")]
        public string? DoktorSoyad { get; set; }
        
        [Required(ErrorMessage = "TC kimlik numarası gereklidir.")]
        [StringLength(11, ErrorMessage = "TCN 11 karakter olmalıdır.", MinimumLength = 11)]
        public string? DoktorTc { get; set; }
        
        [Required(ErrorMessage = "Cinsiyet gereklidir.")]
        public string? DoktorCinsiyet { get; set; }
        
        [Required(ErrorMessage = "Branş gereklidir.")]
        public string? DoktorBrans { get; set; } // 3 farklı branş olacak

        [Required(ErrorMessage = "Ana Bilim gereklidir.")]
        public string SelectedAnaBilimId { get; set; } // Seçilen Ana Bilim Id'si
        
        [Required(ErrorMessage = "Poliklinik gereklidir.")]
        public string SelectedPoliklinikId { get; set; } // Seçilen Poliklinik Id'si

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı gereklidir.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parola eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
