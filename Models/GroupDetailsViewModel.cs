using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class GroupDetailsViewModel
    {
        [Display(Name = "Group Detail ID")]
        public int GD_Id { get; set; }

        [Display(Name = "Group ID")]
        [Required(ErrorMessage = "Group ID is required")]
        public int GD_GrpId { get; set; }

        [Display(Name = "Subject ID")]
        [Required(ErrorMessage = "Subject ID is required")]
        public int GD_SubId { get; set; }
    }

    public class GroupDetailsShowViewModel
    {
        [Display(Name = "Group ID")]
        public int GD_GrpId { get; set; }

        [Display(Name = "Group Name")]
        public string Grp_Name { get; set; }

        [Display(Name = "Subject ID")]
        public int GD_SubId { get; set; }

        [Display(Name = "Subject Name")]
        public string Sub_Name { get; set; }
        public int GD_ID { get; set; }
    }
}