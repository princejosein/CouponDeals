<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="CouponDeals.ResetPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblUser" runat="server" Text="Reset Password"></asp:Label>
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
             <label for="txtEmail">New Password:</label>
            <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
        </div> 
    </div>
        
        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtConfirmPassword">Confirm Password:</label>
            <asp:TextBox ID="txtConfirmPassword" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
        </div> 
    </div>  

        <asp:Button ID="ResetBtn" runat="server" Text="Reset Password" CssClass="btn btn-main" OnClick="ResetBtn_Click"   />    
    </div>

</asp:Content>
