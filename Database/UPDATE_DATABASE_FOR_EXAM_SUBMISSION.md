# Database Update Guide - Exam Submission Fix

## Overview
This guide explains how to update your database to fix the exam submission issue. The system was not saving student answers to the database when exams were submitted.

## What Was Fixed

### Backend Changes (Already Applied)
✅ Updated `StudentService.cs` to save answers to database when submitting
✅ Updated `StudentController.cs` with ClearAnswer action
✅ Fixed button color persistence issues

### Database Changes (YOU NEED TO RUN THESE)

## Required SQL Scripts to Run

### 1. Update SP_MarkTestAsAttempted Stored Procedure

This procedure now accepts the total marks parameter.

**File:** `Database/SP_MarkTestAsAttempted.sql`

Run this file in your SQL Server Management Studio against your `OnlineExamination` database.

### 2. Create SP_SaveTestResult Stored Procedure

This new procedure saves individual question answers to the TestResult table.

**File:** `Database/SP_SaveTestResult.sql`

Run this file in your SQL Server Management Studio against your `OnlineExamination` database.

## How to Run the Scripts

### Option 1: SQL Server Management Studio (SSMS)

1. Open **SQL Server Management Studio**
2. Connect to your SQL Server instance
3. Click **File** → **Open** → **File**
4. Navigate to your project folder: `D:\Application\Database\`
5. Open and run these files in order:
   - `SP_MarkTestAsAttempted.sql`
   - `SP_SaveTestResult.sql`
6. Execute each script by clicking **Execute** or pressing **F5**
7. Verify "Command(s) completed successfully" message

### Option 2: Command Line (sqlcmd)

```bash
# Navigate to the Database directory
cd D:\Application\Database

# Run SP_MarkTestAsAttempted.sql
sqlcmd -S your_server_name -d OnlineExamination -i SP_MarkTestAsAttempted.sql

# Run SP_SaveTestResult.sql  
sqlcmd -S your_server_name -d OnlineExamination -i SP_SaveTestResult.sql
```

Replace `your_server_name` with your actual SQL Server instance name.

## Verification

After running the scripts, verify they were created:

```sql
USE OnlineExamination;
GO

-- Check if stored procedures exist
SELECT name, create_date, modify_date 
FROM sys.procedures 
WHERE name IN ('SP_MarkTestAsAttempted', 'SP_SaveTestResult');
```

You should see both procedures listed.

## Testing the Fix

1. **Build and run your application**
2. **Login as a student**
3. **Start a test**
4. **Answer some questions**
5. **Navigate between questions** - verify button colors persist correctly:
   - White = Not Visited
   - Red = Visited but not answered
   - Green = Answered
   - Purple = Marked for review
   - Green with purple dot = Answered and marked
6. **Click "Submit Exam"**
7. **Check the database** to verify answers were saved:

```sql
USE OnlineExamination;
GO

-- View saved answers
SELECT tr.*, 
       s.Stu_Name as StudentName,
       q.Ques_Question,
       tr.TR_Answer as StudentAnswer,
       tr.TR_IsCorrect as IsCorrect,
       tr.TR_MarksObtained as MarksObtained
FROM TestResult tr
INNER JOIN StudentMaster s ON tr.TR_StudentId = s.Stu_Id
INNER JOIN QuestionMaster q ON tr.TR_QuestionId = q.Ques_Id
ORDER BY tr.TR_SubmittedDate DESC;

-- View test completion status
SELECT ts.*, 
       t.Test_Name,
       s.Stu_Name,
       ts.TS_Mark as TotalMarks,
       ts.TS_IsAttempted as IsCompleted
FROM TestStudent ts
INNER JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
INNER JOIN StudentMaster s ON ts.TS_StudId = s.Stu_Id
WHERE ts.TS_IsAttempted = 1
ORDER BY ts.TS_Id DESC;
```

## What the Fix Does

### When Student Submits Exam:

1. ✅ **Retrieves all answers from Session** (temporary storage)
2. ✅ **Saves each answer to TestResult table** with:
   - Test ID
   - Student ID
   - Question ID
   - Student's answer (A, B, C, or D)
   - Whether answer is correct
   - Marks obtained
   - Submission timestamp
3. ✅ **Calculates total marks** across all questions
4. ✅ **Updates TestStudent record** with:
   - Mark as attempted (completed)
   - Total marks earned
5. ✅ **Clears session data** to free up memory

### Button Color Persistence:

1. ✅ **Red buttons** stay red for visited but unanswered questions
2. ✅ **Purple buttons** show for questions marked for review
3. ✅ **Green buttons with purple dot** show for answered + marked questions
4. ✅ **Colors persist** when navigating between questions
5. ✅ **Status tracked in Session** across page loads

## Troubleshooting

### If answers are not being saved:

1. Verify both stored procedures were created successfully
2. Check SQL Server error logs
3. Enable debug logging in `BLL/StudentService.cs` - check Output window for errors
4. Verify TestResult table exists in your database
5. Check if the database user has INSERT/UPDATE permissions

### If button colors don't persist:

1. Clear browser cache and cookies
2. Check browser console for JavaScript errors (F12 → Console tab)
3. Verify Session is enabled in `Web.config`
4. Test in a different browser

### If marks calculation seems wrong:

Note: Currently, the system assumes **Option A is always the correct answer** (since `Ques_Answer` in the database contains the text of option A, not a letter indicator).

If you need different correct answers per question, you'll need to:
1. Add a `Ques_CorrectOption` column to the QuestionMaster table (storing 'A', 'B', 'C', or 'D')
2. Update the SubmitExam logic to use that column instead of hardcoding "A"

## Need Help?

If you encounter issues:
1. Check the browser console (F12) for JavaScript errors
2. Check the database error log
3. Check the application debug output in Visual Studio
4. Review the Implementation Summary documentation

## Files Modified

- `BLL/StudentService.cs` - Added answer saving logic
- `Controllers/StudentController.cs` - Added ClearAnswer action  
- `Views/Student/TakeExam.cshtml` - Fixed button colors and status tracking
- `Database/SP_MarkTestAsAttempted.sql` - Added marks parameter
- `Database/SP_SaveTestResult.sql` - New procedure to save answers


