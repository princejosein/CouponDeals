<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="CouponDeals.Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
   
<%--Search-Area--%>
        <div class="col-md">
            <asp:TextBox ID="txtSearch" placeholder="Search in Coupon Deals" runat="server" CssClass="form-control p-4 m-2"></asp:TextBox>
		</div>
		<div class="col-md">
            <asp:DropDownList ID="ddlCategories" class="form-control m-2 form-dropdown" runat="server">
			</asp:DropDownList>
		</div>
		<div class="col-md">
            <asp:Button ID="SearchBtn" runat="server" Text="Search" CssClass="btn btn-main m-2" OnClick="SearchBtn_Click"  />
		</div>
<%--Search-Area-End--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="table-responsive bg-light">
                <asp:GridView ID="GridSearch" runat="server" BackColor="White" OnRowDataBound="GridSearch_RowDataBound" 
                    RowStyle-VerticalAlign="Middle" 
                    AutoGenerateColumns="false" ShowHeader="false" GridLines="None" OnPageIndexChanging="GridSearch_PageIndexChanging"
                    DataKeyNames="category_id" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"   AllowSorting="true" AllowPaging="true" PageSize="2"
                            EmptyDataText="--No Record(s) Found--" CssClass="table table-hover table-borderless" HeaderStyle-HorizontalAlign="Center" 
                            
                    >
                    <Columns>
                        <asp:ImageField DataAlternateTextField="name" ControlStyle-Width="75" ItemStyle-HorizontalAlign="Center" DataImageUrlField="profile_image" ></asp:ImageField>             
                        <asp:BoundField ItemStyle-VerticalAlign="Middle"  DataField="code_title" HeaderText="CODE TITLE"  />       
                        <asp:BoundField ItemStyle-VerticalAlign="Middle"  DataField="username" HeaderText="USERNAME"   />    
                        <asp:ButtonField ItemStyle-CssClass="btn-coupon" DataTextField="code"  />
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
