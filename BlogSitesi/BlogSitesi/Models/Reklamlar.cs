using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Reklamlar
    {
        public int id { get; set; }
        public string reklamPath { get; set; }
        public string reklamLink { get; set; }
        public string reklamText { get; set; }
    }
}
