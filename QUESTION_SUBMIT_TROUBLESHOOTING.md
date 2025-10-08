# Question Not Submitting - Complete Fix & Troubleshooting

## 🔧 **All Fixes Applied**

I've fixed **3 critical issues** that were preventing question submission:

---

## ✅ **Fix #1: Missing ClassId Parameter**

**File:** `BLL/QuestionService.cs`

**Problem:** The `Ques_ClassId` was not being sent to the stored procedure

**Fixed:** Added `@Ques_ClassId` parameter in both:
- Add method (line 127)
- Update method (line 179)

---

## ✅ **Fix #2: TinyMCE Content Not Saved**

**File:** `Views/Question/Create.cshtml`

**Problem:** Rich text editor content wasn't being saved to textareas before form submission

**Fixed:** Added form submit handler that:
- Calls `tinymce.triggerSave()` before submission
- Validates all required fields
- Shows helpful error messages
- Logs data to console for debugging

---

## ✅ **Fix #3: Data Type Mismatch**

**File:** `Models/QuestionMasterModel.cs`

**Problem:** `Ques_Negative` was `int` instead of `decimal`

**Fixed:** Changed to `decimal` to match database (supports values like 0.25, 0.5)

---

## 📋 **What You Need to Do Now**

### **Step 1: Run the SQL Script (CRITICAL!)**

**The stored procedures don't exist yet in your database!**

1. **Open SQL Server Management Studio (SSMS)**
2. **Connect to:** `LAPTOP-JTGTC59L\SQLEXPRESS`
3. **Select database:** `OnlineExamination`
4. **Open file:** `D:\Application\Database\ALL_QuestionMaster_StoredProcedures.sql`
5. **Execute:** Press F5
6. **Wait for:** "All Stored Procedures Created Successfully!" message

**This creates 4 stored procedures:**
- `Sel_QuestionMaster_Select` - View questions
- `SP_QuestionMaster_Insert` - Add questions
- `SP_QuestionMaster_Update` - Edit questions
- `SP_QuestionMaster_Delete` - Delete questions

---

### **Step 2: Rebuild the Application**

1. In Visual Studio: **Build → Rebuild Solution**
2. Wait for: **"Rebuild All succeeded"**
3. Check: **0 Errors**

---

### **Step 3: Test Creating a Question**

1. **Run** the application (F5)

2. **Navigate to:** `http://localhost:52734/Question/Create`

3. **Open Browser Console** (Press F12, go to Console tab)

4. **Fill in the form:**
   - Subject: Select Biology (or any subject)
   - Class: Select Class 11 (or any class)
   - Topic: Select Tissue (or any topic)
   - Publication: Select NCERT (or any publication)
   - CET Marks: 1
   - JEE Marks: 4
   - Negative Marks: 0.25
   - Question: Type "What is a tissue?"
   - Correct Answer: Type "A group of cells"
   - Option A: Type "A single cell"
   - Option B: Type "An organ"
   - Option C: Type "A system"

5. **Click:** "Continue" button

6. **Watch the Console** - You'll see:
   ```
   === FORM SUBMIT DEBUG ===
   ✅ TinyMCE content saved
   Form Data: {...}
   ✅ All validations passed
   Action button: Continue
   ```

7. **Expected Results:**
   - ✅ Green alert at top: **"Question saved successfully!"**
   - ✅ Form clears for next question
   - ✅ No errors in console

---

## 🐛 **Troubleshooting by Error Type**

### **Error Type A: Alert "Please select a [Field]!"**

**What it means:** Required field is empty

**Solution:**
1. Make sure ALL dropdowns have a value selected
2. Don't leave "Select Subject" - choose an actual subject
3. Fill all required fields marked with red asterisk (*)

---

### **Error Type B: No alert, nothing happens**

**What it means:** JavaScript error or form not binding

**Solution:**
1. Open Console (F12)
2. Look for RED error messages
3. Common issues:
   - TinyMCE not loaded (check API key)
   - jQuery not loaded
   - Script errors

**Quick Fix:**
```javascript
// Type this in console to check:
typeof tinymce
// Should return "object"

typeof jQuery
// Should return "function"
```

---

### **Error Type C: Red error alert "An error occurred while saving"**

**What it means:** Stored procedure issue or database error

**Solution:**

**Check 1:** Stored procedure exists
```sql
-- Run in SSMS
USE OnlineExamination;
GO

SELECT * FROM sys.objects 
WHERE type = 'P' 
AND name = 'SP_QuestionMaster_Insert';
```

If returns 0 rows: **Run ALL_QuestionMaster_StoredProcedures.sql**

**Check 2:** Database connection
- Verify Web.config connection string is correct
- Test database is accessible

**Check 3:** Master data exists
```sql
-- Check if you have master data
SELECT 'Subjects' AS TableName, COUNT(*) AS Count FROM SubjectMaster WHERE Sub_IsActive = 1
UNION ALL
SELECT 'Classes', COUNT(*) FROM ClassMaster WHERE IsActive = 1
UNION ALL
SELECT 'Topics', COUNT(*) FROM TopicMaster WHERE Top_IsActive = 1
UNION ALL
SELECT 'Publications', COUNT(*) FROM PublicationMaster WHERE Pub_IsActive = 1;
```

