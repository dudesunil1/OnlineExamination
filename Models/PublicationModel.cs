using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class PublicationViewModel
    {
        [Display( Name = "Publication ID")]
        public int Pub_Id { get; set; }
        [Required(ErrorMessage = "Publication name is required")]
        [Display(Name = "Publication Name")]
        public string Pub_Name { get; set; }

    }
}