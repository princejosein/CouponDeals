using CouponDeals.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CouponDeals
{
    public partial class Coupon : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CouponEntity _couponEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {                
                LoadOfferTypes();
                if (Request.QueryString["id"] != null)
                {
                    lblCoupon.Text = "Edit Coupon";
                    hidFldID.Value = Request.QueryString["id"].Trim();
                    btnSet.Visible = true;
                    statusDiv.Visible = true;
                    LoadCoupon();
                    setControlMode(true);
                }
                else
                {
                    statusDiv.Visible = false;
                    //ApprovalDiv.Visible = false;
                    PromotionDiv.Visible = false;
                }
            } else
            {
                if (!txtExpDate.ReadOnly)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "enableDatepicker('#txtExpDate');", true);
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "enableDatepicker('#txtExpDate');", true);
        }
        private void Verify()
        {
            try
            {
                UserID = (int)Session["UserID"];
                UserRoleID = (int)Session["UserRoleID"];
                if (UserID == 0)
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
        [WebMethod]
        public static List<string> GetCompanyWebsite(string companyWebsite)
        {
            List<string> compResult = new List<string>();
            string conString = ConfigurationManager.ConnectionStrings["CouponDeals.Properties.Settings.dbCouponDealsConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select Top 10 website from tblCompany " +
                        "where website LIKE '%'+@SearchCompName+'%'";
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@SearchCompName", companyWebsite);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        compResult.Add(dr["website"].ToString());
                    }
                    conn.Close();
                    return compResult;
                }
            }
        }
        private void LoadOfferTypes()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                DataTable dt = _couponEntity.GetOfferTypes();
                if (dt != null)
                {
                    ddlOfferType.DataSource = dt;
                    ddlOfferType.DataValueField = "offer_type_id";
                    ddlOfferType.DataTextField = "name";
                    ddlOfferType.DataBind();
                }
                _dataAccess.Close();
            }

        }
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            int InsertResult = 0;
            int CouponID = int.Parse(hidFldID.Value);
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                InsertResult = _couponEntity.CreateOrUpdateCoupon(
                    txtCompany.Text.Trim(),
                    int.Parse(ddlOfferType.SelectedValue.Trim()),
                    txtCode.Text.Trim(),
                    txtExpDate.Text.Trim(),
                    txtAreaDesc.InnerText.Trim(),
                    CheckboxPromotion.Checked == true ? 1 : 0,
                    int.Parse(ddlStatus.SelectedValue.Trim()),
                    txtCodeTitle.Text.Trim(),
                    //int.Parse(ddlApproval.SelectedValue.Trim()),
                    UserID,
                    CouponID
                    );
                _dataAccess.Close();
                if (InsertResult != 0)
                {
                    (this.Master as Site).scripToaster(true, "Coupon Information Updated Successfully.", "Success");
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "Coupon Information Updated Error. Try After Some Time!", "Error");
                }
            }
            setControlMode(true);
        }
        private void LoadCoupon()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                DataTable dt = _couponEntity.GetCouponByID(int.Parse(hidFldID.Value));
                if (dt.Rows.Count > 0)
                {
                    txtCouponID.Text = dt.Rows[0]["coupon_id"].ToString().Trim();
                    txtCompany.Text = dt.Rows[0]["company_website"].ToString().Trim();
                    ddlOfferType.SelectedValue = dt.Rows[0]["offer_type_id"].ToString().Trim();
                    txtStatus.Text = dt.Rows[0]["status_name"].ToString().Trim();
                    ddlStatus.SelectedValue = dt.Rows[0]["status"].ToString().Trim();
                    txtCode.Text = dt.Rows[0]["code"].ToString().Trim();
                    txtAreaDesc.InnerText = dt.Rows[0]["code_desc"].ToString().Trim();
                    txtExpDate.Text = dt.Rows[0]["expiration_date"].ToString().Trim();
                    //txtApproval.Text = dt.Rows[0]["approved_name"].ToString().Trim();
                    txtCodeTitle.Text = dt.Rows[0]["code_title"].ToString().Trim();
                    CheckboxPromotion.Checked = int.Parse(dt.Rows[0]["on_promotion"].ToString().Trim()) == 1?true:false;
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
            //txtEmail.ReadOnly = true;
            //txtApproval.ReadOnly = ReadOnly;
        }
        protected void EditBtn_Click(object sender, EventArgs e)
        {
            setControlMode(false);
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            if ((this.Master as Site).ConnectToDB())
            {
                int deleteCoupon = 0;
                _dataAccess = (this.Master as Site).dataAccess;
                _couponEntity = new CouponEntity(_dataAccess.sqlConnection);
                deleteCoupon = _couponEntity.DeleteCouponByID(int.Parse(hidFldID.Value));
                if (deleteCoupon == 0)
                {
                    (this.Master as Site).scripToaster(false, "Coupon Deleting Error. Try After Some Time!", "Error");
                }
                else
                {
                    (this.Master as Site).scripToaster(true, "Coupon Deleted Successfully.", "Success");
                    EditBtn.Visible = false;
                    DeleteBtn.Visible = false;
                }
                _dataAccess.Close();
            }
        }
    }
}