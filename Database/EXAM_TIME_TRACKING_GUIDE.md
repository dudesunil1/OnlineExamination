# Exam Time Tracking Setup Guide

## Overview
This guide explains how to add start time, end time, and break time tracking to your exam system.

## What's Included

### Time Fields Tracked:
1. **TS_ActualStartTime** - When student actually started the exam
2. **TS_ActualEndTime** - When student submitted the exam  
3. **TS_TotalBreakTime** - Total break time during exam (in minutes)
4. **TS_ActualDuration** - Actual time spent on exam (in minutes)

### SQL Components:
1. **New Table Columns** - Added to TestStudent table
2. **SP_MarkTestAsAttempted** - Updated to save timing data
3. **SP_StartExam** - New procedure to record start time
4. **vw_ExamTimingStatistics** - View for timing analysis and reports

## Installation Steps

### Step 1: Run SQL Script

Open SQL Server Management Studio and run:

**File:** `D:\Application\Database\ADD_EXAM_TIME_TRACKING.sql`

This script will:
- ✅ Add time tracking columns to TestStudent table
- ✅ Update SP_MarkTestAsAttempted
- ✅ Create SP_StartExam procedure
- ✅ Create vw_ExamTimingStatistics view

### Step 2: Rebuild Application

1. Open Visual Studio
2. Click **Build** → **Rebuild Solution**
3. Verify no errors

### Step 3: Test

1. Login as a student
2. Start a test
3. Answer questions
4. Submit the test

## Verify Time Tracking Works

Run these queries in SQL Server Management Studio:

```sql
-- View all exam timing statistics
SELECT * FROM vw_ExamTimingStatistics 
ORDER BY ActualStartTime DESC;

-- View specific student's exam times
SELECT 
    StudentName,
    Test_Name,
    ScheduledStartTime,
    ActualStartTime,
    ActualEndTime,
    ActualDuration,
    TotalBreakTime,
    ExamStatus
FROM vw_ExamTimingStatistics
WHERE StudentName = 'John Doe';

-- View students who started late
SELECT 
    StudentName,
    Test_Name,
    MinutesLate,
    ActualStartTime,
    ScheduledStartTime
FROM vw_ExamTimingStatistics
WHERE MinutesLate > 0
ORDER BY MinutesLate DESC;

-- View students who took longer than scheduled
SELECT 
    StudentName,
    Test_Name,
    ActualDuration,
    ScheduledDuration,
    DurationDifference
FROM vw_ExamTimingStatistics
WHERE DurationDifference > 0
ORDER BY DurationDifference DESC;

-- View currently in-progress exams
SELECT 
    StudentName,
    Test_Name,
    ActualStartTime,
    DATEDIFF(MINUTE, ActualStartTime, GETDATE()) AS MinutesElapsed
FROM vw_ExamTimingStatistics
WHERE ExamStatus = 'In Progress'
ORDER BY ActualStartTime;
```

## Database Schema

### TestStudent Table (New Columns)

| Column Name | Data Type | Description |
|-------------|-----------|-------------|
| TS_ActualStartTime | DATETIME | When student actually started |
| TS_ActualEndTime | DATETIME | When student submitted |
| TS_TotalBreakTime | INT | Total break time in minutes |
| TS_ActualDuration | INT | Actual exam duration in minutes |

### vw_ExamTimingStatistics View

Returns:
- Scheduled times (start, end, duration)
- Actual times (start, end, duration)
- Break time tracking
- Status (Not Started, In Progress, Completed)
- Time differences (late start, overtime)
- Marks obtained

## Usage Examples

### Admin Dashboard - View Late Students

```sql
SELECT 
    StudentName,
    StudentEmail,
    Test_Name,
    ScheduledStartTime,
    ActualStartTime,
    MinutesLate
FROM vw_ExamTimingStatistics
WHERE MinutesLate > 5 -- More than 5 minutes late
ORDER BY MinutesLate DESC;
```

### Monitor Exam Completion Rates

