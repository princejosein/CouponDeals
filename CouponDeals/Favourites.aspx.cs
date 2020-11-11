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
    public partial class Favourites : System.Web.UI.Page
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
                LoadFavouriteProducts();
            }
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
                if (UserID == 0 || UserRoleID != 2)
                {
                    throw new HttpException(403, "Not Authorized");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpException(403, "Not Authorized");
            }
        }
        private void LoadFavouriteProducts()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                DataTable dt = _couponEntity.GetFavouriteCoupons(UserID);
                if (dt != null)
                {
                    FavProducts.DataSource = dt;
                    FavProducts.DataBind();
                }
                _dataAccess.Close();
            }
        }
        

    }
}