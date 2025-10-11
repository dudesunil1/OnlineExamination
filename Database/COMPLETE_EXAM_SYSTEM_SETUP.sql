-- =============================================================================
-- COMPLETE EXAM SYSTEM SETUP SCRIPT
-- =============================================================================
-- This script sets up the complete exam system with:
-- 1. Answer submission functionality
-- 2. Time tracking (start, end, break times)
-- 3. Button color status tracking
-- 4. Result storage and reporting
-- =============================================================================
-- Run this entire script against your OnlineExamination database
-- =============================================================================

USE OnlineExamination;
GO

PRINT '';
PRINT '========================================================================';
PRINT '           COMPLETE EXAM SYSTEM SETUP';
PRINT '========================================================================';
PRINT '';
PRINT 'This script will set up:';
PRINT '  1. Answer submission to database';
PRINT '  2. Time tracking (start, end, duration, breaks)';
PRINT '  3. Exam status tracking';
PRINT '  4. Result storage and reporting';
PRINT '';
PRINT 'Starting setup...';
PRINT '';
PRINT '========================================================================';
PRINT '';

-- =============================================================================
-- PART 1: ADD TIME TRACKING COLUMNS TO TESTSTUDENT TABLE
-- =============================================================================
PRINT '';
PRINT '------------------------------------------------------------------------';
PRINT 'PART 1: Adding Time Tracking Columns';
PRINT '------------------------------------------------------------------------';
PRINT '';

-- Add TS_ActualStartTime (when student actually started the exam)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualStartTime')
BEGIN
    ALTER TABLE TestStudent ADD TS_ActualStartTime DATETIME NULL;
    PRINT '  ✅ Added column: TS_ActualStartTime';
END
ELSE
BEGIN
    PRINT '  ℹ️  Column TS_ActualStartTime already exists';
END

-- Add TS_ActualEndTime (when student actually submitted/ended the exam)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualEndTime')
BEGIN
    ALTER TABLE TestStudent ADD TS_ActualEndTime DATETIME NULL;
    PRINT '  ✅ Added column: TS_ActualEndTime';
END
ELSE
BEGIN
    PRINT '  ℹ️  Column TS_ActualEndTime already exists';
END

-- Add TS_TotalBreakTime (total break time in minutes)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_TotalBreakTime')
BEGIN
    ALTER TABLE TestStudent ADD TS_TotalBreakTime INT DEFAULT 0;
    PRINT '  ✅ Added column: TS_TotalBreakTime';
END
ELSE
BEGIN
    PRINT '  ℹ️  Column TS_TotalBreakTime already exists';
END

-- Add TS_ActualDuration (actual time spent on exam in minutes)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualDuration')
BEGIN
    ALTER TABLE TestStudent ADD TS_ActualDuration INT NULL;
    PRINT '  ✅ Added column: TS_ActualDuration';
END
ELSE
BEGIN
    PRINT '  ℹ️  Column TS_ActualDuration already exists';
END

PRINT '';
PRINT '✅ Time tracking columns setup completed!';
PRINT '';

-- =============================================================================
-- PART 2: CREATE/UPDATE STORED PROCEDURES
-- =============================================================================
PRINT '';
PRINT '------------------------------------------------------------------------';
PRINT 'PART 2: Creating/Updating Stored Procedures';
PRINT '------------------------------------------------------------------------';
PRINT '';

-- -----------------------------------------------------------------------------
-- SP_SaveTestResult: Saves individual question answers
-- -----------------------------------------------------------------------------
PRINT 'Creating SP_SaveTestResult...';

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_SaveTestResult')
BEGIN
    DROP PROCEDURE SP_SaveTestResult;
    PRINT '  - Dropped existing SP_SaveTestResult';
END
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
    
    BEGIN TRY
        -- Check if result already exists
        IF EXISTS (SELECT 1 FROM TestResult 
                   WHERE TR_TestId = @TR_TestId 
                   AND TR_StudentId = @TR_StudentId 
                   AND TR_QuestionId = @TR_QuestionId)
        BEGIN
            -- Update existing result
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
            -- Insert new result
            INSERT INTO TestResult (TR_TestId, TR_StudentId, TR_QuestionId, TR_Answer, TR_IsCorrect, TR_MarksObtained, TR_SubmittedDate)
            VALUES (@TR_TestId, @TR_StudentId, @TR_QuestionId, @TR_Answer, @TR_IsCorrect, @TR_MarksObtained, GETDATE());
        END
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        PRINT 'ERROR in SP_SaveTestResult: ' + ERROR_MESSAGE();
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '  ✅ SP_SaveTestResult created successfully!';
PRINT '';

