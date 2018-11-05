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
    public class ModelRepository : IModelRepository
    {
        private string  connection = ConfigurationManager
                    .ConnectionStrings["DefaultConnection"]
                    .ConnectionString;
        public IEnumerable<Model> All()
        {
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Model>("SELECT * FROM Model");
            }
        }

        public bool Delete(int id)
        {
            const string sql = "DELETE FROM Model WHERE ModelId = @ModelId";
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Execute(sql, new { ModelId = id }) > 0;
            }
        }

        public Model FindById(int id)
        {
            const string sql = "SELECT * "
                   + "FROM Model "
                   + "WHERE ModelId = @ModelId";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                return cn.Query<Model>(sql, new { ModelId = id })
                    .FirstOrDefault();
            }
        }

        public Model Save(Model model)
        {
            if (model.ModelId > 0)
            {
                return Update(model);
            }
            return Insert(model);
        }

        private Model Insert(Model model)
        {
            const string sql = "INSERT INTO Model (ModelName, MakeId, DateAdded, AdminUserId) "
                   + "VALUES (@ModelName, @MakeId, @DateAdded, @AdminUserId); "
                   + "SELECT SCOPE_IDENTITY()";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;

                model.ModelId = cn.Query<int>(sql, model).First();
            }
            return model;
        }

        private Model Update(Model model)
        {
            const string sql = "UPDATE Model SET "
                + "ModelName = @ModelName, " +
                "MakeId = @MakeId, " +
                "DateAdded = @DateAdded, " +
                "AdminUserId = @AdminUserId "
                + "WHERE ModelId = @ModelId ";

            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connection;
                if (cn.Execute(sql, model) > 0)
                {
                    return model;
                }
            }

            return null;
        }
    }
}
