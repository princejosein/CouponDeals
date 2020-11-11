using CouponDeals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouponDeals
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public DataAccessLayer dataAccess = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            UpdateMenu();
            updateCart();
        }
        public bool ConnectToDB()
        {

            bool bDbOK = false;
            if (dataAccess == null)
            {
                dataAccess = new DataAccessLayer(CouponDeals.Properties.Settings.Default.dbCouponDealsConnectionString.Trim());
            }
            else
            {
                bDbOK = dataAccess.IsConnected;
            }
            if (!bDbOK)
            {
                bDbOK = dataAccess.Connect();
            }
            return bDbOK;
        }

        public void scripToaster(bool success, string main, string msg)
        {
            if (success == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>errorToastr('" + main + "','" + msg + "')</script>", false);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Success", "<script>successToastr('" + main + "','" + msg + "')</script>", false);
            }
        }
        private void UpdateMenu()
        {
            int UserRoleID = 0;
            try
            {
                UserRoleID = (int)Session["UserRoleID"];

                if (UserRoleID == 1)
                {
                    //Admin
                    RemoveMenu("Favourites");
                }
                if (UserRoleID == 2)
                {
                    //Employer
                    RemoveMenu("Categories");
                    RemoveMenu("Companies");
                    RemoveMenu("Users");
                    RemoveMenu("User Logins");                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Login
            if (UserRoleID == 0)
            {
                BtnLogOut.Visible = false;
                RemoveMenu("Categories");
                RemoveMenu("Companies");
                RemoveMenu("Users");
                RemoveMenu("Favourites");
                RemoveMenu("Coupons");
                RemoveMenu("User Logins");
            }
            else
            {
                RemoveMenu("Register");
                RemoveMenu("Login");
                BtnLogOut.Visible = true;
            }
        }

        protected void BtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Contents.RemoveAll();
            Response.Redirect("Default.aspx");
        }
        private void RemoveMenu(string name)
        {
            MenuItem item = MainMenu.FindItem(name);
            if (item != null)
            {
                MainMenu.Items.Remove(item);
            }
        }
        public void updateCart()
        {
            int CartCount = 0;
            try
            {
                CartCount = (int)Session["Cart"];
            } catch(Exception ex) { }
            CartCountID.InnerText = CartCount.ToString();
        }
    }
}