using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class SiteTakip
    {
        public string Mail { get; set; }
        public Nullable<System.Guid> YazarID { get; set; }
        public Nullable<int> KategoriID { get; set; }
        public virtual Kategori Kategori { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
