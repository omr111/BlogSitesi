using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;

namespace BlogSitesi.App_Classes
{
    public class Setttings
    {
        public static Size KucukResimYol
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["sw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["sh"]);
                return sonuc;
            }

        }
        public static Size OrtaResimYol
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["mw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["mh"]);
                return sonuc;
            }

        }
        public static Size BuyukResimYol
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["lw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["lh"]);
                return sonuc;
            }

        }
        public static Size KullaniciResim
        {
            get
            {
                Size sonuc = new Size();
                sonuc.Width = Convert.ToInt32(ConfigurationManager.AppSettings["kfw"]);
                sonuc.Height = Convert.ToInt32(ConfigurationManager.AppSettings["kfh"]);
                return sonuc;
            }
        }
        public static Size YazarResim
        {
            get
            {
                Size size = new Size();
                size.Height = Convert.ToInt32(ConfigurationManager.AppSettings["yh"]);
                size.Width = Convert.ToInt32(ConfigurationManager.AppSettings["yw"]);
                return size;
            }
        }
    }
}