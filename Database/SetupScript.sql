-- Online Examination System Database Setup Script
-- Version: 1.0
-- Created: 2024

USE master;
GO

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OnlineExamination')
BEGIN
    CREATE DATABASE OnlineExamination;
    PRINT 'Database OnlineExamination created successfully.';
END
ELSE
BEGIN
    PRINT 'Database OnlineExamination already exists.';
END
GO

USE OnlineExamination;
GO

-- Create Tables

-- Subject Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SubjectMaster' AND xtype='U')
BEGIN
    CREATE TABLE SubjectMaster (
        Sub_Id INT IDENTITY(1,1) PRIMARY KEY,
        Sub_Name NVARCHAR(100) NOT NULL,
        Sub_Description NVARCHAR(500),
        Sub_IsActive BIT DEFAULT 1,
        Sub_CreatedDate DATETIME DEFAULT GETDATE(),
        Sub_ModifiedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table SubjectMaster created successfully.';
END
GO

-- Class Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ClassMaster' AND xtype='U')
BEGIN
    CREATE TABLE ClassMaster (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL,
        Description NVARCHAR(200),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table ClassMaster created successfully.';
END
GO

-- Topic Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TopicMaster' AND xtype='U')
BEGIN
    CREATE TABLE TopicMaster (
        Top_Id INT IDENTITY(1,1) PRIMARY KEY,
        Top_Name NVARCHAR(100) NOT NULL,
        Top_SubId INT NOT NULL,
        Top_ClassID INT NOT NULL,
        Top_Description NVARCHAR(500),
        Top_IsActive BIT DEFAULT 1,
        Top_CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (Top_SubId) REFERENCES SubjectMaster(Sub_Id),
        FOREIGN KEY (Top_ClassID) REFERENCES ClassMaster(ID)
    );
    PRINT 'Table TopicMaster created successfully.';
END
GO

-- Publication Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PublicationMaster' AND xtype='U')
BEGIN
    CREATE TABLE PublicationMaster (
        Pub_Id INT IDENTITY(1,1) PRIMARY KEY,
        Pub_Name NVARCHAR(100) NOT NULL,
        Pub_Description NVARCHAR(500),
        Pub_IsActive BIT DEFAULT 1,
        Pub_CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table PublicationMaster created successfully.';
END
GO

-- Question Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='QuestionMaster' AND xtype='U')
BEGIN
    CREATE TABLE QuestionMaster (
        Ques_Id INT IDENTITY(1,1) PRIMARY KEY,
        Ques_SubId INT NOT NULL,
        Ques_ClassId INT NOT NULL,
        Ques_TopId INT NOT NULL,
        Ques_PubId INT NOT NULL,
        Ques_Mark INT NOT NULL,
        Ques_JEEMark INT NOT NULL,
        Ques_Negative INT NOT NULL,
        Ques_Question NVARCHAR(MAX) NOT NULL,
        Ques_Answer NVARCHAR(MAX) NOT NULL,
        Ques_OptionB NVARCHAR(MAX),
        Ques_OptionC NVARCHAR(MAX),
        Ques_OptionD NVARCHAR(MAX),
        Ques_SolutionDetails NVARCHAR(MAX),
        Ques_IsActive BIT DEFAULT 1,
        Ques_CreatedDate DATETIME DEFAULT GETDATE(),
        Ques_ModifiedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (Ques_SubId) REFERENCES SubjectMaster(Sub_Id),
        FOREIGN KEY (Ques_ClassId) REFERENCES ClassMaster(ID),
        FOREIGN KEY (Ques_TopId) REFERENCES TopicMaster(Top_Id),
        FOREIGN KEY (Ques_PubId) REFERENCES PublicationMaster(Pub_Id)
    );
    PRINT 'Table QuestionMaster created successfully.';
END
GO

-- Student Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StudentMaster' AND xtype='U')
BEGIN
    CREATE TABLE StudentMaster (
        Stu_Id INT IDENTITY(1,1) PRIMARY KEY,
        Stu_Name NVARCHAR(100) NOT NULL,
        Stu_Email NVARCHAR(100) UNIQUE NOT NULL,
        Stu_Password NVARCHAR(255) NOT NULL,
        Stu_Phone NVARCHAR(20),
        Stu_Address NVARCHAR(500),
        Stu_ClassId INT,
        Stu_IsActive BIT DEFAULT 1,
        Stu_CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (Stu_ClassId) REFERENCES ClassMaster(ID)
    );
    PRINT 'Table StudentMaster created successfully.';
END
GO

-- Group Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='GroupMaster' AND xtype='U')
BEGIN
    CREATE TABLE GroupMaster (
        Grp_Id INT IDENTITY(1,1) PRIMARY KEY,
        Grp_Name NVARCHAR(100) NOT NULL,
        Grp_Description NVARCHAR(500),
        Grp_IsActive BIT DEFAULT 1,
        Grp_CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table GroupMaster created successfully.';
END
GO

-- Group Details Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='GroupDetails' AND xtype='U')
BEGIN
    CREATE TABLE GroupDetails (
        GD_Id INT IDENTITY(1,1) PRIMARY KEY,
        GD_GrpId INT NOT NULL,
        GD_StuId INT NOT NULL,
        GD_IsActive BIT DEFAULT 1,
        GD_CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (GD_GrpId) REFERENCES GroupMaster(Grp_Id),
        FOREIGN KEY (GD_StuId) REFERENCES StudentMaster(Stu_Id)
    );
    PRINT 'Table GroupDetails created successfully.';
END
GO

-- Test Master Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TestMaster' AND xtype='U')
BEGIN
    CREATE TABLE TestMaster (
        Test_Id INT IDENTITY(1,1) PRIMARY KEY,
        Test_Name NVARCHAR(100) NOT NULL,
        Test_Description NVARCHAR(500),
        Test_Duration INT NOT NULL, -- in minutes
        Test_TotalMarks INT NOT NULL,
        Test_IsActive BIT DEFAULT 1,
        Test_CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table TestMaster created successfully.';
END
GO

-- Test Questions Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TestQuestions' AND xtype='U')
BEGIN
    CREATE TABLE TestQuestions (
        TQ_Id INT IDENTITY(1,1) PRIMARY KEY,
        TQ_TestId INT NOT NULL,
        TQ_QuestionId INT NOT NULL,
        TQ_IsActive BIT DEFAULT 1,
        TQ_CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (TQ_TestId) REFERENCES TestMaster(Test_Id),
        FOREIGN KEY (TQ_QuestionId) REFERENCES QuestionMaster(Ques_Id)
    );
    PRINT 'Table TestQuestions created successfully.';
END
GO

-- Test Result Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TestResult' AND xtype='U')
BEGIN
    CREATE TABLE TestResult (
        TR_Id INT IDENTITY(1,1) PRIMARY KEY,
        TR_TestId INT NOT NULL,
        TR_StudentId INT NOT NULL,
        TR_QuestionId INT NOT NULL,
        TR_Answer NVARCHAR(MAX),
        TR_IsCorrect BIT,
        TR_MarksObtained INT,
        TR_SubmittedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (TR_TestId) REFERENCES TestMaster(Test_Id),
        FOREIGN KEY (TR_StudentId) REFERENCES StudentMaster(Stu_Id),
        FOREIGN KEY (TR_QuestionId) REFERENCES QuestionMaster(Ques_Id)
    );
    PRINT 'Table TestResult created successfully.';
END
GO

-- Admin Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='AdminMaster' AND xtype='U')
BEGIN
    CREATE TABLE AdminMaster (
        Admin_Id INT IDENTITY(1,1) PRIMARY KEY,
        Admin_Name NVARCHAR(100) NOT NULL,
        Admin_Email NVARCHAR(100) UNIQUE NOT NULL,
        Admin_Password NVARCHAR(255) NOT NULL,
        Admin_Role NVARCHAR(50) DEFAULT 'Admin',
        Admin_IsActive BIT DEFAULT 1,
        Admin_CreatedDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Table AdminMaster created successfully.';
END
GO

-- API Key Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='APIKeys' AND xtype='U')
BEGIN
    CREATE TABLE APIKeys (
        Key_Id INT IDENTITY(1,1) PRIMARY KEY,
        Key_Value NVARCHAR(255) NOT NULL,
        Key_IsActive BIT DEFAULT 1,
        Key_CreatedDate DATETIME DEFAULT GETDATE(),
        Key_ExpiryDate DATETIME
    );
    PRINT 'Table APIKeys created successfully.';
END
GO

-- Insert Sample Data

-- Insert Sample Subjects
IF NOT EXISTS (SELECT * FROM SubjectMaster WHERE Sub_Name = 'Mathematics')
BEGIN
    INSERT INTO SubjectMaster (Sub_Name, Sub_Description) VALUES 
    ('Mathematics', 'Mathematical concepts and problem solving'),
    ('Physics', 'Physical sciences and laws'),
    ('Chemistry', 'Chemical sciences and reactions'),
    ('Biology', 'Life sciences and organisms'),
    ('English', 'English language and literature');
    PRINT 'Sample subjects inserted.';
END
GO

-- Insert Sample Classes
IF NOT EXISTS (SELECT * FROM ClassMaster WHERE Name = 'Class 10')
BEGIN
    INSERT INTO ClassMaster (Name, Description) VALUES 
    ('Class 10', 'Tenth Grade'),
    ('Class 11', 'Eleventh Grade'),
    ('Class 12', 'Twelfth Grade'),
    ('JEE Preparation', 'JEE Main and Advanced Preparation'),
    ('NEET Preparation', 'NEET Medical Entrance Preparation');
    PRINT 'Sample classes inserted.';
END
GO

-- Insert Sample Publications
IF NOT EXISTS (SELECT * FROM PublicationMaster WHERE Pub_Name = 'NCERT')
BEGIN
    INSERT INTO PublicationMaster (Pub_Name, Pub_Description) VALUES 
    ('NCERT', 'National Council of Educational Research and Training'),
    ('RD Sharma', 'Mathematics reference book'),
    ('HC Verma', 'Physics reference book'),
    ('OP Tandon', 'Chemistry reference book'),
    ('Arihant', 'Competitive exam preparation books');
    PRINT 'Sample publications inserted.';
END
GO

-- Insert Sample Topics
IF NOT EXISTS (SELECT * FROM TopicMaster WHERE Top_Name = 'Algebra')
BEGIN
    INSERT INTO TopicMaster (Top_Name, Top_SubId, Top_ClassID, Top_Description) VALUES 
    ('Algebra', 1, 1, 'Algebraic expressions and equations'),
    ('Geometry', 1, 1, 'Geometric shapes and properties'),
    ('Trigonometry', 1, 1, 'Trigonometric functions and identities'),
    ('Mechanics', 2, 1, 'Classical mechanics'),
    ('Thermodynamics', 2, 1, 'Heat and temperature'),
    ('Organic Chemistry', 3, 1, 'Carbon compounds and reactions'),
    ('Inorganic Chemistry', 3, 1, 'Non-carbon compounds'),
    ('Cell Biology', 4, 1, 'Cell structure and function'),
    ('Genetics', 4, 1, 'Heredity and variation'),
    ('Grammar', 5, 1, 'English grammar rules');
    PRINT 'Sample topics inserted.';
END
GO

-- Insert Sample Admin
IF NOT EXISTS (SELECT * FROM AdminMaster WHERE Admin_Email = 'admin@example.com')
BEGIN
    INSERT INTO AdminMaster (Admin_Name, Admin_Email, Admin_Password, Admin_Role) VALUES 
    ('System Administrator', 'admin@example.com', 'admin123', 'Admin'),
    ('Teacher', 'teacher@example.com', 'teacher123', 'Teacher');
    PRINT 'Sample admin users inserted.';
END
GO

-- Insert Sample Student
IF NOT EXISTS (SELECT * FROM StudentMaster WHERE Stu_Email = 'student@example.com')
BEGIN
    INSERT INTO StudentMaster (Stu_Name, Stu_Email, Stu_Password, Stu_ClassId) VALUES 
    ('John Doe', 'student@example.com', 'student123', 1),
    ('Jane Smith', 'jane@example.com', 'student123', 1),
    ('Mike Johnson', 'mike@example.com', 'student123', 2);
    PRINT 'Sample students inserted.';
END
GO

-- Insert Sample API Key
IF NOT EXISTS (SELECT * FROM APIKeys WHERE Key_Value = 'sample-api-key-123')
BEGIN
    INSERT INTO APIKeys (Key_Value, Key_IsActive, Key_ExpiryDate) VALUES 
    ('sample-api-key-123', 1, DATEADD(YEAR, 1, GETDATE()));
    PRINT 'Sample API key inserted.';
END
GO

-- Create Stored Procedures

-- SP_SubjectMaster_Select
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Sp_SubjectMaster_Select')
    DROP PROCEDURE Sp_SubjectMaster_Select;
GO

CREATE PROCEDURE Sp_SubjectMaster_Select
    @Sub_Id INT = 0
AS
BEGIN
    IF @Sub_Id = 0
        SELECT * FROM SubjectMaster WHERE Sub_IsActive = 1 ORDER BY Sub_Name;
    ELSE
        SELECT * FROM SubjectMaster WHERE Sub_Id = @Sub_Id AND Sub_IsActive = 1;
END
GO

-- SP_TopicMaster_Select
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'SP_TopicMaster_Select')
    DROP PROCEDURE SP_TopicMaster_Select;
