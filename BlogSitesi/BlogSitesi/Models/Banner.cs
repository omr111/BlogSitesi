using System;
using System.Collections.Generic;

namespace BlogSitesi.Models
{
    public partial class Banner
    {
        public int id { get; set; }
        public string bannerPicPath { get; set; }
        public string buttonName { get; set; }
        public string textArea { get; set; }
    }
}
