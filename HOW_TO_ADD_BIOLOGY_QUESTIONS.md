# How to Add Biology Tissue Questions

## 📋 Quick Start

I've created **10 Biology Tissue questions** ready to be inserted into your database.

---

## 🎯 Questions Included

1. **Apical meristem** - Plant growth in length
2. **Squamous epithelium** - Blood vessel lining
3. **Ligament** - Connects bones to bones
4. **Complex permanent tissue** - Xylem and phloem
5. **Cardiac muscle** - Heart tissue
6. **Adipose tissue** - Fat storage
7. **Nervous tissue** - Brain and spinal cord
8. **Chlorenchyma** - Photosynthesis in plants
9. **Plasma** - Blood matrix
10. **Collenchyma** - Plant flexibility

---

## 🔧 How to Run the Script

### Option 1: Using SQL Server Management Studio (SSMS)

1. **Open SQL Server Management Studio**

2. **Connect to your database server**
   - Server: Your SQL Server instance
   - Database: `OnlineExamination`

3. **Open the script**
   - File → Open → File
   - Navigate to: `D:\Application\Database\Insert_Biology_Tissue_Questions.sql`
   - Or copy the contents

4. **Execute the script**
   - Click **Execute** (F5)
   - Wait for completion message

5. **Verify**
   - You should see: "Successfully inserted 10 Biology Tissue questions!"

---

### Option 2: Using PowerShell (Quick Method)

Run this command from PowerShell in the Application folder:

```powershell
# Change these values to match your setup
$Server = "LAPTOP-JTGTC59L\SQLEXPRESS"
$Database = "OnlineExamination"

# Run the script
sqlcmd -S $Server -d $Database -i "Database\Insert_Biology_Tissue_Questions.sql"
```

---

## ✅ What the Script Does

### 1. **Creates Required Master Data** (if not exists)
   - Biology Subject
   - Class 11
   - Tissue Topic
   - NCERT Publication

### 2. **Inserts 10 Questions** with:
   - Question text (HTML formatted)
   - Correct answer
   - 3 incorrect options (B, C, D)
   - Detailed explanation/solution
   - Marks: 1 for CET, 4 for JEE
   - Negative marking: 0.25

---

## 📊 After Running the Script

### View Questions in the Application

1. **Run your application** (F5)

2. **Login as Admin**

3. **Navigate to Questions**
   - Go to: `http://localhost:52734/Question/Index`

4. **You should see** all 10 Biology Tissue questions

---

## 🎓 Using Questions in Tests

### Create a Test with These Questions

1. **Go to Test Master**
   - Navigate to: `/TestMaster/Create`

2. **Create a new test**
   - Test Name: "Biology - Tissue Test"
   - Duration: 30 minutes
   - Select Subject: Biology
   - Select Topic: Tissue

3. **Add Questions**
   - Select the 10 tissue questions
   - Click "Add to Test"

4. **Assign to Students**
   - Go to Test Student assignment
   - Assign the test to students

---

## 🔍 Troubleshooting

### Issue: Script fails with "Subject not found"

**Solution:** The script will automatically create Biology subject if it doesn't exist. But if you have a different subject name, update these lines in the script:

```sql
DECLARE @SubjectId INT = (SELECT TOP 1 Sub_Id FROM SubjectMaster WHERE Sub_Name LIKE '%Biology%');
```

Change `'%Biology%'` to match your subject name.

---

### Issue: "Class not found"

**Solution:** Update the class search:

```sql
DECLARE @ClassId INT = (SELECT TOP 1 ID FROM ClassMaster WHERE Name LIKE '%11%' OR Name LIKE '%12%');
```

Change `'%11%'` to match your class name (e.g., `'%10%'` for Class 10).

---

### Issue: Duplicate questions

If you run the script multiple times, it will insert duplicate questions. To avoid this:

**Option A:** Delete old questions first:
```sql
DELETE FROM QuestionMaster WHERE Ques_TopId = (SELECT Top_Id FROM TopicMaster WHERE Top_Name = 'Tissue')
```

**Option B:** Add a check in the script before each INSERT (contact me if needed).

---

## 📝 Question Format

Each question is stored as:

```sql
Ques_Question:  '<p>Question text here?</p>'
Ques_Answer:    '<p>Correct answer</p>'
Ques_OptionB:   '<p>Option A (incorrect)</p>'
Ques_OptionC:   '<p>Option B (incorrect)</p>'
Ques_OptionD:   '<p>Option C (incorrect)</p>'
Ques_SolutionDetails: '<p><strong>Explanation:</strong> Detailed solution</p>'
```

**Note:** Questions support HTML formatting and can include:
- Bold: `<strong>text</strong>`
- Italic: `<em>text</em>`
- Math equations: `\(x^2 + y^2 = z^2\)`
- Images: `<img src="..." />`

---

## 🎯 Next Steps

1. ✅ Run the SQL script
2. ✅ Verify questions appear in Question/Index
3. ✅ Create a test using these questions
4. ✅ Assign test to students
5. ✅ Students can take the Biology Tissue test!

---

## 📞 Need More Questions?

If you need more questions on:
- Different topics (Cell, Genetics, etc.)
- Different subjects (Chemistry, Physics, etc.)
- Different difficulty levels

Just let me know the topic and I'll create more!

---

**Script Location:** `D:\Application\Database\Insert_Biology_Tissue_Questions.sql`

**Status:** ✅ Ready to run!
