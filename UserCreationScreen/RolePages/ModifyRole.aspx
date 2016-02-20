<%@ Page Title="Role" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModifyRole.aspx.cs" Inherits="UserCreationScreen.RolePages.ModifyRole"%>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="FeaturedContent">
    <div style="text-align: center">
        <asp:UpdatePanel ID="udpHeader" runat="server">
            <ContentTemplate>
                <h1 aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Aharoni" Font-Size="XX-Large" ForeColor="Blue" Text="Modify Role" CssClass="page-title"></asp:Label>
                </h1>
                <p aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                    <asp:Label ID="lblTesting" runat="server"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="MainContent">
    <asp:UpdatePanel ID="udpTextboxes" runat="server">
        <ContentTemplate>
            <div style="text-align: center">
                Role Name:<br />
                <asp:TextBox ID="txtRoleName" runat="server"></asp:TextBox>
                <br />
                <br />
                Permissions:<br />
                <div style="text-align: left; width: 200px; margin-left: auto; margin-right: auto;">
                    <asp:CheckBoxList ID="cblPermissions" runat="server" Width="200px">
                    </asp:CheckBoxList>
                    <br />
                </div>
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="text-align: center">
        <div style="margin-left: auto; margin-right: auto; text-align: center; width: 265px">
            <div style="width: auto; float: left">
                <asp:UpdatePanel ID="udpButtons" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="75px" OnClick="btnSubmit_Click"/>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnClear" runat="server" Text="Clear" Width="75px" OnClick="btnClear_Click"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="width: auto; text-align: left">
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75px" OnClick="btnCancel_Click" />
            </div>
        </div>
    </div>
</asp:Content>
