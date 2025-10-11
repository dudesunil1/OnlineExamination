-- =============================================================================
-- FIX COLUMN NAMES - Add Test_TotalMarks if missing
-- =============================================================================
-- Your database uses Test_Mark, but we need Test_TotalMarks for reports
-- This script adds Test_TotalMarks column or creates an alias
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Checking TestMaster table columns...';
PRINT '========================================';
PRINT '';

-- Check if Test_TotalMarks column exists
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID('TestMaster') 
               AND name = 'Test_TotalMarks')
BEGIN
    PRINT '❌ Test_TotalMarks column does NOT exist';
    PRINT '✅ Test_Mark column exists';
    PRINT '';
    PRINT 'Solution: Adding Test_TotalMarks column...';
    
    -- Add Test_TotalMarks column
    ALTER TABLE TestMaster 
    ADD Test_TotalMarks INT NULL;
    
    PRINT '✅ Test_TotalMarks column added!';
    PRINT '';
    
    -- Copy data from Test_Mark to Test_TotalMarks
    PRINT 'Copying data from Test_Mark to Test_TotalMarks...';
    UPDATE TestMaster 
    SET Test_TotalMarks = Test_Mark
    WHERE Test_TotalMarks IS NULL;
    
    PRINT '✅ Data copied successfully!';
    PRINT '';
    
    -- Make it NOT NULL if data exists
    IF NOT EXISTS (SELECT * FROM TestMaster WHERE Test_TotalMarks IS NULL)
    BEGIN
        ALTER TABLE TestMaster 
        ALTER COLUMN Test_TotalMarks INT NOT NULL;
        PRINT '✅ Set Test_TotalMarks as NOT NULL';
    END
END
ELSE
BEGIN
    PRINT '✅ Test_TotalMarks column already exists!';
END

PRINT '';
PRINT '========================================';
PRINT 'Column Fix Complete!';
PRINT '========================================';
GO


