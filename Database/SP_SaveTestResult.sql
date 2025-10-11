-- Stored Procedure to Save Test Result
-- This procedure saves individual question answers to the TestResult table

USE OnlineExamination;
GO

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
        END
        ELSE
        BEGIN
            -- Insert new result
            INSERT INTO TestResult (TR_TestId, TR_StudentId, TR_QuestionId, TR_Answer, TR_IsCorrect, TR_MarksObtained, TR_SubmittedDate)
            VALUES (@TR_TestId, @TR_StudentId, @TR_QuestionId, @TR_Answer, @TR_IsCorrect, @TR_MarksObtained, GETDATE());
        END
    END TRY
    BEGIN CATCH
        -- Return error
        SELECT ERROR_MESSAGE() AS ErrorMessage;
        RETURN -1;
    END CATCH
END
GO

PRINT 'Stored Procedure SP_SaveTestResult created successfully.';
GO


