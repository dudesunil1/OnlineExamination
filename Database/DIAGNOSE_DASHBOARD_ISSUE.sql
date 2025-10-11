-- =============================================================================
-- DIAGNOSE DASHBOARD ISSUE - Find out why data shows 0
-- =============================================================================
-- Run this script to see what's missing
-- =============================================================================

USE OnlineExamination;
GO

PRINT '';
PRINT '========================================================================';
PRINT '              DASHBOARD DIAGNOSTIC REPORT';
PRINT '========================================================================';
PRINT '';

-- =============================================================================
-- CHECK 1: Database Tables
-- =============================================================================
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT 'CHECK 1: Database Tables';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT '';

DECLARE @StudentCount INT = (SELECT COUNT(*) FROM StudentMaster WHERE Stu_IsActive = 1);
DECLARE @TestCount INT = (SELECT COUNT(*) FROM TestMaster WHERE Test_IsActive = 1);
DECLARE @QuestionCount INT = (SELECT COUNT(*) FROM QuestionMaster WHERE Ques_IsActive = 1);
DECLARE @SubjectCount INT = (SELECT COUNT(*) FROM SubjectMaster WHERE Sub_IsActive = 1);
DECLARE @TestStudentCount INT = (SELECT COUNT(*) FROM TestStudent);

PRINT '📊 Data Summary:';
PRINT '  Students: ' + CAST(@StudentCount AS VARCHAR);
PRINT '  Tests: ' + CAST(@TestCount AS VARCHAR);
PRINT '  Questions: ' + CAST(@QuestionCount AS VARCHAR);
PRINT '  Subjects: ' + CAST(@SubjectCount AS VARCHAR);
PRINT '  Test Assignments: ' + CAST(@TestStudentCount AS VARCHAR);
PRINT '';

IF @StudentCount = 0
    PRINT '  ❌ ISSUE: No students in database!';
IF @TestCount = 0
    PRINT '  ❌ ISSUE: No tests in database!';
IF @TestStudentCount = 0
    PRINT '  ❌ ISSUE: No test assignments found!';

-- =============================================================================
-- CHECK 2: Time Tracking Columns
-- =============================================================================
PRINT '';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT 'CHECK 2: Time Tracking Columns in TestStudent Table';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT '';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualStartTime')
    PRINT '  ✅ TS_ActualStartTime column exists'
ELSE
    PRINT '  ❌ TS_ActualStartTime column MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualEndTime')
    PRINT '  ✅ TS_ActualEndTime column exists'
ELSE
    PRINT '  ❌ TS_ActualEndTime column MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualDuration')
    PRINT '  ✅ TS_ActualDuration column exists'
ELSE
    PRINT '  ❌ TS_ActualDuration column MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_TotalBreakTime')
    PRINT '  ✅ TS_TotalBreakTime column exists'
ELSE
    PRINT '  ❌ TS_TotalBreakTime column MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

-- =============================================================================
-- CHECK 3: Views
-- =============================================================================
PRINT '';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT 'CHECK 3: Required Views';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT '';

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics')
    PRINT '  ✅ vw_ExamTimingStatistics exists'
ELSE
    PRINT '  ❌ vw_ExamTimingStatistics MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_StudentTestResults')
    PRINT '  ✅ vw_StudentTestResults exists'
ELSE
    PRINT '  ❌ vw_StudentTestResults MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

-- =============================================================================
-- CHECK 4: Stored Procedures
-- =============================================================================
PRINT '';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT 'CHECK 4: Required Stored Procedures';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT '';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_SaveTestResult')
    PRINT '  ✅ SP_SaveTestResult exists'
ELSE
    PRINT '  ❌ SP_SaveTestResult MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_StartExam')
    PRINT '  ✅ SP_StartExam exists'
ELSE
    PRINT '  ❌ SP_StartExam MISSING - Run COMPLETE_EXAM_SYSTEM_SETUP.sql';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_MarkTestAsAttempted')
    PRINT '  ✅ SP_MarkTestAsAttempted exists'
ELSE
    PRINT '  ❌ SP_MarkTestAsAttempted MISSING - Run SP_MarkTestAsAttempted.sql';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetExamTimingStatistics')
    PRINT '  ✅ SP_GetExamTimingStatistics exists'
ELSE
    PRINT '  ❌ SP_GetExamTimingStatistics MISSING - Run ADMIN_DASHBOARD_STORED_PROCEDURES.sql';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetStudentPerformanceStats')
    PRINT '  ✅ SP_GetStudentPerformanceStats exists'
