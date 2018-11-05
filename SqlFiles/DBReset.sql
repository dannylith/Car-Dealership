USE CarDealerShip
GO

--Admin Login
--Username: admin@admin.com
--Password: Admin123!

--Salesman Login
--Username: salesman@salesman.com
--Password: Salesman123!

if exists (select * from INFORMATION_SCHEMA.ROUTINES
		where Routine_Name = 'DbReset')
			Drop Procedure DbReset
GO

Create Procedure DbReset As
Begin
	Delete From AspNetRoles;
	Delete From Contact
	Delete From Car;
	Delete From Special;
	Delete From Sale;
	Delete From Model;
	Delete From Make;		
	DELETE FROM AspNetUserRoles
	DELETE FROM AspNetUsers

	DBCC CHECKIDENT ('Make', RESEED, 0);
	DBCC CHECKIDENT ('Model', RESEED, 0);	
	DBCC CHECKIDENT ('Sale', RESEED, 0);
	DBCC CHECKIDENT ('Special', RESEED, 0);
	DBCC CHECKIDENT ('Car', RESEED, 0);
	DBCC CHECKIDENT ('Contact', RESEED, 0);

	INSERT INTO AspNetUsers(Id, EmailConfirmed, PhoneNumberConfirmed, Email, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName, FirstName, LastName)
	VALUES('00000000-0000-0000-0000-000000000000', 0, 0, 'test@test.com', 0, 0, 0, 'test', 'test', 'test');

	INSERT INTO AspNetUsers
	VALUES ('cfaa2216-ec01-4350-980d-1c8d584fa4ba', 'admin@admin.com', 0, 'AHnfG9BHlEGfl5mqCTpOi6J15/uwwmqAYsLvh0QEikJfKE7XHhEFMprCKslx9PXZBQ==', 'd2141fb4-5bbe-4b39-ae7a-7fd835ca5292', null, 0, 0, null, 1, 0, 'admin@admin.com', 'admin', 'admin'),
	('e9cda645-7396-4e57-8b48-24a9f138f30c', 'salesman@salesman.com', 0, 'AJ5OO8llin2XJ+adjEveYcdeHEd49wcJxpVjr8NLB5yHLWGA27VsUcOj4byUdWRf4g==', '7d9fc956-5deb-4914-acc3-b265a6067252', null, 0, 0, null, 1, 0, 'salesman@salesman.com', 'salesman', 'salesman')

	INSERT INTO AspNetRoles(Id,[Name])
	VALUES(1, 'Admin'),
		(2, 'Salesman')

	INSERT INTO AspNetUserRoles
	VALUES ('cfaa2216-ec01-4350-980d-1c8d584fa4ba', 1),
	('e9cda645-7396-4e57-8b48-24a9f138f30c', 2)
	
	SET IDENTITY_INSERT Make ON;
	INSERT INTO Make(MakeId, MakeName, DateAdded, AdminUserId)
	VALUES (1, 'Toyota', '01/01/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(2, 'Honda', '01/01/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(3, 'Tesla', '01/01/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(4, 'Lexus', '01/01/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(5, 'Jeep', '01/01/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba')
	SET IDENTITY_INSERT Make OFF;

	SET IDENTITY_INSERT Model ON;
	INSERT INTO Model(ModelId, ModelName, MakeId, DateAdded, AdminUserId)
	VALUES (1, 'Corolla', 1, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(2, 'Accord', 2, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(3, 'Roadster', 3, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(4, 'RX Hybrid', 4, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(5, 'Yaris', 1, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(6, 'Prius', 1, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(7, 'Camry', 1, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(8, 'Civic', 2, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(9, 'Pilot', 2, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(10, 'Fit', 2, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(11, 'RX', 4, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(12, 'IS', 4, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba'),
	(13, 'ES', 4, '1/1/2016', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba')

	SET IDENTITY_INSERT Model OFF;

	SET IDENTITY_INSERT Sale ON;
	INSERT INTO Sale(SaleId, CustomerName, Phone, Email, Street1, Street2, City, StateAbbrv, Zipcode, PurchasePrice, PurchaseType, SalesUserId, PurchaseDate)
	VALUES(1, 'John Doe', '123-456-7890', 'something@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'Credit Card', '00000000-0000-0000-0000-000000000000', '1/1/2010'),
	(2, 'A Name', '123-456-7890', 'email@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'VISA', '00000000-0000-0000-0000-000000000000', '1/1/2011'),
	(3, 'John Doe', '123-456-7890', 'something@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'Credit Card', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba', '1/1/2012'),
	(4, 'John Doe', '123-456-7890', 'something@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'Credit Card', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba', '1/1/2013'),
	(5, 'John Doe', '123-456-7890', 'something@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'Credit Card', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba', '1/1/2014'),
	(6, 'John Doe', '123-456-7890', 'something@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'Credit Card', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba', '1/1/2015'),
	(7, 'John Doe', '123-456-7890', 'something@test.com', '123 4th st', null, 'Minneapolis', 'MN', '12345', 20000, 'Credit Card', 'cfaa2216-ec01-4350-980d-1c8d584fa4ba', '1/1/2016')
	SET IDENTITY_INSERT Sale OFF;

	SET IDENTITY_INSERT Special ON;
	INSERT INTO Special(SpecialId, SpecialTitle, SpecialDesc)
	VALUES (1, 'Big Sale', 'Lorem ipsum dolor sit amet, consectetuer ac, condimentum aenean, lectus lacinia, maecenas eget sollicitudin eros amet. Elit felis mauris tortor ac nulla, vel ac nullam et metus. Ligula nullam in suscipit ut vestibulum justo, rutrum eget convallis donec placerat magna. Penatibus amet vestibulum urna vestibulum eligendi non, nisl velit, diam fusce vestibulum diam, risus dolor in felis etiam nulla ultrices. Lacus vehicula, vitae voluptatibus eros ligula, quis sociosqu, accumsan a arcu ante nec, pharetra ac torquent.'),
	(2, 'Big Sale 2', 'Lorem ipsum dolor sit amet, consectetuer ac, condimentum aenean, lectus lacinia, maecenas eget sollicitudin eros amet. Elit felis mauris tortor ac nulla, vel ac nullam et metus. Ligula nullam in suscipit ut vestibulum justo, rutrum eget convallis donec placerat magna. Penatibus amet vestibulum urna vestibulum eligendi non, nisl velit, diam fusce vestibulum diam, risus dolor in felis etiam nulla ultrices. Lacus vehicula, vitae voluptatibus eros ligula, quis sociosqu, accumsan a arcu ante nec, pharetra ac torquent.'),
	(3, 'Big Sale 3', 'Lorem ipsum dolor sit amet, consectetuer ac, condimentum aenean, lectus lacinia, maecenas eget sollicitudin eros amet. Elit felis mauris tortor ac nulla, vel ac nullam et metus. Ligula nullam in suscipit ut vestibulum justo, rutrum eget convallis donec placerat magna. Penatibus amet vestibulum urna vestibulum eligendi non, nisl velit, diam fusce vestibulum diam, risus dolor in felis etiam nulla ultrices. Lacus vehicula, vitae voluptatibus eros ligula, quis sociosqu, accumsan a arcu ante nec, pharetra ac torquent.'),
	(4, 'Big Sale 4', 'Lorem ipsum dolor sit amet, consectetuer ac, condimentum aenean, lectus lacinia, maecenas eget sollicitudin eros amet. Elit felis mauris tortor ac nulla, vel ac nullam et metus. Ligula nullam in suscipit ut vestibulum justo, rutrum eget convallis donec placerat magna. Penatibus amet vestibulum urna vestibulum eligendi non, nisl velit, diam fusce vestibulum diam, risus dolor in felis etiam nulla ultrices. Lacus vehicula, vitae voluptatibus eros ligula, quis sociosqu, accumsan a arcu ante nec, pharetra ac torquent.')
	SET IDENTITY_INSERT Special OFF;

	SET IDENTITY_INSERT Car ON;
	INSERT INTO Car(CarId, ModelId, SaleId, BuildYear, SalePrice, MSRP, BodyStyle, Color, Interior, Mileage, Vin, CarDescription, PictureUrl, CarType, Transmission, IsFeatured)
	VALUES (1, 1, 1, 2015, 18000, 20000, 'Sedan', 'Silver', 'Black', 10, '1HGCM72685A891329', 'Brand new car', 'car.jpg','New', 'Automatic', 1),
	(2, 1, null, 2016, 20000, 22000, 'Sedan', 'Black', 'Black', 10, '1HGCM72795A891329', 'Brand new car', 'car.jpg','New', 'Manual', 0),
	(3, 1, null, 2014, 13000, 15000, 'Sedan', 'Red', 'Black', 50000, 'WVWCA0152HK027365', 'used car', 'car.jpg', 'Used', 'Automatic', 1),
	(4, 6, null, 2013, 10000, 13000, 'Coupe', 'Black', 'Brown', 25000, '1D4GP25B75B110302', 'A car', 'car.jpg', 'Used', 'Automatic', 1),
	(5, 9, null, 2017, 23000, 25000, 'SUV', 'Blue', 'Brown', 10, 'ZDM1RA2KX4B055732', 'A car', 'car.jpg', 'New', 'Automatic', 1),
	(6, 11, null, 2015, 32000, 40000, 'SUV', 'Silver', 'Black', 10, '2G1FT1EW3B9261130', 'A suv', 'car.jpg', 'New', 'Manual', 1),
	(7, 1, null, 2016, 15000, 17000, 'Sedan', 'Blue', 'Black', 10, '1XKDDT9X54J072999', 'A brand new car', 'car.jpg', 'New', 'Manual', 1),
	(8, 2, null, 2000, 8000, 12000, 'Sedan', 'White', 'Black', 75000, '2B4GP25R0YR612968', 'Another Car', 'car.jpg', 'Used', 'Automatic', 1)
	SET IDENTITY_INSERT Car OFF;

	SET IDENTITY_INSERT Contact ON;
	INSERT INTO Contact (ContactId, ContactName, Email, Phone, ContactMessage, CarId)
	VALUES (1, 'John Doe', null, '123-456-7890', 'Want to lookup a car', 1)
	SET IDENTITY_INSERT Contact OFF;







End


