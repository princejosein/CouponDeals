<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CouponDeals.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<%--Search-Area--%>
        <div class="col-md">
            <asp:TextBox ID="txtSearch" placeholder="Search in Coupon Deals" runat="server" CssClass="form-control p-4 m-2"></asp:TextBox>
		</div>
		<div class="col-md">
            <asp:DropDownList ID="ddlCategories" class="form-control m-2 form-dropdown" runat="server">
			</asp:DropDownList>
		</div>
		<div class="col-md">
            <asp:Button ID="SearchBtn" runat="server" Text="Search" CssClass="btn btn-main m-2" OnClick="SearchBtn_Click" />
		</div>
<%--Search-Area-End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--Featured-Area--%>
<div class="featured_area">
<div class="container">
<div class="row ">
	<div class="h2-style mb-4">
		<h2>Featured Offers Today</h2>
	</div> 
	<asp:ListView ID="FeaturedOffers" runat="server">
	<ItemTemplate>
		<div class="col-md-3" runat="server" >
		<div class="single-box shadow-sm mb-5 bg-white">
		<img src="<%# Eval("profile_image") %>" height="200" />
		<div class="single-box-content">
		<span class="code-type"><%# Eval("tbl_offer_type_name") %></span>
		<h5 class="offer-title">
			<%# Eval("code_title") %>
		</h5>
			<div class="row mb-3 mx-auto">
				<div class="col-sm-12">
					<button type="button" class="btn side-btn" onclick="ShowCoupon('<%# Eval("coupon_id") %>');">Get Coupon!  <i style="font-size:18px" class="fas fa-gift"></i></button>
				</div>
				
			</div>
			<div class="row mb-3 mx-auto">
				<div class="col-sm-12">
					<button type="button" class="btn side-btn fav-btn-<%# Eval("coupon_id") %>" onclick="Favourites('<%# Eval("coupon_id") %>');">Add to Favourites  <i style="font-size:18px" class="far fa-heart"></i></button>
				</div>
			</div>
			<hr />
			<span class="code-type">On Promotion</span>			
		</div>
		</div>
		</div>
	</ItemTemplate>
</asp:ListView>
	

<hr class="hom-hr" />	

<div class="h2-style mb-4">
		<h2>Top Offers Today</h2>
	</div> 

<asp:ListView ID="TopProducts" runat="server">
	<ItemTemplate>
		<div class="col-md-3" runat="server" >
		<div class="single-box shadow-sm mb-5 bg-white">
		<img src="<%# Eval("profile_image") %>" height="200" />
		<div class="single-box-content">
		<span class="code-type"><%# Eval("tbl_offer_type_name") %></span>
		<h5 class="offer-title">
			<%# Eval("code_title") %>
		</h5>
			<div class="row mb-3 mx-auto">
				<div class="col-sm-12">
					<button type="button" class="btn side-btn" onclick="ShowCoupon('<%# Eval("coupon_id") %>');">Get Coupon!  <i style="font-size:18px" class="fas fa-gift"></i></button>
				</div>
				
			</div>
			<div class="row mb-3 mx-auto">
				<div class="col-sm-12">
					<button type="button" class="btn side-btn fav-btn-<%# Eval("coupon_id") %>" onclick="Favourites('<%# Eval("coupon_id") %>');">Add to Favourites  <i style="font-size:18px" class="far fa-heart"></i></button>
				</div>
			</div>
			
		</div>
		</div>
		</div>
	</ItemTemplate>
</asp:ListView>


</div>
</div>
</div>

<%--Featured-Area-End--%>
<!-- Modal -->
<div class="modal fade" id="CouponModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="false">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
		  <img src="" class="rounded mx-auto d-block modal-image mt-3 mb-3" width="100" >
		  <h3 class="code-title text-center mb-3"></h3>
		  <p class="copy-text text-center mb-3"></p>
		  <div class="rounded mb-3 code text-center btn-coupon"></div> 
		  <h5>Details</h5>
		  <h6 class="expires mb-3"></h6>
		  <h6 class="code-desc mb-3"></h6>
		  <input type="hidden" value="" id="coupon_id" />
         
         <div class="modal-footer">
			 <button type="button" class="btn btn-raised btn-success" onclick="AddToCart();" >ADD TO CART <i class="fas fa-cart-plus"></i></button>&nbsp;&nbsp;
            <button type="button" class="btn btn-raised btn-danger" data-dismiss="modal">CANCEL <i class="far fa-window-close"></i></button>

		</div>
      </div>
    </div>
  </div>
</div>
    <asp:HiddenField ID="HidCouponID" Value="0" runat="server" />	
</asp:Content>
