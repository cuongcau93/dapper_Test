using Dapper;
using DapperDemo.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDemo.Repository
{

    public class ProductRepository
    {
        private string connectionString;

        public ProductRepository()
        {
            connectionString = @"Server=localhost;Database=DapperDemo;user id=sa;password=123456;Trusted_Connection=true;";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public Product GetDataProductById(int id)
        {
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    var productData = dbConnection.QueryFirst<ProductExt>("spDdGetStudentById", new { Quantity = 15, Id = id}, commandType: CommandType.StoredProcedure);
                    return productData;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Add(Product prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO Products (Name, Quantity, Price)"
                                + " VALUES(@Name, @Quantity, @Price)";

                dbConnection.Open();
                dbConnection.Execute(sQuery, prod);
            }
            //var cars = new List<CarType> { new CarType { CARID = 1, CARNAME = "Volvo" } };

            //var parameters = new DynamicParameters();
            //parameters.AddTable("@Cars", "CarType", cars)

            //var result = con.Query("InsertCars", parameters, commandType: CommandType.StoredProcedure);
            //{
            //    try
            //    {
            //        var contact = new Contact();
            //        Console.WriteLine("Enter Name: ");
            //        contact.Name = Console.ReadLine();
            //        Console.WriteLine("Enter Phone: ");
            //        contact.Phone = Console.ReadLine();
            //        var param = new DynamicParameters();
            //        param.Add("@Id", contact.Id, DbType.Int32, direction: ParameterDirection.InputOutput);
            //        param.Add("Name", contact.Name);
            //        param.Add("Phone", contact.Phone);
            //        db.Execute("SaveContact", param, commandType: CommandType.StoredProcedure);
            //        contact.Id = param.Get<int>("@Id");
            //        Console.WriteLine("Added: {0}", contact.ToString());
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
        }

        public IEnumerable<Product> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Product>("SELECT * FROM Products");
            }
        }

        public Product GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Products"
                               + " WHERE ProductId = @Id";
                dbConnection.Open();
                return dbConnection.Query<Product>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM Products"
                             + " WHERE ProductId = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(Product prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Products SET Name = @Name,"
                               + " Quantity = @Quantity, Price= @Price"
                               + " WHERE ProductId = @ProductId";
                dbConnection.Open();
                dbConnection.Query(sQuery, prod);
            }
        }
    }
}
