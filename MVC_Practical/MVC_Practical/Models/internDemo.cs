using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Practical.Models
{
    public class internDemo
    {
        public int c_internId { get; set; }

        [Required(ErrorMessage = "Inter Name Is Requierd")]
        [Display(Name = "Intern Name")]
        public string? c_internName { get; set; }

        [Required(ErrorMessage = "Gender Is Requierd")]
        [RegularExpression("M|F", ErrorMessage = "Gender Must be 'M' For Male And 'F' For Female" )]
        [Display(Name = "Gender")]
        public char c_gender { get; set; }

        public int c_topicId {get; set; }

        [Required(ErrorMessage = "Presemtation Date Is Requierd")]
        [DataType(DataType.Date)]
        [Display(Name = "Presentation Date")]
        public DateTime c_date_Of_presentation{ get; set; }

        [Display(Name = "Presentation Status")]
        public bool c_status { get; set; }

        [Display(Name = "Topic Image ")]
        public string? c_topicImage{ get; set; }

        [Required(ErrorMessage = "Image Is Requierd")]
        [Display(Name = "Upload Topic Image")]
        public IFormFile? imagePath{ get; set; }

        public topics? assignTopic {get; set; }

    }
}