-- =============================================================================
-- Complete QuestionMaster Stored Procedures
-- =============================================================================
-- This script creates ALL stored procedures needed for Question operations:
-- 1. SELECT (View/List questions)
-- 2. INSERT (Add new questions)
-- 3. UPDATE (Edit questions)
-- 4. DELETE (Remove questions)
-- 
-- Run this in SQL Server Management Studio
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Creating QuestionMaster Stored Procedures';
PRINT '========================================';
PRINT '';

-- =============================================================================
-- 1. SELECT - Get Questions with Filtering
-- =============================================================================
IF OBJECT_ID('Sel_QuestionMaster_Select', 'P') IS NOT NULL
    DROP PROCEDURE Sel_QuestionMaster_Select;
GO

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

PRINT '1. ✅ Sel_QuestionMaster_Select created';

-- =============================================================================
-- 2. INSERT - Add New Question
-- =============================================================================
IF OBJECT_ID('SP_QuestionMaster_Insert', 'P') IS NOT NULL
    DROP PROCEDURE SP_QuestionMaster_Insert;
GO

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
            1,
            GETDATE(),
            GETDATE()
        );
        
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
        SELECT 
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END
GO

PRINT '2. ✅ SP_QuestionMaster_Insert created';

-- =============================================================================
-- 3. UPDATE - Edit Existing Question
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
        IF NOT EXISTS (SELECT 1 FROM QuestionMaster WHERE Ques_Id = @Ques_Id)
        BEGIN
            RAISERROR('Question with ID %d not found', 16, 1, @Ques_Id);
            RETURN;
        END
        
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
        
        SELECT 1 AS Success, 'Question updated successfully' AS Message;
            
    END TRY
    BEGIN CATCH
        SELECT 
            0 AS Success,
            ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

PRINT '3. ✅ SP_QuestionMaster_Update created';

-- =============================================================================
-- 4. DELETE - Remove Question (Soft/Hard Delete)
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
        SELECT 
            0 AS Success,
            ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

PRINT '4. ✅ SP_QuestionMaster_Delete created';

-- =============================================================================
-- Test & Verify
-- =============================================================================
PRINT '';
PRINT '========================================';
PRINT 'All Stored Procedures Created Successfully!';
PRINT '========================================';
PRINT '';

-- Check all procedures exist
SELECT 
    name AS [Stored Procedure Name],
    create_date AS [Created Date],
    modify_date AS [Modified Date]
FROM sys.objects
WHERE type = 'P' 
AND (
    name = 'Sel_QuestionMaster_Select' OR
    name = 'SP_QuestionMaster_Insert' OR
    name = 'SP_QuestionMaster_Update' OR
    name = 'SP_QuestionMaster_Delete'
)
ORDER BY name;

PRINT '';
PRINT '✅ Setup Complete! Your application can now:';
PRINT '   1. View questions (Index page)';
PRINT '   2. Create questions (Create page)';
PRINT '   3. Edit questions (Edit page)';
PRINT '   4. Delete questions (Delete action)';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Navigate to: http://localhost:52734/Question/Index';
PRINT '2. Click "Create New" to add a question';
PRINT '3. Or run Insert_Biology_Tissue_Questions.sql for sample data';
PRINT '';

GO
