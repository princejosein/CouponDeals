<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CouponDeals.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <style type="text/css">
        .auto-style1 {
            display: block;
            width: 100%;
            height: calc(1.5em + .75rem + 2px);
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: #495057;
            background-clip: padding-box;
            border-radius: .25rem;
            transition: none;
            border: 1px solid #ced4da;
            background-color: #fff;
        }
    </style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblUser" runat="server" Text="Register User Account"></asp:Label>
    </h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-main">  
      <div class="row">
        <div class="col-md-12 text-danger mb-3">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
     </div>     

     <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtEmail">Email:</label><asp:RequiredFieldValidator ID="EmailRequiredID" runat="server" ControlToValidate="txtEmail" CssClass="error" ErrorMessage="Email is required" ForeColor="Red">*</asp:RequiredFieldValidator>
&nbsp;<asp:RegularExpressionValidator ID="EmailValidation" runat="server" ControlToValidate="txtEmail" CssClass="error" ErrorMessage="Email  validation error" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
             <asp:TextBox ID="txtEmail" CssClass="form-control" TextMode="Email" runat="server"></asp:TextBox>
        </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-6 ">
        <label for="txtPassword">Password:</label><asp:RequiredFieldValidator ID="PasswordValidator" runat="server" ControlToValidate="txtPassword" CssClass="error" ErrorMessage="Password is required">*</asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="PasswordRuleValidator" runat="server" ControlToValidate="txtPassword" CssClass="error" ErrorMessage="Password must have 9 characters " ValidationExpression="^[a-zA-Z0-9]{9,}$">*</asp:RegularExpressionValidator>
&nbsp;<asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
    </div> 
    <div class="col-md-6 ">
        <label for="txtConfirmPassword">Confirm Password:<asp:RequiredFieldValidator ID="ConfirmPasswordValidator" runat="server" ControlToValidate="txtConfirmPassword" CssClass="error" ErrorMessage="Confirm Password required">*</asp:RequiredFieldValidator>
        </label>
        <asp:CompareValidator ID="CompareValidator" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" CssClass="error" ErrorMessage="Password and Confirm Password should be same">*</asp:CompareValidator>
&nbsp;<asp:TextBox ID="txtConfirmPassword" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
    </div> 
    </div>
    <div class="form-group row">
        <div class="col-md-6 ">
             <label for="txtFirstName">First Name:<asp:RequiredFieldValidator ID="FirstNameValidator" runat="server" ControlToValidate="txtFirstName" CssClass="error" ErrorMessage="First Name Required">*</asp:RequiredFieldValidator>
             </label>
            &nbsp;<asp:TextBox ID="txtFirstName" CssClass="auto-style1" runat="server"></asp:TextBox>
        </div> 
        <div class="col-md-6 ">
             <label for="txtLastName">Last Name:</label><asp:RequiredFieldValidator ID="LastNameValidator" runat="server" ControlToValidate="txtLastName" CssClass="error" ErrorMessage="Last Name required">*</asp:RequiredFieldValidator>
&nbsp;<asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

        <asp:Button ID="SaveBtn" runat="server" Text="Register" CssClass="btn btn-main" OnClick="SaveBtn_Click"  
              /> 

    </div>
</asp:Content>
