-- =============================================================================
-- Fix for SP_GetQuestionsPerSubject Stored Procedure
-- =============================================================================
-- This script ensures the stored procedure returns the TestID field correctly
-- Run this if you're still experiencing navigation issues after code fix
-- =============================================================================

USE OnlineExamination;
GO

-- Drop existing procedure if it exists
IF OBJECT_ID('SP_GetQuestionsPerSubject', 'P') IS NOT NULL
    DROP PROCEDURE SP_GetQuestionsPerSubject;
GO

-- Create or recreate the stored procedure
CREATE PROCEDURE SP_GetQuestionsPerSubject
    @TS_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Return test details with subject information and question count
    SELECT 
        ts.TS_Id,
        ts.TS_TestId AS TestID,              -- This is critical for navigation!
        ISNULL(s.Sub_Name, 'General Test') AS SubjectName,
        tm.Test_Duration AS TestDuration,
        ts.TS_StartTime AS TestStartTime,
        ts.TS_End_Time AS TestEndTime,
        tm.Test_TotalMarks AS TestMark,
        (
            SELECT COUNT(DISTINCT tq.TQ_QuestionId) 
            FROM TestQuestions tq 
            WHERE tq.TQ_TestId = ts.TS_TestId AND ISNULL(tq.TQ_IsActive, 1) = 1
        ) AS NumberOfQuestions
    FROM TestStudent ts
    INNER JOIN TestMaster tm ON ts.TS_TestId = tm.Test_Id
    LEFT JOIN TestQuestions tq_first ON tm.Test_Id = tq_first.TQ_TestId
    LEFT JOIN QuestionMaster qm ON tq_first.TQ_QuestionId = qm.Ques_Id
    LEFT JOIN SubjectMaster s ON qm.Ques_SubId = s.Sub_Id
    WHERE ts.TS_Id = @TS_Id
    GROUP BY 
        ts.TS_Id,
        ts.TS_TestId,
        s.Sub_Name,
        tm.Test_Duration,
        ts.TS_StartTime,
        ts.TS_End_Time,
        tm.Test_TotalMarks;
END
GO

-- =============================================================================
-- Test the stored procedure
-- =============================================================================

PRINT '========================================';
PRINT 'Testing SP_GetQuestionsPerSubject';
PRINT '========================================';

-- Test with a sample TS_Id (replace 2040 with your actual TS_Id)
DECLARE @TestTsId INT;

-- Get the first available TS_Id for testing
SELECT TOP 1 @TestTsId = TS_Id FROM TestStudent;

IF @TestTsId IS NOT NULL
BEGIN
    PRINT 'Testing with TS_Id: ' + CAST(@TestTsId AS VARCHAR(10));
    PRINT '';
    
    EXEC SP_GetQuestionsPerSubject @TS_Id = @TestTsId;
    
    PRINT '';
    PRINT '✅ Stored procedure executed successfully';
    PRINT '';
    PRINT '⚠️  IMPORTANT: Check that the TestID column has a NON-ZERO value!';
    PRINT '';
END
ELSE
BEGIN
    PRINT '⚠️  No test assignments found in TestStudent table';
    PRINT '   Please assign tests to students first';
END

PRINT '========================================';
PRINT 'Verification Query';
PRINT '========================================';

-- Show all test assignments with their IDs for verification
SELECT 
    ts.TS_Id AS 'TestStudent Record ID (TS_Id)',
    ts.TS_TestId AS 'Test Master ID (Should match TestID in SP)',
    ts.TS_StudId AS 'Student ID',
    tm.Test_Name AS 'Test Name',
    s.Stu_Name AS 'Student Name'
FROM TestStudent ts
INNER JOIN TestMaster tm ON ts.TS_TestId = tm.Test_Id
INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id
ORDER BY ts.TS_Id DESC;

PRINT '';
PRINT '========================================';
PRINT 'Script completed successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Verify that TestID column appears in the result set above';
PRINT '2. Verify that TestID values are NON-ZERO';
PRINT '3. Test the application navigation again';
PRINT '4. If issues persist, check the TESTDETAILS_NAVIGATION_FIX.md document';
PRINT '';

GO

