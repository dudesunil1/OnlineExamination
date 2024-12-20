using OnlineExamination.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class TestMasterModel
    {


       
            public int Test_Id { get; set; }
            public string Test_Name { get; set; } 
            public int Test_GrpId { get; set; }
            public int Test_Duration { get; set; }
            public int Test_Mark { get; set; }
            public DateTime Test_StartTime { get; set; }
            public DateTime Test_EndTime { get; set; }
            public int Test_TypeId { get; set; }
        }

    public class TestType
    {
        public int TT_Id { get; set; }      // Maps to TT_Id in the database
        public string TT_Name { get; set; } // Maps to TT_Name in the database
    }


    public class TestQuestion
    {
        public int TQ_Id { get; set; }
        public int TQ_TestId { get; set; }
        public int TQ_QuesId { get; set; }
    }

    public class TestStudent
    {
        public int TS_Id { get; set; }            // Primary key for TestStudent
        public int TS_TestId { get; set; }        // The ID of the test
        
        public string Test_Name { get; set; }
        public int Test_Duration { get; set; }
        public int TS_StudId { get; set; }        // The ID of the student
        public string TS_StudName { get; set; }
        public bool TS_IsAttempted { get; set; }  // Whether the student has attempted the test
        public DateTime TS_StartTime { get; set; }  // The start time of the test
        public DateTime TS_End_Time { get; set; }  // The end time of the test
        public DateTime TS_Expected_Date { get; set; } // Expected date of the test
        public DateTime TS_AddTime { get; set; }  // The time when the record was added
        public TimeSpan TS_BreakFrom { get; set; }  // Break start time during the test
        public TimeSpan TS_BreakTo { get; set; }    // Break end time during the test
        public double TS_Mark { get; set; }         // The marks scored by the student

        
    }

    public class TestSubjectDetailsModel
    {
        public int TS_Id { get; set; }
        public int TestID { get; set; }
        public string SubjectName { get; set; }       
        public int TestDuration { get; set; }          
        public DateTime TestStartTime { get; set; }    
        public DateTime TestEndTime { get; set; }      
        public int TestMark { get; set; }              
        public int NumberOfQuestions { get; set; }     
    }
}



