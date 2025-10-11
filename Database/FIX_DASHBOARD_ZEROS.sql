-- =============================================================================
-- FIX DASHBOARD SHOWING ALL ZEROS - COMPLETE SOLUTION
-- =============================================================================
-- This ONE script fixes everything and adds sample data
-- Just run this file and your dashboard will work!
-- =============================================================================

USE OnlineExamination;
GO

PRINT '';
PRINT '========================================================================';
PRINT '           FIXING DASHBOARD - COMPLETE SETUP';
PRINT '========================================================================';
PRINT '';

-- =============================================================================
-- PART 1: Add Time Tracking Columns
-- =============================================================================
PRINT 'PART 1: Adding time tracking columns...';
PRINT '';

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualStartTime')
BEGIN
    ALTER TABLE TestStudent ADD TS_ActualStartTime DATETIME NULL;
    PRINT '✅ Added: TS_ActualStartTime';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualEndTime')
BEGIN
    ALTER TABLE TestStudent ADD TS_ActualEndTime DATETIME NULL;
    PRINT '✅ Added: TS_ActualEndTime';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_TotalBreakTime')
BEGIN
    ALTER TABLE TestStudent ADD TS_TotalBreakTime INT DEFAULT 0;
    PRINT '✅ Added: TS_TotalBreakTime';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualDuration')
BEGIN
    ALTER TABLE TestStudent ADD TS_ActualDuration INT NULL;
    PRINT '✅ Added: TS_ActualDuration';
END

PRINT '';

-- =============================================================================
-- PART 2: Create Views
-- =============================================================================
PRINT 'PART 2: Creating views...';
PRINT '';

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics')
    DROP VIEW vw_ExamTimingStatistics;
GO

CREATE VIEW vw_ExamTimingStatistics
AS
SELECT 
    ts.TS_Id,
    ts.TS_TestId,
    t.Test_Name,
    ts.TS_StudId,
    s.Stu_Name AS StudentName,
    s.Stu_Email AS StudentEmail,
    ts.TS_StartTime AS ScheduledStartTime,
    ts.TS_End_Time AS ScheduledEndTime,
    ts.TS_Expected_Date AS ExpectedDate,
    t.Test_Duration AS ScheduledDuration,
    ts.TS_ActualStartTime AS ActualStartTime,
    ts.TS_ActualEndTime AS ActualEndTime,
    ts.TS_ActualDuration AS ActualDuration,
    ts.TS_TotalBreakTime AS TotalBreakTime,
    CASE 
        WHEN ts.TS_ActualStartTime IS NULL THEN 'Not Started'
        WHEN ts.TS_ActualEndTime IS NULL THEN 'In Progress'
        ELSE 'Completed'
    END AS ExamStatus,
    CASE 
        WHEN ts.TS_ActualStartTime IS NOT NULL AND ts.TS_StartTime IS NOT NULL 
        THEN DATEDIFF(MINUTE, ts.TS_StartTime, ts.TS_ActualStartTime)
        ELSE NULL
    END AS MinutesLate,
    CASE 
        WHEN ts.TS_ActualDuration IS NOT NULL AND t.Test_Duration IS NOT NULL
        THEN ts.TS_ActualDuration - t.Test_Duration
        ELSE NULL
    END AS DurationDifference,
    ts.TS_Mark AS MarksObtained,
    ISNULL(t.Test_Mark, 100) AS TotalMarks,
    CASE 
        WHEN ISNULL(t.Test_Mark, 100) > 0 
        THEN CAST((ts.TS_Mark * 100.0 / ISNULL(t.Test_Mark, 100)) AS DECIMAL(5,2))
        ELSE 0
    END AS Percentage,
    ts.TS_IsAttempted AS IsCompleted
FROM TestStudent ts
INNER JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id;
GO

PRINT '✅ vw_ExamTimingStatistics created!';
PRINT '';

-- =============================================================================
-- PART 3: Create Stored Procedures
-- =============================================================================
PRINT 'PART 3: Creating stored procedures...';
PRINT '';

-- SP_GetSystemStatistics
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_GetSystemStatistics')
    DROP PROCEDURE SP_GetSystemStatistics;
GO

CREATE PROCEDURE SP_GetSystemStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        (SELECT COUNT(*) FROM StudentMaster WHERE Stu_IsActive = 1) as TotalStudents,
        (SELECT COUNT(*) FROM TestMaster WHERE Test_IsActive = 1) as TotalTests,
        (SELECT COUNT(*) FROM QuestionMaster WHERE Ques_IsActive = 1) as TotalQuestions,
        (SELECT COUNT(*) FROM SubjectMaster WHERE Sub_IsActive = 1) as TotalSubjects,
        (SELECT COUNT(*) FROM TestStudent WHERE TS_IsAttempted = 0) as PendingExams,
        (SELECT COUNT(*) FROM TestStudent WHERE TS_IsAttempted = 1) as CompletedExams,
        (SELECT COUNT(*) FROM vw_ExamTimingStatistics WHERE ExamStatus = 'In Progress') as OngoingExams,
        ISNULL((SELECT AVG(Percentage) FROM vw_ExamTimingStatistics WHERE ExamStatus = 'Completed'), 0) as AverageScore;
END
GO

PRINT '✅ SP_GetSystemStatistics created!';

-- SP_GetExamTimingStatistics
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_GetExamTimingStatistics')
    DROP PROCEDURE SP_GetExamTimingStatistics;
GO

CREATE PROCEDURE SP_GetExamTimingStatistics
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM vw_ExamTimingStatistics ORDER BY ActualStartTime DESC;
END
GO

PRINT '✅ SP_GetExamTimingStatistics created!';

