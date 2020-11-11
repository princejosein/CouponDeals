<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Forgetpassword.aspx.cs" Inherits="CouponDeals.Forgetpassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblUser" runat="server" Text="Forget Password"></asp:Label>
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
             <label for="txtEmail">Email:</label>
            <asp:TextBox ID="txtEmail" CssClass="form-control" TextMode="Email" runat="server"></asp:TextBox>
        </div> 
    </div>    

        <asp:Button ID="ResetBtn" runat="server" Text="Send Email" CssClass="btn btn-main" OnClick="ResetPasswordEmail_Click"  
              />
    <div class="link-line">Don’t have an account? <a href="Register.aspx">Register</a></div>
    </div>

</asp:Content>
