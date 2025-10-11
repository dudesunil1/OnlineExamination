-- =============================================================================
-- ADD EXAM TIME TRACKING (Start Time, End Time, Break Time)
-- =============================================================================
-- This script adds time tracking columns to TestStudent table
-- Tracks: Actual Start Time, End Time, Break Times
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Adding Exam Time Tracking...';
PRINT '========================================';
PRINT '';

-- =============================================================================
-- STEP 1: Add Time Tracking Columns to TestStudent Table (if not exists)
-- =============================================================================
PRINT 'Step 1: Checking/Adding time tracking columns to TestStudent table...';

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

-- =============================================================================
-- STEP 2: Update SP_MarkTestAsAttempted to save timing information
-- =============================================================================
PRINT 'Step 2: Updating SP_MarkTestAsAttempted stored procedure...';

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
        
        -- Calculate actual duration if both times are provided
        IF @TS_ActualStartTime IS NOT NULL AND @TS_ActualEndTime IS NOT NULL
        BEGIN
            SET @ActualDuration = DATEDIFF(MINUTE, @TS_ActualStartTime, @TS_ActualEndTime);
        END
        
        -- Update TestStudent record with all timing information
        UPDATE TestStudent
        SET TS_IsAttempted = 1,
            TS_Mark = @TS_Mark,
            TS_ActualStartTime = ISNULL(@TS_ActualStartTime, TS_ActualStartTime),
            TS_ActualEndTime = ISNULL(@TS_ActualEndTime, GETDATE()),
            TS_TotalBreakTime = @TS_TotalBreakTime,
            TS_ActualDuration = @ActualDuration
        WHERE TS_TestId = @TS_TestId 
        AND TS_StudId = @TS_StudId;
        
        PRINT '  - Test marked as completed';
        PRINT '  - Total Marks: ' + CAST(@TS_Mark AS VARCHAR);
        PRINT '  - Actual Duration: ' + CAST(@ActualDuration AS VARCHAR) + ' minutes';
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        PRINT '  - ERROR: ' + ERROR_MESSAGE();
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '✅ SP_MarkTestAsAttempted updated successfully!';
PRINT '';

-- =============================================================================
-- STEP 3: Create SP_StartExam to track when student starts exam
-- =============================================================================
PRINT 'Step 3: Creating SP_StartExam stored procedure...';

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
            
            PRINT '  - Exam start time recorded: ' + CONVERT(VARCHAR, GETDATE(), 120);
        END
        ELSE
        BEGIN
            PRINT '  - Exam already started previously';
        END
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        PRINT '  - ERROR: ' + ERROR_MESSAGE();
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '✅ SP_StartExam created successfully!';
PRINT '';

-- =============================================================================
-- STEP 4: Create view to show exam timing statistics
-- =============================================================================
PRINT 'Step 4: Creating vw_ExamTimingStatistics view...';

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
    
    -- Timing Analysis
    CASE 
        WHEN ts.TS_ActualStartTime IS NULL THEN 'Not Started'
        WHEN ts.TS_ActualEndTime IS NULL THEN 'In Progress'
        ELSE 'Completed'
    END AS ExamStatus,
    
    -- Time differences
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
    
    -- Marks
    ts.TS_Mark AS MarksObtained,
    ISNULL(t.Test_TotalMarks, t.Test_Mark) AS TotalMarks,
    ts.TS_IsAttempted AS IsCompleted
    
FROM TestStudent ts
INNER JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id;
GO

PRINT '✅ vw_ExamTimingStatistics view created successfully!';
PRINT '';

-- =============================================================================
-- STEP 5: Verification
-- =============================================================================
PRINT 'Step 5: Verifying installation...';
PRINT '';

-- Check columns
PRINT 'Checking columns in TestStudent table:';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'TestStudent'
AND COLUMN_NAME IN ('TS_ActualStartTime', 'TS_ActualEndTime', 'TS_TotalBreakTime', 'TS_ActualDuration')
ORDER BY COLUMN_NAME;

PRINT '';

-- Check stored procedures
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_MarkTestAsAttempted')
    PRINT '✅ SP_MarkTestAsAttempted exists'
ELSE
    PRINT '❌ ERROR: SP_MarkTestAsAttempted NOT found!'

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_StartExam')
    PRINT '✅ SP_StartExam exists'
ELSE
    PRINT '❌ ERROR: SP_StartExam NOT found!'

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics')
    PRINT '✅ vw_ExamTimingStatistics view exists'
ELSE
    PRINT '❌ ERROR: vw_ExamTimingStatistics NOT found!'

PRINT '';

-- =============================================================================
-- STEP 6: Sample Queries
-- =============================================================================
PRINT '';
PRINT '========================================';
PRINT 'SAMPLE QUERIES';
PRINT '========================================';
PRINT '';
PRINT '-- View exam timing statistics for all tests:';
PRINT 'SELECT * FROM vw_ExamTimingStatistics ORDER BY ActualStartTime DESC;';
PRINT '';
PRINT '-- View students who started late:';
PRINT 'SELECT StudentName, Test_Name, MinutesLate';
PRINT 'FROM vw_ExamTimingStatistics';
PRINT 'WHERE MinutesLate > 0';
PRINT 'ORDER BY MinutesLate DESC;';
PRINT '';
PRINT '-- View students who took longer than scheduled:';
PRINT 'SELECT StudentName, Test_Name, ActualDuration, ScheduledDuration';
PRINT 'FROM vw_ExamTimingStatistics';
PRINT 'WHERE DurationDifference > 0';
PRINT 'ORDER BY DurationDifference DESC;';
PRINT '';
PRINT '-- View currently in-progress exams:';
PRINT 'SELECT * FROM vw_ExamTimingStatistics WHERE ExamStatus = ''In Progress'';';
PRINT '';

-- =============================================================================
-- COMPLETION MESSAGE
-- =============================================================================
PRINT '';
PRINT '========================================';
PRINT '✅ EXAM TIME TRACKING SETUP COMPLETED!';
PRINT '========================================';
PRINT '';
PRINT 'What was added:';
PRINT '  ✅ TS_ActualStartTime - When student actually started exam';
PRINT '  ✅ TS_ActualEndTime - When student actually submitted exam';
PRINT '  ✅ TS_TotalBreakTime - Total break time in minutes';
PRINT '  ✅ TS_ActualDuration - Actual time spent on exam';
PRINT '  ✅ SP_MarkTestAsAttempted - Updated to save timing data';
PRINT '  ✅ SP_StartExam - New procedure to record start time';
PRINT '  ✅ vw_ExamTimingStatistics - View for timing analysis';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Update C# code to call SP_StartExam when student starts';
PRINT '  2. Pass timing parameters when calling SP_MarkTestAsAttempted';
PRINT '  3. Test by having a student take an exam';
PRINT '  4. View timing stats using vw_ExamTimingStatistics';
PRINT '';
PRINT '========================================';
GO

