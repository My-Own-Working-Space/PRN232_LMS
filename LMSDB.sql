-- ============================================================
--  Learning Management System (LMS) - SQL Server Script
--  3-Layer Architecture: Database Layer
--  Tables: Semester, Course, Subject, Student, Enrollment
--         + CourseSubject (junction), Grade (extended)
-- ============================================================

USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'LMSDatabase')
    DROP DATABASE LMSDatabase;
GO

CREATE DATABASE LMSDatabase
    COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

USE LMSDatabase;
GO

-- ============================================================
--  SCHEMA CREATION
-- ============================================================

-- 1. Semester
CREATE TABLE Semester (
    SemesterId   INT            NOT NULL IDENTITY(1,1),
    SemesterName NVARCHAR(100)  NOT NULL,
    StartDate    DATETIME       NOT NULL,
    EndDate      DATETIME       NOT NULL,
    CONSTRAINT PK_Semester PRIMARY KEY (SemesterId),
    CONSTRAINT CK_Semester_Dates CHECK (EndDate > StartDate)
);
GO

-- 2. Course
CREATE TABLE Course (
    CourseId     INT            NOT NULL IDENTITY(1,1),
    CourseName   NVARCHAR(100)  NOT NULL,
    SemesterId   INT            NOT NULL,
    CONSTRAINT PK_Course      PRIMARY KEY (CourseId),
    CONSTRAINT FK_Course_Sem  FOREIGN KEY (SemesterId) REFERENCES Semester(SemesterId)
);
GO

-- 3. Subject
CREATE TABLE Subject (
    SubjectId    INT            NOT NULL IDENTITY(1,1),
    SubjectCode  VARCHAR(20)    NOT NULL,
    SubjectName  NVARCHAR(100)  NOT NULL,
    Credit       INT            NOT NULL,
    CONSTRAINT PK_Subject        PRIMARY KEY (SubjectId),
    CONSTRAINT UQ_Subject_Code   UNIQUE      (SubjectCode),
    CONSTRAINT CK_Subject_Credit CHECK (Credit BETWEEN 1 AND 10)
);
GO

-- 4. Student
CREATE TABLE Student (
    StudentId    INT            NOT NULL IDENTITY(1,1),
    FullName     NVARCHAR(100)  NOT NULL,
    Email        VARCHAR(100)   NOT NULL,
    DateOfBirth  DATETIME       NOT NULL,
    CONSTRAINT PK_Student       PRIMARY KEY (StudentId),
    CONSTRAINT UQ_Student_Email UNIQUE      (Email)
);
GO

-- 5. Enrollment
CREATE TABLE Enrollment (
    EnrollmentId INT            NOT NULL IDENTITY(1,1),
    StudentId    INT            NOT NULL,
    CourseId     INT            NOT NULL,
    EnrollDate   DATETIME       NOT NULL DEFAULT GETDATE(),
    Status       VARCHAR(20)    NOT NULL DEFAULT 'Active',
    CONSTRAINT PK_Enrollment          PRIMARY KEY (EnrollmentId),
    CONSTRAINT FK_Enrollment_Student  FOREIGN KEY (StudentId) REFERENCES Student(StudentId),
    CONSTRAINT FK_Enrollment_Course   FOREIGN KEY (CourseId)  REFERENCES Course(CourseId),
    CONSTRAINT UQ_Enrollment          UNIQUE (StudentId, CourseId),
    CONSTRAINT CK_Enrollment_Status   CHECK (Status IN ('Active','Completed','Dropped','Pending'))
);
GO

-- 6. CourseSubject  (many-to-many: Course <-> Subject)
CREATE TABLE CourseSubject (
    CourseSubjectId INT NOT NULL IDENTITY(1,1),
    CourseId        INT NOT NULL,
    SubjectId       INT NOT NULL,
    CONSTRAINT PK_CourseSubject       PRIMARY KEY (CourseSubjectId),
    CONSTRAINT FK_CS_Course           FOREIGN KEY (CourseId)   REFERENCES Course(CourseId),
    CONSTRAINT FK_CS_Subject          FOREIGN KEY (SubjectId)  REFERENCES Subject(SubjectId),
    CONSTRAINT UQ_CourseSubject       UNIQUE (CourseId, SubjectId)
);
GO

