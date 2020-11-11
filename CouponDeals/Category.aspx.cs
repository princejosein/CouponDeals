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
    public partial class Category : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CategoryEntity _categoryEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    lblCategory.Text = "Edit Category";
                    hidFldID.Value = Request.QueryString["id"].Trim();
                    btnSet.Visible = true;
                    statusDiv.Visible = true;
                    LoadCategory();
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
        private void setControlMode(bool ReadOnly = true)
        {
            if(ReadOnly == true)
            {
                uploadImg.Attributes.Add("disabled", "disabled");
                SaveBtn.Visible = false;
                EditBtn.Visible = true;
                txtStatus.Visible = true;
                ddlStatus.Visible = false;
            } else
            {
                uploadImg.Attributes.Remove("disabled");
                SaveBtn.Visible = true;
                EditBtn.Visible = false;
                txtStatus.Visible = false;
                ddlStatus.Visible = true;
            }
            txtCategoryName.ReadOnly = ReadOnly;
        }
        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            int InsertResult = 0;
            int CatID = int.Parse(hidFldID.Value);
            string dbImagePath = ImgExists.Value.ToString();
            if ((uploadImg.PostedFile != null) && (uploadImg.PostedFile.ContentLength > 0))
            {
                string fname = Path.GetFileName(uploadImg.PostedFile.FileName);
                string fileName = Guid.NewGuid().ToString();
                var extention = Path.GetExtension(uploadImg.PostedFile.FileName);
                string SaveLocation = Server.MapPath("Images/Category") + "\\" + fileName + extention;
                try
                {
                    uploadImg.PostedFile.SaveAs(SaveLocation);
                    dbImagePath = "/Images/Category/" + fileName + extention;
                }
                catch (Exception ex)
                {
                    Console.Write("Error: " + ex.Message);
                }
            }
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _categoryEntity = new CategoryEntity(_dataAccess.sqlConnection);
                InsertResult = _categoryEntity.CreateOrUpdateCategory(
                    txtCategoryName.Text.Trim(),
                    dbImagePath,
                    int.Parse(ddlStatus.SelectedValue.Trim()),
                    CatID
                    );
                _dataAccess.Close();
                if (InsertResult != 0)
                {
                    (this.Master as Site).scripToaster(true, "Category Information Updated Successfully.", "Success");
                }
                else
                {
                    (this.Master as Site).scripToaster(false, "Category Information Updated Error. Try After Some Time!", "Error");
                }
            }
            setControlMode(true);
        }
        private void LoadCategory()
        {
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _categoryEntity = new CategoryEntity(_dataAccess.sqlConnection);
                DataTable dt = _categoryEntity.GetCategoryByID(int.Parse(hidFldID.Value));
                if (dt.Rows.Count > 0)
                {
                    txtCategoryName.Text = dt.Rows[0]["name"].ToString().Trim();
                    txtCategoryID.Text = dt.Rows[0]["category_id"].ToString().Trim();
                    updateImg(dt.Rows[0]["cat_image"].ToString().Trim());
                    txtStatus.Text = dt.Rows[0]["status_name"].ToString().Trim();
                    ddlStatus.SelectedValue = dt.Rows[0]["status"].ToString().Trim();
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

        protected void EditBtn_Click(object sender, EventArgs e)
        {
            setControlMode(false);
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            if ((this.Master as Site).ConnectToDB())
            {
                int deleteCategory = 0;
                _dataAccess = (this.Master as Site).dataAccess;
                _categoryEntity = new CategoryEntity(_dataAccess.sqlConnection);
                deleteCategory = _categoryEntity.DeleteCategoryByID(int.Parse(hidFldID.Value));
                if (deleteCategory == 0)
                {
                    (this.Master as Site).scripToaster(false, "Category Deleting Error. Try After Some Time!", "Error");
                }
                else
                {
                    (this.Master as Site).scripToaster(true, "Category Deleted Successfully.", "Success");
                    EditBtn.Visible = false;
                    DeleteBtn.Visible = false;
                }
                _dataAccess.Close();
            }
        }
    }
}