-- =============================================================================
-- CHECK AND ADD TEST DATA
-- =============================================================================
-- This script checks if you have tests in the database
-- and optionally adds sample upcoming tests for testing
-- =============================================================================

USE OnlineExamination;
GO

PRINT '========================================';
PRINT 'Checking Test Data...';
PRINT '========================================';
PRINT '';

-- =============================================================================
-- CHECK EXISTING DATA
-- =============================================================================

PRINT '1. Checking Students:';
SELECT COUNT(*) AS TotalStudents FROM StudentMaster WHERE Stu_IsActive = 1;
SELECT TOP 5 Stu_Id, Stu_Name, Stu_Email FROM StudentMaster WHERE Stu_IsActive = 1;
PRINT '';

PRINT '2. Checking Tests:';
SELECT COUNT(*) AS TotalTests FROM TestMaster WHERE Test_IsActive = 1;
SELECT TOP 5 Test_Id, Test_Name, Test_Duration, Test_Mark FROM TestMaster WHERE Test_IsActive = 1;
PRINT '';

PRINT '3. Checking Test Assignments (TestStudent):';
SELECT COUNT(*) AS TotalAssignments FROM TestStudent;
SELECT TOP 5 
    TS_Id,
    TS_TestId,
    TS_StudId,
    TS_Expected_Date,
    TS_StartTime,
    TS_End_Time,
    TS_IsAttempted
FROM TestStudent
ORDER BY TS_Expected_Date DESC;
PRINT '';

PRINT '4. Checking Upcoming Tests (Not Attempted):';
SELECT COUNT(*) AS UpcomingTestsCount 
FROM TestStudent 
WHERE TS_IsAttempted = 0 
AND TS_Expected_Date >= CAST(GETDATE() AS DATE);

SELECT 
    ts.TS_Id,
    ts.TS_TestId,
    t.Test_Name,
    ts.TS_StudId,
    s.Stu_Name,
    ts.TS_Expected_Date,
    ts.TS_StartTime,
    ts.TS_End_Time,
    ts.TS_IsAttempted
FROM TestStudent ts
INNER JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id
WHERE ts.TS_IsAttempted = 0
AND ts.TS_Expected_Date >= CAST(GETDATE() AS DATE)
ORDER BY ts.TS_Expected_Date, ts.TS_StartTime;

PRINT '';

-- =============================================================================
-- ADD SAMPLE UPCOMING TESTS (OPTIONAL)
-- =============================================================================
PRINT '';
PRINT '========================================';
PRINT 'ADD SAMPLE UPCOMING TESTS';
PRINT '========================================';
PRINT '';
PRINT 'Do you want to add sample upcoming tests? (Comment/Uncomment below)';
PRINT '';

