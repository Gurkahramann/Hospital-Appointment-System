using System.ComponentModel.DataAnnotations;

namespace AspWebProgram.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "TC kimlik numarası gereklidir.")]
        [StringLength(11, ErrorMessage = "TCN 11 karakter olmalıdır.", MinimumLength = 11)]
        public string HastaTc { get; set; }

        [Required(ErrorMessage = "Ad gereklidir.")]
        public string HastaAd { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        public string HastaSoyad { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        public string HastaTel { get; set; }

        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string HastaEposta { get; set; }

        [Required(ErrorMessage = "Cinsiyet gereklidir.")]
        public string HastaCinsiyet { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Parola eşleşmiyor.")]
        public string ConfirmPassword { get; set; } = string.Empty;


    }
}
