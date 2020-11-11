using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CouponDeals.Models
{
    public class CompanyEntity
    {
        SqlConnection _sqlConnection;
        public CompanyEntity(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public int CreateOrUpdateCompany(string CompanyName, string Website,
            string Image, int CatID,int Status, DateTime CreatedDate,
            int CompanyID = 0)
        {
            string query;
            if (CompanyID == 0)
            {
                query = "INSERT INTO tblCompany VALUES(@name, @website," +
                "@profile_image,@category_id,@status, @created_date);SELECT CAST(scope_identity() AS int)";
            }
            else
            {
                query = "UPDATE tblCompany SET name = @name," +
                "website = @website, profile_image = @profile_image, " +
                "category_id = @category_id, status = @status, " +
                "created_date = @created_date WHERE company_id = @company_id";
            }
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@name", CompanyName);
            cmd.Parameters.AddWithValue("@website", Website);
            cmd.Parameters.AddWithValue("@profile_image", Image);
            cmd.Parameters.AddWithValue("@category_id", CatID);
            cmd.Parameters.AddWithValue("@status", Status);
            cmd.Parameters.AddWithValue("@created_date", CreatedDate);
            int InsertID = 0;
            if (CompanyID == 0)
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
                cmd.Parameters.AddWithValue("@company_id", CompanyID);
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
        public DataTable GetCompanyList()
        {
            DataTable dt = new DataTable();
            string query = "SELECT company_id, name, website, profile_image, category_id, created_date, status, CASE WHEN tblCompany.status = 1 THEN 'Active' ELSE 'Not Active' END AS status_name, FORMAT (tblCompany.created_date, 'dd-MM-yyyy') as date FROM tblCompany";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetCompanyByID(int CompanyID)
        {
            DataTable dt = new DataTable();
            string query = "SELECT TOP 1 company_id, name, website, profile_image, category_id, created_date, status, CASE WHEN tblCompany.status = 1 THEN 'Active' ELSE 'Not Active' END AS status_name FROM tblCompany WHERE company_id = @company_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@company_id", CompanyID);
            dap.Fill(dt);
            return dt;
        }
        public int DeleteCompanyByID(int CompanyID)
        {
            int Result = 0;
            string query = "DELETE FROM tblCompany WHERE company_id = @company_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@company_id", CompanyID);
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