using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class TestResultModel
    {

        public int TS_Id { get; set; }
        public int TS_TestId { get; set; }
        public string TS_TestName { get; set; }
        public int TS_StudId { get; set; }
        public string TS_StudName { get; set; }
        public bool TS_IsAttempted { get; set; }
        public DateTime TS_StartTime { get; set; }
        public DateTime TS_EndTime { get; set; }
        public DateTime TS_Expected_Date { get; set; }
        public DateTime TS_AddTime { get; set; }
        public TimeSpan TS_BreakFrom { get; set; }
        public TimeSpan TS_BreakTo { get; set; }
        public double TS_Mark { get; set; }
    }
}