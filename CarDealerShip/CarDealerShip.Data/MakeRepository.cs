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

    public class MakeRepository : IMakeRepository
    {
        private string connection = ConfigurationManager
                   .ConnectionStrings["DefaultConnection"]
                   .ConnectionString;

        public IEnumerable<Make> All()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Make>("SELECT * FROM Make");
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Make WHERE MakeId = @MakeId";
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Execute(sql, new { MakeId = id }) > 0;
            }
        }

        public Make FindById(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Make "
                   + "WHERE MakeId = @MakeId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Make>(sql, new { MakeId = id })
                    .FirstOrDefault();
            }
        }

        public Make Save(Make make)
        {
            if (make.MakeId > 0)
            {
                return Update(make);
            }
            return Insert(make);
        }

        private Make Insert(Make make)
        {
            const string sql = "INSERT INTO Make (MakeName, DateAdded, AdminUserId) "
                   + "VALUES (@MakeName, @DateAdded, @AdminUserId); "
                   + "SELECT SCOPE_IDENTITY()";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                make.MakeId = cn.Query<int>(sql, make).First();
            }
            return make;
        }

        private Make Update(Make make)
        {
            const string sql = "UPDATE Make SET "
                + "MakeName = @MakeName, " +
                "DateAdded = @DateAdded, " +
                "AdminUserId = @AdminUserId "
                + "WHERE MakeId = @MakeId ";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;
                if (cn.Execute(sql, make) > 0)
                {
                    return make;
                }
            }

            return null;
        }
    }
}
