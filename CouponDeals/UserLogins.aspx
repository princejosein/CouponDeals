<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserLogins.aspx.cs" Inherits="CouponDeals.UserLogins" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>Most Login Users List</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
    <div class="row form-group"> 
        
        <div class="col-md-2">
             <label for="txtFromDate">From Date:</label> &nbsp;
            <div class="input-group date ui-datepicker" >
                <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server"  ID="txtFromDate"  ClientIDMode="Static" ></asp:TextBox>
            </div>             
         </div>
        
        <div class="col-md-2">
             <label for="txtToDate">To Date:</label> &nbsp;
            <div class="input-group date ui-datepicker" >
                <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server"  ID="txtToDate"  ClientIDMode="Static" ></asp:TextBox>
            </div>             
         </div>

         <div class="col-md-8">
              <label for="pwd">Name:</label> 
             <div class="row justify-content-start">
                     <div class="col-md-9">              
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

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="table-responsive bg-light">
                <asp:GridView ID="GridUserLogins" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="user_id" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"   AllowSorting="true" AllowPaging="true" PageSize="10"
                            EmptyDataText="--No Record(s) Found--" CssClass="table table-hover table-bordered" HeaderStyle-HorizontalAlign="Center" 
                    >
                    <Columns>
                        <asp:HyperLinkField HeaderText="USER ID" DataNavigateUrlFormatString="User.aspx?id={0}" DataNavigateUrlFields="user_id"   DataTextField="user_id" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="email" HeaderText="EMAIL" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="first_name" HeaderText="FIRST NAME" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="count" HeaderText="NUMBER OF LOGINS" ItemStyle-HorizontalAlign="Center"  />
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