If any count is 0, you need to add master data first!

---

### **Error Type D: Form submits but page reloads without message**

**What it means:** Controller action might be failing silently

**Solution:**

Enable detailed errors in Web.config:
```xml
<customErrors mode="Off">
```

Then try again and you'll see the actual error.

---

## 🔍 **Advanced Debugging**

### **Test the Stored Procedure Directly**

Run this in SSMS:

```sql
USE OnlineExamination;
GO

-- Get sample IDs
DECLARE @SubId INT = (SELECT TOP 1 Sub_Id FROM SubjectMaster WHERE Sub_IsActive = 1);
DECLARE @ClassId INT = (SELECT TOP 1 ID FROM ClassMaster WHERE IsActive = 1);
DECLARE @TopicId INT = (SELECT TOP 1 Top_Id FROM TopicMaster WHERE Top_IsActive = 1);
DECLARE @PubId INT = (SELECT TOP 1 Pub_Id FROM PublicationMaster WHERE Pub_IsActive = 1);

PRINT 'Testing SP_QuestionMaster_Insert with:';
PRINT 'Subject ID: ' + CAST(@SubId AS VARCHAR(10));
PRINT 'Class ID: ' + CAST(@ClassId AS VARCHAR(10));
PRINT 'Topic ID: ' + CAST(@TopicId AS VARCHAR(10));
PRINT 'Publication ID: ' + CAST(@PubId AS VARCHAR(10));

-- Try to insert a test question
EXEC SP_QuestionMaster_Insert
    @Ques_SubId = @SubId,
    @Ques_ClassId = @ClassId,
    @Ques_TopId = @TopicId,
    @Ques_PubId = @PubId,
    @Ques_Mark = 1,
    @Ques_JEEMark = 4,
    @Ques_Negative = 0.25,
    @Ques_Question = '<p>Test Question</p>',
    @Ques_Answer = '<p>Test Answer</p>',
    @Ques_OptionB = '<p>Option A</p>',
    @Ques_OptionC = '<p>Option B</p>',
    @Ques_OptionD = '<p>Option C</p>',
    @Ques_SolutionDetails = '<p>Test Solution</p>';
```

**If this works:** The issue is in the application layer
**If this fails:** The stored procedure needs fixing

---

## 📊 **Complete Checklist**

Before testing, verify:

- [ ] SQL Script run: `ALL_QuestionMaster_StoredProcedures.sql`
- [ ] Application rebuilt (0 errors)
- [ ] CustomErrors mode="Off" in Web.config (for debugging)
- [ ] Master data exists (Subjects, Classes, Topics, Publications)
- [ ] Browser console open (F12)
- [ ] All form fields filled correctly

---

## 🎯 **Expected Complete Flow**

```
1. User fills form
   ↓
2. Clicks "Continue"
   ↓
3. JavaScript validates fields (alerts if invalid)
   ↓
4. TinyMCE saves content to textareas
   ↓
5. Form submits to POST /Question/Create
   ↓
6. Controller receives data
   ↓
7. ModelState validation
   ↓
8. Service layer calls Add method
   ↓
9. Stored procedure SP_QuestionMaster_Insert executes
   ↓
10. Database inserts question
   ↓
11. Returns new Question ID
   ↓
12. Controller sets success message
   ↓
13. Page reloads with green success alert
   ↓
14. Form clears for next question
   ↓
15. Question visible in /Question/Index
```

---

## 🚀 **Quick Test Now**

1. **Rebuild** application
2. **Run** (F5)
3. **Navigate** to `/Question/Create`
4. **Open Console** (F12)
5. **Fill form** completely
6. **Click** "Continue"
7. **Watch console** for debug messages
8. **Check result:**
   - ✅ Success message → Working!
   - ❌ Alert or error → Copy the message and check guide above
   - ❌ Nothing → Check console for errors

---

## 📝 **Files Modified**

1. ✅ `BLL/QuestionService.cs` - Added ClassId parameter
2. ✅ `Models/QuestionMasterModel.cs` - Fixed Ques_Negative type
3. ✅ `Views/Question/Create.cshtml` - Added validation & TinyMCE save

---

## 🆘 **Still Not Working?**

### After clicking Continue, check these:

**1. Browser Console (F12):**
- Any RED errors? → Copy and share
- Debug messages appearing? → What does it say?
- Form data logged? → Are all fields populated?

**2. Top of Page:**
- Green success message? → IT WORKED! ✅
- Red error message? → What does it say?
- No message? → Issue might be earlier in the flow

**3. Network Tab (F12 → Network):**
- Click "Continue"
- Look for POST request to `/Question/Create`
- Click on it
- Check Response tab
- Is it HTML? Check for error messages

---

## 💡 **Most Common Issue**

**YOU HAVEN'T RUN THE SQL SCRIPT YET!**

If the stored procedure `SP_QuestionMaster_Insert` doesn't exist, the question cannot be saved.

**Quick check in SSMS:**
```sql
SELECT OBJECT_ID('SP_QuestionMaster_Insert');
```

- Returns NULL → **Run the SQL script NOW!**
- Returns a number → Stored procedure exists ✅

---

**Status:** All code fixes applied ✅  
**Remaining:** Run SQL script + Rebuild + Test  
**Time Required:** 5 minutes total