-- 7. Grade  (extended – stores per-enrollment subject grades)
CREATE TABLE Grade (
    GradeId       INT            NOT NULL IDENTITY(1,1),
    EnrollmentId  INT            NOT NULL,
    SubjectId     INT            NOT NULL,
    MidtermScore  DECIMAL(5,2)   NULL,
    FinalScore    DECIMAL(5,2)   NULL,
    TotalScore    DECIMAL(5,2)   NULL,
    LetterGrade   VARCHAR(2)     NULL,
    CONSTRAINT PK_Grade             PRIMARY KEY (GradeId),
    CONSTRAINT FK_Grade_Enrollment  FOREIGN KEY (EnrollmentId) REFERENCES Enrollment(EnrollmentId),
    CONSTRAINT FK_Grade_Subject     FOREIGN KEY (SubjectId)    REFERENCES Subject(SubjectId),
    CONSTRAINT UQ_Grade             UNIQUE (EnrollmentId, SubjectId),
    CONSTRAINT CK_Grade_Midterm     CHECK (MidtermScore  BETWEEN 0 AND 100),
    CONSTRAINT CK_Grade_Final       CHECK (FinalScore    BETWEEN 0 AND 100),
    CONSTRAINT CK_Grade_Total       CHECK (TotalScore    BETWEEN 0 AND 100)
);
GO

-- ============================================================
--  INDEXES
-- ============================================================
CREATE INDEX IX_Course_Semester      ON Course     (SemesterId);
CREATE INDEX IX_Enrollment_Student   ON Enrollment (StudentId);
CREATE INDEX IX_Enrollment_Course    ON Enrollment (CourseId);
CREATE INDEX IX_Enrollment_Status    ON Enrollment (Status);
CREATE INDEX IX_Grade_Enrollment     ON Grade      (EnrollmentId);
CREATE INDEX IX_Student_Email        ON Student    (Email);
GO

-- ============================================================
--  SEED DATA
-- ============================================================

-- ------------------------------------------------------------
-- Semesters  (5)
-- ------------------------------------------------------------
SET IDENTITY_INSERT Semester ON;
INSERT INTO Semester (SemesterId, SemesterName, StartDate, EndDate) VALUES
(1, N'Spring 2023',  '2023-01-09', '2023-05-19'),
(2, N'Summer 2023',  '2023-05-29', '2023-08-18'),
(3, N'Fall 2023',    '2023-08-28', '2023-12-15'),
(4, N'Spring 2024',  '2024-01-08', '2024-05-17'),
(5, N'Fall 2024',    '2024-08-26', '2024-12-13');
SET IDENTITY_INSERT Semester OFF;
GO

-- ------------------------------------------------------------
-- Subjects  (10)
-- ------------------------------------------------------------
SET IDENTITY_INSERT Subject ON;
INSERT INTO Subject (SubjectId, SubjectCode, SubjectName, Credit) VALUES
( 1, 'CS101',  N'Introduction to Programming',       3),
( 2, 'CS201',  N'Data Structures & Algorithms',      4),
( 3, 'CS301',  N'Database Management Systems',       3),
( 4, 'CS401',  N'Software Engineering',              3),
( 5, 'CS501',  N'Artificial Intelligence',           3),
( 6, 'MATH101',N'Calculus I',                        4),
( 7, 'MATH201',N'Linear Algebra',                    3),
( 8, 'NET101', N'Computer Networks',                 3),
( 9, 'SE101',  N'Web Development',                   3),
(10, 'SEC101', N'Cybersecurity Fundamentals',        3);
SET IDENTITY_INSERT Subject OFF;
GO

-- ------------------------------------------------------------
-- Courses  (20, spread across 5 semesters – 4 per semester)
-- ------------------------------------------------------------
SET IDENTITY_INSERT Course ON;
INSERT INTO Course (CourseId, CourseName, SemesterId) VALUES
-- Semester 1 (Spring 2023)
( 1, N'CS101 - Intro Programming (Spring 2023)',          1),
( 2, N'MATH101 - Calculus I (Spring 2023)',               1),
( 3, N'NET101 - Computer Networks (Spring 2023)',          1),
( 4, N'SE101 - Web Development (Spring 2023)',             1),
-- Semester 2 (Summer 2023)
( 5, N'CS201 - Data Structures (Summer 2023)',             2),
( 6, N'CS301 - DBMS (Summer 2023)',                       2),
( 7, N'MATH201 - Linear Algebra (Summer 2023)',           2),
( 8, N'SEC101 - Cybersecurity (Summer 2023)',              2),
-- Semester 3 (Fall 2023)
( 9, N'CS401 - Software Engineering (Fall 2023)',          3),
(10, N'CS501 - Artificial Intelligence (Fall 2023)',       3),
(11, N'CS101 - Intro Programming (Fall 2023)',             3),
(12, N'SE101 - Web Development (Fall 2023)',               3),
-- Semester 4 (Spring 2024)
(13, N'CS201 - Data Structures (Spring 2024)',             4),
(14, N'CS301 - DBMS (Spring 2024)',                       4),
(15, N'NET101 - Computer Networks (Spring 2024)',          4),
(16, N'MATH101 - Calculus I (Spring 2024)',               4),
-- Semester 5 (Fall 2024)
(17, N'CS401 - Software Engineering (Fall 2024)',          5),
(18, N'CS501 - Artificial Intelligence (Fall 2024)',       5),
(19, N'SEC101 - Cybersecurity (Fall 2024)',                5),
(20, N'MATH201 - Linear Algebra (Fall 2024)',              5);
SET IDENTITY_INSERT Course OFF;
GO

