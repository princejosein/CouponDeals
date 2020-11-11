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
    public partial class CategoryList : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CategoryEntity _categoryEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {
                LoadCategories();
            }
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
                if (UserID == 0 || UserRoleID != 1)
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
        protected void GridCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridCategory.PageIndex = e.NewPageIndex;
            LoadCategories();
        }
        private void LoadCategories()
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _categoryEntity = new CategoryEntity(_dataAccess.sqlConnection);
                dt = _categoryEntity.GetCategoryList();
            }
            GridCategory.DataSource = dt;
            GridCategory.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
        }
    }
}