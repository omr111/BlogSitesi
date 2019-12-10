using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class Kategori
    {
        public Kategori()
        {
            this.Makales = new List<Makale>();
            this.SiteTakips = new List<SiteTakip>();
        }

        public int id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Adi { get; set; }
        public virtual ICollection<Makale> Makales { get; set; }
        public virtual ICollection<SiteTakip> SiteTakips { get; set; }
    }
}
