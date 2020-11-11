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
    public partial class CouponList : System.Web.UI.Page
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
                int filterStatus = ddlStatusFilter.SelectedIndex;
                LoadCategories(); 
                LoadCoupons(filterStatus);
            }
            else
            {
                if (!txtDate.ReadOnly)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "enableDatepicker('#txtDate');", true);
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "enableDatepicker('#txtDate');", true);
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
                if (UserID == 0 )
                {
                    //Response.StatusCode = 401;
                    //Response.End();
                    throw new HttpException(403,"Not Authorized");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new HttpException(403, "Not Authorized");
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
        protected void GridCoupon_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridCoupon.PageIndex = e.NewPageIndex;
            LoadCoupons();
        }
        private void LoadCoupons(int filterStatus = 0)
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                dt = _couponEntity.GetCouponsList(filterStatus);
            }
            GridCoupon.DataSource = dt;
            GridCoupon.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
        }

        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            refreshCoupons();
        }
        private void refreshCoupons()
        {
            DataTable dt;
            dt = filterCouponstList();
            GridCoupon.DataSource = dt;
            GridCoupon.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
        }
        private DataTable filterCouponstList()
        {
            DataTable dt = new DataTable();

            String filterKeyword = "";
            String filterCompany = "";
            string filterDate = "";

            filterDate = txtDate.Text.Trim();
            int filterStatus = ddlStatusFilter.SelectedIndex;
            int filterCategory = int.Parse(ddlCategories.SelectedValue.Trim());


            if (ddlSearchBy.SelectedValue.ToString().Trim().ToUpper() == "KEYWORD")
            {
                filterKeyword = txtNameFilter.Text.Trim();
            }
            else
            {
                filterCompany = txtNameFilter.Text.Trim();
            }
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                dt = _couponEntity.GetCouponsList(filterStatus, filterCategory, 
                    filterKeyword, filterCompany, filterDate);
            }
            return dt;
        }

    }
}