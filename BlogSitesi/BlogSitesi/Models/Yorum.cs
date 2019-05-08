using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Yorum
    {
        public int id { get; set; }
        public System.Guid YorumYapanID { get; set; }
        public string Baslik { get; set; }
        public string icerik { get; set; }
        public int MakaleID { get; set; }
        public System.DateTime EklemeTarihi { get; set; }
        public bool Aktif { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual Makale Makale { get; set; }
    }
}
