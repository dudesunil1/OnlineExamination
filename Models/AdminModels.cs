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
        
        public int TotalStudents { get; set; }
        public int TotalClasses { get; set; }
        public int TodaysExams { get; set; }

    }
    }