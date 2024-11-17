using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace OnlineExamination.Models
{
    public class TopicViewModel
    {
        [Display(Name ="Topic ID")]
        public int Top_Id { get; set; }
        [Display(Name ="Topic Name")]
        [Required(ErrorMessage ="Topic Name is required")]
        public string Top_Name { get; set; }
        
        [Display(Name ="Subject")]
        public int  Top_SubId  { get; set; }

        [Display(Name ="Class")]
        public int Top_ClassId { get; set; }

    }
    public class TopicShowViewModel
    {
        [Display(Name = "Subject Name")]
        public string Sub_Name { get; set; }

        public int Top_SubId { get; set; }

        [Display(Name = "Topic ID")]
        public int Top_Id { get; set; }
        [Display(Name = "Topic Name")]        
        public string Top_Name { get; set; }

        [Display(Name = "Class")]
        public int Top_ClassId { get; set; }

    }
    public class SubjectsViewModel {
        public int Sub_Id { get; set; }
        public string Sub_Name { get; set; }
    }

    public class ClassViewModel
    {
        public int ID{ get; set; }
        public string Name { get; set; }
    }
    public class SubSubjectsViewModel
    {
        public int SubSubjectID { get; set; }
        public string SubSubjectName { get; set; }
    }
}