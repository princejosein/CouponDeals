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
    public partial class UserList : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        UserEntity _userEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            LoadUsers();
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
        protected void GridUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridUser.PageIndex = e.NewPageIndex;
            LoadUsers();
        }
        private void LoadUsers()
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                dt = _userEntity.GetUsersList();
            }
            GridUser.DataSource = dt;
            GridUser.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
            _dataAccess.Close();
            _dataAccess = null;
        }
        protected void btnSearch_ServerClick(object sender, EventArgs e)
        {
            refreshUserList();
        }
        private void refreshUserList()
        {
            DataTable dt;
            dt = filterUsersList();
            GridUser.DataSource = dt;
            GridUser.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
        }
        private DataTable filterUsersList()
        {
            DataTable dt = new DataTable();

            String filterName = "";
            String filterEmail = "";

            int filterStatus = ddlStatusFilter.SelectedIndex;

            if (ddlSearchBy.SelectedValue.ToString().Trim().ToUpper() == "NAME")
            {
                filterName = txtNameFilter.Text.Trim();
            }
            else
            {
                filterEmail = txtNameFilter.Text.Trim();
            }
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                dt = _userEntity.GetUsersList(filterStatus, filterName,
                    filterEmail);
            }
            return dt;
        }
    }
}