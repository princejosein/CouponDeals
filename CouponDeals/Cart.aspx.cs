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
    public partial class Cart : System.Web.UI.Page
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
                LoadCart();
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
        private void LoadCart()
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                dt = _couponEntity.GetCart(UserID);
            }
            GridCart.DataSource = dt;
            GridCart.DataBind();
            //lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
            int Quantity = 0;
            if(dt.Rows.Count > 0)
            {
                foreach(DataRow dataRow in dt.Rows)
                {
                    Quantity += int.Parse(dataRow["cart_count"].ToString());
                }
            }
            lblPrice.Text = "NZD "+ (Quantity*1.5).ToString();
            lblQuantity.Text = Quantity.ToString();
            _dataAccess.Close();
            _dataAccess = null;
        }


        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void GridCart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int CartID = int.Parse(e.CommandArgument.ToString());
                int Result = 0;
                if ((this.Master as Site).ConnectToDB())
                {
                    _dataAccess = (this.Master as Site).dataAccess;
                    _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                    Result = _couponEntity.RemoveCart(CartID);
                    _dataAccess.Close();
                }
                if(Result == 0)
                {
                    (this.Master as Site).scripToaster(false, "Removing cart Error!!", "Error");
                } else
                {
                    Response.Redirect("cart.aspx");
                }                
            }
            if (e.CommandName == "Update")
            {
                GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                TextBox tb = row.FindControl("txtQuantity") as TextBox;
                int Quantity = int.Parse(tb.Text.ToString());
                int CartID = int.Parse(e.CommandArgument.ToString());
                int Result = 0;
                if ((this.Master as Site).ConnectToDB())
                {
                    _dataAccess = (this.Master as Site).dataAccess;
                    _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                    Result = _couponEntity.UpdateCartID(CartID, Quantity);
                    _dataAccess.Close();
                }
                if (Result == 0)
                {
                    (this.Master as Site).scripToaster(false, "Updating Quntity Error!!", "Error");
                }
                else
                {
                    Response.Redirect("cart.aspx");
                }
            }
        }
    }
}