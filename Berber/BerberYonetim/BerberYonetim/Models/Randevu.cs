using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerberYonetim.Models
{
    public class Randevu
    {
        [Key]
        public int Id { get; set; } // Randevu ID

        [Required]
        public int KullaniciId { get; set; } // Kullanıcı ID
        [ForeignKey("KullaniciId")]
        public Kullanici Kullanici { get; set; } // Kullanıcı detayı

        [Required]
        public int KuaforId { get; set; } // Kuaför ID
        [ForeignKey("KuaforId")]
        public Kuafor Kuafor { get; set; } // Kuaför detayı

        [Required]
        public int IslemId { get; set; } // İşlem ID
        [ForeignKey("IslemId")]
        public Islem Islem { get; set; } // İşlem detayı

        [Required(ErrorMessage = "Tarih seçiniz.")]
        public DateTime Tarih { get; set; } // Randevu Tarihi

        [Required(ErrorMessage = "Saat seçiniz.")]
        public string Saat { get; set; } // Randevu Saati
    }
}
