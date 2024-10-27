using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Practical.Models
{
    public class topics
    {
        public int c_topicId { get; set; }

        [Required(ErrorMessage = "Topic Name is Requierd")]
        [Display(Name = "Topic Name")]
        public string? c_topicName { get; set;}
    }
}