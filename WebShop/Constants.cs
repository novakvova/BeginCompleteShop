using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebShop
{
    public static class Constants
    {
        public static string ProductImagePath
        {
            get { return ConfigurationManager.AppSettings["ProductImagePath"]; }
        }
        public static string ProductThumbnailPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ProductThumbnailPath"];
            }
        }
        public static string ProductDescriptionPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ProductDescriptionPath"];
            }
        }
        public static int PageItems
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PageItems"]);
            }
        }
    }
}