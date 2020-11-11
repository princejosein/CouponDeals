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
    public partial class Register : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        UserEntity _userEntity;
        int UserID = 0;
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
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            int InsertResult = 0;
            int UserID = 0;
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                InsertResult = _userEntity.CreateOrUpdateUser(
                    txtEmail.Text.Trim(),
                    txtFirstName.Text.Trim(),
                    txtLastName.Text.Trim(),
                    2,
                    2,
                    DateTime.Today,
                    txtPassword.Text.Trim(),
                    UserID
                    );
                _dataAccess.Close();
                if (InsertResult != 0)
                {
                    (this.Master as Site).scripToaster(true, "User Registered Successfully.", "Success");
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "User Registration Error. Try After Some Time!", "Error");
                }
            }
        }
    }
}