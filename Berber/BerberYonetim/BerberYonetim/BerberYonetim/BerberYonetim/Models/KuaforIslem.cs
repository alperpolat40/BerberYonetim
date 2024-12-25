namespace BerberYonetim.Models
{
    public class KuaforIslem
    {
        public int KuaforId { get; set; } // Kuaför ID
        public Kuafor Kuafor { get; set; } // Kuaför detayı

        public int IslemId { get; set; } // İşlem ID
        public Islem Islem { get; set; } // İşlem detayı
    }
}
