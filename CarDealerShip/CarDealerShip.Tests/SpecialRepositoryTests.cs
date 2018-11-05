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
    public class SpecialRepositoryTests
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
            var repo = new SpecialRepository();

            var result = repo.All();

            Assert.AreEqual(4, result.Count());
        }

        [Test]
        public void CanInsert()
        {
            var repo = new SpecialRepository();

            repo.Save(new Special {
                SpecialTitle = "Big Sale",
                SpecialDesc = "All Cars 50% off"
            });

            var result = repo.All().Last();

            Assert.AreEqual(5, result.SpecialId);
            Assert.AreEqual("Big Sale", result.SpecialTitle);
            Assert.AreEqual("All Cars 50% off", result.SpecialDesc);
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new SpecialRepository();

            var currentSpecial = repo.FindById(1);

            currentSpecial.SpecialTitle = "Testing Update";
            currentSpecial.SpecialDesc = "Testing Update";

            repo.Save(currentSpecial);

            var result = repo.FindById(1);

            Assert.AreEqual("Testing Update", result.SpecialTitle);
            Assert.AreEqual("Testing Update", result.SpecialDesc);
        }

        [Test]
        public void CanDelete()
        {
            var repo = new SpecialRepository();

            repo.Delete(1);

            var result = repo.All();

            Assert.AreEqual(3, result.Count());
        }
    }
}
