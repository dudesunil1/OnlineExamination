# Online Examination System - API Documentation

## 📋 Overview

This document provides comprehensive API documentation for the Online Examination System built with ASP.NET MVC 5.

## 🔗 Base URL
```
http://localhost:52734/ (Development)
http://yourserver/OnlineExamination/ (Production)
```

## 🔐 Authentication

The system uses session-based authentication with role-based access control.

### Login Endpoint
```
POST /Login/Login
```

**Request Body:**
```json
{
    "Email": "admin@example.com",
    "Password": "admin123"
}
```

**Response:**
```json
{
    "success": true,
    "message": "Login successful",
    "redirectUrl": "/Admin/Dashboard"
}
```

## 📚 Question Management API

### Get All Questions
```
GET /Question
```

**Response:**
```json
[
    {
        "Ques_Id": 1,
        "Ques_SubName": "Mathematics",
        "Ques_ClassName": "Class 10",
        "Ques_TopName": "Algebra",
        "Ques_PubName": "NCERT",
        "Ques_Mark": 5,
        "Ques_JEEMark": 4,
        "Ques_Negative": 1,
        "Ques_Question": "What is 2+2?",
        "Ques_Answer": "4",
        "Ques_OptionB": "3",
        "Ques_OptionC": "5",
        "Ques_OptionD": "6"
    }
]
```

### Create Question
```
POST /Question/Create
```

**Request Body:**
```json
{
    "Ques_SubId": 1,
    "Ques_ClassId": 1,
    "Ques_TopId": 1,
    "Ques_PubId": 1,
    "Ques_Mark": 5,
    "Ques_JEEMark": 4,
    "Ques_Negative": 1,
    "Ques_Question": "What is the derivative of x²?",
    "Ques_Answer": "2x",
    "Ques_OptionB": "x",
    "Ques_OptionC": "2",
    "Ques_OptionD": "x²"
}
```

**Response:**
```json
{
    "success": true,
    "message": "Question created successfully",
    "questionId": 123
}
```

### Update Question
```
POST /Question/Edit
```

**Request Body:**
```json
{
    "Ques_Id": 123,
    "Ques_SubId": 1,
    "Ques_ClassId": 1,
    "Ques_TopId": 1,
    "Ques_PubId": 1,
    "Ques_Mark": 5,
    "Ques_JEEMark": 4,
    "Ques_Negative": 1,
    "Ques_Question": "What is the derivative of x²?",
    "Ques_Answer": "2x",
    "Ques_OptionB": "x",
    "Ques_OptionC": "2",
    "Ques_OptionD": "x²"
}
```

### Delete Question
```
GET /Question/Delete/{id}
```

**Response:**
```json
{
    "success": true,
    "message": "Question deleted successfully"
}
```

## 👥 Student Management API

### Get All Students
```
GET /StudentMaster
```

**Response:**
```json
[
    {
        "Stu_Id": 1,
        "Stu_Name": "John Doe",
        "Stu_Email": "john@example.com",
        "Stu_Phone": "1234567890",
        "Stu_Address": "123 Main St",
        "Stu_ClassName": "Class 10",
        "Stu_IsActive": true
    }
]
```

### Create Student
```
POST /StudentMaster/Create
```

**Request Body:**
```json
{
    "Stu_Name": "Jane Smith",
    "Stu_Email": "jane@example.com",
    "Stu_Password": "password123",
    "Stu_Phone": "9876543210",
    "Stu_Address": "456 Oak Ave",
    "Stu_ClassId": 1
}
```

### Update Student
```
POST /StudentMaster/Edit
```

**Request Body:**
```json
{
    "Stu_Id": 1,
    "Stu_Name": "John Doe Updated",
    "Stu_Email": "john@example.com",
    "Stu_Phone": "1234567890",
    "Stu_Address": "123 Main St Updated",
    "Stu_ClassId": 1
}
```

## 📝 Test Management API

### Get All Tests
```
GET /TestMaster
```

**Response:**
```json
[
    {
        "Test_Id": 1,
        "Test_Name": "Mathematics Quiz",
        "Test_Description": "Basic math concepts",
        "Test_Duration": 60,
        "Test_TotalMarks": 50,
        "Test_IsActive": true
    }
]
```

### Create Test
```
POST /TestMaster/Create
```

**Request Body:**
```json
{
    "Test_Name": "Physics Test",
    "Test_Description": "Physics fundamentals",
    "Test_Duration": 90,
    "Test_TotalMarks": 100
}
```

### Assign Test to Students
```
POST /AssignTest/Create
```

**Request Body:**
```json
{
    "TestId": 1,
    "StudentIds": [1, 2, 3],
    "StartDate": "2024-01-01T09:00:00",
    "EndDate": "2024-01-01T17:00:00"
}
```

## 📊 Test Results API

### Get Test Results
```
GET /TestResult/List
```

**Query Parameters:**
- `testId` (optional): Filter by test ID
- `studentId` (optional): Filter by student ID

**Response:**
```json
[
    {
        "TR_Id": 1,
        "TestName": "Mathematics Quiz",
        "StudentName": "John Doe",
        "QuestionText": "What is 2+2?",
        "StudentAnswer": "4",
        "CorrectAnswer": "4",
        "IsCorrect": true,
        "MarksObtained": 5,
        "SubmittedDate": "2024-01-01T10:30:00"
    }
]
```

### Submit Test Answer
```
POST /Student/SubmitAnswer
```

**Request Body:**
```json
{
    "TestId": 1,
    "QuestionId": 1,
    "Answer": "4"
}
```

## 📚 Master Data API

### Get Subjects
```
GET /Master/GetSubjects
```

