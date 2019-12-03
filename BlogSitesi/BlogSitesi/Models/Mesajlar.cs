using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Mesajlar
    {
        public int id { get; set; }
        public System.Guid GonderenId { get; set; }
        public System.Guid AlanId { get; set; }
        public string Mesaj { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<bool> Goruldu { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public virtual Kullanici Kullanici1 { get; set; }
    }
}
