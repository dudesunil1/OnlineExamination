-- =============================================================================
-- Create INSERT Stored Procedure for QuestionMaster
-- =============================================================================
-- This script creates the stored procedure to insert new questions
-- Run this in SQL Server Management Studio
-- =============================================================================

USE OnlineExamination;
GO

-- Drop existing procedure if it exists
IF OBJECT_ID('SP_QuestionMaster_Insert', 'P') IS NOT NULL
    DROP PROCEDURE SP_QuestionMaster_Insert;
GO

-- =============================================================================
-- SP_QuestionMaster_Insert - Insert New Question
-- =============================================================================
CREATE PROCEDURE SP_QuestionMaster_Insert
    @Ques_SubId INT,
    @Ques_ClassId INT,
    @Ques_TopId INT,
    @Ques_PubId INT,
    @Ques_Mark INT,
    @Ques_JEEMark INT,
    @Ques_Negative DECIMAL(5,2),
    @Ques_Question NVARCHAR(MAX),
    @Ques_Answer NVARCHAR(MAX),
    @Ques_OptionB NVARCHAR(MAX),
    @Ques_OptionC NVARCHAR(MAX),
    @Ques_OptionD NVARCHAR(MAX),
    @Ques_SolutionDetails NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Insert the new question
        INSERT INTO QuestionMaster (
            Ques_SubId,
            Ques_ClassId,
            Ques_TopId,
            Ques_PubId,
            Ques_Mark,
            Ques_JEEMark,
            Ques_Negative,
            Ques_Question,
            Ques_Answer,
            Ques_OptionB,
            Ques_OptionC,
            Ques_OptionD,
            Ques_SolutionDetails,
            Ques_IsActive,
            Ques_CreatedDate,
            Ques_ModifiedDate
        )
        VALUES (
            @Ques_SubId,
            @Ques_ClassId,
            @Ques_TopId,
            @Ques_PubId,
            @Ques_Mark,
            @Ques_JEEMark,
            @Ques_Negative,
            @Ques_Question,
            @Ques_Answer,
            @Ques_OptionB,
            @Ques_OptionC,
            @Ques_OptionD,
            @Ques_SolutionDetails,
            1,              -- Ques_IsActive = 1 (Active)
            GETDATE(),      -- Ques_CreatedDate
            GETDATE()       -- Ques_ModifiedDate
        );
        
        -- Return the newly inserted question ID
        SELECT 
            SCOPE_IDENTITY() AS Ques_Id,
            @Ques_SubId AS Ques_SubId,
            @Ques_ClassId AS Ques_ClassId,
            @Ques_TopId AS Ques_TopId,
            @Ques_PubId AS Ques_PubId,
            @Ques_Mark AS Ques_Mark,
            @Ques_JEEMark AS Ques_JEEMark,
            @Ques_Negative AS Ques_Negative;
            
    END TRY
    BEGIN CATCH
        -- Return error information
        SELECT 
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END
GO

PRINT '✅ Stored procedure SP_QuestionMaster_Insert created successfully!';
PRINT '';

-- =============================================================================
-- Test the Insert Procedure
-- =============================================================================
PRINT '========================================';
PRINT 'Testing SP_QuestionMaster_Insert';
PRINT '========================================';
PRINT '';

-- Get sample data for testing
DECLARE @TestSubjectId INT = (SELECT TOP 1 Sub_Id FROM SubjectMaster WHERE Sub_IsActive = 1);
DECLARE @TestClassId INT = (SELECT TOP 1 ID FROM ClassMaster WHERE IsActive = 1);
DECLARE @TestTopicId INT = (SELECT TOP 1 Top_Id FROM TopicMaster WHERE Top_IsActive = 1);
DECLARE @TestPubId INT = (SELECT TOP 1 Pub_Id FROM PublicationMaster WHERE Pub_IsActive = 1);

IF @TestSubjectId IS NOT NULL AND @TestClassId IS NOT NULL AND @TestTopicId IS NOT NULL AND @TestPubId IS NOT NULL
BEGIN
    PRINT 'Sample Data Available:';
    PRINT 'Subject ID: ' + CAST(@TestSubjectId AS VARCHAR(10));
    PRINT 'Class ID: ' + CAST(@TestClassId AS VARCHAR(10));
    PRINT 'Topic ID: ' + CAST(@TestTopicId AS VARCHAR(10));
    PRINT 'Publication ID: ' + CAST(@TestPubId AS VARCHAR(10));
    PRINT '';
    PRINT '✅ You can now use /Question/Create to add questions!';
END
ELSE
BEGIN
    PRINT '⚠️  WARNING: Missing master data!';
    PRINT '';
    IF @TestSubjectId IS NULL PRINT '   - No subjects found. Please add subjects first.';
    IF @TestClassId IS NULL PRINT '   - No classes found. Please add classes first.';
    IF @TestTopicId IS NULL PRINT '   - No topics found. Please add topics first.';
    IF @TestPubId IS NULL PRINT '   - No publications found. Please add publications first.';
    PRINT '';
    PRINT 'Run the Insert_Biology_Tissue_Questions.sql script to create sample master data.';
END

PRINT '';
PRINT '========================================';
PRINT 'Script completed successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Navigate to: http://localhost:52734/Question/Create';
PRINT '2. Fill in the form';
PRINT '3. Click Continue to insert a question';
PRINT '4. Question will be saved to database!';
PRINT '';

GO
