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
    public partial class OrderPlaced : System.Web.UI.Page
    {
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {
                CheckOrder();
            }
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
                if (UserID == 0 || UserRoleID != 2)
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
        private void CheckOrder()
        {
            string orderCode = Request.QueryString["orderCode"];
            bool Result = false;
            string Message = "Something Wrong in Order. Please connect to the admin by sending email at princejose@gmail.com";
            if (Request.QueryString["orderCode"] != null)
            {
                string sessionOrdercode = (string)Session["orderCode"];
                if (orderCode == sessionOrdercode)
                {
                    Result = true;
                }
            }
            if(Result == true)
            {
                Message = "Ordet Placed Successfully. Your Order Code is " + orderCode;
            }
            lblMessage.Text = Message;
        }
    }
}