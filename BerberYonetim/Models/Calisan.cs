using System.Collections.Generic;

namespace BerberYonetim.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Uzmanlik { get; set; } // Örneğin: "Saç Kesimi", "Sakal Tıraşı"
        public bool Musaitlik { get; set; } // Çalışan uygun mu?

        // İlişkiler
        public int KuaforId { get; set; }
        public Kuafor Kuafor { get; set; }
    }
}
