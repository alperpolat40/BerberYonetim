namespace BerberYonetim.Models
{
    public class Islem
    {
        public int Id { get; set; }
        public string Ad { get; set; } // Örneğin: "Saç Kesimi", "Sakal Tıraşı"
        public decimal Ucret { get; set; }
        public int Sure { get; set; } // Dakika olarak işlem süresi

        // İlişkiler
        public int KuaforId { get; set; }
        public Kuafor Kuafor { get; set; }
    }
}
