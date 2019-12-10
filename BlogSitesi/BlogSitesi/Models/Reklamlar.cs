using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class Reklamlar
    {
        public int id { get; set; }
        public string reklamPath { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(250, ErrorMessage = "En fazla 250 karakter girin")]
        public string reklamLink { get; set; }
        public string reklamText { get; set; }
    }
}
