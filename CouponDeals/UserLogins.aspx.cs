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
    public partial class UserLogins : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        UserEntity _userEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {
                LoadUserLogins();
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "enableDatepicker('#txtFromDate');enableDatepicker('#txtToDate');", true);
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
        private void LoadUserLogins()
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                dt = _userEntity.GetUserLogins();
            }
            GridUserLogins.DataSource = dt;
            GridUserLogins.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
            _dataAccess.Close();
            _dataAccess = null;
        }
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            refreshUserLoginList();
        }
        private void refreshUserLoginList()
        {
            DataTable dt;
            dt = filterUsersLoginList();
            GridUserLogins.DataSource = dt;
            GridUserLogins.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
        }
        private DataTable filterUsersLoginList()
        {
            DataTable dt = new DataTable();

            String filterName = txtNameFilter.Text.Trim();
            string filterFromDate = txtFromDate.Text.Trim();
            string filterToDate = txtToDate.Text.Trim();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                dt = _userEntity.GetUserLogins(filterName, filterFromDate, filterToDate);
            }
            return dt;
        }
    }
}