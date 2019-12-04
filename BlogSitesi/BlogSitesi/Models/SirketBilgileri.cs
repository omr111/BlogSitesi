using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class SirketBilgileri
    {
        public int id { get; set; }
        public string siteAdi { get; set; }
        public string telefon { get; set; }
        public string email { get; set; }
        public string logoPath { get; set; }
        public string pinperestUrl { get; set; }
        public string twitterUrl { get; set; }
        public string googleUrl { get; set; }
        public string facebookUrl { get; set; }
        public string hakkimizda { get; set; }
        public string adres { get; set; }
    }
}