**Response:**
```json
[
    {
        "Value": "1",
        "Text": "Mathematics"
    },
    {
        "Value": "2",
        "Text": "Physics"
    }
]
```

### Get Classes
```
GET /Master/GetClasses
```

**Response:**
```json
[
    {
        "Value": "1",
        "Text": "Class 10"
    },
    {
        "Value": "2",
        "Text": "Class 11"
    }
]
```

### Get Topics by Subject and Class
```
GET /Master/GetTopicsBySubjectIDAndClassID?SubjectID=1&ClassID=1
```

**Response:**
```json
[
    {
        "Top_Id": 1,
        "Top_Name": "Algebra"
    },
    {
        "Top_Id": 2,
        "Top_Name": "Geometry"
    }
]
```

### Get Publications
```
GET /Master/GetPublications
```

**Response:**
```json
[
    {
        "Value": "1",
        "Text": "NCERT"
    },
    {
        "Value": "2",
        "Text": "RD Sharma"
    }
]
```

## 🔑 API Key Management

### Inactivate API Key
```
POST /Question/InActivateKey
```

**Request Body:**
```json
{
    "apiKey": "your-api-key-here"
}
```

**Response:**
```json
{
    "success": true,
    "message": "Key Inactivated Successfully"
}
```

## 📈 Dashboard API

### Admin Dashboard
```
GET /Admin/Dashboard
```

**Response:**
```json
{
    "totalStudents": 150,
    "totalQuestions": 500,
    "totalTests": 25,
    "activeTests": 5,
    "recentActivity": [
        {
            "type": "Test Completed",
            "student": "John Doe",
            "test": "Mathematics Quiz",
            "timestamp": "2024-01-01T10:30:00"
        }
    ]
}
```

### Student Dashboard
```
GET /Student/Dashboard
```

**Response:**
```json
{
    "studentName": "John Doe",
    "className": "Class 10",
    "availableTests": 3,
    "completedTests": 5,
    "averageScore": 85.5,
    "upcomingTests": [
        {
            "testId": 1,
            "testName": "Physics Test",
            "startDate": "2024-01-02T09:00:00",
            "duration": 90
        }
    ]
}
```

## 🚨 Error Responses

### Common Error Codes

**400 Bad Request**
```json
{
    "success": false,
    "message": "Invalid request data",
    "errors": [
        "Email is required",
        "Password must be at least 6 characters"
    ]
}
```

**401 Unauthorized**
```json
{
    "success": false,
    "message": "Authentication required"
}
```

**403 Forbidden**
```json
{
    "success": false,
    "message": "Access denied. Insufficient permissions."
}
```

**404 Not Found**
```json
{
    "success": false,
    "message": "Resource not found"
}
```

**500 Internal Server Error**
```json
{
    "success": false,
    "message": "An unexpected error occurred"
}
```

## 🔒 Security Considerations

### Authentication
- All API endpoints require valid session authentication
- Session timeout: 30 minutes
- Role-based access control enforced

### Data Validation
- Server-side validation on all inputs
- SQL injection prevention using parameterized queries
- XSS protection with HTML encoding

### Rate Limiting
- Implement rate limiting for API endpoints
- Monitor for suspicious activity
- Log all API requests

## 📝 Request/Response Examples

### Creating a Question with Math Content

**Request:**
```json
{
    "Ques_SubId": 1,
    "Ques_ClassId": 1,
    "Ques_TopId": 1,
    "Ques_PubId": 1,
    "Ques_Mark": 5,
    "Ques_JEEMark": 4,
    "Ques_Negative": 1,
    "Ques_Question": "Solve: $\\frac{d}{dx}(x^2 + 3x + 2)$",
    "Ques_Answer": "$2x + 3$",
    "Ques_OptionB": "$x + 3$",
    "Ques_OptionC": "$2x$",
    "Ques_OptionD": "$x^2 + 3$"
}
```

**Response:**
```json
{
    "success": true,
    "message": "Question created successfully",
    "questionId": 124
}
```

## 🔄 Webhook Events

### Test Completion Webhook
```
POST /webhooks/test-completed
```

**Payload:**
```json
{
    "event": "test.completed",
    "data": {
        "testId": 1,
        "studentId": 1,
        "score": 85,
        "totalMarks": 100,
        "completedAt": "2024-01-01T10:30:00"
    }
}
```

## 📊 API Rate Limits

| Endpoint Category | Rate Limit | Window |
|------------------|------------|---------|
| Authentication | 5 requests | 1 minute |
| Question Management | 100 requests | 1 hour |
| Student Management | 200 requests | 1 hour |
| Test Management | 50 requests | 1 hour |
| General API | 1000 requests | 1 hour |

## 🛠️ SDK and Libraries

### JavaScript SDK
```javascript
// Initialize the API client
const api = new OnlineExamAPI({
    baseUrl: 'http://localhost:52734',
    apiKey: 'your-api-key'
});

// Create a question
const question = await api.questions.create({
    subjectId: 1,
    classId: 1,
    topicId: 1,
    question: "What is 2+2?",
    answer: "4"
});
```

### C# SDK
```csharp
// Initialize the API client
var api = new OnlineExamAPIClient("http://localhost:52734");

// Create a question
var question = await api.Questions.CreateAsync(new QuestionRequest
{
    SubjectId = 1,
    ClassId = 1,
    TopicId = 1,
    Question = "What is 2+2?",
    Answer = "4"
});
```

---

## 📞 Support

For API support:
- Check the troubleshooting section
- Review error logs
- Contact the development team
- Create a support ticket

## 📝 Changelog

### Version 1.0.0
- Initial API release
- Question management endpoints
- Student management endpoints
- Test management endpoints
- Authentication system











