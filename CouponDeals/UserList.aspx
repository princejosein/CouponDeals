<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="CouponDeals.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>Users List</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group row">
        <div class="col-md-12 ">
            <asp:HyperLink ID="BackLink" runat="server" CssClass="btn side-btn" NavigateUrl="~/User.aspx" >Create New User</asp:HyperLink>
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
            </asp:DropDownList>
         </div>
         <div class="col-md-8">
              <label for="pwd">Search By:</label> 
             <div class="row justify-content-start">
                 <div class="col-ml-auto">                   
             <asp:DropDownList runat="server" ID="ddlSearchBy" AutoPostBack="true" CssClass="form-control" >
                 <asp:ListItem>Name</asp:ListItem>     
                 <asp:ListItem>Email</asp:ListItem>                         
            </asp:DropDownList>
                     </div>
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
                <asp:GridView ID="GridUser" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="user_id" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"   AllowSorting="true" AllowPaging="true" PageSize="10"
                            EmptyDataText="--No Record(s) Found--" CssClass="table table-hover table-bordered" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="GridUsers_PageIndexChanging"
                            
                    >
                    <Columns>
                        <asp:HyperLinkField HeaderText="USER ID" DataNavigateUrlFormatString="User.aspx?id={0}" DataNavigateUrlFields="user_id"   DataTextField="user_id" ItemStyle-HorizontalAlign="Center"/>
                        <asp:BoundField DataField="email" HeaderText="EMAIL" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="first_name" HeaderText="FIRST NAME" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="user_role" HeaderText="USER ROLE" ItemStyle-HorizontalAlign="Center"  />
                        <asp:BoundField DataField="date" HeaderText="CREATED DATE" ItemStyle-HorizontalAlign="Center"  />
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
