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
    public partial class Company : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CompanyEntity _companyEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {
                LoadCategories();
                if (Request.QueryString["id"] != null)
                {
                    lblCompany.Text = "Edit Company";
                    hidFldID.Value = Request.QueryString["id"].Trim();
                    btnSet.Visible = true;
                    statusDiv.Visible = true;
                    LoadCompany();
                    setControlMode(true);
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
        private void LoadCategories()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                DataTable dt = _dataAccess.GetCategories();
                if (dt != null)
                {
                    ddlCategory.DataSource = dt;
                    ddlCategory.DataValueField = "category_id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();
                }
                _dataAccess.Close();
            }

        }
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            int InsertResult = 0;
            int CompID = int.Parse(hidFldID.Value);
            string dbImagePath = ImgExists.Value.ToString();
            if ((uploadImg.PostedFile != null) && (uploadImg.PostedFile.ContentLength > 0))
            {
                string fname = Path.GetFileName(uploadImg.PostedFile.FileName);
                string fileName = Guid.NewGuid().ToString();
                var extention = Path.GetExtension(uploadImg.PostedFile.FileName);
                string SaveLocation = Server.MapPath("Images/Company") + "\\" + fileName + extention;
                try
                {
                    uploadImg.PostedFile.SaveAs(SaveLocation);
                    dbImagePath = "/Images/Company/" + fileName + extention;
                }
                catch (Exception ex)
                {
                    Console.Write("Error: " + ex.Message);
                }
            }
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _companyEntity = new CompanyEntity(_dataAccess.sqlConnection);
                InsertResult = _companyEntity.CreateOrUpdateCompany(
                    txtCompanyName.Text.Trim(),
                    txtWebsite.Text.Trim(),
                    dbImagePath,
                    int.Parse(ddlCategory.SelectedValue.Trim()),
                    int.Parse(ddlStatus.SelectedValue.Trim()),
                    DateTime.Today,
                    CompID
                    );
                _dataAccess.Close();
                if (InsertResult != 0)
                {
                    (this.Master as Site).scripToaster(true, "Company Information Updated Successfully.", "Success");
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "Company Information Updated Error. Try After Some Time!", "Error");
                }
            }
            setControlMode(true);
        }
        private void LoadCompany()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _companyEntity = new CompanyEntity(_dataAccess.sqlConnection);
                DataTable dt = _companyEntity.GetCompanyByID(int.Parse(hidFldID.Value));
                if (dt.Rows.Count > 0)
                {
                    txtCompanyID.Text = dt.Rows[0]["company_id"].ToString().Trim();
                    txtCompanyName.Text = dt.Rows[0]["name"].ToString().Trim();
                    txtWebsite.Text = dt.Rows[0]["website"].ToString().Trim();
                    updateImg(dt.Rows[0]["profile_image"].ToString().Trim());
                    txtStatus.Text = dt.Rows[0]["status_name"].ToString().Trim();
                    ddlStatus.SelectedValue = dt.Rows[0]["status"].ToString().Trim();
                    ddlCategory.SelectedValue = dt.Rows[0]["category_id"].ToString().Trim();
                }
                _dataAccess.Close();
            }
        }
        private void updateImg(string Path)
        {
            if (Path != null)
            {
                ImgID.ImageUrl = Path;
                ImgExists.Value = Path;
            }
            else
            {
                ImgID.ImageUrl = "Images/placeholder_logo.png";
            }
        }
        private void setControlMode(bool ReadOnly = true)
        {
            if (ReadOnly == true)
            {
                uploadImg.Attributes.Add("disabled", "disabled");
                SaveBtn.Visible = false;
                EditBtn.Visible = true;
                txtStatus.Visible = true;
                ddlStatus.Visible = false;
            }
            else
            {
                uploadImg.Attributes.Remove("disabled");
                SaveBtn.Visible = true;
                EditBtn.Visible = false;
                txtStatus.Visible = false;
                ddlStatus.Visible = true;
            }
            txtCompanyName.ReadOnly = ReadOnly;
            txtWebsite.ReadOnly = ReadOnly;
            ddlCategory.Enabled = !ReadOnly;
        }
        protected void EditBtn_Click(object sender, EventArgs e)
        {
            setControlMode(false);
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            if ((this.Master as Site).ConnectToDB())
            {
                int deleteCompany = 0;
                _dataAccess = (this.Master as Site).dataAccess;
                _companyEntity = new CompanyEntity(_dataAccess.sqlConnection);
                deleteCompany = _companyEntity.DeleteCompanyByID(int.Parse(hidFldID.Value));
                if (deleteCompany == 0)
                {
                    (this.Master as Site).scripToaster(false, "Company Deleting Error. Try After Some Time!", "Error");
                }
                else
                {
                    (this.Master as Site).scripToaster(true, "Company Deleted Successfully.", "Success");
                    EditBtn.Visible = false;
                    DeleteBtn.Visible = false;
                }
                _dataAccess.Close();
            }
        }
    }
}