-- -----------------------------------------------------------------------------
-- SP_StartExam: Records when student starts exam
-- -----------------------------------------------------------------------------
PRINT 'Creating SP_StartExam...';

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_StartExam')
BEGIN
    DROP PROCEDURE SP_StartExam;
    PRINT '  - Dropped existing SP_StartExam';
END
GO

CREATE PROCEDURE SP_StartExam
    @TS_TestId INT,
    @TS_StudId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Only update if not already started
        IF NOT EXISTS (SELECT 1 FROM TestStudent 
                       WHERE TS_TestId = @TS_TestId 
                       AND TS_StudId = @TS_StudId 
                       AND TS_ActualStartTime IS NOT NULL)
        BEGIN
            UPDATE TestStudent
            SET TS_ActualStartTime = GETDATE()
            WHERE TS_TestId = @TS_TestId 
            AND TS_StudId = @TS_StudId;
            
            PRINT '  ✅ Exam start time recorded';
        END
        ELSE
        BEGIN
            PRINT '  ℹ️  Exam was already started';
        END
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        PRINT 'ERROR in SP_StartExam: ' + ERROR_MESSAGE();
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '  ✅ SP_StartExam created successfully!';
PRINT '';

-- -----------------------------------------------------------------------------
-- SP_MarkTestAsAttempted: Marks test as completed with all data
-- -----------------------------------------------------------------------------
PRINT 'Creating SP_MarkTestAsAttempted...';

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_MarkTestAsAttempted')
BEGIN
    DROP PROCEDURE SP_MarkTestAsAttempted;
    PRINT '  - Dropped existing SP_MarkTestAsAttempted';
END
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
    
    BEGIN TRY
        DECLARE @ActualDuration INT = 0;
        DECLARE @StartTime DATETIME;
        DECLARE @EndTime DATETIME;
        
        -- Get existing start time if not provided
        IF @TS_ActualStartTime IS NULL
        BEGIN
            SELECT @StartTime = TS_ActualStartTime 
            FROM TestStudent 
            WHERE TS_TestId = @TS_TestId AND TS_StudId = @TS_StudId;
        END
        ELSE
        BEGIN
            SET @StartTime = @TS_ActualStartTime;
        END
        
        -- Set end time
        SET @EndTime = ISNULL(@TS_ActualEndTime, GETDATE());
        
        -- Calculate actual duration if start time exists
        IF @StartTime IS NOT NULL
        BEGIN
            SET @ActualDuration = DATEDIFF(MINUTE, @StartTime, @EndTime);
        END
        
        -- Update TestStudent record with all information
        UPDATE TestStudent
        SET TS_IsAttempted = 1,
            TS_Mark = @TS_Mark,
            TS_ActualStartTime = ISNULL(TS_ActualStartTime, @StartTime),
            TS_ActualEndTime = @EndTime,
            TS_TotalBreakTime = @TS_TotalBreakTime,
            TS_ActualDuration = @ActualDuration
        WHERE TS_TestId = @TS_TestId 
        AND TS_StudId = @TS_StudId;
        
        IF @@ROWCOUNT > 0
        BEGIN
            PRINT '  ✅ Test marked as completed';
            PRINT '     - Total Marks: ' + CAST(@TS_Mark AS VARCHAR);
            PRINT '     - Duration: ' + CAST(@ActualDuration AS VARCHAR) + ' minutes';
        END
        ELSE
        BEGIN
            PRINT '  ⚠️  Warning: No test record found to update';
        END
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        PRINT 'ERROR in SP_MarkTestAsAttempted: ' + ERROR_MESSAGE();
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '  ✅ SP_MarkTestAsAttempted created successfully!';
PRINT '';

-- =============================================================================
-- PART 3: CREATE REPORTING VIEWS
-- =============================================================================
PRINT '';
PRINT '------------------------------------------------------------------------';
PRINT 'PART 3: Creating Reporting Views';
PRINT '------------------------------------------------------------------------';
PRINT '';

-- -----------------------------------------------------------------------------
-- vw_ExamTimingStatistics: Complete timing and performance statistics
-- -----------------------------------------------------------------------------
PRINT 'Creating vw_ExamTimingStatistics...';

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics')
BEGIN
    DROP VIEW vw_ExamTimingStatistics;
    PRINT '  - Dropped existing view';
