# Online Examination System

A comprehensive web-based examination management system built with ASP.NET MVC 5, featuring modern UI/UX design, mathematical equation support, and robust question management capabilities.

## 🚀 Features

### Core Functionality
- **Question Management**: Create, edit, and manage questions with rich text editing
- **Mathematical Support**: LaTeX equation editor with live preview using MathJax
- **Test Management**: Create and assign tests to students
- **Student Management**: Comprehensive student profile and group management
- **Result Management**: Track and analyze test results
- **Multi-role System**: Admin, Teacher, and Student roles

### Technical Features
- **Modern UI**: Bootstrap 5 with responsive design
- **Rich Text Editing**: TinyMCE integration with math equation support
- **Real-time Validation**: Client-side and server-side validation
- **Progress Tracking**: Visual progress indicators
- **Error Handling**: Comprehensive error handling and user feedback
- **Security**: Session-based authentication and role-based access control

## 🛠️ Technology Stack

- **Framework**: ASP.NET MVC 5 (.NET Framework 4.8)
- **Frontend**: Bootstrap 5, jQuery 3.7, TinyMCE 6
- **Math Support**: MathJax 3, LaTeX
- **Database**: SQL Server (via stored procedures)
- **UI Components**: Font Awesome 6, Select2
- **Pagination**: PagedList.Mvc

## 📋 Prerequisites

- Visual Studio 2019/2022 or Visual Studio Code
- .NET Framework 4.8
- SQL Server 2016 or later
- IIS Express (for local development)

## 🚀 Quick Start

### 1. Clone the Repository
```bash
git clone <repository-url>
cd OnlineExamination
```

### 2. Database Setup
1. Create a new SQL Server database named `OnlineExamination`
2. Run the database setup script (see Database Setup section)
3. Update connection string in `Web.config`

### 3. Build and Run
1. Open `OnlineExamination.sln` in Visual Studio
2. Restore NuGet packages
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

### 4. Default Login
- **Admin**: admin@example.com / admin123
- **Teacher**: teacher@example.com / teacher123
- **Student**: student@example.com / student123

## 🗄️ Database Setup

### Connection String
Update the connection string in `Web.config`:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=YOUR_SERVER;Initial Catalog=OnlineExamination;Integrated Security=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Required Tables
The system uses the following main tables:
- `SubjectMaster` - Subject information
- `ClassMaster` - Class/grade information
- `TopicMaster` - Topic information
- `PublicationMaster` - Publication information
- `QuestionMaster` - Question bank
- `StudentMaster` - Student information
- `TestMaster` - Test information
- `TestResult` - Test results

## 📁 Project Structure

```
OnlineExamination/
├── Controllers/          # MVC Controllers
│   ├── QuestionController.cs
│   ├── StudentController.cs
│   ├── TestController.cs
│   └── ...
├── Models/              # ViewModels and Data Models
│   ├── QuestionMasterModel.cs
│   ├── StudentMasterModel.cs
│   └── ...
├── Views/               # Razor Views
│   ├── Question/
│   ├── Student/
│   └── Shared/
├── BLL/                 # Business Logic Layer
│   ├── QuestionService.cs
│   ├── StudentService.cs
│   └── ...
├── Database/            # Data Access Layer
│   ├── clsSunDAL.cs
│   └── ControlFill.cs
├── Content/             # Static Content
│   ├── css/
│   ├── js/
│   └── images/
└── Scripts/             # JavaScript Libraries
```

## 🎯 Key Features Explained

### Question Management
- **Rich Text Editor**: TinyMCE with mathematical equation support
- **Math Equations**: LaTeX input with live preview using MathJax
- **Question Types**: Multiple choice, descriptive questions
- **Media Support**: Image and file attachments
- **Validation**: Real-time form validation

### Test Management
- **Test Creation**: Create tests with multiple questions
- **Question Selection**: Select questions by subject, topic, difficulty
- **Time Management**: Set time limits for tests
- **Randomization**: Randomize question order
- **Scoring**: Configurable scoring system

### Student Interface
- **Dashboard**: Student-specific dashboard
- **Test List**: Available tests for the student
- **Test Taking**: Interactive test interface
- **Results**: View test results and performance

## 🔧 Configuration

### TinyMCE Configuration
The system uses TinyMCE 6 with custom math equation support:

```javascript
tinymce.init({
    selector: '#editor',
    plugins: ['advlist', 'autolink', 'lists', 'link', 'math'],
    toolbar: 'undo redo | bold italic | math',
    setup: function (editor) {
        // Custom math button implementation
    }
});
```

### MathJax Configuration
MathJax 3 is configured for LaTeX rendering:

```javascript
window.MathJax = {
    tex: {
        inlineMath: [['$', '$'], ['\\(', '\\)']],
        displayMath: [['$$', '$$'], ['\\[', '\\]']]
    }
};
```

## 🚀 Deployment

### IIS Deployment
1. Publish the application to a folder
2. Create a new website in IIS
3. Point to the published folder
4. Configure application pool for .NET Framework 4.8
5. Set appropriate permissions

### Azure Deployment
1. Create an Azure App Service
2. Configure SQL Database
3. Deploy using Visual Studio or Azure DevOps
4. Update connection strings

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify connection string
   - Check SQL Server is running
   - Ensure database exists

2. **TinyMCE Not Loading**
   - Check internet connection for CDN resources
   - Verify API key configuration
   - Check browser console for errors

3. **Math Equations Not Rendering**
   - Ensure MathJax is loaded
   - Check LaTeX syntax
   - Verify MathJax configuration

4. **Session Timeout**
   - Check session timeout settings
   - Verify authentication configuration
   - Check server session state

## 📝 API Documentation

### Question Controller
- `GET /Question` - List all questions
- `GET /Question/Create` - Create question form
- `POST /Question/Create` - Submit new question
- `GET /Question/Edit/{id}` - Edit question form
- `POST /Question/Edit` - Update question
- `GET /Question/Delete/{id}` - Delete question

### Student Controller
- `GET /Student` - Student dashboard
- `GET /Student/TestList` - Available tests
- `GET /Student/TestDetails/{id}` - Test details
- `POST /Student/SubmitTest` - Submit test answers

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📞 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation

## 🔄 Version History

### v1.0.0 (Current)
- Initial release
- Question management system
- Student test interface
- Admin dashboard
- Mathematical equation support

## 🎉 Acknowledgments

- Bootstrap team for the excellent UI framework
- TinyMCE team for the rich text editor
- MathJax team for mathematical rendering
- All contributors and testers

---

**Note**: This is a comprehensive examination management system designed for educational institutions. Ensure proper security measures are in place before deploying to production.











