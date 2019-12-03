using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Kullanici
    {
        public Kullanici()
        {
            this.KullaniciBegenis = new List<KullaniciBegeni>();
            this.Makales = new List<Makale>();
            this.Mesajlars = new List<Mesajlar>();
            this.Mesajlars1 = new List<Mesajlar>();
            this.SiteTakips = new List<SiteTakip>();
            this.YazarlikBasvurusus = new List<YazarlikBasvurusu>();
            this.Yorums = new List<Yorum>();
        }

        public System.Guid id { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Hakkinda { get; set; }
        public string Mail { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string Nick { get; set; }
        public bool YazarMi { get; set; }
        public bool Aktif { get; set; }
        public string parola { get; set; }
        public string kullaniciResimPath { get; set; }
        public string resimAltText { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual ICollection<KullaniciBegeni> KullaniciBegenis { get; set; }
        public virtual ICollection<Makale> Makales { get; set; }
        public virtual ICollection<Mesajlar> Mesajlars { get; set; }
        public virtual ICollection<Mesajlar> Mesajlars1 { get; set; }
        public virtual ICollection<SiteTakip> SiteTakips { get; set; }
        public virtual ICollection<YazarlikBasvurusu> YazarlikBasvurusus { get; set; }
        public virtual ICollection<Yorum> Yorums { get; set; }
    }
}
