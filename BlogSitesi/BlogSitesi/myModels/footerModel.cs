using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlogSitesi.Models;

namespace BlogSitesi.myModels
{
    public class footerModel
    {
        public SirketBilgileri SirketBilgileri { get; set; }
        public List<Makale> populerMakaleler { get; set; }
    }
}