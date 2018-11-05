using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarDealerShip.Data;
using CarDealerShip.Domain;
using NUnit.Framework;

namespace CarDealerShip.Tests
{
    [TestFixture]
    public class SaleRepositoryTests
    {

        [SetUp]
        public void Init()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "DbReset";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Connection = cn;
                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        [Test]
        public void CanGetAll()
        {
            var repo = new SaleRepository();

            var result = repo.All();

            Assert.AreEqual(7, result.Count());
        }

        [Test]
        public void CanInsert()
        {
            var repo = new SaleRepository();
            var newSale = new Sale
            {
                CustomerName = "Jane Smith",
                Phone = "987-654-0123",
                Street1 = "1234 5th st",
                City = "Minneapolis",
                StateAbbrv = "MN",
                Zipcode = 12345,
                PurchasePrice = 23000M,
                PurchaseType = "Cash",
                SalesUserId = "00000000-0000-0000-0000-000000000000",
                PurchaseDate = DateTime.Now
            };

            repo.Save(newSale);

            var result = repo.All().Last();

            Assert.AreEqual(8, result.SaleId);
            Assert.AreEqual("Jane Smith", result.CustomerName);
            Assert.AreEqual("987-654-0123", result.Phone);
            Assert.AreEqual(null, result.Email);
            Assert.AreEqual("1234 5th st", result.Street1);
            Assert.AreEqual(null, result.Street2);
            Assert.AreEqual("Minneapolis", result.City);
            Assert.AreEqual("MN", result.StateAbbrv);
            Assert.AreEqual(12345, result.Zipcode);
            Assert.AreEqual(23000M, result.PurchasePrice);
            Assert.AreEqual("Cash", result.PurchaseType);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", result.SalesUserId);
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new SaleRepository();

            var currentSale = repo.FindById(1);

            currentSale.CustomerName = "Testing Update";
            currentSale.PurchaseType = "Testing Update";
            currentSale.PurchasePrice = 0M;

            repo.Save(currentSale);

            var result = repo.FindById(1);

            Assert.AreEqual(1, result.SaleId);
            Assert.AreEqual("Testing Update", result.CustomerName);
            Assert.AreEqual("123-456-7890", result.Phone);
            Assert.AreEqual("something@test.com", result.Email);
            Assert.AreEqual("123 4th st", result.Street1);
            Assert.AreEqual(null, result.Street2);
            Assert.AreEqual("Minneapolis", result.City);
            Assert.AreEqual("MN", result.StateAbbrv);
            Assert.AreEqual(12345, result.Zipcode);
            Assert.AreEqual(0M, result.PurchasePrice);
            Assert.AreEqual("Testing Update", result.PurchaseType);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", result.SalesUserId);
        }

        [Test]
        public void CanDelete()
        {
            var repo = new SaleRepository();

            var result = repo.Delete(2);

            Assert.IsTrue(result);

            var all = repo.All();

            Assert.AreEqual(6, all.Count());
        }
    }
}
