using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mozzhhevelnik.Models
{
    public class CartIndexViewModel
    {
        public string id { get; set; }
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}