GO

CREATE PROCEDURE SP_TopicMaster_Select
    @Top_Id INT = 0,
    @Top_SubId INT = 0,
    @Top_ClassID INT = 0
AS
BEGIN
    SELECT t.*, s.Sub_Name, c.Name as ClassName 
    FROM TopicMaster t
    INNER JOIN SubjectMaster s ON t.Top_SubId = s.Sub_Id
    INNER JOIN ClassMaster c ON t.Top_ClassID = c.ID
    WHERE t.Top_IsActive = 1
    AND (@Top_Id = 0 OR t.Top_Id = @Top_Id)
    AND (@Top_SubId = 0 OR t.Top_SubId = @Top_SubId)
    AND (@Top_ClassID = 0 OR t.Top_ClassID = @Top_ClassID)
    ORDER BY t.Top_Name;
END
GO

-- GetSubjectsByClassId
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetSubjectsByClassId')
    DROP PROCEDURE GetSubjectsByClassId;
GO

CREATE PROCEDURE GetSubjectsByClassId
    @ClassId INT
AS
BEGIN
    SELECT DISTINCT s.Sub_Id, s.Sub_Name
    FROM SubjectMaster s
    INNER JOIN TopicMaster t ON s.Sub_Id = t.Top_SubId
    WHERE t.Top_ClassID = @ClassId AND s.Sub_IsActive = 1 AND t.Top_IsActive = 1
    ORDER BY s.Sub_Name;
END
GO

-- GetTopicsBySubjectIDAndClassID
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetTopicsBySubjectIDAndClassID')
    DROP PROCEDURE GetTopicsBySubjectIDAndClassID;
GO

CREATE PROCEDURE GetTopicsBySubjectIDAndClassID
    @SubjectID INT,
    @ClassID INT
AS
BEGIN
    SELECT Top_Id, Top_Name
    FROM TopicMaster
    WHERE Top_SubId = @SubjectID AND Top_ClassID = @ClassID AND Top_IsActive = 1
    ORDER BY Top_Name;
END
GO

PRINT 'Database setup completed successfully!';
PRINT 'All tables, sample data, and stored procedures have been created.';
GO







