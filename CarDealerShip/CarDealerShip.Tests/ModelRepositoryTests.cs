using CarDealerShip.Data;
using CarDealerShip.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerShip.Tests
{
    [TestFixture]
    public class ModelRepositoryTests
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
        public void CanInsert()
        {
            var repo = new ModelRepository();

            repo.Save(new Model {
                ModelName = "Civic",
                MakeId = 2,
                DateAdded = DateTime.Now,
                AdminUserId = "00000000-0000-0000-0000-000000000000"
            });

            var result = repo.All().Last();

            Assert.AreEqual(14, result.ModelId);
            Assert.AreEqual("Civic", result.ModelName);
            Assert.AreEqual(2, result.MakeId);
        }

        [Test]
        public void CanGetAll()
        {
            var repo = new ModelRepository();

            var result = repo.All();

            Assert.AreEqual(13, result.Count());
            Assert.AreEqual("Corolla", result.First().ModelName);
        }

        [Test]
        public void CanDelete()
        {
            var repo = new ModelRepository();

            var result = repo.Delete(3);

            Assert.IsTrue(result);

            var all = repo.All();

            Assert.AreEqual(12, all.Count());
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new ModelRepository();

            var model = repo.FindById(1);

            model.ModelName = "Testing Update";
            repo.Save(model);

            var result = repo.FindById(1);

            Assert.AreEqual(1, result.ModelId);
            Assert.AreEqual("Testing Update", result.ModelName);

        }
    }
}
