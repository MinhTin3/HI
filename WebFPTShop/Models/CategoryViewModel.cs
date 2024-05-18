using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFPTShop.Models
{
    public class CategoryViewModel
    {
        public int IDCate { get; set; }
        public string NameCate { get; set; }
        public string CateIcon { get; set; }
        public int ProductCount { get; set; }
    }
}