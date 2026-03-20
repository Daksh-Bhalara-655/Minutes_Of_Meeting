# 📊 Minutes of Meeting Management System (Database)

## 🧱 Database Schema

```sql
CREATE DATABASE MinutesOfMeetingDB;
GO

USE MinutesOfMeetingDB;
GO

-- ================= USERS =================
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ================= AUDIT LOG =================
CREATE TABLE AuditLog (
    AuditLogID INT IDENTITY(1,1) PRIMARY KEY,
    ActionType NVARCHAR(50),
    TableName NVARCHAR(100),
    RecordID INT,
    ActionDate DATETIME DEFAULT GETDATE()
);

-- ================= DEPARTMENT =================
CREATE TABLE MOM_Department (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE()
);

-- ================= MEETING TYPE =================
CREATE TABLE MOM_MeetingType (
    MeetingTypeID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingTypeName NVARCHAR(100) NOT NULL,
    Remarks NVARCHAR(100),
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE()
);

-- ================= MEETING VENUE =================
CREATE TABLE MOM_MeetingVenue (
    MeetingVenueID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingVenueName NVARCHAR(100) NOT NULL,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE()
);

-- ================= MEETINGS =================
CREATE TABLE MOM_Meetings (
    MeetingID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingDate DATE NOT NULL,
    MeetingVenueID INT NOT NULL,
    MeetingTypeID INT NOT NULL,
    DepartmentID INT NOT NULL,
    MeetingDescription NVARCHAR(250),
    DocumentPath NVARCHAR(250),
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE(),
    IsCancelled BIT DEFAULT 0,
    CancellationDateTime DATETIME,
    CancellationReason NVARCHAR(250),

    FOREIGN KEY (MeetingVenueID) REFERENCES MOM_MeetingVenue(MeetingVenueID),
    FOREIGN KEY (MeetingTypeID) REFERENCES MOM_MeetingType(MeetingTypeID),
    FOREIGN KEY (DepartmentID) REFERENCES MOM_Department(DepartmentID)
);

-- ================= STAFF =================
CREATE TABLE MOM_Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentID INT NOT NULL,
    StaffName NVARCHAR(50) NOT NULL,
    MobileNo NVARCHAR(20) NOT NULL,
    EmailAddress NVARCHAR(50) NOT NULL,
    Remarks NVARCHAR(250),
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (DepartmentID) REFERENCES MOM_Department(DepartmentID)
);

-- ================= MEETING MEMBERS =================
CREATE TABLE MOM_MeetingMember (
    MeetingMemberID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingID INT NOT NULL,
    StaffID INT NOT NULL,
    IsPresent BIT DEFAULT 1,
    Remarks NVARCHAR(250),
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (MeetingID) REFERENCES MOM_Meetings(MeetingID),
    FOREIGN KEY (StaffID) REFERENCES MOM_Staff(StaffID),
    UNIQUE (MeetingID, StaffID)
);
```

---

## ⚙️ Stored Procedures

