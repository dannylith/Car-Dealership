using CarDealerShip.Domain;
using CarDealerShip.Domain.Interfaces;
using CarDealerShip.Domain.Queries;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Data
{
    public class SaleRepository : ISaleRepository
    {
        private string connection = ConfigurationManager
                   .ConnectionStrings["DefaultConnection"]
                   .ConnectionString;

        public IEnumerable<Sale> All()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Sale>("SELECT * FROM Sale");
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Sale WHERE SaleId = @SaleId";
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Execute(sql, new { SaleId = id }) > 0;
            }
        }

        public Sale FindById(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Sale "
                   + "WHERE SaleId = @SaleId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Sale>(sql, new { SaleId = id })
                    .FirstOrDefault();
            }
        }

        public Sale Save(Sale sale)
        {
            if (sale.SaleId > 0)
            {
                return Update(sale);
            }
            return Insert(sale);
        }

        private Sale Insert(Sale sale)
        {
            const string sql = "INSERT INTO Sale (CustomerName, Phone, Email, Street1, Street2, City, StateAbbrv, Zipcode, PurchasePrice, PurchaseType, SalesUserId, PurchaseDate) "
                   + "VALUES (@CustomerName, @Phone, @Email, @Street1, @Street2, @City, @StateAbbrv, @Zipcode, @PurchasePrice, @PurchaseType, @SalesUserId, @PurchaseDate); "
                   + "SELECT SCOPE_IDENTITY()";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                sale.SaleId = cn.Query<int>(sql, sale).First();
            }
            return sale;
        }

        private Sale Update(Sale sale)
        {
            const string sql = "UPDATE Sale SET "
                + "CustomerName = @CustomerName, "
                + "Phone = @Phone, "
                + "Email = @Email, "
                + "Street1 = @Street1, "
                + "Street2 = @Street2, "
                + "City = @City, "
                + "StateAbbrv = @StateAbbrv, "
                + "Zipcode = @Zipcode, "
                + "PurchasePrice = @PurchasePrice, "
                + "PurchaseType = @PurchaseType, "
                + "SalesUserId = @SalesUserId, " +
                "PurchaseDate = @PurchaseDate "
                + "WHERE SaleId = @SaleId ";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;
                if (cn.Execute(sql, sale) > 0)
                {
                    return sale;
                }
            }

            return null;
        }

        public IEnumerable<SalesReport> GetSalesReport(SalesReportSearchParameters parameters)
        {
            List<SalesReport> salesReports = new List<SalesReport>();

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                string query = "SELECT CONCAT(asp.FirstName, ' ', asp.LastName) FullName, SUM(s.PurchasePrice) TotalSales, COUNT(s.SaleId) TotalVehicles " +
                    "FROM Sale s " +
                    "INNER JOIN AspNetUsers asp ON s.SalesUserId = asp.Id " +
                    "WHERE 1 = 1 ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;

                if (!string.IsNullOrEmpty(parameters.SalesUserId))
                {
                    query += "AND s.SalesUserId = @SalesUserId ";
                    cmd.Parameters.AddWithValue("@SalesUserId", parameters.SalesUserId);
                }

                if (parameters.FromDate.HasValue)
                {
                    query += "AND s.PurchaseDate > @FromDate ";
                    cmd.Parameters.AddWithValue("@FromDate", parameters.FromDate);
                }

                if (parameters.ToDate.HasValue)
                {
                    query += "AND s.PurchaseDate < @ToDate ";
                    cmd.Parameters.AddWithValue("@ToDate", parameters.ToDate);
                }


                query += "GROUP BY asp.FirstName, asp.LastName ";
                cmd.CommandText = query;
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        SalesReport row = new SalesReport();

                        row.FullName = dr["FullName"].ToString();
                        row.TotalSales = (decimal)dr["TotalSales"];
                        row.TotalVehicles = (int)dr["TotalVehicles"];

                        salesReports.Add(row);
                    }
                }
            }

            return salesReports;
        }
    }
}
