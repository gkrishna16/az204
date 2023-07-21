using System;
using System.Data.SqlClient;
using AlpineSkiHouse.Models;

namespace AlpineSkiHouse.Services
{
    public class ProductService
    {
        private static string db_source = "gopaldbserver2.database.windows.net";
        private static string db_user = "gopalserver";
        private static string db_password = "gopal8459$";
        private static string db_database = "gopaldb";
        private SqlConnection GetConnection()
        {
            var _builder = new SqlConnectionStringBuilder();

            _builder.DataSource = db_source;
            _builder.UserID = db_user;
            _builder.Password = db_password;
            _builder.InitialCatalog = db_database;

            return new SqlConnection(_builder.ConnectionString);
        }

        public List<Product> GetProducts()
        {
            // get the connection
            SqlConnection conn = GetConnection();
            // Empty list of products
            var _product_lst = new List<Product>();

            string statement = "SELECT ProductID, ProductName, Quantity from Products";

            conn.Open();

            SqlCommand cmd = new SqlCommand(statement, conn);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var product = new Product()
                    {
                        ProductID = reader.GetInt32(0),
                        ProductName = reader.GetString(1),
                        Quantity = reader.GetInt32(2)
                    };
                    _product_lst.Add(product);
                }
            };
            // closing our connection
            conn.Close();

            // returning the product list
            return _product_lst;
        }
    }

}