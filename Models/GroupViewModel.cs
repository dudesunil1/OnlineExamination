using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class GroupViewModel
    {
        [Display(Name ="Group ID")]
        public int Grp_Id { get; set; }
        [Required(ErrorMessage = "Group name is required")]
        [Display(Name = "Group Name")]       
        public string Grp_Name { get; set; }
    }
}