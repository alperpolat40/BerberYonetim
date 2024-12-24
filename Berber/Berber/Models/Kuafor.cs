using System.ComponentModel.DataAnnotations;

namespace BerberYonetim.Models
{
    public class Kuafor
    {
        [Key]
        public int Id { get; set; } // Kuaför ID

        [Required(ErrorMessage = "Kuaför adı boş bırakılamaz.")]
        public string Ad { get; set; } // Kuaför adı

        public string Telefon { get; set; } // Kuaför telefonu

        public string CalismaSaatleri { get; set; } // Çalışma saatleri

        // Kuaförün uzmanlık alanları (çoktan çoğa ilişki)
        public ICollection<KuaforIslem> Uzmanliklar { get; set; }
    }
}
