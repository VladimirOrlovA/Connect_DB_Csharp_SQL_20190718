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
        private static readonly string conn_str = "Server = ASUS_P52F\\SQLEXPRESS; Database = InternetShop; user Id=ASUS_P52F\\Orlov; Password=7294";

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

            Console.WriteLine(GetCostSum(7));

            Console.ReadKey();
        }

    }
}

