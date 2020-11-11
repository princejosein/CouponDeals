<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="CouponDeals.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblCart" runat="server" Text="Shopping Cart"></asp:Label>
    </h2>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="table-responsive bg-light">
                <asp:GridView ID="GridCart" runat="server" BackColor="White"
                    RowStyle-VerticalAlign="Middle" OnRowCommand="GridCart_RowCommand" 
                    AutoGenerateColumns="false" ShowHeader="true" GridLines="None" 
                    DataKeyNames="category_id" EmptyDataRowStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="true"  
                            EmptyDataText="--Empty Cart--" CssClass="table table-hover table-borderless" HeaderStyle-HorizontalAlign="Center" 
                            
                    >
                    <Columns>
                        <asp:ImageField DataAlternateTextField="name" ControlStyle-Width="75" ItemStyle-HorizontalAlign="Center" HeaderText="PICTURE" DataImageUrlField="profile_image" ></asp:ImageField>             
                        <asp:BoundField ItemStyle-VerticalAlign="Middle"  DataField="code_title" HeaderText="TITLE"  ItemStyle-HorizontalAlign="Center"  /> 
                        <asp:TemplateField HeaderText="PRICE" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>1</ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtQuantity" CssClass="cart_quantity_list" runat="server" TextMode="Number" Text='<%# Eval("cart_count") %>'></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CommandArgument='<%# Eval("cart_id") %>' ControlStyle-CssClass="btn btn-primary" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnRemove" runat="server" CommandName="Remove" Text="Remove" CommandArgument='<%# Eval("cart_id") %>' ControlStyle-CssClass="btn btn-danger" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                    <EmptyDataRowStyle  />
                    
                </asp:GridView>
            </div>
        </ContentTemplate>       
        
    </asp:UpdatePanel>
        <div class="col-md-6 float-right checkout-section">
            <h4 class="text-center">Summary</h4>
            <div class="form-group row">
                <div class="col-md-6">
                    Total Quantity:
                </div>
                <div class="col-md-6 text-center">
                    <asp:Label ID="lblQuantity" runat="server" Text="0" ></asp:Label>
                </div>
            </div>

            <div class="form-group row">
                <div class="col-md-6">
                    Total Price:
                </div>
                <div class="col-md-6 text-center">
                    <asp:Label ID="lblPrice" runat="server" Text="NZD 0" ></asp:Label>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-md-12 text-center">
                    <asp:HyperLink ID="CheckoutRef" runat="server" CssClass="btn btn-success" NavigateUrl="~/Checkout.aspx" >CheckOut</asp:HyperLink>
                </div>
            </div>
        </div>
</asp:Content>
