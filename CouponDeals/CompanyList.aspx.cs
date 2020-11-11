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
    public partial class CompanyList : System.Web.UI.Page
    {
        DataAccessLayer _dataAccess;
        CompanyEntity _companyEntity;
        int UserID = 0;
        int UserRoleID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Verify();
            LoadCompanies();
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
        protected void GridCompanies_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridCompany.PageIndex = e.NewPageIndex;
            LoadCompanies();
        }
        private void LoadCompanies()
        {
            DataTable dt = new DataTable();
            if ((this.Master as Site).ConnectToDB())
            {
                _dataAccess = (this.Master as Site).dataAccess;
                _companyEntity = new CompanyEntity(_dataAccess.sqlConnection);
                dt = _companyEntity.GetCompanyList();
            }
            GridCompany.DataSource = dt;
            GridCompany.DataBind();
            lblRowCnt.InnerText = dt.Rows.Count.ToString().Trim() + " Row(s)";
            _dataAccess.Close();
            _dataAccess = null;
        }
    }
}