using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Makale
    {
        public Makale()
        {
            this.KullaniciBegenis = new List<KullaniciBegeni>();
            this.MakaleEtikets = new List<MakaleEtiket>();
            this.Yorums = new List<Yorum>();
            this.Resims = new List<Resim>();
        }

        public int id { get; set; }
        public string Baslik { get; set; }
        public string icerik { get; set; }
        public System.DateTime YayinTarihi { get; set; }
        public int KategoriID { get; set; }
        public Nullable<int> MakaleTipID { get; set; }
        public System.Guid YazarID { get; set; }
        public int KapakResimID { get; set; }
        public int Goruntulenme { get; set; }
        public bool Aktif { get; set; }
        public virtual Kategori Kategori { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual ICollection<KullaniciBegeni> KullaniciBegenis { get; set; }
        public virtual MakaleTip MakaleTip { get; set; }
        public virtual Resim Resim { get; set; }
        public virtual ICollection<MakaleEtiket> MakaleEtikets { get; set; }
        public virtual ICollection<Yorum> Yorums { get; set; }
        public virtual ICollection<Resim> Resims { get; set; }
    }
}