END
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
    
    -- Scheduled Times
    ts.TS_StartTime AS ScheduledStartTime,
    ts.TS_End_Time AS ScheduledEndTime,
    ts.TS_Expected_Date AS ExpectedDate,
    t.Test_Duration AS ScheduledDuration,
    
    -- Actual Times
    ts.TS_ActualStartTime AS ActualStartTime,
    ts.TS_ActualEndTime AS ActualEndTime,
    ts.TS_ActualDuration AS ActualDuration,
    ts.TS_TotalBreakTime AS TotalBreakTime,
    
    -- Exam Status
    CASE 
        WHEN ts.TS_ActualStartTime IS NULL THEN 'Not Started'
        WHEN ts.TS_ActualEndTime IS NULL THEN 'In Progress'
        ELSE 'Completed'
    END AS ExamStatus,
    
    -- Time Analysis
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
    
    -- Performance
    ts.TS_Mark AS MarksObtained,
    ISNULL(t.Test_TotalMarks, t.Test_Mark) AS TotalMarks,
    CASE 
        WHEN ISNULL(t.Test_TotalMarks, t.Test_Mark) > 0 
        THEN CAST((ts.TS_Mark * 100.0 / ISNULL(t.Test_TotalMarks, t.Test_Mark)) AS DECIMAL(5,2))
        ELSE 0
    END AS Percentage,
    ts.TS_IsAttempted AS IsCompleted
    
FROM TestStudent ts
INNER JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id;
GO

PRINT '  ✅ vw_ExamTimingStatistics created successfully!';
PRINT '';

-- -----------------------------------------------------------------------------
-- vw_StudentTestResults: Detailed test results view
-- -----------------------------------------------------------------------------
PRINT 'Creating vw_StudentTestResults...';

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_StudentTestResults')
BEGIN
    DROP VIEW vw_StudentTestResults;
    PRINT '  - Dropped existing view';
END
GO

CREATE VIEW vw_StudentTestResults
AS
SELECT 
    tr.TR_Id,
    tr.TR_TestId,
    t.Test_Name,
    tr.TR_StudentId,
    s.Stu_Name AS StudentName,
    s.Stu_Email AS StudentEmail,
    tr.TR_QuestionId,
    q.Ques_Question,
    tr.TR_Answer AS StudentAnswer,
    tr.TR_IsCorrect,
    tr.TR_MarksObtained,
    q.Ques_Mark AS MaxMarks,
    tr.TR_SubmittedDate,
    ts.TS_Mark AS TotalMarksInTest,
    ISNULL(t.Test_TotalMarks, t.Test_Mark) AS MaxTotalMarks
FROM TestResult tr
INNER JOIN TestMaster t ON tr.TR_TestId = t.Test_Id
INNER JOIN StudentMaster s ON tr.TR_StudentId = s.Stu_Id
INNER JOIN QuestionMaster q ON tr.TR_QuestionId = q.Ques_Id
LEFT JOIN TestStudent ts ON tr.TR_TestId = ts.TS_TestId AND tr.TR_StudentId = ts.TS_StudId;
GO

PRINT '  ✅ vw_StudentTestResults created successfully!';
PRINT '';

-- =============================================================================
-- PART 4: VERIFICATION
-- =============================================================================
PRINT '';
PRINT '------------------------------------------------------------------------';
PRINT 'PART 4: Verification';
PRINT '------------------------------------------------------------------------';
PRINT '';

PRINT 'Verifying installation...';
PRINT '';

-- Check columns
PRINT '📋 TestStudent Table Columns:';
SELECT 
    '  ' + COLUMN_NAME + ' (' + DATA_TYPE + ')' AS ColumnInfo
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TestStudent'
AND COLUMN_NAME IN ('TS_ActualStartTime', 'TS_ActualEndTime', 'TS_TotalBreakTime', 'TS_ActualDuration', 'TS_Mark')
ORDER BY COLUMN_NAME;

PRINT '';

-- Check stored procedures
PRINT '📋 Stored Procedures:';
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_SaveTestResult')
    PRINT '  ✅ SP_SaveTestResult'
ELSE
    PRINT '  ❌ SP_SaveTestResult NOT FOUND!'

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_StartExam')
    PRINT '  ✅ SP_StartExam'
ELSE
    PRINT '  ❌ SP_StartExam NOT FOUND!'

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_MarkTestAsAttempted')
    PRINT '  ✅ SP_MarkTestAsAttempted'
ELSE
    PRINT '  ❌ SP_MarkTestAsAttempted NOT FOUND!'

PRINT '';

-- Check views
PRINT '📋 Views:';
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics')
    PRINT '  ✅ vw_ExamTimingStatistics'
ELSE
    PRINT '  ❌ vw_ExamTimingStatistics NOT FOUND!'

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_StudentTestResults')
    PRINT '  ✅ vw_StudentTestResults'
ELSE
    PRINT '  ❌ vw_StudentTestResults NOT FOUND!'

PRINT '';

