# TestDetails Error Fix Guide

## Error Description

**URL**: `http://localhost:52734/Student/TestDetails/2024`
**Error**: "An error occurred while processing your request"

## Root Cause Analysis

The error occurs in the `TestDetails` action when trying to load exam instructions for test ID 2024. The most likely causes are:

### 1. **Missing or Invalid Test Data**
- Test ID 2024 may not exist in the database
- The stored procedure `SP_GetQuestionsPerSubject` is returning null or empty results
- Test may not have any questions assigned to it

### 2. **Stored Procedure Issues**
- `SP_GetQuestionsPerSubject` may have errors
- Incorrect parameters being passed
- Database connection issues

### 3. **Model Mapping Issues**
- Data returned doesn't match `TestSubjectDetailsModel` properties
- Missing required fields in the database result

---

## Solutions Implemented

### ✅ **1. Enhanced Error Handling**

**File**: `Controllers/StudentController.cs`

**Changes Made:**
```csharp
[HttpGet]
public ActionResult TestDetails(int id)
{
    try
    {
        // Check if student is logged in
        string studId = Session["StudentId"] as string;
        if (string.IsNullOrEmpty(studId))
        {
            return RedirectToAction("Login", "Login");
        }

        // Get test details with null checking
        var testDetailsList = objTestService.GetTestSubjectDetails(id);

        if (testDetailsList == null || !testDetailsList.Any())
        {
            TempData["ErrorMessage"] = "Test details not found or no questions available for this test.";
            return RedirectToAction("TestList");
        }

        TestSubjectDetailsModel objtestStudent = testDetailsList.FirstOrDefault();

        if (objtestStudent == null)
        {
            TempData["ErrorMessage"] = "Unable to load test details.";
            return RedirectToAction("TestList");
        }

        return View(objtestStudent);
    }
    catch (Exception ex)
    {
        TempData["ErrorMessage"] = "An error occurred while loading test details: " + ex.Message;
        return RedirectToAction("TestList");
    }
}
```

**Benefits:**
- ✅ Catches all exceptions
- ✅ Provides helpful error messages
- ✅ Redirects to safe page (TestList)
- ✅ Shows error message to user

### ✅ **2. Error Message Display**

**File**: `Views/Student/TestList.cshtml`

**Added:**
```html
@if (TempData["ErrorMessage"] != null)
{
    <div class="alert alert-danger alert-dismissible fade show" role="alert">
        <i class="ph ph-warning-circle"></i> <strong>Error:</strong> @TempData["ErrorMessage"]
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
}
```

**Benefits:**
- ✅ User sees what went wrong
- ✅ Dismissible alert
- ✅ Professional appearance
- ✅ Icon for visual clarity

---

## How to Diagnose the Specific Issue

### Step 1: Check if Test Exists

Run this SQL query:
```sql
SELECT * FROM TestMaster WHERE Test_Id = 2024
```

**If no results:**
- Test ID 2024 doesn't exist
- Use a valid test ID from the TestList page

### Step 2: Check if Test Has Questions

Run this SQL query:
```sql
EXEC SP_GetQuestionsPerSubject @TS_Id = 2024
```

**If no results:**
- Test has no questions assigned
- Need to add questions to the test using Test Master

**If error:**
- Stored procedure has issues
- Check stored procedure definition

### Step 3: Check Model Properties Match

Verify the stored procedure returns these columns:
```
- TS_Id (int)
- TestID (int)
- SubjectName (string)
- TestDuration (int)
- TestStartTime (DateTime)
- TestEndTime (DateTime)
- TestMark (int)
- NumberOfQuestions (int)
```

---

## Common Scenarios & Solutions

### Scenario 1: Test Has No Questions

**Problem:**
```
Test exists but no questions assigned
SP_GetQuestionsPerSubject returns empty
```

**Solution:**
1. Go to Test Master
2. Edit test ID 2024
3. Add questions from question bank
4. Save and try again

**Alternative:**
- Use a test that already has questions
- Check TestList for tests with questions assigned

