using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Mesajlar
    {
        public System.Guid Gonderen { get; set; }
        public System.Guid Alan { get; set; }
        public string Mesaj { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public Nullable<bool> Goruldu { get; set; }
    }
}
