using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class Makale
    {
        public Makale()
        {
            this.KullaniciBegenis = new List<KullaniciBegeni>();
            this.MakaleEtikets = new List<MakaleEtiket>();
            this.Yorums = new List<Yorum>();
        }

        public int id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(150, ErrorMessage = "En fazla 150 karakter girin")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string icerik { get; set; }


        public System.DateTime YayinTarihi { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        public int KategoriID { get; set; }
        public Nullable<int> MakaleTipID { get; set; }
        public System.Guid YazarID { get; set; }
        public int Goruntulenme { get; set; }
        public bool Aktif { get; set; }
        public string BuyukResimYol { get; set; }
        public string kucukResimYol { get; set; }
        public string resimAlt { get; set; }
        public virtual Kategori Kategori { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual ICollection<KullaniciBegeni> KullaniciBegenis { get; set; }
        public virtual MakaleTip MakaleTip { get; set; }
        public virtual ICollection<MakaleEtiket> MakaleEtikets { get; set; }
        public virtual ICollection<Yorum> Yorums { get; set; }
    }
}
