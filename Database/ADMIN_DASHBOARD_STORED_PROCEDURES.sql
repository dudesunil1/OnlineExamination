-- =============================================================================
-- ADMIN DASHBOARD STORED PROCEDURES
-- =============================================================================
-- Creates stored procedures for the advanced admin dashboard
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Creating Admin Dashboard SPs...';
PRINT '========================================';
PRINT '';

-- =============================================================================
-- SP_GetExamTimingStatistics: Get all exam timing data
-- =============================================================================
PRINT 'Creating SP_GetExamTimingStatistics...';

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_GetExamTimingStatistics')
    DROP PROCEDURE SP_GetExamTimingStatistics;
GO

CREATE PROCEDURE SP_GetExamTimingStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT * FROM vw_ExamTimingStatistics
    ORDER BY ActualStartTime DESC;
END
GO

PRINT '✅ SP_GetExamTimingStatistics created!';
PRINT '';

-- =============================================================================
-- SP_GetStudentPerformanceStats: Get student performance data
-- =============================================================================
PRINT 'Creating SP_GetStudentPerformanceStats...';

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_GetStudentPerformanceStats')
    DROP PROCEDURE SP_GetStudentPerformanceStats;
GO

CREATE PROCEDURE SP_GetStudentPerformanceStats
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ts.TS_StudId as StudentId,
        s.Stu_Name as StudentName,
        s.Stu_Email as StudentEmail,
        COUNT(*) as TestsCompleted,
        AVG(ts.TS_Mark) as AverageScore,
        SUM(ts.TS_Mark) as TotalMarks
    FROM TestStudent ts
    INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id
    WHERE ts.TS_IsAttempted = 1
    GROUP BY ts.TS_StudId, s.Stu_Name, s.Stu_Email
    ORDER BY AverageScore DESC;
END
GO

PRINT '✅ SP_GetStudentPerformanceStats created!';
PRINT '';

-- =============================================================================
-- SP_GetSystemStatistics: Get overall system statistics
-- =============================================================================
PRINT 'Creating SP_GetSystemStatistics...';

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_GetSystemStatistics')
    DROP PROCEDURE SP_GetSystemStatistics;
GO

CREATE PROCEDURE SP_GetSystemStatistics
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        (SELECT COUNT(*) FROM StudentMaster WHERE Stu_IsActive = 1) as TotalStudents,
        (SELECT COUNT(*) FROM TestMaster WHERE Test_IsActive = 1) as TotalTests,
        (SELECT COUNT(*) FROM QuestionMaster WHERE Ques_IsActive = 1) as TotalQuestions,
        (SELECT COUNT(*) FROM SubjectMaster WHERE Sub_IsActive = 1) as TotalSubjects,
        (SELECT COUNT(*) FROM TestStudent WHERE TS_IsAttempted = 0) as PendingExams,
        (SELECT COUNT(*) FROM TestStudent WHERE TS_IsAttempted = 1) as CompletedExams,
        (SELECT COUNT(*) FROM vw_ExamTimingStatistics WHERE ExamStatus = 'In Progress') as OngoingExams,
        (SELECT AVG(Percentage) FROM vw_ExamTimingStatistics WHERE ExamStatus = 'Completed') as AverageScore;
END
GO

PRINT '✅ SP_GetSystemStatistics created!';
PRINT '';

PRINT '========================================';
PRINT '✅ All Admin Dashboard SPs Created!';
PRINT '========================================';
GO

