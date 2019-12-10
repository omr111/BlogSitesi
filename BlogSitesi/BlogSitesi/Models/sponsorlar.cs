using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class sponsorlar
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(150, ErrorMessage = "En fazla 150 karakter girin")]
        public string sponsorName { get; set; }
        public string sponsorPath { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(250, ErrorMessage = "En fazla 250 karakter girin")]
        public string sponsorLink { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(250, ErrorMessage = "En fazla 250 karakter girin")]
        public string soponsorAciklama { get; set; }
    }
}