```sql
SELECT 
    Test_Name,
    COUNT(*) AS TotalStudents,
    SUM(CASE WHEN ExamStatus = 'Completed' THEN 1 ELSE 0 END) AS CompletedCount,
    SUM(CASE WHEN ExamStatus = 'In Progress' THEN 1 ELSE 0 END) AS InProgressCount,
    SUM(CASE WHEN ExamStatus = 'Not Started' THEN 1 ELSE 0 END) AS NotStartedCount
FROM vw_ExamTimingStatistics
GROUP BY Test_Name;
```

### Average Exam Duration by Test

```sql
SELECT 
    Test_Name,
    ScheduledDuration,
    AVG(ActualDuration) AS AvgActualDuration,
    MIN(ActualDuration) AS MinDuration,
    MAX(ActualDuration) AS MaxDuration
FROM vw_ExamTimingStatistics
WHERE ExamStatus = 'Completed'
GROUP BY Test_Name, ScheduledDuration
ORDER BY Test_Name;
```

## What Happens When Student Takes Exam

### 1. Student Clicks "Start Exam" Button:
```
Frontend → Backend → SP_StartExam
Saves: TS_ActualStartTime = NOW()
Session: Stores start time
```

### 2. Student Answers Questions:
```
Button colors update (red, green, purple)
Answers saved to session
Status tracked (visited, answered, marked for review)
```

### 3. Student Clicks "Submit Exam":
```
Frontend → Backend → SubmitExam()
├─ Retrieves start time from session
├─ Records end time = NOW()
├─ Calculates duration = end - start
├─ Saves all answers to TestResult table
├─ Calls SP_MarkTestAsAttempted with:
│  ├─ Marks earned
│  ├─ Start time
│  ├─ End time
│  └─ Duration
└─ Clears session data
```

### 4. Database Saves:
```
TestStudent table:
├─ TS_IsAttempted = 1
├─ TS_Mark = total marks
├─ TS_ActualStartTime = when started
├─ TS_ActualEndTime = when submitted
└─ TS_ActualDuration = time spent

TestResult table:
└─ One row per question answered
```

## Advanced Features (Future Enhancement)

### Break Time Tracking

To track breaks during the exam:

1. Add JavaScript to detect when student tabs away:
```javascript
document.addEventListener('visibilitychange', function() {
    if (document.hidden) {
        // Student switched away - start break timer
        breakStartTime = new Date();
    } else {
        // Student came back - end break timer
        if (breakStartTime) {
            let breakDuration = new Date() - breakStartTime;
            totalBreakTime += breakDuration;
        }
    }
});
```

2. Send break time when submitting:
```javascript
fetch('/Student/SubmitExam', {
    body: JSON.stringify({ 
        testId: testId,
        totalBreakTime: totalBreakTime / 60000 // Convert to minutes
    })
});
```

## Troubleshooting

### Times not being recorded:

1. **Check if script ran successfully:**
```sql
SELECT * FROM sys.columns 
WHERE object_id = OBJECT_ID('TestStudent')
AND name IN ('TS_ActualStartTime', 'TS_ActualEndTime');
```

2. **Check if procedures exist:**
```sql
SELECT name, create_date, modify_date
FROM sys.procedures
WHERE name IN ('SP_StartExam', 'SP_MarkTestAsAttempted');
```

3. **Check application logs** in Visual Studio Output window

### View not showing data:

```sql
-- Check if view exists
SELECT * FROM sys.views WHERE name = 'vw_ExamTimingStatistics';

-- Check if TestStudent has data
SELECT TOP 10 * FROM TestStudent;
```

## Files Modified

### SQL Scripts:
- `Database/ADD_EXAM_TIME_TRACKING.sql` (NEW) - Complete setup script

### C# Backend:
- `BLL/StudentService.cs`:
  - `StartExamSession()` - Now records start time
  - `SubmitExam()` - Now saves start/end times and duration

## Support

If you encounter issues:
1. Check SQL error messages in SSMS
2. Check application debug output in Visual Studio
3. Verify database permissions
4. Check if columns were added successfully

---

**Created:** 2024
**Version:** 1.0


