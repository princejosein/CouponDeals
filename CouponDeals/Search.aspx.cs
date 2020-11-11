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
    public partial class Search : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CouponEntity _couponEntity;
        private int cat_id = 0;
        private string keyword = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["cat_id"] != null)
                {
                    cat_id = int.Parse(Request.QueryString["cat_id"]);
                }
                if (Request.QueryString["keyword"] != null)
                {
                    keyword = Request.QueryString["keyword"].ToString();
                }
                SearchCoupon(cat_id,keyword);
                LoadDefault();
            }
                
        }
        private void LoadDefault()
        {
            txtSearch.Text = keyword;
            LoadCategories();
            if(cat_id != 0)
            {
                ddlCategories.Items.FindByValue(cat_id.ToString()).Selected = true;
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
        protected void GridSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridSearch.PageIndex = e.NewPageIndex;
            SearchCoupon(cat_id, keyword);
        }
        private void SearchCoupon(int cat_id = 0, string keyword = "")
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                dt = _couponEntity.Search(cat_id, keyword);
            }
            GridSearch.DataSource = dt;
            GridSearch.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
            _dataAccess.Close();
            _dataAccess = null;
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            string cat_id = ddlCategories.SelectedValue.Trim();
            Response.Redirect("Search.aspx?keyword=" + keyword + "&cat_id=" + cat_id);
        }

        protected void GridSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string usernameField = e.Row.Cells[2].Text.ToString();
                e.Row.Cells[2].Text = "Added by : " + usernameField;
            }
        }
    }
}