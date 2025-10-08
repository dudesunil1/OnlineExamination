using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineExamination.Models
{
    public class StudentMasterModel
    {

        [Key]
        public int Stud_Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Stud_Name { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Stud_Mobile { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2024", ErrorMessage = "Please enter a valid date.")]
        [DataType(DataType.Date)]
        public DateTime Stud_DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Stud_Gender { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters")]
        public string Stud_UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Stud_Password { get; set; }

        [Required(ErrorMessage = "Admission date is required")]
        [DataType(DataType.Date)]
        public DateTime Stud_AdmissionDate { get; set; }

        [Required(ErrorMessage = "Group is required")]
        public string Stud_Group { get; set; }
        [Required(ErrorMessage = "Class is required")]
        public int Stud_Class { get; set; }
        public string classname { get; set; }
        public string Stud_Photo { get; set; }  // Path or URL to the photo

        public bool Stud_IsActive { get; set; }

        public string FormattedStudDOB => Stud_DOB.ToString("yyyy-MM-dd");
    }

    public class StudentDashboardCountData
    {
        public int TodaysTestsCount { get; set; }
        public int AttemptedTestsCount { get; set; }
        public int NonAttemptedTestsCount { get; set; }
        public int UpcomingTestsCount { get; set; }
    }

    public class StudentDashboardViewModel
    {
        // Student Information
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentPhoto { get; set; }
        public string StudentClass { get; set; }
        
        // Dashboard Counts
        public int TodaysTestsCount { get; set; }
        public int AttemptedTestsCount { get; set; }
        public int NonAttemptedTestsCount { get; set; }
        public int UpcomingTestsCount { get; set; }
        public double TotalMarksScored { get; set; }
        public double AverageMarks { get; set; }
        
        // Test Lists
        public List<DashboardTestInfo> TodaysTests { get; set; }
        public List<DashboardTestInfo> UpcomingTests { get; set; }
        public List<SubjectPerformance> SubjectPerformances { get; set; }
        
        public StudentDashboardViewModel()
        {
            TodaysTests = new List<DashboardTestInfo>();
            UpcomingTests = new List<DashboardTestInfo>();
            SubjectPerformances = new List<SubjectPerformance>();
        }
    }
    
    public class DashboardTestInfo
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string SubjectName { get; set; }
        public DateTime TestDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public int TotalMarks { get; set; }
        public bool IsAttempted { get; set; }
    }
    
    public class SubjectPerformance
    {
        public string SubjectName { get; set; }
        public double AverageMarks { get; set; }
        public int TestsAttempted { get; set; }
    }

    public class ExamInterfaceViewModel
    {
        // Test Information
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string SubjectName { get; set; }
        public int TestDuration { get; set; } // in minutes
        public int TotalMarks { get; set; }
        public int NumberOfQuestions { get; set; }
        public DateTime TestStartTime { get; set; }
        public DateTime TestEndTime { get; set; }
        
        // Current Question
        public int CurrentQuestionNumber { get; set; }
        public TestQuestionViewModel CurrentQuestion { get; set; }
        
        // Question Navigation
        public List<QuestionStatus> QuestionStatuses { get; set; }
        public Dictionary<int, string> StudentAnswers { get; set; }
        
        // Timer Information
        public int TimeRemaining { get; set; } // in seconds
        public bool IsExamStarted { get; set; }
        public bool IsExamCompleted { get; set; }
        
        // Instructions
        public List<string> GeneralInstructions { get; set; }
        public List<string> AnsweringInstructions { get; set; }
        
        public ExamInterfaceViewModel()
        {
            QuestionStatuses = new List<QuestionStatus>();
            StudentAnswers = new Dictionary<int, string>();
            GeneralInstructions = new List<string>();
            AnsweringInstructions = new List<string>();
        }
    }
    
    public class QuestionStatus
    {
        public int QuestionNumber { get; set; }
        public QuestionStatusType Status { get; set; }
        public string StudentAnswer { get; set; }
        public bool IsMarkedForReview { get; set; }
    }
    
    public enum QuestionStatusType
    {
        NotVisited = 1,      // Grey/White - Not visited yet
        VisitedNotAnswered = 2,  // Red - Visited but not answered
        Answered = 3,        // Green - Answered and will be considered
        MarkedForReview = 4, // Purple - Marked for review, not answered
        AnsweredAndMarked = 5 // Green with purple dot - Answered and marked for review
    }

}