-- UNCOMMENT THIS SECTION TO ADD SAMPLE TESTS
/*
DECLARE @StudentId INT;
DECLARE @TestId INT;

-- Get first active student
SELECT TOP 1 @StudentId = Stu_Id FROM StudentMaster WHERE Stu_IsActive = 1;

-- Get first active test
SELECT TOP 1 @TestId = Test_Id FROM TestMaster WHERE Test_IsActive = 1;

IF @StudentId IS NOT NULL AND @TestId IS NOT NULL
BEGIN
    PRINT 'Adding sample upcoming tests for Student ID: ' + CAST(@StudentId AS VARCHAR);
    PRINT 'Using Test ID: ' + CAST(@TestId AS VARCHAR);
    PRINT '';
    
    -- Delete existing upcoming tests for this student (optional - be careful!)
    -- DELETE FROM TestStudent WHERE TS_StudId = @StudentId AND TS_IsAttempted = 0;
    
    -- Add Test 1: Tomorrow at 10:00 AM
    IF NOT EXISTS (SELECT 1 FROM TestStudent WHERE TS_StudId = @StudentId AND TS_TestId = @TestId AND TS_Expected_Date = CAST(DATEADD(DAY, 1, GETDATE()) AS DATE))
    BEGIN
        INSERT INTO TestStudent (TS_TestId, TS_StudId, TS_Expected_Date, TS_StartTime, TS_End_Time, TS_IsAttempted, TS_AddTime)
        VALUES (@TestId, @StudentId, 
                CAST(DATEADD(DAY, 1, GETDATE()) AS DATE),
                CAST(DATEADD(DAY, 1, GETDATE()) AS DATE) + CAST('10:00:00' AS TIME),
                CAST(DATEADD(DAY, 1, GETDATE()) AS DATE) + CAST('12:00:00' AS TIME),
                0,
                GETDATE());
        PRINT '✅ Added: Tomorrow at 10:00 AM';
    END
    
    -- Add Test 2: In 2 hours from now
    DECLARE @TwoHoursFromNow DATETIME = DATEADD(HOUR, 2, GETDATE());
    DECLARE @FourHoursFromNow DATETIME = DATEADD(HOUR, 4, GETDATE());
    
    INSERT INTO TestStudent (TS_TestId, TS_StudId, TS_Expected_Date, TS_StartTime, TS_End_Time, TS_IsAttempted, TS_AddTime)
    VALUES (@TestId, @StudentId, 
            CAST(@TwoHoursFromNow AS DATE),
            @TwoHoursFromNow,
            @FourHoursFromNow,
            0,
            GETDATE());
    PRINT '✅ Added: In 2 hours from now';
    
    -- Add Test 3: In 3 days at 2:00 PM
    INSERT INTO TestStudent (TS_TestId, TS_StudId, TS_Expected_Date, TS_StartTime, TS_End_Time, TS_IsAttempted, TS_AddTime)
    VALUES (@TestId, @StudentId, 
            CAST(DATEADD(DAY, 3, GETDATE()) AS DATE),
            CAST(DATEADD(DAY, 3, GETDATE()) AS DATE) + CAST('14:00:00' AS TIME),
            CAST(DATEADD(DAY, 3, GETDATE()) AS DATE) + CAST('16:00:00' AS TIME),
            0,
            GETDATE());
    PRINT '✅ Added: In 3 days at 2:00 PM';
    
    PRINT '';
    PRINT '✅ Sample tests added successfully!';
END
ELSE
BEGIN
    PRINT '❌ Cannot add tests: No active student or test found';
    PRINT '   Please create a student and test first';
END
*/

PRINT '';
PRINT '========================================';
PRINT 'TROUBLESHOOTING';
PRINT '========================================';
PRINT '';
PRINT 'If no upcoming tests show on dashboard:';
PRINT '';
PRINT '1. Check if you have active students:';
PRINT '   SELECT * FROM StudentMaster WHERE Stu_IsActive = 1;';
PRINT '';
PRINT '2. Check if you have active tests:';
PRINT '   SELECT * FROM TestMaster WHERE Test_IsActive = 1;';
PRINT '';
PRINT '3. Check if tests are assigned to students:';
PRINT '   SELECT * FROM TestStudent WHERE TS_IsAttempted = 0;';
PRINT '';
PRINT '4. Check if test dates are in the future:';
PRINT '   SELECT TS_Expected_Date, TS_StartTime FROM TestStudent';
PRINT '   WHERE TS_Expected_Date >= CAST(GETDATE() AS DATE);';
PRINT '';
PRINT '5. To manually add an upcoming test:';
PRINT '   -- Replace @TestId and @StudentId with actual IDs';
PRINT '   INSERT INTO TestStudent (TS_TestId, TS_StudId, TS_Expected_Date, TS_StartTime, TS_End_Time, TS_IsAttempted, TS_AddTime)';
PRINT '   VALUES (1, 1, DATEADD(DAY, 1, GETDATE()), DATEADD(DAY, 1, GETDATE()) + CAST(''10:00'' AS TIME), DATEADD(DAY, 1, GETDATE()) + CAST(''12:00'' AS TIME), 0, GETDATE());';
PRINT '';

GO