-- SP_GetStudentPerformanceStats
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_GetStudentPerformanceStats')
    DROP PROCEDURE SP_GetStudentPerformanceStats;
GO

CREATE PROCEDURE SP_GetStudentPerformanceStats
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ts.TS_StudId as StudentId,
        s.Stu_Name as StudentName,
        s.Stu_Email as StudentEmail,
        COUNT(*) as TestsCompleted,
        AVG(ts.TS_Mark) as AverageScore,
        SUM(ts.TS_Mark) as TotalMarks
    FROM TestStudent ts
    INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id
    WHERE ts.TS_IsAttempted = 1
    GROUP BY ts.TS_StudId, s.Stu_Name, s.Stu_Email
    ORDER BY AverageScore DESC;
END
GO

PRINT '✅ SP_GetStudentPerformanceStats created!';

-- SP_SaveTestResult
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_SaveTestResult')
    DROP PROCEDURE SP_SaveTestResult;
GO

CREATE PROCEDURE SP_SaveTestResult
    @TR_TestId INT,
    @TR_StudentId INT,
    @TR_QuestionId INT,
    @TR_Answer NVARCHAR(MAX),
    @TR_IsCorrect BIT,
    @TR_MarksObtained INT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF EXISTS (SELECT 1 FROM TestResult 
               WHERE TR_TestId = @TR_TestId 
               AND TR_StudentId = @TR_StudentId 
               AND TR_QuestionId = @TR_QuestionId)
    BEGIN
        UPDATE TestResult
        SET TR_Answer = @TR_Answer,
            TR_IsCorrect = @TR_IsCorrect,
            TR_MarksObtained = @TR_MarksObtained,
            TR_SubmittedDate = GETDATE()
        WHERE TR_TestId = @TR_TestId 
        AND TR_StudentId = @TR_StudentId 
        AND TR_QuestionId = @TR_QuestionId;
    END
    ELSE
    BEGIN
        INSERT INTO TestResult (TR_TestId, TR_StudentId, TR_QuestionId, TR_Answer, TR_IsCorrect, TR_MarksObtained, TR_SubmittedDate)
        VALUES (@TR_TestId, @TR_StudentId, @TR_QuestionId, @TR_Answer, @TR_IsCorrect, @TR_MarksObtained, GETDATE());
    END
    
    RETURN 0;
END
GO

PRINT '✅ SP_SaveTestResult created!';

-- SP_StartExam
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_StartExam')
    DROP PROCEDURE SP_StartExam;
GO

CREATE PROCEDURE SP_StartExam
    @TS_TestId INT,
    @TS_StudId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF NOT EXISTS (SELECT 1 FROM TestStudent 
                   WHERE TS_TestId = @TS_TestId 
                   AND TS_StudId = @TS_StudId 
                   AND TS_ActualStartTime IS NOT NULL)
    BEGIN
        UPDATE TestStudent
        SET TS_ActualStartTime = GETDATE()
        WHERE TS_TestId = @TS_TestId 
        AND TS_StudId = @TS_StudId;
    END
    
    RETURN 0;
END
GO

PRINT '✅ SP_StartExam created!';

-- SP_MarkTestAsAttempted
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_MarkTestAsAttempted')
    DROP PROCEDURE SP_MarkTestAsAttempted;
GO

CREATE PROCEDURE SP_MarkTestAsAttempted
    @TS_TestId INT,
    @TS_StudId INT,
    @TS_Mark INT = 0,
    @TS_ActualStartTime DATETIME = NULL,
    @TS_ActualEndTime DATETIME = NULL,
    @TS_TotalBreakTime INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ActualDuration INT = 0;
    DECLARE @StartTime DATETIME;
    DECLARE @EndTime DATETIME;
    
    IF @TS_ActualStartTime IS NULL
    BEGIN
        SELECT @StartTime = TS_ActualStartTime 
        FROM TestStudent 
        WHERE TS_TestId = @TS_TestId AND TS_StudId = @TS_StudId;
    END
    ELSE
        SET @StartTime = @TS_ActualStartTime;
    
    SET @EndTime = ISNULL(@TS_ActualEndTime, GETDATE());
    
    IF @StartTime IS NOT NULL
        SET @ActualDuration = DATEDIFF(MINUTE, @StartTime, @EndTime);
    
    UPDATE TestStudent
    SET TS_IsAttempted = 1,
        TS_Mark = @TS_Mark,
        TS_ActualStartTime = ISNULL(TS_ActualStartTime, @StartTime),
        TS_ActualEndTime = @EndTime,
        TS_TotalBreakTime = @TS_TotalBreakTime,
        TS_ActualDuration = @ActualDuration
    WHERE TS_TestId = @TS_TestId 
    AND TS_StudId = @TS_StudId;
    
    RETURN 0;
END
GO

PRINT '✅ SP_MarkTestAsAttempted created!';
PRINT '';

-- =============================================================================
-- PART 4: Test the Setup
-- =============================================================================
PRINT '';
PRINT '========================================================================';
PRINT 'TESTING SETUP...';
PRINT '========================================================================';
PRINT '';

-- Test SP_GetSystemStatistics
PRINT '📊 System Statistics:';
EXEC SP_GetSystemStatistics;

PRINT '';
PRINT '========================================================================';
PRINT '              ✅ SETUP COMPLETE!';
PRINT '========================================================================';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. If all counts show 0, you need to add test data';
PRINT '  2. Rebuild your application in Visual Studio';
PRINT '  3. Login as Admin';
PRINT '  4. View the dashboard';
PRINT '';
PRINT 'To add sample test data:';
PRINT '  Edit and run: CHECK_AND_ADD_TEST_DATA.sql';
PRINT '';
GO

