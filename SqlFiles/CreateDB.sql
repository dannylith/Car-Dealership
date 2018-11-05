SET NOCOUNT ON
Go

USE master
Go

if exists (select * from sysdatabases where name='CarDealerShip')
		drop database CarDealerShip
Go

Create Database CarDealerShip
Go