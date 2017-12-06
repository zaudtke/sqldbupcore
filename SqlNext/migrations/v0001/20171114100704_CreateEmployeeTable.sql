/* Migration Script */
CREATE table dbo.Employee
(
	Id bigint identity (1,1) NOT NULL Primary Key
	, CreateDate datetime2 NOT NULL Default SysUTCDateTime()  
	, ChangeDate datetime2 NOT NULL Default SysUTCDateTime()
	, CompanyId bigint NOT NULL
	, [Version] rowversion
	, [Data] nvarchar(max) NOT NULL 
)

go

ALTER TABLE dbo.Employee
    ADD CONSTRAINT [EmployeeAsJson]
        CHECK (ISJSON([Data]) > 0)

go
 
