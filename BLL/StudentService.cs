using OnlineExamination.Models;
using SunTech.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.BLL
{
    public class StudentService
    {

        public bool Login(string userName, string password)
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@UserName", userName);
                hash.Add("@Password", password);
                DataTable dtData = ControlFill.FillDataTable("Sp_Student_Login", hash);
                if (dtData != null && dtData.Rows.Count > 0)
                {

                    var studId = dtData.Rows[0]["stud_Id"].ToString();
                    HttpContext.Current.Session["UserData"] = dtData;
                    HttpContext.Current.Session["StudentId"] = studId;
                    HttpContext.Current.Session["UserRole"] = "STUDENT";
                    return true;
                    
                }
                else
                {
                    return false;
                    //lblError.Text = "Please enter a valid username and password.";
                    //lblError.Visible = true;
                }

            }
            catch (Exception Ex)
            {
                return false;

                //Response.Write(Ex.Message);
            }
        }
        public List<StudentMasterModel> GetStudent()
        {
            try
            {
                Hashtable hash = new Hashtable();
                DataTable dt = ControlFill.FillDataTable("SP_StudentMaster_Select", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<StudentMasterModel> list = ConversionFunctions.DataTableToList<StudentMasterModel>(dt);
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }


        public List<StudentMasterModel> GetStudentByClassId(int id)
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@ClassName", id);

                DataTable dt = ControlFill.FillDataTable("SP_GetStudentsByClass", hashTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<StudentMasterModel> list = ConversionFunctions.DataTableToList<StudentMasterModel>(dt);
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public List<StudentMasterModel> GetStudentById(int id)
        {
            try
            {
                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Id", id);

                DataTable dt = ControlFill.FillDataTable("SP_StudentMaster_Select", hashTable);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<StudentMasterModel> list = ConversionFunctions.DataTableToList<StudentMasterModel>(dt);
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }

     
        public StudentMasterModel Add(StudentMasterModel objStudent, string extension)
        {
            string _errMsg;
            try
            {

               

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Name", objStudent.Stud_Name);
                hashTable.Add("@Stud_Mobile", objStudent.Stud_Mobile);
                hashTable.Add("@Stud_DOB", objStudent.Stud_DOB);
                hashTable.Add("@Stud_Gender", objStudent.Stud_Gender);
                hashTable.Add("@Stud_UserName", objStudent.Stud_UserName);
                hashTable.Add("@Stud_Password", objStudent.Stud_Password);
                hashTable.Add("@Stud_AdmissionDate", objStudent.Stud_AdmissionDate);
                hashTable.Add("@Stud_Group", objStudent.Stud_Group);
                hashTable.Add("@Stud_Class", objStudent.Stud_Class);
                hashTable.Add("@Stud_Photo", extension);


             //   hashTable.Add("@Stud_IsActive", objStudent.Stud_IsActive);
                DataTable dt = clsSunDAL.FillDataTable("SP_StudentMaster_Insert", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objStudent.Stud_Id = dt.Rows[0]["Stud_Id"].ToInt();

                    return objStudent;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }


        public StudentMasterModel Update(StudentMasterModel objStudent,string extension)
        {
            string _errMsg;
            try
            {

                Hashtable hashTable = new Hashtable();
                hashTable.Add("@Stud_Id", objStudent.Stud_Id);
                hashTable.Add("@Stud_Name", objStudent.Stud_Name);
                hashTable.Add("@Stud_Mobile", objStudent.Stud_Mobile);
                hashTable.Add("@Stud_DOB", objStudent.Stud_DOB);
                hashTable.Add("@Stud_Gender", objStudent.Stud_Gender);
                hashTable.Add("@Stud_UserName", objStudent.Stud_UserName);
                hashTable.Add("@Stud_Password", objStudent.Stud_Password);
                hashTable.Add("@Stud_AdmissionDate", objStudent.Stud_AdmissionDate);
                hashTable.Add("@Stud_Group", objStudent.Stud_Group);
                hashTable.Add("@Stud_Photo", extension);
                hashTable.Add("@Stud_Class", objStudent.Stud_Class);
                //   hashTable.Add("@Stud_IsActive", objStudent.Stud_IsActive);
                DataTable dt = clsSunDAL.FillDataTable("SP_StudentMaster_Update", hashTable);
                _errMsg = clsSunDAL._errMsg;
                if (dt != null && dt.Rows.Count > 0)
                {
                    objStudent.Stud_Id = dt.Rows[0]["Stud_Id"].ToInt();
                    return objStudent;
                }
                return null;

            }
            catch (Exception Ex)
            {
                _errMsg = Ex.Message;
                return null;
            }
        }

        public List<StudentDashboardCountData> StudentDashboard(int id)
        {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@Stud_Id", id);

                DataTable dt = ControlFill.FillDataTable("GetStudentDashboardData", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<StudentDashboardCountData> list = ConversionFunctions.DataTableToList<StudentDashboardCountData>(dt);
                    return list;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public StudentDashboardViewModel GetStudentDashboardData(int studentId)
        {
            try
            {
                StudentDashboardViewModel dashboard = new StudentDashboardViewModel();
                
                // Get student basic information
                var studentInfo = GetStudentById(studentId)?.FirstOrDefault();
                if (studentInfo != null)
                {
                    dashboard.StudentId = studentInfo.Stud_Id;
                    dashboard.StudentName = studentInfo.Stud_Name;
                    dashboard.StudentPhoto = studentInfo.Stud_Photo;
                    dashboard.StudentClass = studentInfo.classname;
                }
                
                // Get dashboard counts
                var dashboardCounts = StudentDashboard(studentId)?.FirstOrDefault();
                if (dashboardCounts != null)
                {
                    dashboard.TodaysTestsCount = dashboardCounts.TodaysTestsCount;
                    dashboard.AttemptedTestsCount = dashboardCounts.AttemptedTestsCount;
                    dashboard.NonAttemptedTestsCount = dashboardCounts.NonAttemptedTestsCount;
                    dashboard.UpcomingTestsCount = dashboardCounts.UpcomingTestsCount;
                }
                
                // Get today's tests and upcoming tests
                StudentTestService testService = new StudentTestService();
                var allTests = testService.GetstudetTest(studentId);
                
                if (allTests != null && allTests.Count > 0)
                {
                    DateTime today = DateTime.Today;
                    
                    // Filter today's tests
                    dashboard.TodaysTests = allTests
                        .Where(t => t.TS_Expected_Date.Date == today)
                        .Select(t => new DashboardTestInfo
                        {
                            TestId = t.TS_TestId,
                            TestName = t.Test_Name,
                            SubjectName = "",
                            TestDate = t.TS_Expected_Date,
                            StartTime = t.TS_StartTime.TimeOfDay,
                            EndTime = t.TS_End_Time.TimeOfDay,
                            Duration = t.Test_Duration,
                            IsAttempted = t.TS_IsAttempted
                        }).ToList();
                    
                    // Filter upcoming tests (future dates, not today)
                    dashboard.UpcomingTests = allTests
                        .Where(t => t.TS_Expected_Date.Date > today)
                        .OrderBy(t => t.TS_Expected_Date)
                        .Take(5)
                        .Select(t => new DashboardTestInfo
                        {
                            TestId = t.TS_TestId,
                            TestName = t.Test_Name,
                            SubjectName = "",
                            TestDate = t.TS_Expected_Date,
                            StartTime = t.TS_StartTime.TimeOfDay,
                            EndTime = t.TS_End_Time.TimeOfDay,
                            Duration = t.Test_Duration,
                            IsAttempted = t.TS_IsAttempted
                        }).ToList();
                    
                    // Calculate total marks and average
                    var attemptedTests = allTests.Where(t => t.TS_IsAttempted).ToList();
                    if (attemptedTests.Count > 0)
                    {
                        dashboard.TotalMarksScored = attemptedTests.Sum(t => t.TS_Mark);
                        dashboard.AverageMarks = attemptedTests.Average(t => t.TS_Mark);
                    }
                }
                
                return dashboard;
            }
            catch (Exception Ex)
            {
                return new StudentDashboardViewModel();
            }
        }

        public ExamInterfaceViewModel GetExamInterfaceData(int testId, int studentId, int currentQuestionNumber)
        {
            try
            {
                ExamInterfaceViewModel examData = new ExamInterfaceViewModel();
                
                // Get test details
                var testDetails = GetTestDetails(testId, studentId);
                if (testDetails == null)
                {
                    return null;
                }

                examData.TestId = testId;
                examData.TestName = testDetails.Test_Name;
                examData.TestDuration = testDetails.Test_Duration;
                examData.TestStartTime = testDetails.TS_StartTime;
                examData.TestEndTime = testDetails.TS_End_Time;
                examData.CurrentQuestionNumber = currentQuestionNumber;
                examData.IsExamStarted = true; // Assume started when accessing this method

                // Get questions for the test
                QuestionMasterService questionService = new QuestionMasterService();
                var allQuestions = questionService.GetTestQuestions(testId);
                
                if (allQuestions == null || allQuestions.Count == 0)
                {
                    return null;
                }

                examData.NumberOfQuestions = allQuestions.Count;
                
                // Get current question
                if (currentQuestionNumber > 0 && currentQuestionNumber <= allQuestions.Count)
                {
                    examData.CurrentQuestion = allQuestions[currentQuestionNumber - 1];
                }

                // Initialize question statuses
                for (int i = 1; i <= examData.NumberOfQuestions; i++)
                {
                    examData.QuestionStatuses.Add(new QuestionStatus
                    {
                        QuestionNumber = i,
                        Status = QuestionStatusType.NotVisited,
                        StudentAnswer = "",
                        IsMarkedForReview = false
                    });
                }

                // Load saved answers and statuses
                LoadStudentAnswers(examData, studentId, testId);

                // Calculate time remaining
                examData.TimeRemaining = CalculateTimeRemaining(testDetails.TS_StartTime, testDetails.Test_Duration);

                // Set up instructions
                SetupInstructions(examData);

                return examData;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private TestStudent GetTestDetails(int testId, int studentId)
        {
            try
            {
                StudentTestService testService = new StudentTestService();
                var allTests = testService.GetstudetTest(studentId);
                return allTests?.FirstOrDefault(t => t.TS_TestId == testId);
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        private void LoadStudentAnswers(ExamInterfaceViewModel examData, int studentId, int testId)
        {
            try
            {
                // This would typically load from a StudentAnswers table
                // For now, we'll simulate with empty data
                // In a real implementation, you'd query the database for saved answers
            }
            catch (Exception Ex)
            {
                // Handle error silently
            }
        }

        private int CalculateTimeRemaining(DateTime startTime, int durationMinutes)
        {
            try
            {
                DateTime endTime = startTime.AddMinutes(durationMinutes);
                TimeSpan remaining = endTime - DateTime.Now;
                
                if (remaining.TotalSeconds <= 0)
                {
                    return 0;
                }
                
                return (int)remaining.TotalSeconds;
            }
            catch (Exception Ex)
            {
                return durationMinutes * 60; // Fallback to full duration
            }
        }

        private void SetupInstructions(ExamInterfaceViewModel examData)
        {
            examData.GeneralInstructions.AddRange(new[]
            {
                "The total duration of the examination is " + examData.TestDuration + " minutes.",
                "The clock is server-set and a countdown timer will show the remaining time.",
                "When the timer reaches zero, the examination will end automatically.",
                "The Question Palette shows the status of each question using symbols.",
                "Click on any question number to navigate directly to that question."
            });

            examData.AnsweringInstructions.AddRange(new[]
            {
                "Click on the button of one of the options to select an answer.",
                "To deselect, click the chosen option again or click 'Clear Response'.",
                "To change an answer, click on another option.",
                "To save an answer, you MUST click on the 'Save & Next' button.",
                "To mark for review, click 'Review & Next'.",
                "Note: Your answer will not be saved if you navigate directly by clicking question number."
            });
        }

        public bool StartExamSession(int testId, int studentId)
        {
            try
            {
                // This would typically update the database to mark exam as started
                // For now, we'll return true as a placeholder
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public bool SaveStudentAnswer(int testId, int studentId, int questionNumber, string answer, bool markForReview = false)
        {
            try
            {
                // This would typically save the answer to a StudentAnswers table
                // For now, we'll return true as a placeholder
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        public bool SubmitExam(int testId, int studentId)
        {
            try
            {
                // This would typically mark the exam as completed and calculate results
                // For now, we'll return true as a placeholder
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

    }
}