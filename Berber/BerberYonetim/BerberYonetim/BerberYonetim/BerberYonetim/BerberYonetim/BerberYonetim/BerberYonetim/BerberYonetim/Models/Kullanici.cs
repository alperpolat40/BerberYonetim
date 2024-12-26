using System.ComponentModel.DataAnnotations;

namespace BerberYonetim.Models
{
    public class Kullanici
    {
        [Key]
        public int Id { get; set; } // Kullanıcı ID'si

        [Required(ErrorMessage = "Ad Soyad alanı boş bırakılamaz.")]
        public string AdSoyad { get; set; } // Kullanıcı adı

        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } // Kullanıcı e-posta

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Şifre 3 ile 10 karakter arasında olmalıdır.")]
        public string Sifre { get; set; } // Kullanıcı şifresi
    }
}
