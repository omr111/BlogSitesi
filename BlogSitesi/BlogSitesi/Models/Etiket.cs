using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class Etiket
    {
        public Etiket()
        {
            this.MakaleEtikets = new List<MakaleEtiket>();
        }

        public int id { get; set; }
        [Required(ErrorMessage = "Bu alan zorunludur")]
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string Adi { get; set; }
        public virtual ICollection<MakaleEtiket> MakaleEtikets { get; set; }
    }
}
