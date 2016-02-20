<%@ Page Title="Login" MasterPageFile="~/Site.Master" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UserCreationScreen.CustomAccountPages.Login" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" runat="server" contentplaceholderid="FeaturedContent">
    <div style="text-align: center">
        <asp:UpdatePanel ID="udpHeader" runat="server">
            <ContentTemplate>
                <h1 class=page-title aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Aharoni" Font-Size="XX-Large" ForeColor="Blue" Text="Login"></asp:Label>
                </h1>
                <p aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                    <asp:Label ID="lblTesting" runat="server"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="MainContent">
    <div style="text-align: center; margin-right: auto; margin-left: auto">
        <asp:Panel runat="server" DefaultButton="btnSubmit">
            <table style="margin:auto">
                <tr>
                    <td style="text-align: left"><asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label></td>
                    <td style="padding-right: 15px"><asp:TextBox ID="txtUsername" runat="server" TabIndex="1"></asp:TextBox></td>
                    <td rowspan="2" style="border-left: 3px solid; border-color: #B6AFA9; text-align: center; padding-left: 15px;"><asp:Button runat="server" ID="btnRegister" Text="Register" OnClick="btnRegister_Click" TabIndex="4"/></td>
                </tr>
                <tr>
                    <td style="text-align: left"><asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label></td>
                    <td style="padding-right: 15px"><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox></td>
                </tr>
            </table>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" TabIndex="3"/>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="HeadContent">
    
</asp:Content>
