using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class YazarlikBasvurusu
    {
        public int BasvuruID { get; set; }
        public System.Guid kID { get; set; }
        public string Hakkinda { get; set; }
        public string Nick { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string mail { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
