using System.ComponentModel.DataAnnotations;

namespace BerberYonetim.Models
{
    public class Islem
    {
        [Key]
        public int Id { get; set; } // İşlem ID

        [Required(ErrorMessage = "İşlem adı boş bırakılamaz.")]
        public string Ad { get; set; } // İşlem adı (örneğin: Saç Kesimi)

        [Required(ErrorMessage = "İşlem süresi boş bırakılamaz.")]
        public int Sure { get; set; } // İşlem süresi (dakika cinsinden)

        [Required(ErrorMessage = "İşlem ücreti boş bırakılamaz.")]
        public decimal Ucret { get; set; } // İşlem ücreti

        // İşlemi yapan kuaförler (çoktan çoğa ilişki)
        public ICollection<KuaforIslem> Kuaforler { get; set; }
    }
}
