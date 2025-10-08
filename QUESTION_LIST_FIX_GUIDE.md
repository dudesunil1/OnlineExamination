# Question List Not Showing - Fix Guide

## 🐛 **Problem**
Questions are not appearing in the Question List page (`/Question/Index`)

---

## ✅ **Solution**

The stored procedure `Sel_QuestionMaster_Select` is **missing** from your database.

---

## 🔧 **How to Fix (3 Easy Steps)**

### **Step 1: Open SQL Server Management Studio (SSMS)**

1. Open SQL Server Management Studio
2. Connect to your server: `LAPTOP-JTGTC59L\SQLEXPRESS`
3. Select database: `OnlineExamination`

---

### **Step 2: Run the Fix Script**

1. **Open the file:** `D:\Application\Database\CREATE_QuestionMaster_Procedures.sql`
2. **Execute:** Press F5 or click Execute button
3. **Wait for:** "✅ Stored procedure created successfully!" message

---

### **Step 3: Test the Application**

1. **Restart** your application (if running)
2. **Navigate to:** `http://localhost:52734/Question/Index`
3. **Result:** Questions should now appear! ✅

---

## 🎯 **What This Script Does**

1. ✅ Creates the missing stored procedure `Sel_QuestionMaster_Select`
2. ✅ Enables filtering by Question ID, Subject, or Topic
3. ✅ Joins with Subject, Class, Topic, and Publication tables
4. ✅ Returns all active questions
5. ✅ Tests the procedure automatically

---

## 📊 **After Running the Script**

You should see output like this:

```
✅ Stored procedure Sel_QuestionMaster_Select created successfully!
========================================
Testing Sel_QuestionMaster_Select
========================================

(Results will show here if you have questions)

Total questions in database: X
✅ Questions are available!

========================================
Script completed successfully!
========================================
```

---

## 🔍 **Still Not Showing Questions?**

### **Check 1: Do You Have Questions in the Database?**

Run this in SSMS:

```sql
SELECT COUNT(*) AS TotalQuestions 
FROM QuestionMaster 
WHERE ISNULL(Ques_IsActive, 1) = 1;
```

**If result is 0:**
- You need to add questions first
- Run: `Database/Insert_Biology_Tissue_Questions.sql` to add 10 sample questions

**If result is > 0:**
- Questions exist, continue to Check 2

---

### **Check 2: Is the Stored Procedure Created?**

Run this in SSMS:

```sql
SELECT * 
FROM sys.objects 
WHERE type = 'P' 
AND name = 'Sel_QuestionMaster_Select';
```

**If no results:**
- The stored procedure wasn't created
- Re-run `CREATE_QuestionMaster_Procedures.sql`

**If 1 row returned:**
- Stored procedure exists ✅
- Continue to Check 3

---

### **Check 3: Test the Stored Procedure Directly**

Run this in SSMS:

```sql
EXEC Sel_QuestionMaster_Select 
    @QuestionId = 0, 
    @SubjectId = 0, 
    @TopicId = 0;
```

**If questions appear:**
- Database is working ✅
- Issue might be in the application
- Check application logs

**If no questions appear:**
- Questions might not be active
- Run: `UPDATE QuestionMaster SET Ques_IsActive = 1;`

---

### **Check 4: Application Connection String**

Verify `Web.config` has correct connection:

```xml
<connectionStrings>
  <add name="MyDbContext" 
       connectionString="Data Source=LAPTOP-JTGTC59L\SQLEXPRESS;User ID=sa;password=sa;Initial Catalog=OnlineExamination;..." 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Make sure:
- ✅ Server name matches your SQL Server instance
- ✅ Database name is `OnlineExamination`
- ✅ Credentials are correct

---

## 🚀 **Complete Workflow**

```
1. Run CREATE_QuestionMaster_Procedures.sql
   ↓
2. (Optional) Run Insert_Biology_Tissue_Questions.sql
   ↓
3. Restart Application
   ↓
4. Navigate to /Question/Index
   ↓
5. ✅ Questions appear in list!
```

---

## 📝 **Adding Questions Manually**

If you want to add questions through the UI:

1. **Navigate to:** `http://localhost:52734/Question/Create`
2. **Fill in all fields:**
   - Subject, Class, Topic, Publication
   - Marks (CET, JEE, Negative)
   - Question text
   - Correct Answer
   - 3 Other Options (A, B, C)
   - Solution Details
3. **Click:** "Continue" button
4. **Result:** Question added successfully!
5. **Go back to:** `/Question/Index` to see your question

---

## 🆘 **Still Having Issues?**

### **Enable Detailed Errors**

In `Web.config`, temporarily change:

```xml
<customErrors mode="Off">
```

This will show detailed error messages on the page.

### **Check Browser Console**

1. Press F12 in your browser
2. Go to Console tab
3. Look for any JavaScript errors (red text)
4. Share the error message for help

### **Check Application Logs**

Look in `D:\Application\App_Data\Logs` for error logs (if logging is configured).

---

## ✅ **Expected Result**

After the fix, your Question List page should show:

```
┌────────────────────────────────────────────────────┐
│ Question Master                    [Create New]    │
├────────────────────────────────────────────────────┤
│ ID │ Question          │ Actions                   │
├────┼───────────────────┼──────────────────────────┤
│ 10 │ Which tissue...   │ [Edit] [Details]         │
│  9 │ What is the...    │ [Edit] [Details]         │
│  8 │ Which tissue...   │ [Edit] [Details]         │
│ ... (more questions)                                │
└────────────────────────────────────────────────────┘
```

---

## 📂 **Files Created for This Fix**

1. ✅ `Database/CREATE_QuestionMaster_Procedures.sql` - Creates missing stored procedure
2. ✅ `Database/Insert_Biology_Tissue_Questions.sql` - Adds 10 sample questions
3. ✅ `QUESTION_LIST_FIX_GUIDE.md` - This troubleshooting guide

---

## 🎓 **Summary**

**Problem:** Missing stored procedure `Sel_QuestionMaster_Select`  
**Solution:** Run `CREATE_QuestionMaster_Procedures.sql`  
**Result:** Questions will appear in the list  
**Time to Fix:** 2 minutes  

---

**Status:** Ready to fix!  
**Priority:** High - Required for viewing questions  
**Difficulty:** Easy - Just run the SQL script
