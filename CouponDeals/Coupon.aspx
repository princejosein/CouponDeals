<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Coupon.aspx.cs" Inherits="CouponDeals.Coupon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        $(document).ready(function () {
            SearchCompanyWebsite();
        }); 
        function SearchCompanyWebsite() {
            $("#<%=txtCompany.ClientID %>").autocomplete({
                source: function (request, response) {
                    var searchString = document.getElementById('<%= txtCompany.ClientID %>').value;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Coupon.aspx/GetCompanyWebsite",
                        data: "{'companyWebsite':'" + searchString + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            //
                        }
                    });
                }
            });
        }
        function DeleteCategory() {
            swal({
                title: "Are you sure?",
                text: "This Action Cannot be Undone",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            })
                .then((willDelete) => {
                    if (willDelete) {
                        return true;
                    } else {
                        return false;
                    }
                });
        }    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblCoupon" runat="server" Text="Submit A Coupon"></asp:Label>
    </h2>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-main">  
        <div class="form-group row" id="statusDiv" runat="server">
        <div class="col-md-2" runat="server">
            <label for="txtStatus">Status:</label>
                <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server" ReadOnly="true"  ID="txtStatus" Text="" ></asp:TextBox>
                <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control" Visible="false">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="2">Not Active</asp:ListItem> 
                    <asp:ListItem Value="3">Waiting For Approval</asp:ListItem> 
                    <asp:ListItem Value="4">Expired</asp:ListItem> 
                </asp:DropDownList>
        </div>
        <div class="col-md-10" id="btnSet" style="text-align:right;" runat="server">
                <asp:Button ID="EditBtn" runat="server" Text="Edit Coupon" CssClass="btn side-btn " OnClick="EditBtn_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button CssClass="btn side-btn " ID="DeleteBtn" runat="server" Text="Delete"
                              CausesValidation="false" OnClick="DeleteBtn_Click"  />&nbsp;
        </div>
        </div>

      <div class="row">
        <div class="col-md-12 text-danger ">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
     </div>     
     
        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtCouponID">Coupon ID:</label>
            <asp:TextBox ID="txtCouponID" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
        </div> 
    </div>

        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtCompany">Company Website:</label>
            <asp:TextBox ID="txtCompany" CssClass="form-control" runat="server"></asp:TextBox>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCompany" ErrorMessage="RegularExpressionValidator" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- .?%&amp;=]*)?"></asp:RegularExpressionValidator>
        </div> 
    </div>

        <div class="form-group row">
    <div class="col-md-12 ">
    <label for="ddlOfferType">Offer Type:</label> 
    <asp:DropDownList ID="ddlOfferType" runat="server" CssClass="form-control"></asp:DropDownList>
    </div> 
    </div>

    <div class="form-group row">
        <div class="col-md-12 ">
        <label for="txtCode">Code Title:</label>
        <asp:TextBox ID="txtCodeTitle" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtCode">Code:</label>
            <asp:TextBox ID="txtCode" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
        <label for="txtExpDate">Expiration Date:</label>&nbsp;
        <div class="input-group date ui-datepicker" >
            <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server"  ID="txtExpDate"  ClientIDMode="Static" ></asp:TextBox>
        </div>
        </div>    
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
        <label for="txtAreaDesc">Code Description:</label>    
        <textarea id="txtAreaDesc" runat="server" rows="4" style="width:100%" CssClass="form-control"></textarea>
    </div> 
    </div>

    <div class="row form-group" runat="server" id="PromotionDiv">
    <div class="col-md-12">
        <label for="CheckboxPromotion">On Promotion:</label><br />
        <asp:CheckBox ID="CheckboxPromotion" Text="Show Coupon in Promotion Section" CssClass="mycheckbox" Checked="false" runat="server" />
    </div>
    </div>
<%--        <div class="form-group row">
        <div class="col-md-12" runat="server" id="ApprovalDiv">
            <label for="txtApproved">Approved:</label>
                <asp:TextBox CssClass="form-control" ToolTip="" placeholder=""  runat="server" ReadOnly="true"  ID="txtApproval" Text="" ></asp:TextBox>
                <asp:DropDownList runat="server" ID="ddlApproval" CssClass="form-control" Visible="false">
                    <asp:ListItem Value="1">Waiting For Approval</asp:ListItem>
                    <asp:ListItem Value="2">Approved</asp:ListItem>
                </asp:DropDownList>
        </div>
            </div>--%>

        <asp:Button ID="SaveBtn" runat="server" CausesValidation="true" Text="Save" CssClass="btn btn-main" OnClick="SaveBtn_Click"  
              />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="BackLink" runat="server" CssClass="btn side-btn" NavigateUrl="~/CouponList.aspx" >Back</asp:HyperLink>

    </div>
    <asp:HiddenField ID="hidFldID" Value="0" runat="server" />

</asp:Content>
