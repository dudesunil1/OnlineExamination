# Online Examination System - Setup Instructions

## 🚀 Quick Setup Guide

### Prerequisites
- Windows 10/11 or Windows Server 2016+
- Visual Studio 2019/2022 or Visual Studio Code
- SQL Server 2016 or later
- .NET Framework 4.8
- IIS Express (for local development)

## 📋 Step-by-Step Setup

### 1. Download and Extract
1. Download the project files
2. Extract to a folder (e.g., `C:\Projects\OnlineExamination`)
3. Ensure all files are extracted properly

### 2. Database Setup

#### Option A: Using SQL Server Management Studio
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Open the file `Database\SetupScript.sql`
4. Execute the script
5. Verify database `OnlineExamination` is created

#### Option B: Using Command Line
```bash
# Navigate to project directory
cd C:\Projects\OnlineExamination

# Run SQL script
sqlcmd -S localhost -i Database\SetupScript.sql
```

### 3. Configure Connection String
1. Open `Web.config` in a text editor
2. Find the connection string section
3. Update the connection string:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=OnlineExamination;Integrated Security=True;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

### 4. Build the Project

#### Using Visual Studio
1. Open `OnlineExamination.sln` in Visual Studio
2. Right-click on the solution → Restore NuGet Packages
3. Build the solution (Ctrl+Shift+B)
4. Ensure build is successful

#### Using Command Line
```bash
# Navigate to project directory
cd C:\Projects\OnlineExamination

# Restore packages
nuget restore OnlineExamination.sln

# Build project
msbuild OnlineExamination.sln /p:Configuration=Debug
```

### 5. Run the Application

#### Using Visual Studio
1. Press F5 or click "Start Debugging"
2. The application will open in your default browser
3. Default URL: `http://localhost:52734`

#### Using IIS Express
1. Right-click on the project → "View in Browser"
2. Or navigate to the URL shown in the output

### 6. Initial Configuration

#### Login with Default Credentials
- **Admin**: admin@example.com / admin123
- **Teacher**: teacher@example.com / teacher123
- **Student**: student@example.com / student123

#### Change Default Passwords
1. Log in as admin
2. Navigate to User Management
3. Change default passwords for security

## 🔧 Configuration Options

### Application Settings

#### Web.config Settings
```xml
<appSettings>
  <!-- Session timeout in minutes -->
  <add key="SessionTimeout" value="30" />
  
  <!-- Maximum file upload size in MB -->
  <add key="MaxFileSize" value="5" />
  
  <!-- Enable/disable debug mode -->
  <add key="DebugMode" value="false" />
  
  <!-- API key for external services -->
  <add key="TinyMCEApiKey" value="your-api-key-here" />
</appSettings>
```

#### TinyMCE Configuration
Update the TinyMCE API key in the views:
```javascript
<script src="https://cdn.tiny.cloud/1/YOUR_API_KEY/tinymce/6/tinymce.min.js"></script>
```

### Database Configuration

#### Connection String Options
```xml
<!-- Windows Authentication -->
<add name="DefaultConnection" 
     connectionString="Data Source=ServerName;Initial Catalog=OnlineExamination;Integrated Security=True;" 
     providerName="System.Data.SqlClient" />

<!-- SQL Server Authentication -->
<add name="DefaultConnection" 
     connectionString="Data Source=ServerName;Initial Catalog=OnlineExamination;User ID=username;Password=password;" 
     providerName="System.Data.SqlClient" />
```

## 🚀 Deployment Options

### Local Development
1. Use IIS Express (default)
2. Configure port in project properties
3. Enable SSL if needed

### IIS Deployment
1. Publish the application
2. Create application pool (.NET Framework 4.8)
3. Create website pointing to published folder
4. Configure permissions

### Azure Deployment
1. Create Azure App Service
2. Create SQL Database
3. Deploy using Visual Studio or Azure DevOps
4. Update connection strings

## 🔍 Troubleshooting

### Common Issues

#### Build Errors
**Error**: "Could not load file or assembly"
**Solution**:
- Restore NuGet packages
- Check .NET Framework version
- Clean and rebuild solution

