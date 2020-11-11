<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="CouponDeals.User" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function DeleteCategory() {
            swal({
                title: "Are you sure?",
                text: "This Action Cannot be Undone",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        return true;
                    } else {
                        return false;
                    }
                });
        }    
    }
    </script>
    <style type="text/css">
        .auto-style1 {
            left: 0px;
            top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblUser" runat="server" Text="Create User"></asp:Label>
    </h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-main">  
        <div class="form-group row" id="statusDiv" runat="server">
        <div class="col-md-2" runat="server">
            <label for="txtStatus">Status:</label>
                <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server" ReadOnly="true"  ID="txtStatus" Text="" ></asp:TextBox>
                <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control" Visible="false">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="2">Not Active</asp:ListItem> 
                </asp:DropDownList>
        </div>
        <div class="col-md-10" id="btnSet" style="text-align:right;" runat="server">
                <asp:Button ID="EditBtn" runat="server" Text="Edit User" CssClass="btn side-btn " OnClick="EditBtn_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button CssClass="btn side-btn " ID="DeleteBtn" runat="server" Text="Delete"
                              CausesValidation="false" OnClick="DeleteBtn_Click"  />&nbsp;
        </div>
        </div>

      <div class="row">
        <div class="col-md-12 text-danger ">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
     </div>     
     
        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtUserID">User ID:</label>
            <asp:TextBox ID="txtUserID" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
        </div> 
    </div>

     <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtEmail">Email:</label><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail" CssClass="error" ErrorMessage="Email Required">*</asp:RequiredFieldValidator>
&nbsp;<asp:TextBox ID="txtEmail" CssClass="form-control" TextMode="Email" runat="server"></asp:TextBox>
        </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
            <label for="txtPassword">Password:</label>
&nbsp;<asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
    </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
            <label for="txtConfirmPassword">Confirm Password:
            </label>
            
&nbsp;<asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
    </div> 
    </div>

    <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtFirstName">First Name:</label><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtFirstName" CssClass="error" ErrorMessage="First Name required">*</asp:RequiredFieldValidator>
&nbsp;<asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>
        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtLastName">Last Name:</label><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtLastName" CssClass="error" ErrorMessage="Last Name required">*</asp:RequiredFieldValidator>
&nbsp;<asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
    <label for="ddlUserRoles">User Roles:</label> 
    <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server" ReadOnly="true" Visible="false"  ID="txtUserRole" Text="" ></asp:TextBox>                
    <asp:DropDownList ID="ddlUserRoles" runat="server" CssClass="form-control"></asp:DropDownList>
    </div> 
    </div>

        <asp:Button ID="SaveBtn" runat="server" Text="Save" CssClass="btn btn-main" OnClick="SaveBtn_Click"  
              />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="BackLink" runat="server" CssClass="btn side-btn" NavigateUrl="~/UserList.aspx" >Back</asp:HyperLink>

    </div>
    <asp:HiddenField ID="hidFldID" Value="0" runat="server" />

</asp:Content>
