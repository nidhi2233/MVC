using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_PRACTICAL.Models
{
    public class InternClass

    {
        [Display(Name = "intern Id")]
        public int InternId { get; set; }

        [Required(ErrorMessage = "Intern Name is Required")]
        [StringLength(100,ErrorMessage = "Intern name can't exceed 100 caharacters")]
        [Display(Name = "Intern name")]
        public string? InternName { get; set;}



        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("M|F", ErrorMessage = "Gender must be 'M' for male or 'F' for Female")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        public int TopicId { get; set; }

        [Required(ErrorMessage = "presentation date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Presentation")]
        public DateTime DateOfPresentation { get; set; }

        [Display(Name = "Presentation Status")]
        public bool Status { get; set; }


        [Display(Name = "Topic Image")]
        public string? TopicImage { get; set; }

        [Display(Name ="Upload Topic Image")]
        public IFormFile? TopicImageFile    { get; set; }

        public topic? AssignedTopic { get; set; }
    }
}