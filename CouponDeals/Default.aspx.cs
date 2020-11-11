using CouponDeals.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouponDeals
{
    public partial class Default : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CouponEntity _couponEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {
                LoadTodaysProduct();
                LoadPromotions();
                LoadCategories();
            }
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void LoadTodaysProduct()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                DataTable dt = _couponEntity.GetTodaysCoupons();
                if (dt != null)
                {
                    TopProducts.DataSource = dt;
                    TopProducts.DataBind();
                }
                _dataAccess.Close();
            }
        }
        private void LoadPromotions()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                DataTable dt = _couponEntity.GetPromotions();
                if (dt != null)
                {
                    FeaturedOffers.DataSource = dt;
                    FeaturedOffers.DataBind();
                }
                _dataAccess.Close();
            }
        }
        private void LoadCategories()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                DataTable dt = _couponEntity.GetCategories();
                if (dt != null)
                {
                     ddlCategories.DataSource = dt;
                    ddlCategories.DataTextField = "name";
                    ddlCategories.DataValueField = "category_id";
                    ddlCategories.DataBind();
                    ddlCategories.Items.Insert(0, new ListItem("Select Category", "0"));

                }
                _dataAccess.Close();
            }
        }
        [WebMethod]
        public static SortedList<string, string> ShowCoupon(string CouponID)
        {
            string conString = ConfigurationManager.ConnectionStrings["CouponDeals.Properties.Settings.dbCouponDealsConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "SELECT tblCoupons.coupon_id, tblCoupons.code_title, tblCoupons.code, tblCoupons.company_id, tblCoupons.offer_type_id, tblCoupons.code_desc, " +
                "tblCoupons.code_desc, tblCoupons.expiration_date, " +
                "tblCoupons.on_promotion," +
                "tblCompany.website as company_website, tblCompany.name as company_name, tblCompany.profile_image " +
                "FROM tblCoupons LEFT JOIN tblCompany ON tblCompany.company_id = tblCoupons.company_id " +
                "WHERE tblCoupons.coupon_id = @coupon_id";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@coupon_id", CouponID);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    SortedList<string, string> rowObj = new SortedList<string, string>();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        
                        rowObj.Add("CODE", row["code"].ToString());
                        rowObj.Add("CODE_TITLE", row["code_title"].ToString());
                        rowObj.Add("CODE_DESC", row["code_desc"].ToString());
                        rowObj.Add("CODE_IMG", row["profile_image"].ToString());
                        rowObj.Add("CODE_EXP", row["expiration_date"].ToString());
                        rowObj.Add("COMPANY_ID", row["company_id"].ToString());
                        rowObj.Add("COMPANY_NAME", row["company_name"].ToString());
                        rowObj.Add("COMPANY_WEBSITE", row["company_website"].ToString());

                    }
                    conn.Close();
                    return rowObj;
                }
            }
        }
        [WebMethod]
        public static SortedList<string, string> Favourites(string CouponID)
        {
            int UserID = 0;
            int UserRoleID = 0;
            HttpContext httpContext = HttpContext.Current;
            try
            {
                UserID = (int)httpContext.Session["UserID"];
                UserRoleID = (int)httpContext.Session["UserRoleID"];
            } catch(Exception ex)
            {
            }
            SortedList<string, string> rowObj = new SortedList<string, string>();
            //if no login
            if(UserID == 0)
            {
                //not login
                rowObj.Add("SUCCESS", "3");
                rowObj.Add("MESSAGE", "You Should Login First To Add Coupons into Favourites List!!");
                return rowObj;
            }
            if (UserRoleID == 1)
            {
                //admin user
                rowObj.Add("SUCCESS", "3");
                rowObj.Add("MESSAGE", "Admin User Cannot add coupon to favourites list");
                return rowObj;
            }
            string conString = ConfigurationManager.ConnectionStrings["CouponDeals.Properties.Settings.dbCouponDealsConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    string query = "SELECT favourite_id " +
                "FROM tblUserFavouriteCoupons " +
                "WHERE coupon_id = @coupon_id " +
                "AND user_id  = @user_id ";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@coupon_id", CouponID);
                    da.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
                    DataTable dt = new DataTable();
                    da.Fill(dt); 
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        int FavID = int.Parse(row["favourite_id"].ToString());
                        string deleteQuery = "DELETE FROM tblUserFavouriteCoupons WHERE " +
                            "favourite_id = @fav_id";
                        SqlCommand cmdDelete = new SqlCommand(deleteQuery, conn);
                        cmdDelete.Parameters.AddWithValue("@fav_id", FavID);
                        cmdDelete.Connection = conn;
                        int Result = 0;
                        try
                        {
                            Result = (int)cmdDelete.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            rowObj.Add("SUCCESS", "4");
                            rowObj.Add("MESSAGE", "Failed! Try Again Later");
                            conn.Close();
                            return rowObj;
                        }
                        rowObj.Add("SUCCESS", "2");
                        rowObj.Add("MESSAGE", "Item Removed from favourites list!!");
                        conn.Close();
                        return rowObj;
                    } else
                    {
                        string insertQuery = "INSERT INTO tblUserFavouriteCoupons (user_id, coupon_id) " +
                "VALUES(@user_id, @coupon_id);SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmdInsert = new SqlCommand(insertQuery, conn);
                        cmdInsert.Parameters.AddWithValue("@user_id", UserID);
                        cmdInsert.Parameters.AddWithValue("@coupon_id", CouponID);
                        cmdInsert.Connection = conn;
                        int InsertID = 0;
                        try
                        {
                            InsertID = (int)cmdInsert.ExecuteScalar();
                        }
                        catch (SqlException ex)
                        {
                            rowObj.Add("SUCCESS", "4");
                            rowObj.Add("MESSAGE", "Failed! Try Again Later");
                            conn.Close();
                            return rowObj;
                        }
                        rowObj.Add("SUCCESS", "1");
                        rowObj.Add("MESSAGE", "Item Added to favourites list!!");
                        conn.Close();
                        return rowObj;
                    }
                    return rowObj;
                }
            }
        }
        [WebMethod]
        public static SortedList<string, string> AddToCart(string CouponID)
        {
            int UserID = 0;
            int UserRoleID = 0;
            HttpContext httpContext = HttpContext.Current;
            SortedList<string, string> rowObjs = new SortedList<string, string>();
            try
            {
                UserID = (int)httpContext.Session["UserID"];
                UserRoleID = (int)httpContext.Session["UserRoleID"];
            }
            catch (Exception ex)
            {
            }
            if (UserID == 0)
            {
                //not login
                rowObjs.Add("SUCCESS", "0");
                rowObjs.Add("MESSAGE", "You Should Login First To Add Coupons into Cart!");
                return rowObjs;
            }
            if (UserRoleID == 1)
            {
                //admin user
                rowObjs.Add("SUCCESS", "0");
                rowObjs.Add("MESSAGE", "Admin User Cannot add coupon to Cart!");
                return rowObjs;
            }
            string Count = "0";
            int CartCount = 0;
            string conString = ConfigurationManager.ConnectionStrings["CouponDeals.Properties.Settings.dbCouponDealsConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    string query = "SELECT TOP 1 * FROM tblCarts WHERE user_id = @user_id AND coupon_id = @coupon_id ";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
                    da.SelectCommand.Parameters.AddWithValue("@coupon_id", CouponID);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        CartCount = int.Parse(row["cart_count"].ToString()) + 1;
                        int CartID = int.Parse(row["cart_id"].ToString());
                        string queryUpdate = "UPDATE tblCarts SET cart_count = @cart_count WHERE cart_id = @cart_id";
                        SqlCommand cmdUpdate = new SqlCommand(queryUpdate, conn);
                        cmdUpdate.Parameters.AddWithValue("@cart_count", CartCount);
                        cmdUpdate.Parameters.AddWithValue("@cart_id", CartID);
                        try
                        {
                            cmdUpdate.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            rowObjs.Add("SUCCESS", "0");
                            rowObjs.Add("MESSAGE", "Add to cart error!");
                            conn.Close();
                            return rowObjs;
                            Console.WriteLine(ex.Message);
                        }
                    } else
                    {
                        string insertQuery = "INSERT INTO tblCarts (user_id, coupon_id) VALUES(@user_id, @coupon_id);";
                        SqlCommand cmdInsert = new SqlCommand(insertQuery, conn);
                        cmdInsert.Parameters.AddWithValue("@user_id", UserID);
                        cmdInsert.Parameters.AddWithValue("@coupon_id", CouponID);
                        cmdInsert.Connection = conn;
                        try
                        {
                            cmdInsert.ExecuteScalar();
                        }
                        catch (SqlException ex)
                        {
                            rowObjs.Add("SUCCESS", "0");
                            rowObjs.Add("MESSAGE", "Add to cart error!");
                            conn.Close();
                            return rowObjs;
                        }
                    }
                    string querySelect = "SELECT COUNT(cart_id) as Count FROM tblCarts WHERE user_id = @user_id";
                    SqlDataAdapter dap = new SqlDataAdapter(querySelect, conn);
                    dap.SelectCommand.Parameters.AddWithValue("@user_id", UserID);
                    DataTable dt1 = new DataTable();
                    dap.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                    {
                        DataRow row = dt1.Rows[0];
                        Count = row["Count"].ToString();
                    }
                    rowObjs.Add("SUCCESS", "1");
                    rowObjs.Add("MESSAGE", "Coupon Added to Cart");
                    rowObjs.Add("Count", Count);
                    conn.Close();
                    return rowObjs;
                }
            }
        }
        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string cat_id = ddlCategories.SelectedValue.Trim();
            Response.Redirect("Search.aspx?keyword="+keyword+"&cat_id="+cat_id);
        }
    }
}