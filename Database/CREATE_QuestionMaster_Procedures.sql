-- =============================================================================
-- Create Missing Stored Procedures for QuestionMaster
-- =============================================================================
-- This script creates the stored procedure needed to display questions
-- Run this in SQL Server Management Studio
-- =============================================================================

USE OnlineExamination;
GO

-- Drop existing procedures if they exist
IF OBJECT_ID('Sel_QuestionMaster_Select', 'P') IS NOT NULL
    DROP PROCEDURE Sel_QuestionMaster_Select;
GO

-- =============================================================================
-- Sel_QuestionMaster_Select - Get Questions with Filtering
-- =============================================================================
CREATE PROCEDURE Sel_QuestionMaster_Select
    @QuestionId INT = 0,
    @SubjectId INT = 0,
    @TopicId INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        qm.Ques_Id,
        qm.Ques_SubId,
        sm.Sub_Name AS Ques_SubName,
        qm.Ques_ClassId,
        cm.Name AS Ques_ClassName,
        qm.Ques_TopId,
        tm.Top_Name AS Ques_TopName,
        qm.Ques_PubId,
        pm.Pub_Name AS Ques_PubName,
        qm.Ques_Mark,
        qm.Ques_JEEMark,
        qm.Ques_Negative,
        qm.Ques_Question,
        qm.Ques_Answer,
        qm.Ques_OptionB,
        qm.Ques_OptionC,
        qm.Ques_OptionD,
        qm.Ques_SolutionDetails,
        qm.Ques_IsActive,
        qm.Ques_CreatedDate
    FROM QuestionMaster qm
    LEFT JOIN SubjectMaster sm ON qm.Ques_SubId = sm.Sub_Id
    LEFT JOIN ClassMaster cm ON qm.Ques_ClassId = cm.ID
    LEFT JOIN TopicMaster tm ON qm.Ques_TopId = tm.Top_Id
    LEFT JOIN PublicationMaster pm ON qm.Ques_PubId = pm.Pub_Id
    WHERE 
        (@QuestionId = 0 OR qm.Ques_Id = @QuestionId)
        AND (@SubjectId = 0 OR qm.Ques_SubId = @SubjectId)
        AND (@TopicId = 0 OR qm.Ques_TopId = @TopicId)
        AND ISNULL(qm.Ques_IsActive, 1) = 1
    ORDER BY qm.Ques_CreatedDate DESC, qm.Ques_Id DESC;
END
GO

PRINT '✅ Stored procedure Sel_QuestionMaster_Select created successfully!';
PRINT '';

-- =============================================================================
-- Test the Procedure
-- =============================================================================
PRINT '========================================';
PRINT 'Testing Sel_QuestionMaster_Select';
PRINT '========================================';

-- Test: Get all questions
EXEC Sel_QuestionMaster_Select 
    @QuestionId = 0, 
    @SubjectId = 0, 
    @TopicId = 0;

DECLARE @QuestionCount INT;
SELECT @QuestionCount = COUNT(*) FROM QuestionMaster WHERE ISNULL(Ques_IsActive, 1) = 1;

PRINT '';
PRINT 'Total questions in database: ' + CAST(@QuestionCount AS VARCHAR(10));
PRINT '';

IF @QuestionCount = 0
BEGIN
    PRINT '⚠️  WARNING: No questions found in the database!';
    PRINT '   Please run the Insert_Biology_Tissue_Questions.sql script to add questions.';
    PRINT '';
END
ELSE
BEGIN
    PRINT '✅ Questions are available!';
    PRINT '';
END

PRINT '========================================';
PRINT 'Script completed successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Refresh your application';
PRINT '2. Navigate to: http://localhost:52734/Question/Index';
PRINT '3. Questions should now appear in the list!';
PRINT '';

GO
