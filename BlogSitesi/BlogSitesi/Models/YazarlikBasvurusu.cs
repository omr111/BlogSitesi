using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class YazarlikBasvurusu
    {
        public int BasvuruID { get; set; }
        public System.Guid kID { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        public string Hakkinda { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Nick { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string mail { get; set; }


        public virtual Kullanici Kullanici { get; set; }
    }
}