ELSE
    PRINT '  ❌ SP_GetStudentPerformanceStats MISSING - Run ADMIN_DASHBOARD_STORED_PROCEDURES.sql';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetSystemStatistics')
    PRINT '  ✅ SP_GetSystemStatistics exists'
ELSE
    PRINT '  ❌ SP_GetSystemStatistics MISSING - Run ADMIN_DASHBOARD_STORED_PROCEDURES.sql';

-- =============================================================================
-- CHECK 5: Test Data Availability
-- =============================================================================
PRINT '';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT 'CHECK 5: Test Data';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT '';

-- Show sample data
PRINT '📋 Sample Students:';
SELECT TOP 3 Stu_Id, Stu_Name, Stu_Email FROM StudentMaster WHERE Stu_IsActive = 1;

PRINT '';
PRINT '📋 Sample Tests:';
SELECT TOP 3 Test_Id, Test_Name, Test_Duration FROM TestMaster WHERE Test_IsActive = 1;

PRINT '';
PRINT '📋 Test Assignments (TestStudent):';
SELECT TOP 5 
    ts.TS_Id,
    t.Test_Name,
    s.Stu_Name,
    ts.TS_Expected_Date,
    ts.TS_StartTime,
    ts.TS_IsAttempted
FROM TestStudent ts
LEFT JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
LEFT JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id
ORDER BY ts.TS_Id DESC;

-- =============================================================================
-- CHECK 6: Try to Execute Stored Procedures
-- =============================================================================
PRINT '';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT 'CHECK 6: Testing Stored Procedures';
PRINT '━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━';
PRINT '';

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetSystemStatistics')
BEGIN
    PRINT '📊 Testing SP_GetSystemStatistics:';
    EXEC SP_GetSystemStatistics;
    PRINT '';
END
ELSE
BEGIN
    PRINT '❌ Cannot test - SP_GetSystemStatistics does not exist';
END

-- =============================================================================
-- DIAGNOSIS SUMMARY
-- =============================================================================
PRINT '';
PRINT '========================================================================';
PRINT '              DIAGNOSIS SUMMARY';
PRINT '========================================================================';
PRINT '';

DECLARE @IssuesFound INT = 0;

-- Check for missing columns
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('TestStudent') AND name = 'TS_ActualStartTime')
BEGIN
    PRINT '❌ ISSUE 1: Time tracking columns are missing';
    PRINT '   ACTION: Run COMPLETE_EXAM_SYSTEM_SETUP.sql';
    PRINT '';
    SET @IssuesFound = @IssuesFound + 1;
END

-- Check for missing views
IF NOT EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics')
BEGIN
    PRINT '❌ ISSUE 2: vw_ExamTimingStatistics view is missing';
    PRINT '   ACTION: Run COMPLETE_EXAM_SYSTEM_SETUP.sql';
    PRINT '';
    SET @IssuesFound = @IssuesFound + 1;
END

-- Check for missing stored procedures
IF NOT EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetSystemStatistics')
BEGIN
    PRINT '❌ ISSUE 3: Admin dashboard stored procedures are missing';
    PRINT '   ACTION: Run ADMIN_DASHBOARD_STORED_PROCEDURES.sql';
    PRINT '';
    SET @IssuesFound = @IssuesFound + 1;
END

-- Check for no data
IF @StudentCount = 0 OR @TestCount = 0 OR @TestStudentCount = 0
BEGIN
    PRINT '❌ ISSUE 4: No test data in database';
    PRINT '   ACTION: Create students, tests, and assign tests to students';
    PRINT '';
    SET @IssuesFound = @IssuesFound + 1;
END

IF @IssuesFound = 0
BEGIN
    PRINT '✅ All checks passed! Dashboard should work correctly.';
    PRINT '';
    PRINT 'If dashboard still shows zeros, check:';
    PRINT '  1. Application was rebuilt after SQL changes';
    PRINT '  2. Browser cache cleared (Ctrl+Shift+Delete)';
    PRINT '  3. Check Visual Studio Output window for errors';
END
ELSE
BEGIN
    PRINT '⚠️  Found ' + CAST(@IssuesFound AS VARCHAR) + ' issue(s)';
    PRINT '';
    PRINT '📋 QUICK FIX - Run these scripts in order:';
    PRINT '';
    PRINT '  1. COMPLETE_EXAM_SYSTEM_SETUP.sql';
    PRINT '  2. ADMIN_DASHBOARD_STORED_PROCEDURES.sql';
    PRINT '  3. CHECK_AND_ADD_TEST_DATA.sql (uncomment to add sample data)';
    PRINT '';
    PRINT 'Then rebuild your application in Visual Studio';
END

PRINT '';
PRINT '========================================================================';
GO

