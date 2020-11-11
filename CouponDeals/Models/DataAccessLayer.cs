using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace CouponDeals.Models
{
    public class DataAccessLayer
    {
        SqlConnection conn = null;
        string connString = "";
        public DataAccessLayer(string connectionString)
        {
            connString = connectionString;
        }
        public bool IsConnected
        {
            get
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        public SqlConnection sqlConnection
        {
            get { return conn; }

        }

        public string ConnectionString
        {
            set
            {
                connString = value;
            }
        }


        public bool Connect()
        {
            connString = CouponDeals.Properties.Settings.Default.dbCouponDealsConnectionString.Trim();
            conn = new SqlConnection(connString);
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
            return false;
        }

        public void Close()
        {
            try
            {
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

            }
        }
        public DataTable GetCategories()
        {
            string query = "SELECT * FROM tblCategories";
            SqlDataAdapter dap = new SqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            dap.Fill(dt);
            dap.Dispose();
            return dt;
        }
    }
}