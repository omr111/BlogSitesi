using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class SirketBilgileri
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string siteAdi { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(11, ErrorMessage = "En fazla 11 karakter girin")]
    
        public string telefon { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string email { get; set; }
        public string logoPath { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string pinperestUrl { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string twitterUrl { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string googleUrl { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string facebookUrl { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string hakkimizda { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(200, ErrorMessage = "En fazla 200 karakter girin")]
        public string adres { get; set; }
    }
}
