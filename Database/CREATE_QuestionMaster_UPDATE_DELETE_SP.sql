-- =============================================================================
-- Create UPDATE and DELETE Stored Procedures for QuestionMaster
-- =============================================================================
-- This script creates UPDATE and DELETE stored procedures
-- Run this in SQL Server Management Studio
-- =============================================================================

USE OnlineExamination;
GO

-- =============================================================================
-- SP_QuestionMaster_Update - Update Existing Question
-- =============================================================================
IF OBJECT_ID('SP_QuestionMaster_Update', 'P') IS NOT NULL
    DROP PROCEDURE SP_QuestionMaster_Update;
GO

CREATE PROCEDURE SP_QuestionMaster_Update
    @Ques_Id INT,
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
        -- Check if question exists
        IF NOT EXISTS (SELECT 1 FROM QuestionMaster WHERE Ques_Id = @Ques_Id)
        BEGIN
            RAISERROR('Question with ID %d not found', 16, 1, @Ques_Id);
            RETURN;
        END
        
        -- Update the question
        UPDATE QuestionMaster
        SET 
            Ques_SubId = @Ques_SubId,
            Ques_ClassId = @Ques_ClassId,
            Ques_TopId = @Ques_TopId,
            Ques_PubId = @Ques_PubId,
            Ques_Mark = @Ques_Mark,
            Ques_JEEMark = @Ques_JEEMark,
            Ques_Negative = @Ques_Negative,
            Ques_Question = @Ques_Question,
            Ques_Answer = @Ques_Answer,
            Ques_OptionB = @Ques_OptionB,
            Ques_OptionC = @Ques_OptionC,
            Ques_OptionD = @Ques_OptionD,
            Ques_SolutionDetails = @Ques_SolutionDetails,
            Ques_ModifiedDate = GETDATE()
        WHERE Ques_Id = @Ques_Id;
        
        -- Return success
        SELECT 1 AS Success, 'Question updated successfully' AS Message;
            
    END TRY
    BEGIN CATCH
        -- Return error
        SELECT 
            0 AS Success,
            ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

PRINT '✅ Stored procedure SP_QuestionMaster_Update created successfully!';
PRINT '';

-- =============================================================================
-- SP_QuestionMaster_Delete - Soft Delete Question
-- =============================================================================
IF OBJECT_ID('SP_QuestionMaster_Delete', 'P') IS NOT NULL
    DROP PROCEDURE SP_QuestionMaster_Delete;
GO

CREATE PROCEDURE SP_QuestionMaster_Delete
    @Ques_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Check if question exists
        IF NOT EXISTS (SELECT 1 FROM QuestionMaster WHERE Ques_Id = @Ques_Id)
        BEGIN
            RAISERROR('Question with ID %d not found', 16, 1, @Ques_Id);
            RETURN;
        END
        
        -- Check if question is used in any tests
        DECLARE @TestCount INT;
        SELECT @TestCount = COUNT(*) 
        FROM TestQuestions 
        WHERE TQ_QuestionId = @Ques_Id AND ISNULL(TQ_IsActive, 1) = 1;
        
        IF @TestCount > 0
        BEGIN
            -- Soft delete (mark as inactive)
            UPDATE QuestionMaster
            SET 
                Ques_IsActive = 0,
                Ques_ModifiedDate = GETDATE()
            WHERE Ques_Id = @Ques_Id;
            
            SELECT 1 AS Success, 
                   'Question marked as inactive (used in ' + CAST(@TestCount AS VARCHAR(10)) + ' test(s))' AS Message;
        END
        ELSE
        BEGIN
            -- Hard delete (actually remove)
            DELETE FROM QuestionMaster
            WHERE Ques_Id = @Ques_Id;
            
            SELECT 1 AS Success, 'Question deleted successfully' AS Message;
        END
            
    END TRY
    BEGIN CATCH
        -- Return error
        SELECT 
            0 AS Success,
            ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

PRINT '✅ Stored procedure SP_QuestionMaster_Delete created successfully!';
PRINT '';

-- =============================================================================
-- Test the Procedures
-- =============================================================================
PRINT '========================================';
PRINT 'All Question Stored Procedures Created:';
PRINT '========================================';
PRINT '1. ✅ SP_QuestionMaster_Insert';
PRINT '2. ✅ SP_QuestionMaster_Update';
PRINT '3. ✅ SP_QuestionMaster_Delete';
PRINT '';
PRINT '========================================';
PRINT 'Script completed successfully!';
PRINT '========================================';
PRINT '';

GO
