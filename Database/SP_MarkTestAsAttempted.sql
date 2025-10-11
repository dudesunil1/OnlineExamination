-- =============================================================================
-- Mark Test as Attempted (Completed) - WITH TIME TRACKING
-- =============================================================================
-- This marks a test as completed when student submits
-- Saves: Marks, Start Time, End Time, Duration, Break Time
-- Completed tests are removed from the test list
-- =============================================================================

USE OnlineExamination;
GO

IF OBJECT_ID('SP_MarkTestAsAttempted', 'P') IS NOT NULL
    DROP PROCEDURE SP_MarkTestAsAttempted;
GO

CREATE PROCEDURE SP_MarkTestAsAttempted
    @TS_TestId INT,
    @TS_StudId INT,
    @TS_Mark INT = 0,
    @TS_ActualStartTime DATETIME = NULL,
    @TS_ActualEndTime DATETIME = NULL,
    @TS_TotalBreakTime INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        DECLARE @ActualDuration INT = 0;
        DECLARE @StartTime DATETIME;
        DECLARE @EndTime DATETIME;
        
        -- Get existing start time if not provided (student might have started earlier)
        IF @TS_ActualStartTime IS NULL
        BEGIN
            SELECT @StartTime = TS_ActualStartTime 
            FROM TestStudent 
            WHERE TS_TestId = @TS_TestId AND TS_StudId = @TS_StudId;
        END
        ELSE
        BEGIN
            SET @StartTime = @TS_ActualStartTime;
        END
        
        -- Set end time (use provided or current time)
        SET @EndTime = ISNULL(@TS_ActualEndTime, GETDATE());
        
        -- Calculate actual duration if start time exists
        IF @StartTime IS NOT NULL
        BEGIN
            SET @ActualDuration = DATEDIFF(MINUTE, @StartTime, @EndTime);
            
            -- Ensure duration is not negative
            IF @ActualDuration < 0
                SET @ActualDuration = 0;
        END
        
        -- Update TestStudent record with all information
        UPDATE TestStudent
        SET TS_IsAttempted = 1,
            TS_Mark = @TS_Mark,
            TS_ActualStartTime = ISNULL(TS_ActualStartTime, @StartTime),
            TS_ActualEndTime = @EndTime,
            TS_TotalBreakTime = @TS_TotalBreakTime,
            TS_ActualDuration = @ActualDuration
        WHERE TS_TestId = @TS_TestId 
        AND TS_StudId = @TS_StudId;
        
        -- Return success
        IF @@ROWCOUNT > 0
        BEGIN
            SELECT 1 AS Success, 
                   'Test marked as completed' AS Message,
                   @TS_Mark AS TotalMarks,
                   @ActualDuration AS DurationMinutes;
        END
        ELSE
        BEGIN
            SELECT 0 AS Success, 
                   'Test record not found' AS Message;
        END
        
        RETURN 0; -- Success
    END TRY
    BEGIN CATCH
        -- Return error information
        SELECT 0 AS Success, 
               ERROR_MESSAGE() AS Message;
        RETURN -1; -- Error
    END CATCH
END
GO

PRINT '✅ SP_MarkTestAsAttempted created successfully!';
PRINT '';
PRINT 'Features:';
PRINT '  ✅ Marks test as completed (TS_IsAttempted = 1)';
PRINT '  ✅ Saves total marks earned';
PRINT '  ✅ Records start time (if provided)';
PRINT '  ✅ Records end time';
PRINT '  ✅ Calculates actual duration';
PRINT '  ✅ Tracks break time';
PRINT '';
PRINT 'This procedure is called automatically when student submits exam.';
PRINT 'Completed tests will be hidden from the Test List.';
GO


