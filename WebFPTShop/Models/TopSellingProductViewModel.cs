using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFPTShop.Models
{
    public class TopSellingProductViewModel
    {
        public int ProductID { get; set; }
        public string NamePro { get; set; }
        public string ImagePro { get; set; }
        public int QuantitySold { get; set; }
    }
}