-- CourseSubject mappings (each course maps to its primary subject)
INSERT INTO CourseSubject (CourseId, SubjectId) VALUES
( 1, 1),( 2, 6),( 3, 8),( 4, 9),
( 5, 2),( 6, 3),( 7, 7),( 8,10),
( 9, 4),(10, 5),(11, 1),(12, 9),
(13, 2),(14, 3),(15, 8),(16, 6),
(17, 4),(18, 5),(19,10),(20, 7);
GO

-- ------------------------------------------------------------
-- Students  (50)
-- ------------------------------------------------------------
SET IDENTITY_INSERT Student ON;
INSERT INTO Student (StudentId, FullName, Email, DateOfBirth) VALUES
( 1, N'Nguyen Van An',        'an.nguyen@student.edu',       '2002-03-15'),
( 2, N'Tran Thi Bich',        'bich.tran@student.edu',       '2001-07-22'),
( 3, N'Le Minh Cuong',        'cuong.le@student.edu',        '2003-01-10'),
( 4, N'Pham Thi Dung',        'dung.pham@student.edu',       '2002-11-05'),
( 5, N'Hoang Van Em',         'em.hoang@student.edu',        '2001-09-30'),
( 6, N'Vo Thi Phuong',        'phuong.vo@student.edu',       '2003-04-18'),
( 7, N'Dang Minh Quan',       'quan.dang@student.edu',       '2002-06-25'),
( 8, N'Bui Thi Hoa',          'hoa.bui@student.edu',         '2001-12-08'),
( 9, N'Dinh Van Kiet',        'kiet.dinh@student.edu',       '2003-02-14'),
(10, N'Nguyen Thi Lan',       'lan.nguyen@student.edu',      '2002-08-19'),
(11, N'Tran Van Minh',        'minh.tran@student.edu',       '2001-05-03'),
(12, N'Le Thi Ngoc',          'ngoc.le@student.edu',         '2003-10-27'),
(13, N'Pham Van Oanh',        'oanh.pham@student.edu',       '2002-07-11'),
(14, N'Hoang Thi Phuong',     'phuong.hoang@student.edu',    '2001-03-22'),
(15, N'Vo Minh Quang',        'quang.vo@student.edu',        '2003-09-16'),
(16, N'Dang Thi Ry',          'ry.dang@student.edu',         '2002-01-29'),
(17, N'Bui Van Son',          'son.bui@student.edu',         '2001-11-14'),
(18, N'Dinh Thi Thu',         'thu.dinh@student.edu',        '2003-06-07'),
(19, N'Nguyen Van Uyen',      'uyen.nguyen@student.edu',     '2002-04-23'),
(20, N'Tran Thi Vang',        'vang.tran@student.edu',       '2001-08-31'),
(21, N'Le Van Xuan',          'xuan.le@student.edu',         '2003-03-05'),
(22, N'Pham Thi Yen',         'yen.pham@student.edu',        '2002-10-20'),
(23, N'Hoang Van Zung',       'zung.hoang@student.edu',      '2001-06-17'),
(24, N'Vo Thi Anh',           'anh.vo@student.edu',          '2003-12-09'),
(25, N'Dang Minh Binh',       'binh.dang@student.edu',       '2002-02-28'),
(26, N'Bui Thi Chi',          'chi.bui@student.edu',         '2001-07-04'),
(27, N'Dinh Van Duc',         'duc.dinh@student.edu',        '2003-05-21'),
(28, N'Nguyen Thi Huong',     'huong.nguyen2@student.edu',   '2002-09-13'),
(29, N'Tran Van Long',        'long.tran@student.edu',       '2001-04-06'),
(30, N'Le Thi Mai',           'mai.le@student.edu',          '2003-11-25'),
(31, N'Pham Van Nam',         'nam.pham@student.edu',        '2002-01-18'),
(32, N'Hoang Thi Oanh',       'oanh.hoang@student.edu',      '2001-08-09'),
(33, N'Vo Minh Phuc',         'phuc.vo@student.edu',         '2003-03-30'),
(34, N'Dang Thi Quynh',       'quynh.dang@student.edu',      '2002-06-12'),
(35, N'Bui Van Rong',         'rong.bui@student.edu',        '2001-10-01'),
(36, N'Dinh Thi Suong',       'suong.dinh@student.edu',      '2003-07-24'),
(37, N'Nguyen Van Tuan',      'tuan.nguyen@student.edu',     '2002-12-16'),
(38, N'Tran Thi Uyen',        'uyen.tran@student.edu',       '2001-05-08'),
(39, N'Le Minh Viet',         'viet.le@student.edu',         '2003-02-19'),
(40, N'Pham Thi Xuan',        'xuan.pham@student.edu',       '2002-09-07'),
(41, N'Hoang Van Yen',        'yen.hoang@student.edu',       '2001-11-28'),
(42, N'Vo Thi Bao',           'bao.vo@student.edu',          '2003-04-02'),
(43, N'Dang Minh Cong',       'cong.dang@student.edu',       '2002-08-15'),
(44, N'Bui Thi Dao',          'dao.bui@student.edu',         '2001-06-26'),
(45, N'Dinh Van Eo',          'eo.dinh@student.edu',         '2003-01-11'),
(46, N'Nguyen Thi Phuoc',     'phuoc.nguyen@student.edu',    '2002-05-04'),
(47, N'Tran Van Giang',       'giang.tran@student.edu',      '2001-09-22'),
(48, N'Le Thi Hang',          'hang.le@student.edu',         '2003-07-15'),
(49, N'Pham Minh Huyen',      'huyen.pham@student.edu',      '2002-03-08'),
(50, N'Hoang Thi Iris',       'iris.hoang@student.edu',      '2001-12-31');
SET IDENTITY_INSERT Student OFF;
GO

