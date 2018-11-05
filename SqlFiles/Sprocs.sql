USE CarDealerShip
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'CarSelectFeatured')
      DROP PROCEDURE CarSelectFeatured
GO

CREATE PROCEDURE CarSelectFeatured AS
BEGIN
	SELECT TOP 8 c.BuildYear, ma.MakeName, mo.ModelName, c.SalePrice, c.PictureUrl
	FROM Car c
		INNER JOIN Model mo ON mo.ModelID = c.ModelId
		INNER JOIN Make ma ON ma.MakeID = mo.MakeId
	WHERE IsFeatured = 1
END

GO

USE CarDealerShip
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
   WHERE ROUTINE_NAME = 'CarDetailsSelect')
      DROP PROCEDURE CarDetailsSelect
GO

CREATE PROCEDURE CarDetailsSelect AS
BEGIN
	SELECT c.BuildYear, ma.MakeName, mo.ModelName, c.BodyStyle, c.Transmission, c.Color, c.Interior, c.Mileage, c.Vin, c.SalePrice, c.MSRP, c.CarDescription, c.PictureUrl
	FROM Car c
		INNER JOIN Model mo ON mo.ModelID = c.ModelId
		INNER JOIN Make ma ON ma.MakeID = mo.MakeId
END

GO