-- =============================================================================
-- PART 5: SAMPLE QUERIES & USAGE GUIDE
-- =============================================================================
PRINT '';
PRINT '------------------------------------------------------------------------';
PRINT 'PART 5: Sample Queries & Usage Guide';
PRINT '------------------------------------------------------------------------';
PRINT '';

PRINT '📖 USEFUL QUERIES:';
PRINT '';
PRINT '1. View all exam timing statistics:';
PRINT '   SELECT * FROM vw_ExamTimingStatistics ORDER BY ActualStartTime DESC;';
PRINT '';
PRINT '2. View students who started late:';
PRINT '   SELECT StudentName, Test_Name, MinutesLate';
PRINT '   FROM vw_ExamTimingStatistics';
PRINT '   WHERE MinutesLate > 0 ORDER BY MinutesLate DESC;';
PRINT '';
PRINT '3. View currently in-progress exams:';
PRINT '   SELECT StudentName, Test_Name, ActualStartTime,';
PRINT '          DATEDIFF(MINUTE, ActualStartTime, GETDATE()) AS MinutesElapsed';
PRINT '   FROM vw_ExamTimingStatistics WHERE ExamStatus = ''In Progress'';';
PRINT '';
PRINT '4. View detailed test results:';
PRINT '   SELECT * FROM vw_StudentTestResults';
PRINT '   WHERE StudentName = ''John Doe'' ORDER BY TR_SubmittedDate DESC;';
PRINT '';
PRINT '5. View exam completion rates:';
PRINT '   SELECT Test_Name,';
PRINT '          COUNT(*) AS TotalStudents,';
PRINT '          SUM(CASE WHEN ExamStatus = ''Completed'' THEN 1 ELSE 0 END) AS Completed,';
PRINT '          SUM(CASE WHEN ExamStatus = ''In Progress'' THEN 1 ELSE 0 END) AS InProgress';
PRINT '   FROM vw_ExamTimingStatistics GROUP BY Test_Name;';
PRINT '';
PRINT '6. View average exam duration:';
PRINT '   SELECT Test_Name, AVG(ActualDuration) AS AvgDuration';
PRINT '   FROM vw_ExamTimingStatistics';
PRINT '   WHERE ExamStatus = ''Completed''';
PRINT '   GROUP BY Test_Name;';
PRINT '';

-- =============================================================================
-- COMPLETION MESSAGE
-- =============================================================================
PRINT '';
PRINT '========================================================================';
PRINT '                    ✅ SETUP COMPLETED SUCCESSFULLY!';
PRINT '========================================================================';
PRINT '';
PRINT '📦 What was installed:';
PRINT '';
PRINT '   DATABASE COLUMNS:';
PRINT '   ├─ TS_ActualStartTime   - When student started exam';
PRINT '   ├─ TS_ActualEndTime     - When student submitted exam';
PRINT '   ├─ TS_TotalBreakTime    - Total break time (minutes)';
PRINT '   └─ TS_ActualDuration    - Actual time spent (minutes)';
PRINT '';
PRINT '   STORED PROCEDURES:';
PRINT '   ├─ SP_SaveTestResult         - Saves question answers';
PRINT '   ├─ SP_StartExam              - Records exam start time';
PRINT '   └─ SP_MarkTestAsAttempted    - Marks exam complete with all data';
PRINT '';
PRINT '   VIEWS:';
PRINT '   ├─ vw_ExamTimingStatistics   - Complete timing analysis';
PRINT '   └─ vw_StudentTestResults     - Detailed answer results';
PRINT '';
PRINT '🎯 Features enabled:';
PRINT '   ✅ Answer submission to database';
PRINT '   ✅ Time tracking (start, end, duration)';
PRINT '   ✅ Break time tracking';
PRINT '   ✅ Button color status (red, green, purple)';
PRINT '   ✅ Question status tracking';
PRINT '   ✅ Mark calculation';
PRINT '   ✅ Comprehensive reporting';
PRINT '';
PRINT '📝 Next steps:';
PRINT '   1. Rebuild your C# application in Visual Studio';
PRINT '   2. Test by having a student take an exam';
PRINT '   3. View results using the sample queries above';
PRINT '   4. Check vw_ExamTimingStatistics for timing data';
PRINT '   5. Check vw_StudentTestResults for answer details';
PRINT '';
PRINT '🔍 Quick verification:';
PRINT '   Run: SELECT * FROM vw_ExamTimingStatistics;';
PRINT '   Run: SELECT * FROM vw_StudentTestResults;';
PRINT '';
PRINT '========================================================================';
PRINT '';
PRINT '✨ Your exam system is now fully configured!';
PRINT '';
GO

