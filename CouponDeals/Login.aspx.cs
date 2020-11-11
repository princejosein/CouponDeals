using CouponDeals.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouponDeals
{
    public partial class Login : System.Web.UI.Page
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
            }
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
                if (UserID != 0 )
                {
                    Response.Redirect("Default.aspx");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected void Login_Click(object sender, EventArgs e)
        {
            bool LoginResult = false;
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                LoginResult = _userEntity.Login(
                        txtEmail.Text.Trim(),
                        txtPassword.Text.Trim()
                    );
                _dataAccess.Close();
                if (LoginResult == true)
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "Login Error. Email or Password wrong", "Error");
                }
            }
        }
    }
}