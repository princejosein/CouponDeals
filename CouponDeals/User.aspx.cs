using CouponDeals.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouponDeals
{
    public partial class User : System.Web.UI.Page
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
                LoadRoles();
                if (Request.QueryString["id"] != null)
                {
                    lblUser.Text = "Edit User";
                    hidFldID.Value = Request.QueryString["id"].Trim();
                    btnSet.Visible = true;
                    statusDiv.Visible = true;
                    LoadUser();
                    setControlMode(true);
                    ddlUserRoles.Visible = false;
                    txtUserRole.Visible = true;
                }
                else
                {
                    statusDiv.Visible = false;
                }
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
        private void LoadRoles()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                DataTable dt = _userEntity.GetUserRoles();
                if (dt != null)
                {
                    ddlUserRoles.DataSource = dt;
                    ddlUserRoles.DataValueField = "user_role_id";
                    ddlUserRoles.DataTextField = "name";
                    ddlUserRoles.DataBind();
                }
                _dataAccess.Close();
            }

        }
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            int InsertResult = 0;
            int UserID = int.Parse(hidFldID.Value);
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                InsertResult = _userEntity.CreateOrUpdateUser(
                    txtEmail.Text.Trim(),
                    txtFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    int.Parse(ddlUserRoles.SelectedValue.Trim()),
                    int.Parse(ddlStatus.SelectedValue.Trim()),
                    DateTime.Today,
                    txtPassword.Text.Trim(),
                    UserID
                    );
                _dataAccess.Close();
                if (InsertResult != 0)
                {
                    (this.Master as Site).scripToaster(true, "User Information Updated Successfully.", "Success");
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "User Information Updated Error. Try After Some Time!", "Error");
                }
            }
            setControlMode(true);
        }
        private void LoadUser()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                DataTable dt = _userEntity.GetUserByID(int.Parse(hidFldID.Value));
                if (dt.Rows.Count > 0)
                {
                    txtUserID.Text = dt.Rows[0]["user_id"].ToString().Trim();
                    txtEmail.Text = dt.Rows[0]["email"].ToString().Trim();
                    txtFirstName.Text = dt.Rows[0]["first_name"].ToString().Trim();
                    txtLastName.Text = dt.Rows[0]["last_name"].ToString().Trim();
                    txtStatus.Text = dt.Rows[0]["status_name"].ToString().Trim();
                    ddlStatus.SelectedValue = dt.Rows[0]["status"].ToString().Trim();
                    txtUserRole.Text = dt.Rows[0]["user_role"].ToString().Trim();
                }
                _dataAccess.Close();
            }
        }
        
        private void setControlMode(bool ReadOnly = true)
        {
            if (ReadOnly == true)
            {
                SaveBtn.Visible = false;
                EditBtn.Visible = true;
                txtStatus.Visible = true;
                ddlStatus.Visible = false;
            }
            else
            {
                SaveBtn.Visible = true;
                EditBtn.Visible = false;
                txtStatus.Visible = false;
                ddlStatus.Visible = true;
            }
            txtEmail.ReadOnly = true;
            txtFirstName.ReadOnly = ReadOnly;
            txtLastName.ReadOnly = ReadOnly;
        }
        protected void EditBtn_Click(object sender, EventArgs e)
        {
            setControlMode(false);
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            if ((this.Master as Site).ConnectToDB())
            {
                int deleteUser = 0;
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                deleteUser = _userEntity.DeleteUserByID(int.Parse(hidFldID.Value));
                if (deleteUser == 0)
                {
                    (this.Master as Site).scripToaster(false, "User Deleting Error. Try After Some Time!", "Error");
                }
                else
                {
                    (this.Master as Site).scripToaster(true, "User Deleted Successfully.", "Success");
                    EditBtn.Visible = false;
                    DeleteBtn.Visible = false;
                }
                _dataAccess.Close();
            }
        }
    }
}