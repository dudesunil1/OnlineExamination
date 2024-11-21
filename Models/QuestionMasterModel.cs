
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OnlineExamination.Models
{
    public class QuestionMasterViewModel
    {
        public int Ques_Id { get; set; }
       
      

        public int Ques_SubId { get; set; }
        [Display(Name = "Subject")]
        public string Ques_SubName { get; set; }

        public int Ques_ClassId { get; set; }
        [Display(Name = "Class")]
        public string Ques_ClassName { get; set; }
        public int Ques_TopId { get; set; }
        [Display(Name = "Topic")]
        public string Ques_TopName { get; set; }

        public int Ques_PubId { get; set; }
        [Display(Name = "Publication")]
        public string Ques_PubName { get; set; }

        public int Ques_Mark { get; set; }

        public int Ques_JEEMark { get; set; }

        public int Ques_Negative { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [Display(Name = "Question")]
        [AllowHtml]

        public string Ques_Question { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Answer is required")]
        [Display(Name = "Answer")]
        public string Ques_Answer { get; set; }

        [Required(ErrorMessage = "Option B is required")]
        [Display(Name = "Option 1")]
        [AllowHtml]
        public string Ques_OptionB { get; set; }

        [Display(Name = "Option 2")]
        [AllowHtml]
        public string Ques_OptionC { get; set; }

        [Display(Name = "Option 3")]
        [AllowHtml]
        public string Ques_OptionD { get; set; }

        [Display(Name = "Solution")]
        [AllowHtml]
        public string Ques_SolutionDetails { get; set; }

    }



    public class QuestionMasterFilterViewModel
    {
        public int? Ques_ClassId { get; set; }
        public int? Ques_SubId { get; set; }
        public int? Ques_TopId { get; set; }
        public int Ques_Id { get; set; }
        string Ques_Name { get; set; }

        string Ques_SubName { get; set; }
        public string Ques_Question { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Answer is required")]
        [Display(Name = "Answer")]
        public string Ques_Answer { get; set; }


        string Ques_TopName { get; set; }
        public IEnumerable<QuestionMasterViewModel> Questions { get; set; }
    }

}