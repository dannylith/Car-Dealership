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
    public class SpecialRepository : ISpecialRepository
    {
        private string connection = ConfigurationManager
                  .ConnectionStrings["DefaultConnection"]
                  .ConnectionString;

        public IEnumerable<Special> All()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Special>("SELECT * FROM Special");
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Special WHERE SpecialId = @SpecialId";
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Execute(sql, new { SpecialId = id }) > 0;
            }
        }

        public Special FindById(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Special "
                   + "WHERE SpecialId = @SpecialId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Special>(sql, new { SpecialId = id })
                    .FirstOrDefault();
            }
        }

        public Special Save(Special special)
        {
            if (special.SpecialId > 0)
            {
                return Update(special);
            }
            return Insert(special);
        }

        private Special Insert(Special special)
        {
            const string sql = "INSERT INTO Special (SpecialTitle, SpecialDesc) "
                   + "VALUES (@SpecialTitle, @SpecialDesc); "
                   + "SELECT SCOPE_IDENTITY()";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                special.SpecialId = cn.Query<int>(sql, special).First();
            }
            return special;
        }

        private Special Update(Special special)
        {
            const string sql = "UPDATE Special SET "
                + "SpecialTitle = @SpecialTitle, "
                + "SpecialDesc = @SpecialDesc "
                + "WHERE SpecialId = @SpecialId ";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;
                if (cn.Execute(sql, special) > 0)
                {
                    return special;
                }
            }

            return null;
        }
    }
}
