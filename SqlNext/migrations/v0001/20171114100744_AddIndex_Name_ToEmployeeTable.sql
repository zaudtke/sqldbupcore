/* Migration Script */
ALTER TABLE dbo.Employee
ADD LastName  AS Convert(varchar(60), JSON_VALUE([Data], '$.lastname') )

GO

CREATE NONCLUSTERED INDEX IX_Employee_LastName
ON dbo.Employee ([LastName])

Go