<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="CouponDeals.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        let price = 0.0;
        $(document).ready(function () {
            $(".price").each(function (index) {
                price += parseFloat($(this).text());
            });
            $(".final_price").html("$" + price);
        });
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>Checkout</h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group row checkout-area">
        <div class="col-md-7 checkout-billing">
            <h3>Billing Address</h3>
            <div class="form-group row">
        <div class="col-md-6 ">
             <label for="txtFirstName">First Name:</label>
            <asp:TextBox ID="txtFirstName" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
        <div class="col-md-6 ">
             <label for="txtLastName">Last Name:</label>
            <asp:TextBox ID="txtLastName" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

            <div class="form-group row">
        <div class="col-md-6 ">
             <label for="txtAddress">Address:</label>
            <asp:TextBox ID="txtAddress" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
        <div class="col-md-6 ">
             <label for="txtCity">City:</label>
            <asp:TextBox ID="txtCity" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

            <div class="form-group row">
        <div class="col-md-6 ">
             <label for="txtState">State:</label>
            <asp:TextBox ID="txtState" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
        <div class="col-md-6 ">
             <label for="txtZipCode">Zip Code:</label>
            <asp:TextBox ID="txtZipCode" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

            <div class="form-group row">
        <div class="col-md-6 ">
             <label for="txtEmailAddress">Email Address:</label>
            <asp:TextBox ID="txtEmailAddress" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
        <div class="col-md-6 ">
             <label for="txtPhone">Phone Number:</label>
            <asp:TextBox ID="txtPhone" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>
        </div>
        <div class="col-md-4 checkout-cart float-right">
            <h3>Review Order</h3>
            <ul class="list-group mb-3">
            <asp:Repeater ID="ReviewCart" runat="server">
                <ItemTemplate>            
                <li class="list-group-item d-flex justify-content-between lh-condensed">
                  <div>
                      <img src='<%# Eval("profile_image").ToString() %>' class="mb-3" width="50" />
                    <h6 class="my-0"><%# Eval("code_title").ToString() %></h6>
                    <small class="text-muted">Quantity: <%# Eval("cart_count").ToString() %></small>
                  </div>
                  <span class="text-muted">$ <span class="price"><%# ((int.Parse(Eval("cart_count").ToString())*1.5)) %></span></span>
                </li>
                </ItemTemplate>
            </asp:Repeater>
                <li class="list-group-item d-flex justify-content-between">
                  <span>Total (NZD)</span>
                  <strong class="final_price">$0.0</strong>
                </li>
          </ul>
            
        </div>
        <div class="row checkout-payment">
            <div class="col-md-12">
                <h4>Payment Details</h4>
                <p>Specify Order Number when you make the bank transfer.</p>
                <asp:Button ID="BtnPlaceOrder" OnClick="BtnPlaceOrder_Click" runat="server" Text="Place Order" CssClass="btn btn-success" />
            </div>
        </div>
    </div>    
</asp:Content>
