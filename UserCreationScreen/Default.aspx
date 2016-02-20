<%@ Page Title="User Creation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UserCreationScreen._Default" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div style="text-align: center">
    <asp:UpdatePanel ID="udpHeader" runat="server">
        <ContentTemplate>
            <h1 aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Aharoni" Font-Size="XX-Large" ForeColor="Blue" Text="User Creation" CssClass="page-title"></asp:Label>
            </h1>
            <p aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                <asp:Label ID="lblTesting" runat="server"></asp:Label>
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script>
        function VisiblyValidatePasswords()
        {
            document.getElementById("txt");
        }
    </script>
    <div style="margin-left: auto; margin-right: auto; text-align: center;">
        <div class="pop-background">
            <br />
            <asp:UpdatePanel ID="udpContent" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="centered-container">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSubmit">
                            <table class="main-table table-no-padding">
                                <tr>
                                    <td>First Name:*</td>
                                    <td><asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox></td>
                                    <td>Phone Number:*</td>
                                    <td><telerik:RadMaskedTextBox ID="txtPhoneNumber" runat="server" DisplayMask="(###)###-####" EnabledStyle-HorizontalAlign="Center" Height="22px" Mask="(###)###-####" RenderMode="Lightweight" Width="150px">
                                    </telerik:RadMaskedTextBox></td>
                                </tr>
                                <tr>
                                    <td>Name:*</td>
                                    <td><asp:TextBox ID="txtLastName" runat="server"></asp:TextBox></td>
                                    <td>Email Address:*</td>
                                    <td><asp:TextBox ID="txtEmailAddress" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr><td><br /> </td></tr>
                                <tr><td> </td></tr>
                                <tr>
                                    <td>Address:*</td>
                                    <td><asp:TextBox ID="txtAddress1" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtAddress2" runat="server"></asp:TextBox></td>
                                    <td><asp:TextBox ID="txtAddress3" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr><td><br /> </td></tr>
                                <tr><td> </td></tr>
                                <tr>
                                    <td>Country:*</td>
                                    <td><asp:TextBox ID="txtCountry" runat="server"></asp:TextBox></td>
                                    <td>State:*</td>
                                    <td><asp:TextBox ID="txtState" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>City:*</td>
                                    <td><asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                                    <td>Zip Code:*</td>
                                    <td><asp:TextBox ID="txtZip" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <p>
                        </p>
                        <asp:Panel ID="pnlUsernamePassword" runat="server" style="display: none" Visible="False" DefaultButton="btnSubmit">
                            <br /><br />
                            <table class="main-table table-no-padding">
                                <tr>
                                    <td>Username:</td>
                                    <td><asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></td>
                                    <td>Password:</td>
                                    <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>Validate Password:</td>
                                    <td><asp:TextBox ID="txtPasswordValidate" runat="server" TextMode="Password"></asp:TextBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="X-Small" ForeColor="Red" Text="*Required"></asp:Label>
                        <p>
                        </p>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <p>
                <div class="centered-container">
                    <div class="element">
                    <asp:UpdatePanel ID="udpSubmitClear" runat="server" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btnSubmit" runat="server" Font-Size="Small" Height="40px" Text="Submit" Width="140px" OnClick="btnSubmit_Click" />   
                            &nbsp;&nbsp;   
                            <asp:Button ID="btnClear" runat="server" Font-Size="Small" Height="40px" Text="Clear" Width="140px" OnClick="btnClear_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>   
                    <div class="element">
                        &nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" Font-Size="Small" Height="40px" Text="Cancel" Width="140px" OnClick="btnCancel_Click" />
                    </div>
                </div>
            </p>
        </div>
    </div>
</asp:Content>
