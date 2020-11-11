<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CouponList.aspx.cs" Inherits="CouponDeals.CouponList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>Coupons List</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group row">
        <div class="col-md-12 ">
            <asp:HyperLink ID="BackLink" runat="server" CssClass="btn side-btn" NavigateUrl="~/Coupon.aspx" >Create New Coupon</asp:HyperLink>
        </div> 
    </div>
    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
    <div class="row form-group">          
         <div class="col-md-2">
             <label for="ddlStatusFilter">Status:</label> 
            <asp:DropDownList runat="server" ID="ddlStatusFilter" CssClass="form-control">
                               <asp:ListItem>All</asp:ListItem>
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Not Active</asp:ListItem>
                                <asp:ListItem>Waiting for Approval</asp:ListItem>
                                <asp:ListItem>Expired</asp:ListItem>
                           </asp:DropDownList>
         </div>
        <div class="col-md-2">
             <label for="ddlCategories">Category:</label> 
            <asp:DropDownList runat="server" ID="ddlCategories" CssClass="form-control" ></asp:DropDownList>
         </div>
        <div class="col-md-2">
             <label for="txtDate">Created Date:</label> &nbsp;
            <div class="input-group date ui-datepicker" >
                <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server"  ID="txtDate"  ClientIDMode="Static" ></asp:TextBox>
            </div>             
         </div>
         <div class="col-md-6">
              <label for="pwd">Search By:</label> 
             <div class="row justify-content-start">
                 <div class="col-ml-auto">                   
             <asp:DropDownList runat="server" ID="ddlSearchBy" AutoPostBack="true" CssClass="form-control" >
                 <asp:ListItem>Keyword</asp:ListItem>     
                 <asp:ListItem>Company</asp:ListItem>                         
            </asp:DropDownList>
                     </div>
                 <div class="col-md-6">
              
              <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server"  ID="txtNameFilter"></asp:TextBox>
                            </div>
                 <div class="col-ml-auto align-content-center">
             <button id="btnSearch" class="search-btn" type="submit" onserverclick="btnSearch_ServerClick"  runat="server" ><i class="fa fa-search" ></i></button>
                     </div>
                 
          </div>            
            <br />
     </div>
         </div>
                </ContentTemplate>
                </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="table-responsive bg-light">
                <asp:GridView ID="GridCoupon" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="coupon_id" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"   AllowSorting="true" AllowPaging="true" PageSize="10"
                            EmptyDataText="--No Record(s) Found--" CssClass="table table-hover table-bordered" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="GridCoupon_PageIndexChanging"
                            
                    >
                    <Columns>
                        <asp:HyperLinkField HeaderText="COUPON ID" DataNavigateUrlFormatString="Coupon.aspx?id={0}" DataNavigateUrlFields="coupon_id"   DataTextField="coupon_id" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="company_name" HeaderText="COMPANY" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="offer_type" HeaderText="OFFER TYPE" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="cat_name" HeaderText="CATEGORY" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="expiration_date_formatted" HeaderText="EXPIRATION DATE" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="is_approved_formatted" HeaderText="APPROVED" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="status_name" HeaderText="STATUS" ItemStyle-HorizontalAlign="Center"  />
                    </Columns>
                    <EmptyDataRowStyle  />
                </asp:GridView>
            </div>
            <div style="color:blue;margin-left:15px;text-align:left;font-weight:bold;">
                    <label class="control-label" runat="server" id="lblRowCnt">0 Row(s)</label>  
                </div>
        </ContentTemplate>       
        <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="ServerClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
