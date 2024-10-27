using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Models
{
    public class ProductClass
    {
        public int c_sid { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is Required")]
        public string c_name { get; set; }

        [Display(Name = "Size")]
        [Required(ErrorMessage = ("Size is Required"))]
        public int c_size;

        [Display(Name = ("Color"))]
        [Required(ErrorMessage = ("Color is Required"))]
        public string c_color { get; set; }

        [Display(Name = ("Qty"))]
        [Required(ErrorMessage = ("Qty is Required"))]
        public int c_qty { get; set; }

        [Display(Name = ("Price"))]
        [Required(ErrorMessage = ("Price is Required"))]
        public int c_price { get; set; }

        [Display(Name = ("Date"))]
        [Required(ErrorMessage = ("Date is Required"))]
        public DateTime c_date { get; set; }

        [Display(Name = ("brand is"))]
        [Required(ErrorMessage = ("brand is Required"))]
        public int c_bid { get; set; }

        public string img{get;set;}
        public string brand{get;set;}
    }
}