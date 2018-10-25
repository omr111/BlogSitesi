using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Kullanici
    {
        public Kullanici()
        {
            this.Makales = new List<Makale>();
            this.Resims = new List<Resim>();
            this.SiteTakips = new List<SiteTakip>();
            this.YazarlikBasvurusus = new List<YazarlikBasvurusu>();
            this.Yorums = new List<Yorum>();
            this.Makales1 = new List<Makale>();
        }

        public System.Guid id { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Hakkinda { get; set; }
        public string Mail { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public string Nick { get; set; }
        public bool YazarMi { get; set; }
        public Nullable<int> ResimID { get; set; }
        public bool Aktif { get; set; }
        public string KullaniciAdi { get; set; }
        public string parola { get; set; }
        public virtual aspnet_Users aspnet_Users { get; set; }
        public virtual Resim Resim { get; set; }
        public virtual ICollection<Makale> Makales { get; set; }
        public virtual ICollection<Resim> Resims { get; set; }
        public virtual ICollection<SiteTakip> SiteTakips { get; set; }
        public virtual ICollection<YazarlikBasvurusu> YazarlikBasvurusus { get; set; }
        public virtual ICollection<Yorum> Yorums { get; set; }
        public virtual ICollection<Makale> Makales1 { get; set; }
    }
}
