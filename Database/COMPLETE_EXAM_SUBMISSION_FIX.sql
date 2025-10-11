-- =============================================================================
-- COMPLETE EXAM SUBMISSION FIX - SQL SCRIPT
-- =============================================================================
-- This script fixes the exam submission issue by:
-- 1. Creating/updating SP_SaveTestResult to save student answers
-- 2. Updating SP_MarkTestAsAttempted to save total marks
-- =============================================================================
-- Run this entire script against your OnlineExamination database
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Starting Exam Submission Fix...';
PRINT '========================================';
PRINT '';

-- =============================================================================
-- STEP 1: Create/Update SP_SaveTestResult Stored Procedure
-- =============================================================================
PRINT 'Step 1: Creating SP_SaveTestResult stored procedure...';

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
        -- Check if result already exists for this test, student, and question
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
            
            PRINT '  - Updated existing answer for Question ID: ' + CAST(@TR_QuestionId AS VARCHAR);
        END
        ELSE
        BEGIN
            -- Insert new result
            INSERT INTO TestResult (TR_TestId, TR_StudentId, TR_QuestionId, TR_Answer, TR_IsCorrect, TR_MarksObtained, TR_SubmittedDate)
            VALUES (@TR_TestId, @TR_StudentId, @TR_QuestionId, @TR_Answer, @TR_IsCorrect, @TR_MarksObtained, GETDATE());
            
            PRINT '  - Inserted new answer for Question ID: ' + CAST(@TR_QuestionId AS VARCHAR);
        END
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        PRINT '  - ERROR: ' + ERROR_MESSAGE();
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '✅ SP_SaveTestResult created successfully!';
PRINT '';

-- =============================================================================
-- STEP 2: Create/Update SP_MarkTestAsAttempted Stored Procedure
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
    @TS_Mark INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Check if TestStudent record exists
        IF NOT EXISTS (SELECT 1 FROM TestStudent 
                       WHERE TS_TestId = @TS_TestId 
                       AND TS_StudId = @TS_StudId)
        BEGIN
            PRINT '  - ERROR: TestStudent record not found';
            RETURN -1;
        END
        
        -- Update TestStudent record to mark as attempted with total marks
        UPDATE TestStudent
        SET TS_IsAttempted = 1,
            TS_Mark = @TS_Mark
        WHERE TS_TestId = @TS_TestId 
        AND TS_StudId = @TS_StudId;
        
        PRINT '  - Test marked as completed with ' + CAST(@TS_Mark AS VARCHAR) + ' marks';
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
-- STEP 3: Verify Stored Procedures
-- =============================================================================
PRINT 'Step 3: Verifying stored procedures...';
PRINT '';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_SaveTestResult')
    PRINT '✅ SP_SaveTestResult exists in database'
ELSE
    PRINT '❌ ERROR: SP_SaveTestResult NOT found!'

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_MarkTestAsAttempted')
    PRINT '✅ SP_MarkTestAsAttempted exists in database'
ELSE
    PRINT '❌ ERROR: SP_MarkTestAsAttempted NOT found!'

PRINT '';

-- =============================================================================
-- STEP 4: Show Procedure Details
-- =============================================================================
PRINT 'Step 4: Stored Procedure Details...';
PRINT '';

SELECT 
    name AS ProcedureName,
    create_date AS CreatedDate,
    modify_date AS ModifiedDate
FROM sys.procedures 
WHERE name IN ('SP_SaveTestResult', 'SP_MarkTestAsAttempted');

PRINT '';

-- =============================================================================
-- STEP 5: Test Query - View Recent Test Results
-- =============================================================================
PRINT 'Step 5: Sample query to view test results...';
PRINT '';
PRINT 'Run this query after students submit exams to verify answers are being saved:';
PRINT '';
PRINT 'SELECT tr.TR_Id, tr.TR_TestId, tr.TR_StudentId, tr.TR_QuestionId,';
PRINT '       tr.TR_Answer, tr.TR_IsCorrect, tr.TR_MarksObtained,';
PRINT '       tr.TR_SubmittedDate, s.Stu_Name, t.Test_Name';
PRINT 'FROM TestResult tr';
PRINT 'INNER JOIN StudentMaster s ON tr.TR_StudentId = s.Stu_Id';
PRINT 'INNER JOIN TestMaster t ON tr.TR_TestId = t.Test_Id';
PRINT 'ORDER BY tr.TR_SubmittedDate DESC;';
PRINT '';

-- =============================================================================
-- COMPLETION MESSAGE
-- =============================================================================
PRINT '';
PRINT '========================================';
PRINT '✅ EXAM SUBMISSION FIX COMPLETED!';
PRINT '========================================';
PRINT '';
PRINT 'What was fixed:';
PRINT '  ✅ SP_SaveTestResult - Saves individual question answers';
PRINT '  ✅ SP_MarkTestAsAttempted - Saves total marks and completion status';
PRINT '';
PRINT 'Next steps:';
PRINT '  1. Rebuild your application in Visual Studio';
PRINT '  2. Test by having a student take and submit an exam';
PRINT '  3. Verify answers are saved in TestResult table';
PRINT '  4. Verify marks are saved in TestStudent table';
PRINT '';
PRINT 'To verify answers were saved, run:';
PRINT '  SELECT * FROM TestResult ORDER BY TR_SubmittedDate DESC;';
PRINT '';
PRINT 'To verify test completion and marks, run:';
PRINT '  SELECT * FROM TestStudent WHERE TS_IsAttempted = 1;';
PRINT '';
PRINT '========================================';

GO


