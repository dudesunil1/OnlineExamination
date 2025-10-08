# Online Examination System - User Manual

## 📖 Table of Contents

1. [Getting Started](#getting-started)
2. [Admin Guide](#admin-guide)
3. [Teacher Guide](#teacher-guide)
4. [Student Guide](#student-guide)
5. [Question Management](#question-management)
6. [Test Management](#test-management)
7. [Troubleshooting](#troubleshooting)

## 🚀 Getting Started

### System Requirements
- Modern web browser (Chrome, Firefox, Safari, Edge)
- Internet connection for CDN resources
- JavaScript enabled

### Login Process
1. Navigate to the application URL
2. Enter your credentials:
   - **Admin**: admin@example.com / admin123
   - **Teacher**: teacher@example.com / teacher123
   - **Student**: student@example.com / student123
3. Click "Login"

## 👨‍💼 Admin Guide

### Dashboard Overview
The admin dashboard provides:
- Total students count
- Total questions in database
- Active tests
- Recent system activity
- Quick access to all modules

### Managing Subjects
1. Navigate to **Master Data** → **Subject Master**
2. Click **Create New** to add a subject
3. Fill in:
   - Subject Name (required)
   - Description (optional)
4. Click **Save**

### Managing Classes
1. Navigate to **Master Data** → **Class Master**
2. Click **Create New** to add a class
3. Fill in:
   - Class Name (required)
   - Description (optional)
4. Click **Save**

### Managing Publications
1. Navigate to **Master Data** → **Publication Master**
2. Click **Create New** to add a publication
3. Fill in:
   - Publication Name (required)
   - Description (optional)
4. Click **Save**

### Managing Topics
1. Navigate to **Master Data** → **Topic Master**
2. Click **Create New** to add a topic
3. Fill in:
   - Topic Name (required)
   - Subject (required)
   - Class (required)
   - Description (optional)
4. Click **Save**

### Managing Students
1. Navigate to **Student Management** → **Student Master**
2. Click **Create New** to add a student
3. Fill in:
   - Student Name (required)
   - Email (required, must be unique)
   - Password (required)
   - Phone (optional)
   - Address (optional)
   - Class (required)
4. Click **Save**

### Managing Groups
1. Navigate to **Student Management** → **Group Master**
2. Click **Create New** to add a group
3. Fill in:
   - Group Name (required)
   - Description (optional)
4. Click **Save**

### Assigning Students to Groups
1. Navigate to **Student Management** → **Group Details**
2. Click **Create New**
3. Select:
   - Group (required)
   - Student (required)
4. Click **Save**

## 👩‍🏫 Teacher Guide

### Question Management

#### Creating Questions
1. Navigate to **Question Management** → **Create Question**
2. Fill in basic information:
   - Subject (required)
   - Class (required)
   - Topic (required)
   - Publication (required)
3. Set scoring:
   - CET Marks (required)
   - JEE Marks (required)
   - Negative Marks (required)
4. Enter question content:
   - Use the rich text editor for question text
   - Use the Math button for mathematical equations
5. Add answer options:
   - Correct Answer (required)
   - Option A (required)
   - Option B (optional)
   - Option C (optional)
6. Add solution details (optional)
7. Click **Save Question**

#### Using the Math Editor
1. Click the **Math** button in any text editor
2. Enter LaTeX code in the input field
3. Preview your equation in real-time
4. Click **Insert Math** to add to your content

**Common LaTeX Examples:**
- Fractions: `\frac{a}{b}`
- Square root: `\sqrt{x}`
- Superscript: `x^2`
- Subscript: `x_1`
- Greek letters: `\alpha`, `\beta`, `\gamma`
- Integrals: `\int_{a}^{b} f(x) dx`

#### Editing Questions
1. Navigate to **Question Management** → **Question List**
2. Click **Edit** next to the question
3. Make your changes
4. Click **Save Question**

#### Deleting Questions
1. Navigate to **Question Management** → **Question List**
2. Click **Delete** next to the question
3. Confirm deletion

### Test Management

#### Creating Tests
1. Navigate to **Test Management** → **Create Test**
2. Fill in test details:
   - Test Name (required)
   - Description (optional)
   - Duration in minutes (required)
   - Total Marks (required)
3. Select questions for the test
4. Click **Save Test**

#### Assigning Tests
1. Navigate to **Test Management** → **Assign Test**
2. Select:
   - Test (required)
   - Students or Groups (required)
   - Start Date and Time
   - End Date and Time
3. Click **Assign Test**

## 👨‍🎓 Student Guide

### Student Dashboard
The student dashboard shows:
- Available tests
- Completed tests
- Average score
- Upcoming tests

### Taking a Test
1. Navigate to **Available Tests**
2. Click **Start Test** next to the test you want to take
3. Read the instructions carefully
4. Answer each question:
   - Select your answer for multiple choice questions
   - Type your answer for descriptive questions
5. Use the **Next** and **Previous** buttons to navigate
6. Click **Submit Test** when finished

### Viewing Results
1. Navigate to **Test Results**
2. Click on a completed test to view:
   - Your answers
   - Correct answers
   - Marks obtained
   - Detailed solution (if available)

## ❓ Question Management

### Question Types

#### Multiple Choice Questions
- One correct answer
- Up to 4 options (A, B, C, D)
- Single selection only

#### Descriptive Questions
- Text-based answers
- No predefined options
- Manual evaluation required

### Question Difficulty Levels
- **Easy**: Basic concepts
- **Medium**: Application of concepts
- **Hard**: Complex problem solving

### Question Categories
- **Theory**: Conceptual questions
- **Numerical**: Calculation-based questions
- **Application**: Real-world problem solving

## 📝 Test Management

### Test Types

#### Practice Tests
- No time limit
- Immediate feedback
- Can be retaken

#### Assessment Tests
- Time-limited
- One attempt only
- Results recorded

#### Mock Tests
- Simulate real exam conditions
- Full duration
- Comprehensive evaluation

### Test Settings

#### Time Management
- Set individual question time limits
- Set overall test duration
- Auto-submit when time expires

#### Question Order
- Sequential order
- Random order
- Difficulty-based order

#### Scoring
- Equal marks for all questions
- Weighted scoring
- Negative marking

## 🔧 Troubleshooting

### Common Issues

#### Login Problems
**Issue**: Cannot log in
**Solutions**:
- Check username and password
- Ensure caps lock is off
- Clear browser cache
- Try a different browser

#### Math Equations Not Displaying
**Issue**: Mathematical equations appear as code
**Solutions**:
- Check internet connection
- Refresh the page
- Clear browser cache
- Contact administrator

#### Test Not Loading
**Issue**: Test page shows error
**Solutions**:
- Check internet connection
- Ensure JavaScript is enabled
- Try refreshing the page
- Contact support

#### File Upload Issues
**Issue**: Cannot upload images or files
**Solutions**:
- Check file size (max 5MB)
- Ensure file format is supported
- Check internet connection
- Try a different file

### Browser Compatibility

#### Supported Browsers
- Chrome 80+
- Firefox 75+
- Safari 13+
- Edge 80+

#### Required Settings
- JavaScript enabled
- Cookies enabled
- Pop-ups allowed for test windows
- Local storage enabled

### Performance Tips

#### For Students
- Use a stable internet connection
- Close unnecessary browser tabs
- Don't refresh during a test
- Save answers frequently

#### For Teachers
- Use a modern browser
- Keep question content concise
- Optimize images before uploading
- Test questions before publishing

### Error Messages

#### "Session Expired"
- Solution: Log in again
- Prevention: Keep the page active

#### "Question Not Found"
- Solution: Refresh the page
- Prevention: Don't bookmark question URLs

#### "Test Already Completed"
- Solution: Check test results
- Prevention: Don't attempt test multiple times

#### "Invalid Answer Format"
- Solution: Check answer format
- Prevention: Follow answer guidelines

## 📞 Support

### Getting Help
1. Check this user manual
2. Contact your teacher or administrator
3. Submit a support ticket
4. Check the FAQ section

### Reporting Issues
When reporting issues, include:
- Your role (Student/Teacher/Admin)
- Browser and version
- Steps to reproduce the issue
- Screenshots if applicable
- Error messages

### Contact Information
- **Technical Support**: support@example.com
- **Administrator**: admin@example.com
- **Phone**: +1-234-567-8900

## 📚 Additional Resources

### Video Tutorials
- Getting Started Guide
- Question Creation Tutorial
- Test Taking Guide
- Math Editor Tutorial

### Documentation
- API Documentation
- Developer Guide
- Database Schema
- Deployment Guide

### Best Practices
- Regular password updates
- Backup important data
- Use strong passwords
- Report suspicious activity

---

## 📝 Version History

### Version 1.0.0
- Initial release
- Basic question management
- Student test interface
- Admin dashboard
- Math equation support

---

**Note**: This user manual is regularly updated. Check for the latest version on the system dashboard.







