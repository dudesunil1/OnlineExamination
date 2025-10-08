# Online Examination System - Deployment Guide

## 🚀 Quick Deployment Steps

### Prerequisites
- Windows Server 2016 or later
- IIS 8.5 or later
- .NET Framework 4.8
- SQL Server 2016 or later
- Visual Studio 2019/2022 (for building)

### 1. Database Setup

#### Step 1: Create Database
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Run the `Database/SetupScript.sql` file
4. Verify all tables and data are created

#### Step 2: Update Connection String
1. Open `Web.config` in your project
2. Update the connection string:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=OnlineExamination;Integrated Security=True;MultipleActiveResultSets=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 2. Build and Publish

#### Option A: Visual Studio
1. Open `OnlineExamination.sln` in Visual Studio
2. Right-click on the project → Publish
3. Choose "IIS, FTP, etc." as publish target
4. Set the target location (e.g., `C:\inetpub\wwwroot\OnlineExamination`)
5. Click Publish

#### Option B: Command Line
```bash
# Navigate to project directory
cd D:\Application

# Build the project
msbuild OnlineExamination.sln /p:Configuration=Release

# Publish using MSBuild
msbuild OnlineExamination.csproj /p:Configuration=Release /p:PublishUrl="C:\inetpub\wwwroot\OnlineExamination\" /p:WebPublishMethod=FileSystem
```

### 3. IIS Configuration

#### Step 1: Create Application Pool
1. Open IIS Manager
2. Right-click "Application Pools" → Add Application Pool
3. Name: `OnlineExamination`
4. .NET CLR Version: `.NET CLR Version v4.0`
5. Managed Pipeline Mode: `Integrated`
6. Click OK

#### Step 2: Create Website
1. Right-click "Sites" → Add Website
2. Site name: `OnlineExamination`
3. Application pool: `OnlineExamination`
4. Physical path: `C:\inetpub\wwwroot\OnlineExamination`
5. Port: `80` (or your preferred port)
6. Click OK

#### Step 3: Configure Application Pool
1. Select the `OnlineExamination` application pool
2. Click "Advanced Settings"
3. Set the following:
   - Identity: `ApplicationPoolIdentity`
   - Load User Profile: `True`
   - Enable 32-Bit Applications: `False`

### 4. File Permissions

#### Set Permissions for IIS
1. Right-click the application folder → Properties
2. Go to Security tab
3. Click Edit → Add
4. Add `IIS_IUSRS` with Full Control
5. Add `IUSR` with Read & Execute permissions

### 5. Configuration Files

#### Web.config Settings
Ensure these settings are correct:

```xml
<system.web>
  <compilation debug="false" targetFramework="4.8" />
  <httpRuntime targetFramework="4.8" maxRequestLength="51200" executionTimeout="300" />
  <authentication mode="Forms">
    <forms loginUrl="~/Login/Login" timeout="30" />
  </authentication>
  <sessionState mode="InProc" timeout="30" />
</system.web>
```

### 6. Testing Deployment

#### Step 1: Test Database Connection
1. Navigate to `http://yourserver/OnlineExamination`
2. Check if the application loads without errors
3. Try logging in with default credentials

#### Step 2: Test Core Features
1. **Login**: Test admin, teacher, and student login
2. **Question Management**: Create a test question
3. **Student Registration**: Register a new student
4. **Test Creation**: Create a sample test

### 7. Production Optimizations

#### Performance Settings
```xml
<system.web>
  <compilation debug="false" targetFramework="4.8" />
  <httpRuntime targetFramework="4.8" maxRequestLength="51200" executionTimeout="300" />
  <sessionState mode="StateServer" stateConnectionString="tcpip=127.0.0.1:42424" timeout="30" />
</system.web>
```

#### Security Settings
```xml
<system.web>
  <httpCookies httpOnlyCookies="true" requireSSL="false" />
  <customErrors mode="RemoteOnly" defaultRedirect="~/Error" />
</system.web>
```

### 8. Monitoring and Maintenance

#### Log Files
- Application logs: `C:\inetpub\logs\LogFiles\`
- Event Viewer: Windows Logs → Application

#### Performance Monitoring
- Use IIS Manager → Worker Processes
- Monitor CPU and Memory usage
- Check database performance

### 9. Backup Strategy

#### Database Backup
```sql
-- Full backup
BACKUP DATABASE OnlineExamination TO DISK = 'C:\Backup\OnlineExamination_Full.bak'

-- Transaction log backup
BACKUP LOG OnlineExamination TO DISK = 'C:\Backup\OnlineExamination_Log.trn'
```

#### Application Backup
- Backup the entire application folder
- Backup configuration files
- Document customizations

### 10. Troubleshooting

#### Common Issues

**Issue**: "Could not load file or assembly" error
**Solution**: 
- Ensure .NET Framework 4.8 is installed
- Check application pool .NET version
- Verify all DLLs are in the bin folder

**Issue**: Database connection error
**Solution**:
- Verify connection string
- Check SQL Server is running
- Ensure database exists and is accessible

**Issue**: TinyMCE not loading
**Solution**:
- Check internet connectivity for CDN resources
- Verify API key configuration
- Check browser console for errors

**Issue**: Math equations not rendering
**Solution**:
- Ensure MathJax CDN is accessible
- Check LaTeX syntax
- Verify MathJax configuration

### 11. SSL Configuration (Optional)

#### Install SSL Certificate
1. Obtain SSL certificate from CA
2. Install certificate in IIS
3. Bind HTTPS to port 443
4. Redirect HTTP to HTTPS

#### Update Application for HTTPS
```xml
<system.web>
  <httpCookies httpOnlyCookies="true" requireSSL="true" />
</system.web>
```

### 12. Load Balancing (For High Traffic)

#### Multiple Server Setup
1. Deploy application on multiple servers
2. Use load balancer (e.g., Application Request Routing)
3. Configure shared session state
4. Use shared database

### 13. Maintenance Schedule

#### Daily
- Check application logs
- Monitor database performance
- Verify backup completion

#### Weekly
- Review error logs
- Check disk space
- Update security patches

#### Monthly
- Full database backup
- Performance analysis
- Security audit

---

## 📞 Support

For deployment issues:
1. Check the troubleshooting section
2. Review application logs
3. Contact system administrator
4. Create support ticket

## 📝 Notes

- Always test in a staging environment first
- Keep backups before major changes
- Document all customizations
- Monitor system performance regularly






