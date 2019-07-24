using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;

namespace Connect_DB_Csharp_SQL_20190718
{
    class Program
    {
        private static readonly string conn_str = "Server = ASUS_P52F\\SQLEXPRESS; Database = InternetShop; user Id = OVA; Password=123";

        private static void getDataReader()
        {
            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select product_id, product_name, product_model from Product", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Console.WriteLine(dr["product_id"].ToString() + "-" + dr.GetSqlString(1) + "\t" + dr["product_name"].ToString());
                }
                // cmd.Dispose();
            }
        }

        private static DataSet GetDataSet()
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select product_id, product_name, product_model from Product", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }

        private static DataTable getDataSet_OleDB()
        {
            DataSet ds = new DataSet();
            using (OleDbConnection conn = new OleDbConnection(conn_str_ole))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("select product_id, product_name, cost from Product", conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(ds);
                return ds.Tables[0];
            }
        }

        private static DataSet GetDataSetById(int product_id)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select product_id, product_name, product_model " +
                                                "from Product where product_id = " + product_id, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }


        private static DataSet GetDataSetById_proc(int product_id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("p_get_Product_byId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@product_id", product_id);

                //cmd.Parameters.Add("@product_id", SqlDbType.Int, 10).Value = product_id;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }

        private static void UpdateProdById(int product_id, string description)
        {

            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("p_upd_prod_byId", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.ExecuteNonQuery();
            }
        }

    
        private static decimal GetCostSum(int product_id)
        {

            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("prod_sum", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@id", SqlDbType.Int, 10).Value = product_id;
                cmd.Parameters.Add("@res", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return Convert.ToDecimal(cmd.Parameters["@res"].Value.ToString());
            }
        }

        // использование DAPPER

        private static void Dapper_sel()
        {
            Console.WriteLine("DAPPER");
            using (IDbConnection db = new SqlConnection(conn_str))
            {
                List<ProductCategory> res = db.Query<ProductCategory>("SELECT category_id, category_name FROM ProductCategory", commandType: CommandType.Text).ToList();
                foreach (var item in res)
                {
                    Console.WriteLine($"{item.category_id} - {item.category_name}");
                }
            }
        }

        private static void Linq()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(conn_str))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT category_id, category_name FROM ProductCategory", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            List<ProductCategory> res_1 = (from p in ds.Tables[0].AsEnumerable()
                                           select new ProductCategory() { category_id = p.Field<int>(0), category_name = p.Field<string>(1) }).ToList();

            List<ProductCategory> res_2 = ds.Tables[0].AsEnumerable()
                                          .Select(z => new ProductCategory() { category_id = z.Field<int>(0), category_name = z.Field<string>(1) }).ToList();

            Console.WriteLine("LINQ_1");
            foreach (var item in res_1)
            {
                Console.WriteLine($"{item.category_id} - {item.category_name}");
            }

            Console.WriteLine("LINQ_2");
            foreach (var item in res_2)
            {
                Console.WriteLine($"{item.category_id} - {item.category_name}");
            }


        }

        private static void getProductCat(int? category_id)
        {
            Console.WriteLine("DAPPER");
            using (IDbConnection db = new SqlConnection(conn_str))
            {
                List<ProdCatProducts> res = db.Query<ProdCatProducts>("p_get_pc_p", new { category_id = category_id }, commandType: CommandType.StoredProcedure).ToList();
                foreach (var item in res)
                {
                    Console.WriteLine($"{item.category_name} - {item.product_name}- {item.product_model}- {item.cost}");
                }
            }

        }

        private static void getCustomerEF()
        {
            using (Context db = new Context())
            {
                foreach (var item in db.Customer.ToList())
                {
                    Console.WriteLine($"{item.first_name} - {item.last_name}");
                }

            }
        }

        private static void addCust()
        {
            using (Context db = new Context())
            {
                db.Customer.Add(new Customer { first_name = "asda" });
                db.SaveChanges();

                db.Customer.Remove(new Customer { });
                db.Database.ExecuteSqlCommand("delete from ....");
                db.SaveChanges();

            }
        }

        static void Main(string[] args)
        {
            //getDataReader();

            // var ds = GetDataSet();

            //DataSet ds = GetDataSet();
            //DataTable dt = ds.Tables[0];

            //var res_1 = (from p in ds.Tables[0].AsEnumerable()
            //             where p.Field<Int32>(0) == 2
            //             select p);

            //var rows = ds.Tables[0].Rows;

            //foreach (var item in res_1)
            //{
            //    Console.WriteLine(item[0].ToString() + "-" + item[1].ToString() + "-" + item[2].ToString());

            //}

            //foreach (DataRow item in ds.Tables[0].Rows)
            //{
            //    Console.WriteLine(item[0].ToString() + "-" + item[1].ToString() + "-" + item[2].ToString());

            //}

            ///////////////////////////////////////////


            //foreach (DataRow item in GetDataSetById_proc(7).Tables[0].Rows)
            //{
            //    Console.WriteLine(item[0].ToString() + "-" + item[1].ToString() + "-" + item[2].ToString());

            //}

            //UpdateProdById(7, "model");

            Console.WriteLine(GetCostSum(1));

            Console.ReadKey();
        }
    }

    class ProductCategory
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
    }
}

