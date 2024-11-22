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
            public string Test_Name { get; set; } // Assuming it's nvarchar, adjust if needed.
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


    }



