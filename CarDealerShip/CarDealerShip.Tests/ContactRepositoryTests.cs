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
    public class ContactRepositoryTests
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
            var repo = new ContactRepository();

            var result = repo.All();

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void CanInsert()
        {
            var repo = new ContactRepository();

            repo.Save(new Contact {
                ContactName = "contact name",
                Phone = "123-456-7890",
                ContactMessage = "A message goes here"

            });

            var result = repo.All().Last();

            Assert.AreEqual(2, result.ContactId);
            Assert.AreEqual(null, result.Email);
            Assert.AreEqual("123-456-7890", result.Phone);
            Assert.AreEqual("A message goes here", result.ContactMessage);
            Assert.AreEqual(null, result.CarId);
        }

        [Test]
        public void CanUpdate()
        {
            var repo = new ContactRepository();

            var currentContact = repo.FindById(1);

            currentContact.ContactName = "Testing Update";
            currentContact.ContactMessage = "Testing Update";

            repo.Save(currentContact);

            var result = repo.FindById(1);

            Assert.AreEqual("Testing Update", result.ContactName);
            Assert.AreEqual("Testing Update", result.ContactMessage);
        }

    }
}
