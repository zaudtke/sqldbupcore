/* Migration Script */
CREATE table dbo.Unit
(
	Id bigint identity (1,1) NOT NULL Primary Key
	, CreateDate datetime2 NOT NULL Default SysUTCDateTime()  
	, ChangeDate datetime2 NOT NULL Default SysUTCDateTime()
	, CompanyId bigint NOT NULL
	, [Version] rowversion
	, [Data] nvarchar(max) NOT NULL 
)

go

ALTER TABLE dbo.Unit
    ADD CONSTRAINT [UnitAsJson]
        CHECK (ISJSON([Data]) > 0)

go

ALTER TABLE dbo.Unit
ADD UnitCode  AS Convert(varchar(60), JSON_VALUE([Data], '$.unitCode') )

GO

CREATE NONCLUSTERED INDEX IX_Unit_UnitCode
ON dbo.Unit ([UnitCode])

Go