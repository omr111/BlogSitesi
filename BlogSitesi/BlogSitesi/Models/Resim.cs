using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Resim
    {
        public Resim()
        {
            this.Kullanicis = new List<Kullanici>();
            this.Makales = new List<Makale>();
            this.Makales1 = new List<Makale>();
        }

        public int id { get; set; }
        public string Adi { get; set; }
        public string KucukResimYol { get; set; }
        public string OrtaResimYol { get; set; }
        public string BuyukResimYol { get; set; }
        public Nullable<System.Guid> EkleyenID { get; set; }
        public Nullable<System.DateTime> EklemeTarihi { get; set; }
        public Nullable<int> Goruntulenme { get; set; }
        public Nullable<int> Begeni { get; set; }
        public virtual ICollection<Kullanici> Kullanicis { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual ICollection<Makale> Makales { get; set; }
        public virtual ICollection<Makale> Makales1 { get; set; }
    }
}
