using System.Collections.Generic;

namespace BerberYonetim.Models
{
    public class Kuafor
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }

        // İlişkiler
        public List<Calisan> Calisanlar { get; set; }
        public List<Islem> Islemler { get; set; }
    }
}
