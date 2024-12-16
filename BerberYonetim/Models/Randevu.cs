using System;

namespace BerberYonetim.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public string MusteriAd { get; set; }
        public string MusteriTelefon { get; set; }

        // İlişkiler
        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }
        public int IslemId { get; set; }
        public Islem Islem { get; set; }
    }
}
