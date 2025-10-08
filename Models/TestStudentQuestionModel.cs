using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class TestStudentQuestionModel
    {
        public int STQ_Id { get; set; }
        public int STQ_TestId { get; set; }
        public int STQ_StudId { get; set; }
        public int STQ_QuesId { get; set; }
        public int STQ_Sequence { get; set; }
        public bool STQ_Isview { get; set; }
        public DateTime? STQ_ViewTime { get; set; }
        public DateTime? STQ_AnsTime { get; set; }
        public string STQ_Option1 { get; set; }
        public string STQ_Option2 { get; set; }
        public string STQ_Option3 { get; set; }
        public string STQ_Option4 { get; set; }
        public string STQ_AnsOption { get; set; }
        public string STQ_StudentAns { get; set; }
        public int STQ_QueType { get; set; }
        public bool STQ_IsCorrect { get; set; }
        public int STQ_Mark { get; set; }
    }
}