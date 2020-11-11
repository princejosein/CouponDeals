using CouponDeals.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouponDeals
{
    public partial class Checkout : System.Web.UI.Page
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
                LoadCheckout();
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
        public void LoadCheckout()
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                dt = _couponEntity.GetCheckout();
            }
            ReviewCart.DataSource = dt;
            ReviewCart.DataBind();
        }

        protected void BtnPlaceOrder_Click(object sender, EventArgs e)
        {
            string OrderCode = Guid.NewGuid().ToString();
            HttpContext httpContext = HttpContext.Current;
            httpContext.Session["orderCode"] = OrderCode;
            //Delete Cart
            int Result = 0;
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                Result = _couponEntity.CartDeleteAll();
                _dataAccess.Close();
            }
            if(Result == 0)
            {
                (this.Master as Site).scripToaster(false, "Placing Order Error. Try After Some Time!", "Error");
            } else
            {
                Response.Redirect("OrderPlaced.aspx?orderCode=" + OrderCode);
            }            
        }
    }
}