```sql
-- ================= AUTH =================

CREATE OR ALTER PROCEDURE sp_RegisterUser
    @Username NVARCHAR(100),
    @Email NVARCHAR(150),
    @Password NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        SELECT 'EXISTS' AS Status;
        RETURN;
    END

    INSERT INTO Users (Username, Email, Password)
    VALUES (@Username, @Email, @Password);

    SELECT 'SUCCESS' AS Status;
END
GO

CREATE OR ALTER PROCEDURE sp_LoginUser
    @Email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        SELECT 'NOT_FOUND' AS Status;
        RETURN;
    END

    SELECT Email, Password, 'FOUND' AS Status
    FROM Users
    WHERE Email = @Email;
END
GO

-- ================= DEPARTMENT =================

CREATE OR ALTER PROCEDURE SP_INSERT_DEPARTMENT
    @DEPARTMENTNAME NVARCHAR(100)
AS
BEGIN
    INSERT INTO MOM_Department (DepartmentName, Created, Modified)
    VALUES (@DEPARTMENTNAME, GETDATE(), GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_DEPARTMENT
    @DEPARTMENTID INT,
    @DEPARTMENTNAME NVARCHAR(100)
AS
BEGIN
    UPDATE MOM_Department
    SET DepartmentName = @DEPARTMENTNAME,
        Modified = GETDATE()
    WHERE DepartmentID = @DEPARTMENTID;

    INSERT INTO AuditLog VALUES ('UPDATE','MOM_Department',@DEPARTMENTID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_DELETE_DEPARTMENT
    @DEPARTMENTID INT
AS
BEGIN
    DELETE FROM MOM_Department WHERE DepartmentID = @DEPARTMENTID;

    INSERT INTO AuditLog VALUES ('DELETE','MOM_Department',@DEPARTMENTID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_GET_ALL_DEPARTMENTS
AS
BEGIN
    SELECT * FROM MOM_Department ORDER BY DepartmentID;
END
GO

CREATE OR ALTER PROCEDURE SP_GET_DEPARTMENT_BY_ID
    @DepartmentID INT
AS
BEGIN
    SELECT * FROM MOM_Department WHERE DepartmentID = @DepartmentID;
END
GO

-- ================= MEETING TYPE =================

CREATE OR ALTER PROCEDURE SP_INSERT_MEETING_TYPE
    @MEETINGTYPENAME NVARCHAR(100),
    @REMARKS NVARCHAR(100) = NULL
AS
BEGIN
    INSERT INTO MOM_MeetingType (MeetingTypeName, Remarks, Created, Modified)
    VALUES (@MEETINGTYPENAME, @REMARKS, GETDATE(), GETDATE());

    RETURN SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_MEETING_TYPE
    @MEETINGTYPEID INT,
    @MEETINGTYPENAME NVARCHAR(100),
    @REMARKS NVARCHAR(100) = NULL
AS
BEGIN
    UPDATE MOM_MeetingType
    SET MeetingTypeName = @MEETINGTYPENAME,
        Remarks = @REMARKS,
        Modified = GETDATE()
    WHERE MeetingTypeID = @MEETINGTYPEID;

    INSERT INTO AuditLog VALUES ('UPDATE','MOM_MeetingType',@MEETINGTYPEID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_DELETE_MEETING_TYPE
    @MEETINGTYPEID INT
AS
BEGIN
    DELETE FROM MOM_MeetingType WHERE MeetingTypeID = @MEETINGTYPEID;

    INSERT INTO AuditLog VALUES ('DELETE','MOM_MeetingType',@MEETINGTYPEID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_GET_ALL_MEETING_TYPES
AS
BEGIN
    SELECT * FROM MOM_MeetingType ORDER BY MeetingTypeID;
END
GO

CREATE OR ALTER PROCEDURE SP_GET_ALL_MEETINGTYPE_NAME
AS
BEGIN
    SELECT MeetingTypeName FROM MOM_MeetingType;
END
GO

CREATE OR ALTER PROCEDURE SP_GET_MEETING_TYPE_BY_ID
    @MEETINGTYPEID INT
AS
BEGIN
    SELECT * FROM MOM_MeetingType WHERE MeetingTypeID = @MEETINGTYPEID;
END
GO

-- ================= MEETING VENUE =================

CREATE OR ALTER PROCEDURE SP_INSERT_MEETING_VENUE
    @MEETINGVENUENAME NVARCHAR(100)
AS
BEGIN
    INSERT INTO MOM_MeetingVenue (MeetingVenueName, Created, Modified)
    VALUES (@MEETINGVENUENAME, GETDATE(), GETDATE());

    RETURN SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_MEETING_VENUE
    @MEETINGVENUEID INT,
    @MEETINGVENUENAME NVARCHAR(100)
AS
BEGIN
    UPDATE MOM_MeetingVenue
    SET MeetingVenueName = @MEETINGVENUENAME,
        Modified = GETDATE()
    WHERE MeetingVenueID = @MEETINGVENUEID;

    INSERT INTO AuditLog VALUES ('UPDATE','MOM_MeetingVenue',@MEETINGVENUEID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_DELETE_MEETING_VENUE
    @MEETINGVENUEID INT
AS
BEGIN
    DELETE FROM MOM_MeetingVenue WHERE MeetingVenueID = @MEETINGVENUEID;

    INSERT INTO AuditLog VALUES ('DELETE','MOM_MeetingVenue',@MEETINGVENUEID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_GET_ALL_MEETING_VENUES
AS
BEGIN
    SELECT * FROM MOM_MeetingVenue ORDER BY MeetingVenueID;
END
GO

CREATE OR ALTER PROCEDURE SP_GET_BY_ID_MEETING_VENUE
    @MEETINGVENUEID INT
AS
BEGIN
    SELECT * FROM MOM_MeetingVenue WHERE MeetingVenueID = @MEETINGVENUEID;
END
GO

-- ================= MEETING =================

CREATE OR ALTER PROCEDURE SP_INSERT_MEETING
(
    @MeetingDate DATETIME,
    @MeetingTypeID INT,
    @MeetingVenueID INT,
    @DepartmentID INT,
    @MeetingDescription NVARCHAR(250),
    @DocumentPath NVARCHAR(250) = NULL,
    @NewMeetingID INT OUTPUT
)
AS
BEGIN
    INSERT INTO MOM_Meetings
    VALUES
    (
        @MeetingDate,
        @MeetingVenueID,
        @MeetingTypeID,
        @DepartmentID,
        @MeetingDescription,
        @DocumentPath,
        GETDATE(),
        GETDATE(),
        0,NULL,NULL
    );

    SET @NewMeetingID = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE SP_UPDATE_MEETING
(
    @MeetingID INT,
    @MeetingDate DATETIME,
    @MeetingTypeID INT,
    @MeetingVenueID INT,
    @DepartmentID INT,
    @MeetingDescription NVARCHAR(250),
    @DocumentPath NVARCHAR(250) = NULL
)
AS
BEGIN
    UPDATE MOM_Meetings
    SET MeetingDate = @MeetingDate,
        MeetingTypeID = @MeetingTypeID,
        MeetingVenueID = @MeetingVenueID,
        DepartmentID = @DepartmentID,
        MeetingDescription = @MeetingDescription,
        DocumentPath = @DocumentPath,
        Modified = GETDATE()
    WHERE MeetingID = @MeetingID;
END
GO

CREATE OR ALTER PROCEDURE SP_DELETE_MEETING
    @MEETINGID INT
AS
BEGIN
    DELETE FROM MOM_Meetings WHERE MeetingID = @MEETINGID;

    INSERT INTO AuditLog VALUES ('DELETE','MOM_Meetings',@MEETINGID,GETDATE());
END
GO

CREATE OR ALTER PROCEDURE SP_GET_ALL_MEETINGS
AS
BEGIN
    SELECT 
        m.MeetingID,
        d.DepartmentName,
        m.MeetingDate,
        m.MeetingDescription,
        m.IsCancelled,
        mt.MeetingTypeName,
        mv.MeetingVenueName
    FROM MOM_Meetings m
    LEFT JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    LEFT JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    ORDER BY m.MeetingID DESC;
END
GO

-- ================= ATTENDANCE =================

CREATE OR ALTER PROCEDURE SP_GET_MEETING_ATTENDANCE_PERCENTAGE
    @MEETINGID INT
AS
BEGIN
    SELECT 
        CASE 
            WHEN COUNT(*) = 0 THEN 0
            ELSE (SUM(CASE WHEN IsPresent = 1 THEN 1 ELSE 0 END) * 100.0 / COUNT(*))
        END AS AttendancePercentage
    FROM MOM_MeetingMember
    WHERE MeetingID = @MEETINGID;
END
GO

CREATE OR ALTER PROCEDURE SP_SAVE_ATTENDANCE
    @StaffID INT,
    @IsPresent BIT
AS
BEGIN
    UPDATE MOM_MeetingMember
    SET IsPresent = @IsPresent
    WHERE StaffID = @StaffID;
END
GO
```

---

## 🚀 How to Run

```sql
EXEC SP_GET_ALL_MEETINGS;
```

---
