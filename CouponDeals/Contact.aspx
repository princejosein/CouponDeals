<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="CouponDeals.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script
      src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC2gej5S_9ROjGPfgMwh4QLcP6skw7raMU&callback=initMap&libraries=&v=weekly"
      defer
    ></script>
    <script>
        let map;

        function initMap() {
            var myLatLng = { lat: -36.863970, lng: 174.736060 };

            var map = new google.maps.Map(document.getElementById('map'), {
                zoom: 6,
                center: myLatLng
            });

            var marker = new google.maps.Marker({
                position: myLatLng,
                map: map,
                title: 'Coupon Deals'
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <h2>
        <asp:Label ID="lblUser" runat="server" Text="Contact"></asp:Label>
    </h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-3 text-center">
            <div class="contact-bx">
                <p><i class="fa fa-phone" aria-hidden="true"></i>
                </p>
                <h4>MOBILE</h4>
                <p>+(10) 123 456 7896</p>
            </div>
        </div>       

        <div class="col-md-3 text-center">
            <div class="contact-bx">
                <p><i class="fa fa-envelope" aria-hidden="true"></i>
                </p>
                <h4>EMAIL</h4>
                <p>princejose@gmail.com</p>
            </div>
        </div>

        <div class="col-md-3 text-center">
            <div class="contact-bx">
                <p><i class="fa fa-globe" aria-hidden="true"></i>
                </p>
                <h4>WEBSITE</h4>
                <p>www.princejose.com</p>
            </div>
        </div>

        <div class="col-md-3 text-center">
            <div class="contact-bx">
                <p><i class="fa fa-map-marker" aria-hidden="true"></i>
                </p>
                <h4>ADDRESS</h4>
                <p>622 Great North Road</p>
                <p>New Lynn, Auckland, New Zealand</p>
            </div>
        </div>
    </div>
    <div class="row mt-4">
        <div class="col-10" style="margin:0 auto">
            <div id="map"></div>
        </div>
    </div>    
</asp:Content>
