using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace OnlineExamination.Models
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        [Display(Name ="Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Current password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class DashboardStats
    {
        
        public int total_students { get; set; }
        public int total_classes { get; set; }
        public int todays_exams { get; set; }

    }

    public class AdminDashboardViewModel
    {
        // Basic Statistics
        public int TotalStudents { get; set; }
        public int TotalTests { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalSubjects { get; set; }
        public int TodaysExams { get; set; }
        public int OngoingExams { get; set; }
        public int CompletedExamsToday { get; set; }
        public int UpcomingExams { get; set; }
        
        // Performance Statistics
        public double AverageScore { get; set; }
        public int TotalExamsCompleted { get; set; }
        public int TotalExamsInProgress { get; set; }
        
        // Lists
        public List<ExamTimingInfo> OngoingExamsList { get; set; }
        public List<ExamTimingInfo> RecentCompletedExams { get; set; }
        public List<ExamTimingInfo> UpcomingExamsList { get; set; }
        public List<StudentPerformanceInfo> TopPerformers { get; set; }
        public List<StudentPerformanceInfo> LowPerformers { get; set; }
        public List<ExamTimingInfo> LateStartedExams { get; set; }
        
        public AdminDashboardViewModel()
        {
            OngoingExamsList = new List<ExamTimingInfo>();
            RecentCompletedExams = new List<ExamTimingInfo>();
            UpcomingExamsList = new List<ExamTimingInfo>();
            TopPerformers = new List<StudentPerformanceInfo>();
            LowPerformers = new List<StudentPerformanceInfo>();
            LateStartedExams = new List<ExamTimingInfo>();
        }
    }

    public class ExamTimingInfo
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? ActualDuration { get; set; }
        public int ScheduledDuration { get; set; }
        public double MarksObtained { get; set; }
        public double TotalMarks { get; set; }
        public double Percentage { get; set; }
        public int? MinutesLate { get; set; }
        public string ExamStatus { get; set; }
    }

    public class StudentPerformanceInfo
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int TestsCompleted { get; set; }
        public double AverageScore { get; set; }
        public double TotalMarks { get; set; }
    }
    }