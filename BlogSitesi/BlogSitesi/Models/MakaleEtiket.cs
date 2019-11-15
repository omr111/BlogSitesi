using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class MakaleEtiket
    {
        public int id { get; set; }
        public int MakaleID { get; set; }
        public int EtiketID { get; set; }
        public virtual Etiket Etiket { get; set; }
        public virtual Makale Makale { get; set; }
    }
}
