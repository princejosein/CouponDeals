using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CouponDeals.Models
{
    public class CategoryEntity
    {
        SqlConnection _sqlConnection;
        public CategoryEntity(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public int CreateOrUpdateCategory(string CategoryName, string ImagePath,
            int Status, int CatID = 0)
        {
            string query;
            if (CatID == 0)
            {
                query = "INSERT INTO tblCategories VALUES(@name, @cat_image," +
                "@status);SELECT CAST(scope_identity() AS int)";
            }
            else
            {
                query = "UPDATE tblCategories SET name = @name," +
                "cat_image = @cat_image, status = @status " +
                "WHERE category_id = @category_id";
            }
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@name", CategoryName);
            cmd.Parameters.AddWithValue("@cat_image", ImagePath);
            cmd.Parameters.AddWithValue("@status", Status);
            int InsertID = 0;
            if (CatID == 0)
            {
                try
                {
                    InsertID = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@category_id", CatID);
                try
                {
                    InsertID = (int)cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return InsertID;
        }
        public DataTable GetCategoryList()
        {
            DataTable dt = new DataTable();
            string query = "SELECT category_id, name, cat_image, status, CASE WHEN tblCategories.status = 1 THEN 'Active' ELSE 'Not Active' END AS status_name FROM tblCategories";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetCategoryByID(int CatID)
        {
            DataTable dt = new DataTable();
            string query = "SELECT TOP 1 category_id, name, cat_image, status, CASE WHEN tblCategories.status = 1 THEN 'Active' ELSE 'Not Active' END AS status_name FROM tblCategories WHERE category_id = @cat_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@cat_id", CatID);
            dap.Fill(dt);
            return dt;
        }
        public int DeleteCategoryByID(int CategoryID)
        {
            int Result = 0;
            string query = "DELETE FROM tblCategories WHERE category_id = @cat_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@cat_id", CategoryID);
            try
            {
                Result = (int)cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Result;
        }
    }
}