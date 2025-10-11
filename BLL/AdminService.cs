using OnlineExamination.Models;
using SunTech.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web.SunTechDB;

namespace OnlineExamination.BLL
{
    public class AdminService
    {
        public bool Login(string userName,string password) {
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("@Comp_UserName", userName);
                hash.Add("@Comp_Password", password);
                DataTable dtData = ControlFill.FillDataTable("Sp_Company_Login", hash);
                if (dtData != null && dtData.Rows.Count > 0)
                {
                    HttpContext.Current.Session["UserData"] = dtData;
                    HttpContext.Current.Session["UserRole"] = "ADMIN";
                    return true;
                    //Response.Redirect("Dashboard.aspx", false);
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
        public List<DashboardStats> AdminDashboard()
        {
            try
            {
                Hashtable hash = new Hashtable();
                DataTable dt = ControlFill.FillDataTable("SP_GetDashboardStats", hash);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<DashboardStats> list = ConversionFunctions.DataTableToList<DashboardStats>(dt);
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

        public AdminDashboardViewModel GetAdvancedDashboardData()
        {
            try
            {
                AdminDashboardViewModel dashboard = new AdminDashboardViewModel();
                DateTime today = DateTime.Today;
                
                // Get basic statistics using stored procedure
                DataTable statsData = clsSunDAL.FillDataTable("SP_GetSystemStatistics");
                if (statsData != null && statsData.Rows.Count > 0)
                {
                    DataRow row = statsData.Rows[0];
                    dashboard.TotalStudents = Convert.ToInt32(row["TotalStudents"]);
                    dashboard.TotalTests = Convert.ToInt32(row["TotalTests"]);
                    dashboard.TotalQuestions = Convert.ToInt32(row["TotalQuestions"]);
                    dashboard.TotalSubjects = Convert.ToInt32(row["TotalSubjects"]);
                    dashboard.OngoingExams = Convert.ToInt32(row["OngoingExams"]);
                    dashboard.TotalExamsCompleted = Convert.ToInt32(row["CompletedExams"]);
                    dashboard.AverageScore = row["AverageScore"] != DBNull.Value ? Convert.ToDouble(row["AverageScore"]) : 0;
                }
                
                // Get exam timing statistics
                DataTable timingData = clsSunDAL.FillDataTable("SP_GetExamTimingStatistics");
                
                if (timingData != null && timingData.Rows.Count > 0)
                {
                    var allExams = ConvertToExamTimingInfo(timingData);
                    
                    // Ongoing exams (started but not submitted)
                    dashboard.OngoingExamsList = allExams
                        .Where(e => e.ExamStatus == "In Progress")
                        .OrderBy(e => e.ActualStartTime)
                        .ToList();
                    dashboard.OngoingExams = dashboard.OngoingExamsList.Count;
                    
                    // Completed exams today
                    dashboard.RecentCompletedExams = allExams
                        .Where(e => e.ExamStatus == "Completed" && 
                                    e.ActualEndTime.HasValue && 
                                    e.ActualEndTime.Value.Date == today)
                        .OrderByDescending(e => e.ActualEndTime)
                        .Take(10)
                        .ToList();
                    dashboard.CompletedExamsToday = dashboard.RecentCompletedExams.Count;
                    
                    // Upcoming exams
                    dashboard.UpcomingExamsList = allExams
                        .Where(e => e.ExamStatus == "Not Started")
                        .OrderBy(e => e.ActualStartTime)
                        .Take(10)
                        .ToList();
                    dashboard.UpcomingExams = dashboard.UpcomingExamsList.Count;
                    
                    // Exams started late
                    dashboard.LateStartedExams = allExams
                        .Where(e => e.MinutesLate.HasValue && e.MinutesLate > 5)
                        .OrderByDescending(e => e.MinutesLate)
                        .Take(5)
                        .ToList();
                    
                    // Calculate average score
                    var completedExams = allExams.Where(e => e.ExamStatus == "Completed").ToList();
                    if (completedExams.Count > 0)
                    {
                        dashboard.AverageScore = completedExams.Average(e => e.Percentage);
                        dashboard.TotalExamsCompleted = completedExams.Count;
                    }
                    
                    dashboard.TotalExamsInProgress = dashboard.OngoingExams;
                }
                
                // Get student performance data
                DataTable performanceData = clsSunDAL.FillDataTable("SP_GetStudentPerformanceStats");
                
                if (performanceData != null && performanceData.Rows.Count > 0)
                {
                    var performances = ConvertToStudentPerformanceInfo(performanceData);
                    dashboard.TopPerformers = performances.OrderByDescending(p => p.AverageScore).Take(5).ToList();
                    dashboard.LowPerformers = performances.OrderBy(p => p.AverageScore).Take(5).ToList();
                }
                
                return dashboard;
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getting admin dashboard data: " + Ex.Message);
                return new AdminDashboardViewModel();
            }
        }

        private int GetTotalCount(string query)
        {
            try
            {
                DataTable dt = ExecuteQuery(query);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0]);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        private DataTable ExecuteQuery(string query)
        {
            try
            {
                // Use clsSunDAL.FillDataTable for views/stored procedures
                return clsSunDAL.FillDataTable(query);
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine("Error executing query: " + Ex.Message);
                return null;
            }
        }

        private List<ExamTimingInfo> ConvertToExamTimingInfo(DataTable dt)
        {
            var list = new List<ExamTimingInfo>();
            
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ExamTimingInfo
                {
                    TestId = Convert.ToInt32(row["TS_TestId"]),
                    TestName = row["Test_Name"].ToString(),
                    StudentId = Convert.ToInt32(row["TS_StudId"]),
                    StudentName = row["StudentName"].ToString(),
                    StudentEmail = row["StudentEmail"].ToString(),
                    ActualStartTime = row["ActualStartTime"] != DBNull.Value ? Convert.ToDateTime(row["ActualStartTime"]) : (DateTime?)null,
                    ActualEndTime = row["ActualEndTime"] != DBNull.Value ? Convert.ToDateTime(row["ActualEndTime"]) : (DateTime?)null,
                    ActualDuration = row["ActualDuration"] != DBNull.Value ? Convert.ToInt32(row["ActualDuration"]) : (int?)null,
                    ScheduledDuration = Convert.ToInt32(row["ScheduledDuration"]),
                    MarksObtained = row["MarksObtained"] != DBNull.Value ? Convert.ToDouble(row["MarksObtained"]) : 0,
                    TotalMarks = row["TotalMarks"] != DBNull.Value ? Convert.ToDouble(row["TotalMarks"]) : 0,
                    Percentage = row["Percentage"] != DBNull.Value ? Convert.ToDouble(row["Percentage"]) : 0,
                    MinutesLate = row["MinutesLate"] != DBNull.Value ? Convert.ToInt32(row["MinutesLate"]) : (int?)null,
                    ExamStatus = row["ExamStatus"].ToString()
                });
            }
            
            return list;
        }

        private List<StudentPerformanceInfo> ConvertToStudentPerformanceInfo(DataTable dt)
        {
            var list = new List<StudentPerformanceInfo>();
            
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new StudentPerformanceInfo
                {
                    StudentId = Convert.ToInt32(row["StudentId"]),
                    StudentName = row["StudentName"].ToString(),
                    StudentEmail = row["StudentEmail"].ToString(),
                    TestsCompleted = Convert.ToInt32(row["TestsCompleted"]),
                    AverageScore = row["AverageScore"] != DBNull.Value ? Convert.ToDouble(row["AverageScore"]) : 0,
                    TotalMarks = row["TotalMarks"] != DBNull.Value ? Convert.ToDouble(row["TotalMarks"]) : 0
                });
            }
            
            return list;
        }

    }
}