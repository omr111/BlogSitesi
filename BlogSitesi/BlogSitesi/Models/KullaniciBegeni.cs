using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class KullaniciBegeni
    {
        public int id { get; set; }
        public System.Guid KullaniciID { get; set; }
        public int MakaleID { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual Makale Makale { get; set; }
    }
}
