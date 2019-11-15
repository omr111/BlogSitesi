using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Etiket
    {
        public Etiket()
        {
            this.MakaleEtikets = new List<MakaleEtiket>();
        }

        public int id { get; set; }
        public string Adi { get; set; }
        public virtual ICollection<MakaleEtiket> MakaleEtikets { get; set; }
    }
}
