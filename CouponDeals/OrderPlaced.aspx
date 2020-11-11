<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderPlaced.aspx.cs" Inherits="CouponDeals.OrderPlaced" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblOC" runat="server" Text="Order Confirmed"></asp:Label>
    </h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-main text-center">  
        <h4 class="text-center mb-5"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></h4>
        <asp:HyperLink ID="HomeBtn" runat="server" CssClass="btn btn-success" NavigateUrl="~/Default.aspx">Back to Home</asp:HyperLink>
    </div>
</asp:Content>
