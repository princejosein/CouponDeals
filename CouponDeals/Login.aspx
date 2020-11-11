<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CouponDeals.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblUser" runat="server" Text="Login"></asp:Label>
    </h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-main">  
      <div class="row">
        <div class="col-md-12 text-danger ">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
     </div>     

     <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtEmail">Email:</label><asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="txtEmail" CssClass="error" ErrorMessage="Email is required">*</asp:RequiredFieldValidator>
&nbsp;<asp:TextBox ID="txtEmail" CssClass="form-control" TextMode="Email" runat="server"></asp:TextBox>
        </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
        <label for="txtPassword">Password:</label><asp:RequiredFieldValidator ID="PasswordValidator" runat="server" ControlToValidate="txtPassword" CssClass="error" ErrorMessage="Password is required">*</asp:RequiredFieldValidator>
&nbsp;<asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
    </div> 
    </div>
    

        <asp:Button ID="LoginBtn" runat="server" Text="Login" CssClass="btn btn-main" OnClick="Login_Click"  
              />
    <div class="link-line">Don’t have an account? <a href="Register.aspx">Register</a></div>
        <div class="link-line">Forget Password? <a href="Forgetpassword.aspx">Reset Password</a></div>
    
    </div>

</asp:Content>