-- ------------------------------------------------------------
-- Enrollments  (500 rows)
-- Strategy: every student enrolls in ~10 courses across all
-- semesters; no student enrolls in the same course twice.
-- Status distribution: ~70% Completed/Active, ~20% Dropped,
-- ~10% Pending.
-- ------------------------------------------------------------

SET IDENTITY_INSERT Enrollment ON;

-- We generate 500 rows using a deterministic pattern.
-- Students 1-50 each get 10 enrollments in different courses.
-- Course IDs cycle 1-20 with offsets per student.

DECLARE @EnrollId INT = 1;
DECLARE @Sid     INT = 1;
DECLARE @offset  INT;
DECLARE @cid     INT;
DECLARE @status  VARCHAR(20);
DECLARE @edate   DATETIME;
DECLARE @statusNum INT;

WHILE @Sid <= 50
BEGIN
    SET @offset = ((@Sid - 1) * 3) % 20;  -- stagger starting course

    DECLARE @slot INT = 0;
    WHILE @slot < 10
    BEGIN
        SET @cid = ((@offset + @slot * 2) % 20) + 1;

        -- Derive enroll date from course semester
        SET @edate = (
            SELECT DATEADD(DAY, (@Sid % 14), s.StartDate)
            FROM Course c
            JOIN Semester s ON c.SemesterId = s.SemesterId
            WHERE c.CourseId = @cid
        );

        -- Status: cycle through values
        SET @statusNum = (@Sid + @slot) % 10;
        SET @status = CASE
            WHEN @statusNum IN (0,1,2,3,4) THEN 'Completed'
            WHEN @statusNum IN (5,6,7)     THEN 'Active'
            WHEN @statusNum = 8            THEN 'Dropped'
            ELSE                                'Pending'
        END;

        INSERT INTO Enrollment (EnrollmentId, StudentId, CourseId, EnrollDate, Status)
        VALUES (@EnrollId, @Sid, @cid, @edate, @status);

        SET @EnrollId = @EnrollId + 1;
        SET @slot     = @slot + 1;
    END;

    SET @Sid = @Sid + 1;
END;

SET IDENTITY_INSERT Enrollment OFF;
GO

