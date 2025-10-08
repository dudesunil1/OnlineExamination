-- =============================================================================
-- Add Ques_ClassId Column to QuestionMaster Table
-- =============================================================================
-- This script adds the missing Ques_ClassId column to QuestionMaster table
-- Run this in SQL Server Management Studio
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Adding Ques_ClassId Column';
PRINT '========================================';
PRINT '';

-- Check if column already exists
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID('QuestionMaster') 
    AND name = 'Ques_ClassId'
)
BEGIN
    PRINT 'Adding Ques_ClassId column to QuestionMaster table...';
    
    -- Add the column
    ALTER TABLE QuestionMaster
    ADD Ques_ClassId INT NULL;
    
    PRINT '✅ Column Ques_ClassId added successfully!';
    PRINT '';
    
    -- Add foreign key constraint to ClassMaster
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.foreign_keys 
        WHERE name = 'FK_QuestionMaster_ClassMaster'
    )
    BEGIN
        ALTER TABLE QuestionMaster
        ADD CONSTRAINT FK_QuestionMaster_ClassMaster
        FOREIGN KEY (Ques_ClassId) REFERENCES ClassMaster(ID);
        
        PRINT '✅ Foreign key constraint added!';
        PRINT '';
    END
    
    -- Update existing questions with a default class
    DECLARE @DefaultClassId INT;
    SELECT TOP 1 @DefaultClassId = ID FROM ClassMaster WHERE IsActive = 1;
    
    IF @DefaultClassId IS NOT NULL
    BEGIN
        UPDATE QuestionMaster
        SET Ques_ClassId = @DefaultClassId
        WHERE Ques_ClassId IS NULL;
        
        PRINT '✅ Existing questions updated with default Class ID: ' + CAST(@DefaultClassId AS VARCHAR(10));
        PRINT '';
    END
    ELSE
    BEGIN
        PRINT '⚠️  WARNING: No classes found. Please add classes first.';
        PRINT '';
    END
END
ELSE
BEGIN
    PRINT '⚠️  Column Ques_ClassId already exists!';
    PRINT '';
END

-- Show updated table structure
PRINT '========================================';
PRINT 'QuestionMaster Table Structure:';
PRINT '========================================';

SELECT 
    COLUMN_NAME AS [Column Name],
    DATA_TYPE AS [Data Type],
    CHARACTER_MAXIMUM_LENGTH AS [Max Length],
    IS_NULLABLE AS [Nullable]
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'QuestionMaster'
ORDER BY ORDINAL_POSITION;

PRINT '';
PRINT '========================================';
PRINT 'Script Completed Successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Now run: ALL_QuestionMaster_StoredProcedures.sql';
PRINT '2. Rebuild your application';
PRINT '3. Test creating a question';
PRINT '';

GO
