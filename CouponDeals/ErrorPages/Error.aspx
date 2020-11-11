<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="CouponDeals.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>Unexpected Error</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3 class="text-center">Error Occured. Please Contact administrator using princeforever619@gmail.com</h3><br />
    <div class="text-center">
        <asp:HyperLink ID="HomeLink" runat="server" CssClass="btn btn-success text-center" NavigateUrl="~/Default.aspx" >Back to Home  <i class="fas fa-home"></i></asp:HyperLink>
    </div>
    
</asp:Content>