-- Verify enrollment count
SELECT COUNT(*) AS TotalEnrollments FROM Enrollment;  -- Should be 500
GO

-- ------------------------------------------------------------
-- Grade data for Completed enrollments
-- ------------------------------------------------------------
INSERT INTO Grade (EnrollmentId, SubjectId, MidtermScore, FinalScore, TotalScore, LetterGrade)
SELECT
    e.EnrollmentId,
    cs.SubjectId,
    -- Midterm: deterministic score based on IDs
    CAST(50 + ((e.StudentId * 7 + e.CourseId * 3) % 50) AS DECIMAL(5,2))     AS MidtermScore,
    CAST(45 + ((e.StudentId * 5 + e.CourseId * 11) % 55) AS DECIMAL(5,2))    AS FinalScore,
    CAST(
        0.4 * (50 + ((e.StudentId * 7 + e.CourseId * 3) % 50))
      + 0.6 * (45 + ((e.StudentId * 5 + e.CourseId * 11) % 55))
    AS DECIMAL(5,2))                                                            AS TotalScore,
    CASE
        WHEN (0.4*(50+((e.StudentId*7+e.CourseId*3)%50))
             +0.6*(45+((e.StudentId*5+e.CourseId*11)%55))) >= 90 THEN 'A+'
        WHEN (0.4*(50+((e.StudentId*7+e.CourseId*3)%50))
             +0.6*(45+((e.StudentId*5+e.CourseId*11)%55))) >= 85 THEN 'A'
        WHEN (0.4*(50+((e.StudentId*7+e.CourseId*3)%50))
             +0.6*(45+((e.StudentId*5+e.CourseId*11)%55))) >= 80 THEN 'B+'
        WHEN (0.4*(50+((e.StudentId*7+e.CourseId*3)%50))
             +0.6*(45+((e.StudentId*5+e.CourseId*11)%55))) >= 70 THEN 'B'
        WHEN (0.4*(50+((e.StudentId*7+e.CourseId*3)%50))
             +0.6*(45+((e.StudentId*5+e.CourseId*11)%55))) >= 60 THEN 'C'
        ELSE 'F'
    END AS LetterGrade
FROM Enrollment e
JOIN CourseSubject cs ON cs.CourseId = e.CourseId
WHERE e.Status = 'Completed';
GO

-- ============================================================
--  USEFUL VIEWS
-- ============================================================

-- V1: Full enrollment details
CREATE VIEW vw_EnrollmentDetails AS
SELECT
    e.EnrollmentId,
    st.StudentId,
    st.FullName       AS StudentName,
    st.Email,
    c.CourseId,
    c.CourseName,
    sm.SemesterId,
    sm.SemesterName,
    sm.StartDate      AS SemesterStart,
    sm.EndDate        AS SemesterEnd,
    e.EnrollDate,
    e.Status
FROM Enrollment e
JOIN Student  st ON e.StudentId  = st.StudentId
JOIN Course   c  ON e.CourseId   = c.CourseId
JOIN Semester sm ON c.SemesterId = sm.SemesterId;
GO

-- V2: Student GPA summary
CREATE VIEW vw_StudentGPA AS
SELECT
    st.StudentId,
    st.FullName,
    st.Email,
    COUNT(g.GradeId)             AS SubjectsCompleted,
    AVG(g.TotalScore)            AS AverageScore,
    SUM(sb.Credit)               AS TotalCreditsEarned
FROM Student st
LEFT JOIN Enrollment  e  ON st.StudentId  = e.StudentId  AND e.Status = 'Completed'
LEFT JOIN Grade       g  ON e.EnrollmentId = g.EnrollmentId
LEFT JOIN Subject     sb ON g.SubjectId   = sb.SubjectId
GROUP BY st.StudentId, st.FullName, st.Email;
GO

-- V3: Course enrollment statistics
CREATE VIEW vw_CourseStats AS
SELECT
    c.CourseId,
    c.CourseName,
    sm.SemesterName,
    COUNT(e.EnrollmentId)                              AS TotalEnrolled,
    SUM(CASE WHEN e.Status = 'Completed' THEN 1 ELSE 0 END) AS Completed,
    SUM(CASE WHEN e.Status = 'Dropped'   THEN 1 ELSE 0 END) AS Dropped,
    SUM(CASE WHEN e.Status = 'Active'    THEN 1 ELSE 0 END) AS Active,
    SUM(CASE WHEN e.Status = 'Pending'   THEN 1 ELSE 0 END) AS Pending,
    AVG(g.TotalScore)                                  AS AvgScore
