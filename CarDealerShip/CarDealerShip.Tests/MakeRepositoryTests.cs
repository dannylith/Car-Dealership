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
    public class MakeRepositoryTests
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
        public void CanAdd()
        {
            var repo = new MakeRepository();

            var result = repo.Save(new Make {
                MakeName = "Nissan",
                DateAdded = DateTime.Now,
                AdminUserId= "00000000-0000-0000-0000-000000000000"
            });

            Assert.AreEqual(6, result.MakeId);
        }

        [Test]
        public void CanGetAll()
        {
            var repo = new MakeRepository();

            var result = repo.All();

            Assert.AreEqual(5, result.Count());
            Assert.AreEqual("Toyota", result.First().MakeName);
        }

        [Test]
        public void CanDelete()
        {
            var repo = new MakeRepository();

            var result = repo.Delete(5);

            Assert.IsTrue(result);

            var all = repo.All();

            Assert.AreEqual(4, all.Count());

            
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new MakeRepository();

            var currentMake = repo.FindById(1);
            currentMake.MakeName = "Toyota Updated";
            repo.Save(currentMake);

            var newMake = repo.FindById(1);

            Assert.AreEqual("Toyota Updated", newMake.MakeName);
        }
    }
}
