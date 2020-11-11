<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="CouponDeals.Category" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
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
        function ValidateInput() {
            if (document.getElementById("<%=uploadImg.ClientID %>").value == "" &&
                document.getElementById("ImgExists").value == "") {
            swalAlert("Image Upload Error", "Please Upload Company Logo", "error");
            return false;
        }
        return true;      
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <h2>
        <asp:Label ID="lblCategory" runat="server" Text="Create Category"></asp:Label>
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
                </asp:DropDownList>
        </div>
        <div class="col-md-10" id="btnSet" style="text-align:right;" runat="server">
                <asp:Button ID="EditBtn" runat="server" Text="Edit Profile" CssClass="btn side-btn " OnClick="EditBtn_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button CssClass="btn side-btn " ID="DeleteBtn" runat="server" Text="Delete"
                              CausesValidation="false" OnClick="DeleteBtn_Click"  />&nbsp;
        </div>
        </div>

      <asp:Image ID="ImgID" ImageUrl="Images/placeholder_logo.png" Width="200" runat="server" class="rounded mx-auto d-block" />
      <div class="row">
        <div class="col-md-12 text-danger ">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
     </div>     
     
        <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtCategoryID">Category ID:</label>
            <asp:TextBox ID="txtCategoryID" CssClass="form-control" runat="server" ReadOnly="true"></asp:TextBox>
        </div> 
    </div>

     <div class="form-group row">
        <div class="col-md-12 ">
             <label for="txtCategoryName">Category Name:</label>
            <asp:TextBox ID="txtCategoryName" CssClass="form-control" runat="server"></asp:TextBox>
        </div> 
    </div>

    <div class="form-group row">
    <div class="col-md-12 ">
    <label for="uploadImg">Upload Category Image:</label>
        <asp:FileUpload ID="uploadImg" runat="server" CssClass="form-control-file" />
    </div> 
    </div>
        <asp:Button ID="SaveBtn" runat="server" Text="Save" CssClass="btn btn-main" OnClick="SaveBtn_Click"  
              />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="BackLink" runat="server" CssClass="btn side-btn" NavigateUrl="~/CategoryList.aspx" >Back</asp:HyperLink>

    </div>
    <asp:HiddenField ID="ImgExists" Value="" runat="server"  />
    <asp:HiddenField ID="hidFldID" Value="0" runat="server" />

</asp:Content>
