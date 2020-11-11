using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace CouponDeals.Models
{
    public class CouponEntity
    {
        SqlConnection _sqlConnection;
        public CouponEntity(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public int CreateOrUpdateCoupon(string CompanyWebsite, 
            int OfferTypeID, string Code, string ExpDate,string CodeDesc, 
            int Promotion, int Status, string CodeTitle, int CustomerID=1, int CouponID = 0)        {
            int CompanyID = GetOrCreateCompanyByName(CompanyWebsite);
            DateTime ExpirationDate = DateTime.Parse(ExpDate);
            SqlCommand cmd;
            string query;
            if (CouponID == 0)
            {
                Status = 1;
                query = "INSERT INTO tblCoupons (customer_id,company_id, " +
                "offer_type_id, code_title, code, code_desc, expiration_date, created_date, " +
                "is_approved, status) VALUES(@customer_id, @company_id," +
                "@offer_type_id,@code_title, @code,@code_desc, @expiration_date, @created_date," +
                "@is_approved, @status);SELECT CAST(scope_identity() AS int)";
                cmd = new SqlCommand(query, _sqlConnection);
                DateTime CreatedDate = DateTime.Now;
                cmd.Parameters.AddWithValue("@created_date", CreatedDate);
                cmd.Parameters.AddWithValue("@is_approved", 1);
            }
            else
            {
                query = "UPDATE tblCoupons SET company_id = @company_id," +
                "offer_type_id = @offer_type_id, code = @code, " +
                "code_desc = @code_desc, code_title = @code_title, expiration_date = @expiration_date, " +
                "on_promotion = @on_promotion, status = @status WHERE coupon_id = @coupon_id";
                cmd = new SqlCommand(query, _sqlConnection);
            }
            
            cmd.Parameters.AddWithValue("@company_id", CompanyID);
            cmd.Parameters.AddWithValue("@offer_type_id", OfferTypeID);
            cmd.Parameters.AddWithValue("@code_title", CodeTitle);
            cmd.Parameters.AddWithValue("@code", Code);
            cmd.Parameters.AddWithValue("@code_desc", CodeDesc);
            cmd.Parameters.AddWithValue("@expiration_date", ExpirationDate);
            cmd.Parameters.AddWithValue("@on_promotion", Promotion);
            cmd.Parameters.AddWithValue("@status", Status);
            int CouponInsUpdate = 0;
            if (CouponID == 0)
            {
                cmd.Parameters.AddWithValue("@customer_id", CustomerID);
                try
                {
                    CouponInsUpdate = (int)cmd.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@coupon_id", CouponID);
                try
                {
                    CouponInsUpdate = (int)cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return CouponInsUpdate;
        }
        private int GetOrCreateCompanyByName(string CompanyWebsite)
        {
            int CompanyID = 0;
            string query = "SELECT TOP 1 company_id FROM tblCompany WHERE website = @website";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@website", CompanyWebsite);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    CompanyID = int.Parse(reader["company_id"].ToString());
                }
            }
            if(CompanyID == 0)
            {
                CompanyID = CreateCompany(CompanyWebsite);
            }
            reader.Close();
            return CompanyID;
        }
        private int CreateCompany(string CompanyWebsite)
        {
            string query = "INSERT INTO tblCompany (website, status) " +
                "VALUES(@website, @status);SELECT CAST(scope_identity() AS int)";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@website", CompanyWebsite);
            //Waiting for approval
            cmd.Parameters.AddWithValue("@status", 3);
            int InsertID = 0;
            try
            {
                InsertID = (int)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return InsertID;
        }
        public DataTable GetOfferTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT offer_type_id, name FROM tblOfferTypes";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetCouponsList(int filterStatus = 0, int filterCategory = 0,
            string filterKeyword = "", string filterCompany = "", string filterDate = "")
        {
            //DateTime ExpirationDate = DateTime.Parse(ExpDate);
            DataTable dt = new DataTable();
            string query = "SELECT tblCoupons.coupon_id, tblCoupons.company_id, tblCategories.name as cat_name,   " +
                "CASE WHEN tblCoupons.status = 1 THEN 'Active' " +
                "WHEN tblCoupons.status = 2 THEN 'Not Active' WHEN tblCoupons.status = 3 THEN 'Waiting for approval' " +
                " ELSE 'Expired' END AS status_name, " +
                "CASE WHEN tblCoupons.is_approved = 0 THEN 'Waiting for approval' ELSE 'Approved' END AS is_approved_formatted, " +
                "FORMAT (tblCoupons.expiration_date, 'dd-MM-yyyy') as expiration_date_formatted, tblOfferTypes.name as offer_type, " +
                "tblCompany.name as company_name FROM tblCoupons " +
                "LEFT JOIN tblCompany ON tblCompany.company_id = tblCoupons.company_id " +
                "LEFT JOIN tblCategories ON tblCategories.category_id = tblCompany.category_id " +
                "LEFT JOIN tblOfferTypes ON tblOfferTypes.offer_type_id = tblCoupons.offer_type_id";
            int count = 0;
            if(filterStatus != 0)
            {
                query = query + " WHERE tblCoupons.status = '" + filterStatus + "'";
                count++;
            }
            if (filterCategory != 0)
            {
                if (count != 0)
                {
                    query = query + " AND tblCompany.category_id = '" + filterCategory + "'";
                } else
                {
                    query = query + " WHERE tblCompany.category_id = '" + filterCategory + "'";
                }
                count++;
            }
            if (filterKeyword != "")
            {
                if (count != 0)
                {
                    query = query + " AND tblCoupons.code_title LIKE '%" + filterKeyword + "%'";
                } else
                {
                    query = query + " WHERE tblCoupons.code_title LIKE '%" + filterKeyword + "%'";
                }
                count++;
            }
            if (filterCompany != "")
            {
                if (count != 0)
                {
                    query = query + " AND tblCompany.name LIKE '%" + filterCompany + "%'";
                } else
                {
                    query = query + " WHERE tblCompany.name LIKE '%" + filterCompany + "%'";
                }
                count++;
            }
            if (filterDate != "")
            {
                DateTime fDate = DateTime.Parse(filterDate);
                string filDate = fDate.ToString("yyyy-MM-dd hh:mm:ss.fff");
                if (count != 0)
                {
                    query = query + " AND tblCoupons.created_date > '" + filDate + "' ";
                }
                else
                {
                    query = query + " WHERE tblCoupons.created_date >  '" + filDate + "'";
                }
                count++;
            }
            HttpContext httpContext = HttpContext.Current;
            int RoleID = 0;
            int UserID = 0;
            try
            {
                RoleID = (int)httpContext.Session["UserRoleID"];
                UserID = (int)httpContext.Session["UserID"];
            } catch(Exception ex) { }
            if(RoleID == 2)
            {
                if(count != 0)
                {
                    query = query + " AND tblCoupons.customer_id =  '" + UserID + "'";
                } else
                {
                    query = query + " WHERE tblCoupons.customer_id =  '" + UserID + "'";
                }
            }
            Console.WriteLine(query);
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetCouponByID(int CouponID)
        {
            DataTable dt = new DataTable();
        string query = "SELECT tblCoupons.coupon_id, tblCoupons.code_title, tblCoupons.customer_id, tblCoupons.company_id, tblCoupons.offer_type_id, tblCoupons.code, " +
                "tblCoupons.code_desc, tblCoupons.expiration_date, CASE WHEN tblCoupons.status = 1 THEN 'Active' WHEN tblCoupons.status = 2 THEN 'Not Active' WHEN tblCoupons.status = 3 THEN 'Waiting For Approval' ELSE 'Expired' END AS status_name, " +
                "CASE WHEN tblCoupons.is_approved = 1 THEN 'Approved' ELSE 'Not Approved' END AS approved_name, " +
                "tblCoupons.on_promotion, tblCoupons.is_approved, tblCoupons.approved_date, " +
                "tblCompany.website as company_website," +
                "tblCoupons.created_date, tblCoupons.status " +
                "FROM tblCoupons LEFT JOIN tblCompany ON tblCompany.company_id = tblCoupons.company_id " +
                "WHERE tblCoupons.coupon_id = @coupon_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@coupon_id", CouponID);
            dap.Fill(dt);
            return dt;
        }
        public int DeleteCouponByID(int CouponID)
        {
            int Result = 0;
            string query = "UPDATE tblCoupons SET status = 4 WHERE coupon_id = @coupon_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@coupon_id", CouponID);
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
        public DataTable GetTodaysCoupons()
        {
            DataTable dt = new DataTable();
            string query = "SELECT TOP 20 tblCoupons.*,tblCompany.*, tblOfferTypes.name as tbl_offer_type_name  FROM  tblCoupons " +
                "INNER JOIN tblCompany ON tblCoupons.company_id = tblCompany.company_id " +
                "INNER JOIN tblOfferTypes ON tblOfferTypes.offer_type_id = tblCoupons.offer_type_id " +
                "WHERE on_promotion = 0 AND tblCoupons.status = 1 ORDER BY NEWID()";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable Search(int cat_id, string keyword)
        {
            DataTable dt = new DataTable();
            string query = "SELECT tblCoupons.*,tblCompany.*, tblOfferTypes.name as tbl_offer_type_name, " +
                "tblUsers.first_name as username, " +
                "tblCategories.category_id as cat_id  FROM  tblCoupons " +
                "INNER JOIN tblCompany ON tblCoupons.company_id = tblCompany.company_id " +
                "INNER JOIN tblOfferTypes ON tblOfferTypes.offer_type_id = tblCoupons.offer_type_id " +
                "INNER JOIN tblCategories ON tblCategories.category_id = tblCompany.category_id " +
                "INNER JOIN tblUsers ON tblUsers.user_id = tblCoupons.customer_id " +
                "WHERE (tblCompany.name LIKE @keyword OR tblCoupons.code_title LIKE @keyword)";
            if(cat_id != 0)
            {
                query = query + " AND tblCategories.category_id = @cat_id";
            }
                                                                                                                        
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            if (cat_id != 0)
            {
                dap.SelectCommand.Parameters.AddWithValue("@cat_id", cat_id);
            }
            dap.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetCategories()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM  tblCategories WHERE status = 1";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetPromotions()
        {
            DataTable dt = new DataTable();
            string query = "SELECT TOP 4 tblCoupons.*,tblCompany.*, tblOfferTypes.name as " +
                "tbl_offer_type_name  FROM  tblCoupons " +
                "INNER JOIN tblCompany ON tblCoupons.company_id = tblCompany.company_id " +
                "INNER JOIN tblOfferTypes ON tblOfferTypes.offer_type_id = tblCoupons.offer_type_id " +
                "WHERE tblCoupons.on_promotion = 1 AND tblCoupons.status = 1 " +
                "ORDER BY NEWID()";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetFavouriteCoupons(int UserID)
        {
            DataTable dt = new DataTable();
            string query = "SELECT tblCoupons.*,tblCompany.*, tblOfferTypes.name as tbl_offer_type_name  FROM  tblUserFavouriteCoupons " +
                "LEFT JOIN tblCoupons ON tblUserFavouriteCoupons.coupon_id = tblCoupons.coupon_id " +
                "INNER JOIN tblCompany ON tblCoupons.company_id = tblCompany.company_id " +
                "INNER JOIN tblOfferTypes ON tblOfferTypes.offer_type_id = tblCoupons.offer_type_id " +
                "WHERE tblUserFavouriteCoupons.user_id = @user_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
            dap.Fill(dt);
            return dt;
        }
        public DataTable GetCart(int UserID)
        {
            DataTable dt = new DataTable();
            string query = "SELECT tblCarts.*,tblCompany.*, tblCoupons.* " +
                "FROM tblCarts " +
                "INNER JOIN tblCoupons ON tblCoupons.coupon_id = tblCarts.coupon_id " +
                "INNER JOIN tblCompany ON tblCompany.company_id = tblCoupons.company_id " +
                "INNER JOIN tblUsers ON tblUsers.user_id = tblCoupons.customer_id " +
                "WHERE tblCarts.user_id = @user_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
            dap.Fill(dt);
            return dt;
        }
        public int RemoveCart(int CartID)
        {
            int Result = 0;
            string query = "DELETE FROM tblCarts WHERE cart_id = @cart_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@cart_id", CartID);
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
        public int UpdateCartID(int CartID, int Quantity)
        {
            int Result = 0;
            string query = "UPDATE tblCarts SET cart_count = @cart_count WHERE cart_id = @cart_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@cart_id", CartID);
            cmd.Parameters.AddWithValue("@cart_count", Quantity);
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
        public DataTable GetCheckout()
        {
            HttpContext httpContext = HttpContext.Current;
            int UserID = 0;
            try
            {
                UserID = (int)httpContext.Session["UserID"];
            }
            catch (Exception ex) { }
            DataTable dt = new DataTable();
            string query = "SELECT tblCarts.*,tblCompany.*, tblCoupons.* " +
                "FROM tblCarts " +
                "INNER JOIN tblCoupons ON tblCoupons.coupon_id = tblCarts.coupon_id " +
                "INNER JOIN tblCompany ON tblCompany.company_id = tblCoupons.company_id " +
                "INNER JOIN tblUsers ON tblUsers.user_id = tblCoupons.customer_id " +
                "WHERE tblCarts.user_id = @user_id";
            SqlDataAdapter dap = new SqlDataAdapter(query, _sqlConnection);
            dap.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
            dap.Fill(dt);
            return dt;
        }
        public int CartDeleteAll()
        {
            HttpContext httpContext = HttpContext.Current;
            int UserID = 0;
            try
            {
                UserID = (int)httpContext.Session["UserID"];
            }
            catch (Exception ex) { }
            int Result = 0;
            string query = "DELETE FROM tblCarts WHERE user_id = @user_id";
            SqlCommand cmd = new SqlCommand(query, _sqlConnection);
            cmd.Parameters.AddWithValue("@user_id", UserID);
            try
            {
                Result = (int)cmd.ExecuteNonQuery();
                httpContext.Session["Cart"] = 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Result;
        }
    }

}