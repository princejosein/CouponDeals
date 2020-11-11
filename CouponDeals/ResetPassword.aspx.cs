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
    public partial class ResetPassword : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        UserEntity _userEntity;
        int UserID = 0;
        private string resetLink = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (Request.QueryString["reset"] != null)
            {
                resetLink = Request.QueryString["reset"];
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                if (UserID != 0)
                {
                    throw new HttpException(403, "Not Authorized");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected void ResetBtn_Click(object sender, EventArgs e)
        {
            bool ResetResult = false;
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _userEntity = new UserEntity(_dataAccess.sqlConnection);
                ResetResult = _userEntity.ResetPassword(
                        txtPassword.Text.Trim(),
                        resetLink
                    );
                _dataAccess.Close();
                if (ResetResult == true)
                {
                    (this.Master as Site).scripToaster(true, "Password Updated. Please login", "Success");
                    ResetBtn.Visible = false;
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "Password Update Error. Try After Some Time!", "Error");
                }
            }
        }
    }
}