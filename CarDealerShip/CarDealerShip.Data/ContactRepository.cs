using CarDealerShip.Domain;
using CarDealerShip.Domain.Interfaces;
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
    public class ContactRepository : IContactRepository
    {
        private string connection = ConfigurationManager
                 .ConnectionStrings["DefaultConnection"]
                 .ConnectionString;

        public IEnumerable<Contact> All()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Contact>("SELECT * FROM Contact");
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Contact WHERE ContactId = @ContactId";
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Execute(sql, new { ContactId = id }) > 0;
            }
        }

        public Contact FindById(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Contact "
                   + "WHERE ContactId = @ContactId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Contact>(sql, new { ContactId = id })
                    .FirstOrDefault();
            }
        }

        public Contact FindByCarId(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Contact "
                   + "WHERE CarId = @CarId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Contact>(sql, new { CarId = id })
                    .FirstOrDefault();
            }
        }

        public Contact Save(Contact contact)
        {
            if (contact.ContactId > 0)
            {
                return Update(contact);
            }
            return Insert(contact);
        }

        private Contact Insert(Contact contact)
        {
            const string sql = "INSERT INTO Contact (ContactName, Email, Phone, ContactMessage, CarId) "
                   + "VALUES (@ContactName, @Email, @Phone, @ContactMessage, @CarId); "
                   + "SELECT SCOPE_IDENTITY()";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                contact.CarId = cn.Query<int>(sql, contact).First();
            }
            return contact;
        }

        private Contact Update(Contact contact)
        {
            const string sql = "UPDATE Contact SET "
                + "ContactName = @ContactName, "
                + "Email = @Email, "
                + "Phone = @Phone, "
                + "ContactMessage = @ContactMessage "
                + "WHERE CarId = @CarId ";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;
                if (cn.Execute(sql, contact) > 0)
                {
                    return contact;
                }
            }

            return null;
        }
    }
}
