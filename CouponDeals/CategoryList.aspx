<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoryList.aspx.cs" Inherits="CouponDeals.CategoryList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>Category List</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group row">
        <div class="col-md-12 ">
            <asp:HyperLink ID="BackLink" runat="server" CssClass="btn side-btn" NavigateUrl="~/Category.aspx" >Create New Category</asp:HyperLink>
        </div> 
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="table-responsive bg-light">
                <asp:GridView ID="GridCategory" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="category_id" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"   AllowSorting="true" AllowPaging="true" PageSize="5"
                            EmptyDataText="--No Record(s) Found--" CssClass="table table-hover table-bordered" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="GridCategories_PageIndexChanging"
                            
                    >
                    <Columns>
                        <asp:HyperLinkField HeaderText="CAT ID" DataNavigateUrlFormatString="Category.aspx?id={0}" DataNavigateUrlFields="category_id"   DataTextField="category_id" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="name" HeaderText="CATEGORY NAME" ItemStyle-HorizontalAlign="Center"  />
                        <asp:ImageField DataAlternateTextField="name" ControlStyle-Width="100" ItemStyle-HorizontalAlign="Center" DataImageUrlField="cat_image" ></asp:ImageField>
                        <asp:BoundField DataField="status_name" HeaderText="STATUS" ItemStyle-HorizontalAlign="Center"  />
                    </Columns>
                    <EmptyDataRowStyle  />
                </asp:GridView>
            </div>
            <div style="color:blue;margin-left:15px;text-align:left;font-weight:bold;">
                    <label class="control-label" runat="server" id="lblRowCnt">0 Row(s)</label>  
                </div>
        </ContentTemplate>       
        
    </asp:UpdatePanel>
</asp:Content>
