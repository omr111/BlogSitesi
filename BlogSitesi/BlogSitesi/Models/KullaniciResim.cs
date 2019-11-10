using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class KullaniciResim
    {
        public int id { get; set; }
        public string resimPath { get; set; }
        public System.Guid kullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
