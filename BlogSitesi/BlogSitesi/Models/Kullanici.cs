using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Adi { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Soyadi { get; set; }


        [MaxLength(1000, ErrorMessage = "En fazla 1000 karakter girin")]
        public string Hakkinda { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(70, ErrorMessage = "En fazla 70 karakter girin")]
        public string Mail { get; set; }


        public System.DateTime KayitTarihi { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Nick { get; set; }
        public bool YazarMi { get; set; }
        public bool Aktif { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
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
