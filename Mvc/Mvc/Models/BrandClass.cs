using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Models
{
    public class BrandClass
    {
        [Display(Name = "Brand Id")]
        [Required(ErrorMessage = ("Brand Id is Required"))]
        public int c_bid { get; set; }

        [Display(Name = "Brand Name")]
        [Required(ErrorMessage = ("Brand Name is Required"))]
        public string c_bname { get; set; }
    }
}