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
                    
                    // Filter upcoming tests (today or future, not attempted)
                    DateTime now = DateTime.Now;
                    dashboard.UpcomingTests = allTests
                        .Where(t => !t.TS_IsAttempted && 
                                    (t.TS_Expected_Date.Date >= today))
                        .OrderBy(t => t.TS_Expected_Date)
                        .ThenBy(t => t.TS_StartTime)
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
                // Load saved answers from Session
                string sessionKey = $"ExamAnswers_{testId}_{studentId}";
                var savedAnswers = HttpContext.Current.Session[sessionKey] as Dictionary<int, string>;
                
                // Load review flags from Session
                string reviewKey = $"ExamReviews_{testId}_{studentId}";
                var reviewedQuestions = HttpContext.Current.Session[reviewKey] as Dictionary<int, bool>;
                
                // Load visited questions from Session
                string visitedKey = $"ExamVisited_{testId}_{studentId}";
                var visitedQuestions = HttpContext.Current.Session[visitedKey] as HashSet<int>;
                
                // Update each question's status
                for (int i = 1; i <= examData.QuestionStatuses.Count; i++)
                {
                    var questionStatus = examData.QuestionStatuses[i - 1];
                    bool hasAnswer = savedAnswers != null && savedAnswers.ContainsKey(i) && !string.IsNullOrEmpty(savedAnswers[i]);
                    bool isReviewed = reviewedQuestions != null && reviewedQuestions.ContainsKey(i) && reviewedQuestions[i];
                    bool isVisited = visitedQuestions != null && visitedQuestions.Contains(i);
                    
                    // Set student answer if exists
                    if (hasAnswer)
                    {
                        questionStatus.StudentAnswer = savedAnswers[i];
                        examData.StudentAnswers[i] = savedAnswers[i];
                    }
                    
                    // Determine status based on answer and review flag
                    if (hasAnswer && isReviewed)
                    {
                        questionStatus.Status = QuestionStatusType.AnsweredAndMarked;
                        questionStatus.IsMarkedForReview = true;
                    }
                    else if (hasAnswer)
                    {
                        questionStatus.Status = QuestionStatusType.Answered;
                    }
                    else if (isReviewed)
                    {
                        questionStatus.Status = QuestionStatusType.MarkedForReview;
                        questionStatus.IsMarkedForReview = true;
                    }
                    else if (isVisited)
                    {
                        questionStatus.Status = QuestionStatusType.VisitedNotAnswered;
                    }
                    else
                    {
                        questionStatus.Status = QuestionStatusType.NotVisited;
                    }
                }
            }
            catch (Exception Ex)
            {
                // Handle error silently
                System.Diagnostics.Debug.WriteLine("Error loading student answers: " + Ex.Message);
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
                // Initialize exam start in session
                string sessionKey = $"ExamStarted_{testId}_{studentId}";
                DateTime startTime = DateTime.Now;
                HttpContext.Current.Session[sessionKey] = startTime;
                
                // Save actual start time to database
                Hashtable hash = new Hashtable();
                hash.Add("@TS_TestId", testId);
                hash.Add("@TS_StudId", studentId);
                
                clsSunDAL.ExecuteDMLQuery("SP_StartExam", hash);
                
                return true;
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine("Error starting exam session: " + Ex.Message);
                return false;
            }
        }

        public bool SaveStudentAnswer(int testId, int studentId, int questionNumber, string answer, bool markForReview = false)
        {
            try
            {
                // Save answer in Session (only if not empty)
                string sessionKey = $"ExamAnswers_{testId}_{studentId}";
                var savedAnswers = HttpContext.Current.Session[sessionKey] as Dictionary<int, string>;
                
                if (savedAnswers == null)
                {
                    savedAnswers = new Dictionary<int, string>();
                    HttpContext.Current.Session[sessionKey] = savedAnswers;
                }
                
                // Update or add the answer only if it's not empty
                if (!string.IsNullOrEmpty(answer))
                {
                    if (savedAnswers.ContainsKey(questionNumber))
                    {
                        savedAnswers[questionNumber] = answer;
                    }
                    else
                    {
                        savedAnswers.Add(questionNumber, answer);
                    }
                }
                
                // Save mark for review status in separate Session key
                string reviewKey = $"ExamReviews_{testId}_{studentId}";
                var reviewedQuestions = HttpContext.Current.Session[reviewKey] as Dictionary<int, bool>;
                
                if (reviewedQuestions == null)
                {
                    reviewedQuestions = new Dictionary<int, bool>();
                    HttpContext.Current.Session[reviewKey] = reviewedQuestions;
                }
                
                // Update or add the review flag
                if (reviewedQuestions.ContainsKey(questionNumber))
                {
                    reviewedQuestions[questionNumber] = markForReview;
                }
                else
                {
                    reviewedQuestions.Add(questionNumber, markForReview);
                }
                
                // Also track visited questions
                string visitedKey = $"ExamVisited_{testId}_{studentId}";
                var visitedQuestions = HttpContext.Current.Session[visitedKey] as HashSet<int>;
                
                if (visitedQuestions == null)
                {
                    visitedQuestions = new HashSet<int>();
                    HttpContext.Current.Session[visitedKey] = visitedQuestions;
                }
                
                visitedQuestions.Add(questionNumber);
                
                return true;
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine("Error saving student answer: " + Ex.Message);
                return false;
            }
        }

        public bool ClearStudentAnswer(int testId, int studentId, int questionNumber)
        {
            try
            {
                // Remove answer from Session
                string sessionKey = $"ExamAnswers_{testId}_{studentId}";
                var savedAnswers = HttpContext.Current.Session[sessionKey] as Dictionary<int, string>;
                
                if (savedAnswers != null && savedAnswers.ContainsKey(questionNumber))
                {
                    savedAnswers.Remove(questionNumber);
                }
                
                // Remove review flag
                string reviewKey = $"ExamReviews_{testId}_{studentId}";
                var reviewedQuestions = HttpContext.Current.Session[reviewKey] as Dictionary<int, bool>;
                
                if (reviewedQuestions != null && reviewedQuestions.ContainsKey(questionNumber))
                {
                    reviewedQuestions.Remove(questionNumber);
                }
                
                return true;
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine("Error clearing student answer: " + Ex.Message);
                return false;
            }
        }

        public bool SubmitExam(int testId, int studentId)
        {
            try
            {
                // Get all saved answers from Session
                string sessionKey = $"ExamAnswers_{testId}_{studentId}";
                var savedAnswers = HttpContext.Current.Session[sessionKey] as Dictionary<int, string>;
                
                // Get all test questions to save results
                QuestionMasterService questionService = new QuestionMasterService();
                var allQuestions = questionService.GetTestQuestions(testId);
                
                if (allQuestions != null && savedAnswers != null)
                {
                    // Save each answer to the database
                    foreach (var answer in savedAnswers)
                    {
                        int questionNumber = answer.Key;
                        string studentAnswer = answer.Value;
                        
                        if (questionNumber > 0 && questionNumber <= allQuestions.Count)
                        {
                            var question = allQuestions[questionNumber - 1];
                            
                            // Check if answer is correct (Ques_Answer contains the correct option letter)
                            // Note: In QuestionMaster, Ques_Answer actually contains option A's text, not the letter
                            // We need to compare based on which option letter is selected
                            bool isCorrect = false;
                            string correctOption = "A"; // Default to A since Ques_Answer is option A
                            
                            isCorrect = studentAnswer.Trim().ToUpper() == correctOption.ToUpper();
                            int marksObtained = isCorrect ? question.Ques_Mark : 0;
                            
                            // Save to TestResult table
                            Hashtable resultHash = new Hashtable();
                            resultHash.Add("@TR_TestId", testId);
                            resultHash.Add("@TR_StudentId", studentId);
                            resultHash.Add("@TR_QuestionId", question.Ques_Id);
                            resultHash.Add("@TR_Answer", studentAnswer);
                            resultHash.Add("@TR_IsCorrect", isCorrect ? 1 : 0);
                            resultHash.Add("@TR_MarksObtained", marksObtained);
                            
                            clsSunDAL.ExecuteDMLQuery("SP_SaveTestResult", resultHash);
                        }
                    }
                }
                
                // Calculate total marks
                int totalMarks = 0;
                if (savedAnswers != null && allQuestions != null)
                {
                    foreach (var answer in savedAnswers)
                    {
                        int questionNumber = answer.Key;
                        string studentAnswer = answer.Value;
                        
                        if (questionNumber > 0 && questionNumber <= allQuestions.Count)
                        {
                            var question = allQuestions[questionNumber - 1];
                            // Option A is always correct since Ques_Answer contains option A's text
                            string correctOption = "A";
                            bool isCorrect = studentAnswer.Trim().ToUpper() == correctOption.ToUpper();
                            if (isCorrect)
                            {
                                totalMarks += question.Ques_Mark;
                            }
                        }
                    }
                }
                
                // Get start time from session
                string startSessionKey = $"ExamStarted_{testId}_{studentId}";
                DateTime? startTime = HttpContext.Current.Session[startSessionKey] as DateTime?;
                DateTime endTime = DateTime.Now;
                
                // Mark exam as attempted with total marks and timing information
                Hashtable hash = new Hashtable();
                hash.Add("@TS_TestId", testId);
                hash.Add("@TS_StudId", studentId);
                hash.Add("@TS_Mark", totalMarks);
                hash.Add("@TS_ActualStartTime", startTime.HasValue ? (object)startTime.Value : DBNull.Value);
                hash.Add("@TS_ActualEndTime", endTime);
                hash.Add("@TS_TotalBreakTime", 0); // Can be enhanced to track actual breaks
                
                // Update TestStudent to mark as attempted
                bool updated = clsSunDAL.ExecuteDMLQuery("SP_MarkTestAsAttempted", hash);
                
                // Clear session data after successful submission
                if (updated)
                {
                    HttpContext.Current.Session.Remove(sessionKey);
                    HttpContext.Current.Session.Remove($"ExamReviews_{testId}_{studentId}");
                    HttpContext.Current.Session.Remove($"ExamVisited_{testId}_{studentId}");
                    HttpContext.Current.Session.Remove(startSessionKey);
                }
                
                return updated;
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Debug.WriteLine("Error submitting exam: " + Ex.Message);
                return false;
            }
        }

    }
}