using CarDealerShip.Domain;
using CarDealerShip.Domain.Interfaces;
using CarDealerShip.Domain.Queries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Data
{
    public class CarRepository : ICarRepository
    {
        private string connection = ConfigurationManager
                 .ConnectionStrings["DefaultConnection"]
                 .ConnectionString;

        public IEnumerable<Car> All()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Car>("SELECT * FROM Car");
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Car WHERE CarId = @CarId";
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Execute(sql, new { CarId = id }) > 0;
            }
        }

        public Car FindById(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Car "
                   + "WHERE CarId = @CarId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Car>(sql, new { CarId = id })
                    .FirstOrDefault();
            }
        }

        public IEnumerable<CarDetails> GetFeatured()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<CarDetails>("CarSelectFeatured", commandType: CommandType.StoredProcedure);
            }
        }

        public Car Save(Car car)
        {
            if (car.CarId > 0)
            {
                return Update(car);
            }
            return Insert(car);
        }

        public IEnumerable<CarDetails> Search(CarSearchParameters parameters)
        {
            List<CarDetails> carDetails = new List<CarDetails>();

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                string query = "SELECT TOP 20 c.CarId, c.SaleId, c.BuildYear, ma.MakeName, mo.ModelName, c.BodyStyle, c.Transmission, c.Color, c.Interior, c.Mileage, c.Vin, c.SalePrice, c.MSRP, c.CarDescription, c.PictureUrl " +
                    "FROM Car c " +
                    "INNER JOIN Model mo ON mo.ModelId = c.ModelId " +
                    "INNER JOIN Make ma ON ma.MakeId = mo.MakeId " +
                    "WHERE 1 = 1 ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;

                if (parameters.MaxPrice.HasValue)
                {
                    query += "AND C.SalePrice <= @MaxPrice ";
                    cmd.Parameters.AddWithValue("@MaxPrice", parameters.MaxPrice.Value);
                }

                if (parameters.MinPrice.HasValue)
                {
                    query += "AND C.SalePrice >= @MinPrice ";
                    cmd.Parameters.AddWithValue("@MinPrice", parameters.MinPrice.Value);
                }

                if (parameters.MinYear.HasValue)
                {
                    query += "AND c.BuildYear >= @MinYear ";
                    cmd.Parameters.AddWithValue("@MinYear", parameters.MinYear.Value);
                }

                if (parameters.MaxYear.HasValue)
                {
                    query += "AND c.BuildYear <= @MaxYear ";
                    cmd.Parameters.AddWithValue("@MaxYear", parameters.MaxYear.Value);
                }

                if (!string.IsNullOrEmpty(parameters.QuickSearch))
                {
                    int output;
                    if (int.TryParse(parameters.QuickSearch, out output))
                    {
                        query += "AND c.BuildYear = @output ";
                        cmd.Parameters.AddWithValue("@output", output);
                    }
                    else
                    {
                        query += "AND (mo.ModelName LIKE @ModelName OR ma.MakeName LIKE @MakeName) ";
                        cmd.Parameters.AddWithValue("@ModelName", parameters.QuickSearch + '%');
                        cmd.Parameters.AddWithValue("@MakeName", parameters.QuickSearch + '%');
                    }
                }

                if (parameters.CarType == "New")
                {
                    query += "AND c.CarType = 'New' ";
                }
                else if(parameters.CarType == "Used")
                {
                    query += "AND c.CarType = 'Used' ";
                }

                cmd.CommandText = query;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        CarDetails row = new CarDetails();

                        row.CarId = (int)dr["CarId"];
                        if (dr["SaleId"] != DBNull.Value)
                        {
                            row.SaleId = (int)dr["SaleId"];
                        }
                        
                        row.BuildYear = (int)dr["BuildYear"];
                        row.MakeName = dr["MakeName"].ToString();
                        row.ModelName = dr["ModelName"].ToString();
                        row.BodyStyle = dr["BodyStyle"].ToString();
                        row.Transmission = dr["Transmission"].ToString();
                        row.Color = dr["Color"].ToString();
                        row.Interior = dr["Interior"].ToString();
                        row.Mileage = (decimal)dr["Mileage"];
                        row.Vin = dr["Vin"].ToString();
                        row.SalePrice = (decimal)dr["SalePrice"];
                        row.MSRP = (decimal)dr["MSRP"];
                        row.CarDescription = dr["CarDescription"].ToString();
                        row.PictureUrl = dr["PictureUrl"].ToString();

                        carDetails.Add(row);
                    }
                }
            }

            return carDetails;
        }

        private Car Insert(Car car)
        {
            const string sql = "INSERT INTO Car (ModelId, SaleId, BuildYear, SalePrice, MSRP, BodyStyle, Color, Interior, Mileage, Vin, CarDescription, PictureUrl, CarType, Transmission, IsFeatured) "
                   + "VALUES (@ModelId, @SaleId, @BuildYear, @SalePrice, @MSRP, @BodyStyle, @Color, @Interior, @Mileage, @Vin, @CarDescription, @PictureUrl, @CarType, @Transmission, @IsFeatured); "
                   + "SELECT SCOPE_IDENTITY()";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                car.CarId = cn.Query<int>(sql, car).First();
            }
            return car;
        }

        private Car Update(Car car)
        {
            const string sql = "UPDATE Car SET "
                + "ModelId = @ModelId, "
                + "SaleId = @SaleId, "
                + "BuildYear = @BuildYear, "
                + "SalePrice = @SalePrice, "
                + "MSRP = @MSRP, "
                + "BodyStyle = @BodyStyle, "
                + "Color = @Color, "
                + "Interior = @Interior, "
                + "Mileage = @Mileage, "
                + "Vin = @Vin, "
                + "CarDescription = @CarDescription, "
                + "PictureUrl = @PictureUrl, "
                + "CarType = @CarType, "
                + "Transmission = @Transmission, " +
                "IsFeatured = @IsFeatured  "
                + "WHERE CarId = @CarId ";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;
                if (cn.Execute(sql, car) > 0)
                {
                    return car;
                }
            }

            return null;
        }

        public IEnumerable<InventoryReport> GetNewInventoryReport()
        {
            List<InventoryReport> inventoryReports = new List<InventoryReport>();

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                string query = "SELECT c.BuildYear, ma.MakeName Make, mo.ModelName Model, COUNT(c.CarId) CarCount, SUM(c.SalePrice) StockValue " +
                    "FROM Car c " +
                    "INNER JOIN Model mo ON mo.ModelId = c.ModelId " +
                    "INNER JOIN Make ma ON ma.MakeId = mo.MakeId " +
                    "WHERE CarType = 'New' " +
                    "GROUP BY c.BuildYear, ma.MakeName, mo.ModelName ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;


                cmd.CommandText = query;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InventoryReport row = new InventoryReport();

                        row.Year = (int)dr["BuildYear"];
                        row.Make = dr["Make"].ToString();
                        row.Model = dr["Model"].ToString();
                        row.Count = (int)dr["CarCount"];
                        row.StockValue = (decimal)dr["StockValue"];

                        inventoryReports.Add(row);
                    }
                }
            }

            return inventoryReports;
        }

        public IEnumerable<InventoryReport> GetUsedInventoryReport()
        {
            List<InventoryReport> inventoryReports = new List<InventoryReport>();

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                string query = "SELECT c.BuildYear, ma.MakeName Make, mo.ModelName Model, COUNT(c.CarId) CarCount, SUM(c.MSRP) StockValue " +
                    "FROM Car c " +
                    "INNER JOIN Model mo ON mo.ModelId = c.ModelId " +
                    "INNER JOIN Make ma ON ma.MakeId = mo.MakeId " +
                    "WHERE CarType = 'Used' " +
                    "GROUP BY c.BuildYear, ma.MakeName, mo.ModelName ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;


                cmd.CommandText = query;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InventoryReport row = new InventoryReport();

                        row.Year = (int)dr["BuildYear"];
                        row.Make = dr["Make"].ToString();
                        row.Model = dr["Model"].ToString();
                        row.Count = (int)dr["CarCount"];
                        row.StockValue = (decimal)dr["StockValue"];

                        inventoryReports.Add(row);
                    }
                }
            }

            return inventoryReports;
        }
    }
}
