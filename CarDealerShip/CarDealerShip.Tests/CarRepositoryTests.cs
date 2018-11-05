using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarDealerShip.Data;
using CarDealerShip.Domain;
using CarDealerShip.Domain.Queries;
using NUnit.Framework;

namespace CarDealerShip.Tests
{
    [TestFixture]
    public class CarRepositoryTests
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
            var repo = new CarRepository();

            var result = repo.All();

            Assert.AreEqual(8, result.Count());
        }

        [Test]
        public void CanInsert()
        {
            var repo = new CarRepository();

            repo.Save(new Car {
                ModelId = 2,
                BuildYear = 2010,
                MSRP = 15000M,
                BodyStyle = "Hatchback",
                Color = "Red",
                Interior = "Blue",
                Mileage = 100M,
                Vin = "1FAFP44454F272207",
                CarDescription = "exotic car",
                PictureUrl = "no link",
                CarType = "New",
                Transmission = "Automatic"
            });

            var result = repo.All().Last();

            Assert.AreEqual(9, result.CarId);
            Assert.AreEqual(2010, result.BuildYear);
            Assert.AreEqual(15000M, result.MSRP);
            Assert.AreEqual("Hatchback", result.BodyStyle);
            Assert.AreEqual("Red", result.Color);
            Assert.AreEqual("Blue", result.Interior);
            Assert.AreEqual(100M, result.Mileage);
            Assert.AreEqual("1FAFP44454F272207", result.Vin);
            Assert.AreEqual("exotic car", result.CarDescription);
            Assert.AreEqual("no link", result.PictureUrl);
            Assert.AreEqual("New", result.CarType);
            Assert.AreEqual("Automatic", result.Transmission);
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new CarRepository();

            var currentCar = repo.FindById(1);

            currentCar.MSRP = 0M;
            currentCar.PictureUrl = "Testing Update";
            currentCar.Vin = "Testing Update";
            currentCar.CarDescription = "Testing Update";

            repo.Save(currentCar);

            var result = repo.FindById(1);

            Assert.AreEqual(0M, result.MSRP);
            Assert.AreEqual("Testing Update", result.PictureUrl);
            Assert.AreEqual("Testing Update", result.Vin);
            Assert.AreEqual("Testing Update", result.CarDescription);
        }

        [Test]
        public void CanDelete()
        {
            var repo = new CarRepository();

            repo.Delete(2);

            var result = repo.All();

            Assert.AreEqual(7, result.Count());
        }

        [Test]
        public void CanSearch()
        {
            var repo = new CarRepository();
            var parameters = new CarSearchParameters {
                QuickSearch = "Toyota"
            };

            var result = repo.Search(parameters);

            Assert.AreEqual(5, result.Count());

            parameters.QuickSearch = null;
            parameters.MinPrice = 15000M;

            result = repo.Search(parameters);

            Assert.AreEqual(5, result.Count());
            Assert.AreEqual("Automatic", result.First().Transmission);
        }

        [Test]
        public void CanGetFeatured()
        {
            var repo = new CarRepository();
            var result = repo.GetFeatured();

            Assert.AreEqual(7, result.Count());
        }
    }
}
