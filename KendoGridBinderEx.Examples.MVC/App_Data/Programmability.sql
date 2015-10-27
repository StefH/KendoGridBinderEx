/* Created by MyDbUtils - 0.9.8.0 at 24-10-2015 15:36:04 */
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[VW_EmployeeDetails]')) 
exec('CREATE VIEW [dbo].[VW_EmployeeDetails] AS SELECT 0 AS X')
GO
ALTER VIEW [dbo].[VW_EmployeeDetails]
AS
SELECT
	e.*,
	cast(case when e.LastName like '%smith' then 1 else 0 end as bit) as IsManager,
	CONCAT(e.FirstName, ' ', e.LastName) as FullName,
	cast(case when e.Assigned > 1 then 1 else 0 end as bit) as IsAssigned,
	c.Code as CountryCode,
	c.Name as CountryName
FROM dbo.KendoGrid_Employee as e
INNER JOIN dbo.KendoGrid_Country as c ON e.Country_Id = c.Id
GO

