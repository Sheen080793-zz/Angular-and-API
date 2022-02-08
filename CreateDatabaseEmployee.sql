create database EmployeeDB

Create table dbo.Employee(
	EmployeeId int identity(1,1),
	EmployeeName varchar(500),
	Department varchar(500),
	Position varchar(500)
)



insert into Employee values
('Sheen', 'ZOZO', 'Software Engineer'),
('Michael', 'ZOZOdb', 'Supervisor')