### Scenario 2: Wrong Test ID in URL

**Problem:**
```
URL shows /TestDetails/2024
But test ID in database is different
```

**Solution:**
1. Go to TestList page
2. Click "View Details" on the correct test
3. System will use correct ID automatically

### Scenario 3: Stored Procedure Error

**Problem:**
```
SP_GetQuestionsPerSubject throws exception
or returns incompatible data
```

**Solution:**
Check stored procedure definition:
```sql
-- View stored procedure
SELECT OBJECT_DEFINITION(OBJECT_ID('SP_GetQuestionsPerSubject'))
```

Expected structure:
```sql
CREATE PROCEDURE SP_GetQuestionsPerSubject
    @TS_Id INT
AS
BEGIN
    SELECT 
        ts.TS_Id,
        t.Test_Id AS TestID,
        s.Subject_Name AS SubjectName,
        t.Test_Duration AS TestDuration,
        t.Test_StartTime AS TestStartTime,
        t.Test_EndTime AS TestEndTime,
        t.Test_Mark AS TestMark,
        COUNT(tq.TQ_QuesId) AS NumberOfQuestions
    FROM TestStudent ts
    INNER JOIN TestMaster t ON ts.TS_TestId = t.Test_Id
    LEFT JOIN SubjectMaster s ON t.Subject_Id = s.Subject_Id
    LEFT JOIN TestQuestion tq ON t.Test_Id = tq.TQ_TestId
    WHERE ts.TS_Id = @TS_Id
    GROUP BY ts.TS_Id, t.Test_Id, s.Subject_Name, 
             t.Test_Duration, t.Test_StartTime, 
             t.Test_EndTime, t.Test_Mark
END
```

---

## Testing After Fix

### Test 1: Valid Test with Questions
1. Click on a test from TestList that has questions
2. Should see instructions page
3. All details should display correctly

### Test 2: Invalid Test ID
1. Manually navigate to `/Student/TestDetails/99999`
2. Should see error message on TestList page
3. Error: "Test details not found or no questions available"

### Test 3: Test Without Questions
1. Create test but don't add questions
2. Try to view details
3. Should see error message
4. Redirected to TestList

---

## Prevention

### For Admins/Teachers:
1. **Always add questions** before publishing test
2. **Verify test setup** using preview feature
3. **Test the test** before assigning to students

### For Developers:
1. ✅ Enhanced error handling implemented
2. ✅ Null checking at every level
3. ✅ User-friendly error messages
4. ✅ Graceful fallback to safe pages

---

## Error Messages Guide

| Error Message | Cause | Solution |
|---------------|-------|----------|
| "Test details not found or no questions available for this test" | Test ID doesn't exist or has no questions | Add questions to test or use valid test ID |
| "Unable to load test details" | Data issue after fetch | Check database data integrity |
| "An error occurred while loading test details: [message]" | Exception thrown | Check exception message for details |
| Session expired error | Student not logged in | Re-login |

---

## Quick Fix Checklist

- [x] Enhanced error handling in TestDetails action
- [x] Added try-catch block
- [x] Added null checking
- [x] Added helpful error messages
- [x] Added error display on TestList page
- [x] Redirect to safe page on error
- [ ] Check if test ID 2024 exists (user action)
- [ ] Verify test has questions assigned (user action)
- [ ] Run stored procedure manually to verify (user action)

---

## Next Steps

1. **Check Database**: Verify test ID 2024 exists and has questions
2. **Test Again**: Try accessing TestDetails with valid test ID
3. **View Error**: If error persists, you'll now see helpful message
4. **Debug**: Use error message to identify specific issue
5. **Fix Data**: Add missing questions or use different test

---

## Support

If the error persists after these fixes:

1. Check browser console for JavaScript errors
2. Check server logs for detailed exception
3. Verify database connection is working
4. Test with a known-good test ID
5. Contact system administrator

---

**Status**: ✅ Error Handling Implemented
**Impact**: Now shows user-friendly errors instead of generic error page
**Next**: User needs to verify test data in database


