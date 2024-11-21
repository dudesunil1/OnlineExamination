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

        public string Stud_Photo { get; set; }  // Path or URL to the photo

        public bool Stud_IsActive { get; set; }

        public string FormattedStudDOB => Stud_DOB.ToString("yyyy-MM-dd");
    }
}