FROM Course c
JOIN Semester  sm ON c.SemesterId   = sm.SemesterId
LEFT JOIN Enrollment e  ON c.CourseId    = e.CourseId
LEFT JOIN CourseSubject cs ON c.CourseId = cs.CourseId
LEFT JOIN Grade g ON e.EnrollmentId = g.EnrollmentId AND cs.SubjectId = g.SubjectId
GROUP BY c.CourseId, c.CourseName, sm.SemesterName;
GO

-- ============================================================
--  STORED PROCEDURES (for 3-layer Repository/Service use)
-- ============================================================

-- SP1: Get all enrollments for a student
CREATE OR ALTER PROCEDURE sp_GetStudentEnrollments
    @StudentId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM vw_EnrollmentDetails
    WHERE  StudentId = @StudentId
    ORDER BY SemesterStart DESC, EnrollDate DESC;
END;
GO

-- SP2: Get all students in a course
CREATE OR ALTER PROCEDURE sp_GetCourseStudents
    @CourseId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        e.EnrollmentId,
        s.StudentId,
        s.FullName,
        s.Email,
        e.EnrollDate,
        e.Status
    FROM Enrollment e
    JOIN Student s ON e.StudentId = s.StudentId
    WHERE e.CourseId = @CourseId
    ORDER BY s.FullName;
END;
GO

-- SP3: Enroll a student in a course
CREATE OR ALTER PROCEDURE sp_EnrollStudent
    @StudentId  INT,
    @CourseId   INT,
    @EnrollDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Enrollment WHERE StudentId = @StudentId AND CourseId = @CourseId)
    BEGIN
        RAISERROR('Student is already enrolled in this course.', 16, 1);
        RETURN;
    END;

    IF @EnrollDate IS NULL SET @EnrollDate = GETDATE();

    INSERT INTO Enrollment (StudentId, CourseId, EnrollDate, Status)
    VALUES (@StudentId, @CourseId, @EnrollDate, 'Active');

    SELECT SCOPE_IDENTITY() AS NewEnrollmentId;
END;
GO

-- SP4: Update enrollment status
CREATE OR ALTER PROCEDURE sp_UpdateEnrollmentStatus
    @EnrollmentId INT,
    @Status       VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Status NOT IN ('Active','Completed','Dropped','Pending')
    BEGIN
        RAISERROR('Invalid status value.', 16, 1);
        RETURN;
    END;

    UPDATE Enrollment
    SET    Status = @Status
    WHERE  EnrollmentId = @EnrollmentId;

    SELECT @@ROWCOUNT AS RowsAffected;
END;
GO

-- SP5: Get semester summary
CREATE OR ALTER PROCEDURE sp_GetSemesterSummary
    @SemesterId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        sm.SemesterId,
        sm.SemesterName,
        sm.StartDate,
        sm.EndDate,
        COUNT(DISTINCT c.CourseId)   AS TotalCourses,
        COUNT(DISTINCT e.StudentId)  AS TotalStudents,
        COUNT(e.EnrollmentId)        AS TotalEnrollments
    FROM Semester sm
    LEFT JOIN Course     c ON sm.SemesterId = c.SemesterId
    LEFT JOIN Enrollment e ON c.CourseId    = e.CourseId
    WHERE sm.SemesterId = @SemesterId
    GROUP BY sm.SemesterId, sm.SemesterName, sm.StartDate, sm.EndDate;
END;
GO

-- SP6: Search students
CREATE OR ALTER PROCEDURE sp_SearchStudents
    @Keyword NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT StudentId, FullName, Email, DateOfBirth
    FROM   Student
    WHERE  (@Keyword IS NULL
            OR FullName LIKE N'%' + @Keyword + N'%'
            OR Email    LIKE '%'  + @Keyword + '%')
    ORDER BY FullName;
END;
GO

-- ============================================================
--  QUICK VERIFICATION QUERIES
-- ============================================================

SELECT 'Semesters'   AS [Table], COUNT(*) AS [Rows] FROM Semester
UNION ALL
SELECT 'Subjects',    COUNT(*) FROM Subject
UNION ALL
SELECT 'Courses',     COUNT(*) FROM Course
UNION ALL
SELECT 'Students',    COUNT(*) FROM Student
UNION ALL
SELECT 'Enrollments', COUNT(*) FROM Enrollment
UNION ALL
SELECT 'Grades',      COUNT(*) FROM Grade;
GO