#### Database Connection Error
**Error**: "Cannot connect to database"
**Solution**:
- Verify SQL Server is running
- Check connection string
- Ensure database exists
- Check firewall settings

#### TinyMCE Not Loading
**Error**: Rich text editor not appearing
**Solution**:
- Check internet connection
- Verify API key
- Check browser console for errors
- Try different browser

#### Math Equations Not Rendering
**Error**: LaTeX code showing instead of equations
**Solution**:
- Check MathJax CDN access
- Verify LaTeX syntax
- Check browser compatibility
- Clear browser cache

### Performance Issues

#### Slow Loading
**Solutions**:
- Enable browser caching
- Optimize images
- Use CDN for static resources
- Check database performance

#### Memory Issues
**Solutions**:
- Increase application pool memory
- Optimize database queries
- Enable output caching
- Monitor resource usage

## 📊 System Requirements

### Minimum Requirements
- **OS**: Windows 10 or Windows Server 2016
- **RAM**: 4GB
- **Storage**: 10GB free space
- **CPU**: 2 cores, 2.0 GHz
- **Browser**: Chrome 80+, Firefox 75+, Safari 13+, Edge 80+

### Recommended Requirements
- **OS**: Windows 11 or Windows Server 2019
- **RAM**: 8GB or more
- **Storage**: 20GB free space
- **CPU**: 4 cores, 3.0 GHz or higher
- **Browser**: Latest version of Chrome or Edge

### Database Requirements
- **SQL Server**: 2016 or later
- **RAM**: 4GB minimum for SQL Server
- **Storage**: 5GB for database files
- **CPU**: 2 cores minimum

## 🔒 Security Considerations

### Initial Security Setup
1. Change default passwords
2. Enable HTTPS in production
3. Configure firewall rules
4. Set up regular backups
5. Enable SQL Server authentication if needed

### User Management
1. Create strong password policies
2. Implement account lockout policies
3. Regular password updates
4. Monitor user activity

### Data Protection
1. Encrypt sensitive data
2. Regular database backups
3. Secure file uploads
4. Input validation and sanitization

## 📈 Monitoring and Maintenance

### Regular Tasks
- **Daily**: Check application logs
- **Weekly**: Review error logs
- **Monthly**: Database maintenance
- **Quarterly**: Security audit

### Backup Strategy
1. **Database**: Full backup daily
2. **Application**: Weekly backup
3. **Configuration**: Monthly backup
4. **Test**: Backup before major changes

### Performance Monitoring
1. Monitor CPU and memory usage
2. Check database performance
3. Monitor user activity
4. Review error rates

## 🆘 Getting Help

### Documentation
- README.md - Project overview
- API_DOCUMENTATION.md - API reference
- USER_MANUAL.md - User guide
- DEPLOYMENT_GUIDE.md - Deployment instructions

### Support Channels
- **Email**: support@example.com
- **Phone**: +1-234-567-8900
- **Documentation**: Check project files
- **Community**: Online forums

### Common Solutions
1. **Restart the application**
2. **Clear browser cache**
3. **Check internet connection**
4. **Verify database connectivity**
5. **Check file permissions**

## 📝 Next Steps

After successful setup:
1. **Configure Users**: Add teachers and students
2. **Create Content**: Add subjects, topics, and questions
3. **Set Up Tests**: Create and assign tests
4. **Train Users**: Provide training to teachers and students
5. **Monitor Usage**: Track system usage and performance

## 🔄 Updates and Maintenance

### Keeping the System Updated
1. Regular security updates
2. Feature updates
3. Bug fixes
4. Performance improvements

### Backup Before Updates
1. Database backup
2. Application backup
3. Configuration backup
4. Test the update in staging

---

## ✅ Setup Checklist

- [ ] Prerequisites installed
- [ ] Project files extracted
- [ ] Database created and configured
- [ ] Connection string updated
- [ ] Project built successfully
- [ ] Application running
- [ ] Default login working
- [ ] Passwords changed
- [ ] Basic functionality tested
- [ ] Documentation reviewed

---

**Note**: This setup guide is for the standard installation. For custom configurations or enterprise deployments, contact the development team.











