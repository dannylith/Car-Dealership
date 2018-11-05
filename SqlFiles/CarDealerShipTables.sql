USE CarDealerShip
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Contact')
DROP TABLE Contact
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Special')
DROP TABLE Special
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Car')
DROP TABLE Car
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Sale')
DROP TABLE Sale
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Model')
DROP TABLE Model
GO

IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Make')
DROP TABLE Make
GO


CREATE TABLE Make (
	MakeID int identity(1,1) not null primary key,
	MakeName varchar(50) not null,
	DateAdded datetime2 not null,
	AdminUserId nvarchar(128) not null foreign key references AspNetUsers(Id)
)
GO

CREATE TABLE Model (
	ModelID int identity(1,1) not null primary key,
	ModelName varchar(50) not null,
	MakeId int not null foreign key references Make(MakeId),
	DateAdded datetime2 not null,
	AdminUserId nvarchar(128) not null foreign key references AspNetUsers(Id)
)
GO

CREATE TABLE Sale (
	SaleId int identity(1,1) primary key,
	CustomerName varchar(50) not null,
	Phone varchar(13) not null,
	Email varchar(50) null,
	Street1 varchar(50) not null,
	Street2 varchar(50) null,
	City varchar(50) not null,
	StateAbbrv varchar(2) not null,
	Zipcode int not null,
	PurchasePrice decimal(19,2) not null,
	PurchaseType varchar(50) not null,
	SalesUserId nvarchar(128) not null foreign key references AspNetUsers(Id),
	PurchaseDate datetime2 not null
)
GO

CREATE TABLE Special(
	SpecialId int identity(1,1) not null primary key,
	SpecialTitle varchar(50) not null,
	SpecialDesc varchar(1000) not null
)
GO

CREATE TABLE Car(
	CarId int identity(1,1) not null primary key,
	ModelId int not null foreign key references Model(ModelId),
	SaleId int null foreign key references Sale(SaleId),
	BuildYear int not null,
	SalePrice decimal(19,2) null,
	MSRP decimal(19,2) not null,
	BodyStyle varchar(50) not null,
	Color varchar(20) not null,
	Interior varchar(50) not null,
	Mileage decimal(19,2) not null,
	Vin varchar(17) not null,
	CarDescription varchar(200) null,
	PictureUrl varchar(100) not null,
	CarType varchar(4) not null,
	Transmission varchar(50) not null,
	IsFeatured bit not null

)
GO

CREATE TABLE Contact(
	ContactId int identity(1,1) not null primary key,
	ContactName varchar(50) not null,
	Email varchar(50) null,
	Phone varchar(13) not null,
	ContactMessage varchar(200) not null,
	CarId int null foreign key references Car(CarId)
)
GO