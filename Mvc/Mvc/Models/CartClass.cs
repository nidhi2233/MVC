using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Models
{
    public class CartClass
    {
        public int c_oid{get;set;}
        public int c_sid { get; set; }

        public string c_name { get; set; }

        public int c_size { get; set; }

        public string c_color { get; set; }


        public int c_price { get; set; }

        public string c_gender { get; set; }

        public int c_qty { get; set; }

        public int price { get; set; }

        public int total{get;set;}

        public int grandtotal{get;set;}

        public bool cancel{get;set;}


        public string c_bname { get; set; }

        public string path { get; set; }

    }
}