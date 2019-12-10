using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogSitesi.Models
{
    public partial class Banner
    {
        public int id { get; set; }
        public string bannerPicPath { get; set; }
        public string buttonName { get; set; }
        
        [MaxLength(50, ErrorMessage = "En fazla 50 karakter girin")]
        public string textArea { get; set; }
    }
}
