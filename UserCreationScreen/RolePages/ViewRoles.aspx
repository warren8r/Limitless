<%@ Page Title="Role" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="~/RolePages/ViewRoles.aspx.cs" Inherits="UserCreationScreen.RolePages.ViewRoles"%>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="FeaturedContent">
    <div style="text-align: center">
        <asp:UpdatePanel ID="udpHeader" runat="server">
            <ContentTemplate>
                <h1 aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="Select User">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Aharoni" Font-Size="XX-Large" ForeColor="Blue" Text="View Roles" CssClass="page-title"></asp:Label>
                </h1>
                <p aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="Select User">
                    <asp:Label ID="lblTesting" runat="server" Visible="false"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
</asp:Content>

<asp:Content ID="Content2" runat="server" contentplaceholderid="MainContent">
    <asp:UpdatePanel ID="udpSearchBar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="search-table">
                <caption>Filter</caption>
                <tr>
                    <th>ID</th>
                    <th>Role Name</th>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updRoleName" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchID" runat="server" Width="100px" TabIndex="1" AutoPostBack="true" OnTextChanged="txtSearchID_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchRoleName" runat="server" Width="100px" TabIndex="2" AutoPostBack="true" OnTextChanged="txtSearchRoleName_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="9" />   </td>
                </tr>
            </table>
                
                <br /><br /><br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
        
    <asp:UpdatePanel ID="udpGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="text-align: center; margin-right: auto; margin-left: auto">
                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Roles (by ID number):"></asp:Label>
                <br />
                <telerik:RadGrid runat="server" ID="rdgDisplayGrid" GridLines="Both" DataSourceID="FilteredUsersTable" AllowSorting="True" TabIndex="10" ClientSettings-EnablePostBackOnRowClick="true">
                    <GroupingSettings MainTableCaption="Users" />
                    <ClientSettings ActiveRowIndex="0">
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView DataKeyNames="role_id" DataSourceID="FilteredUsersTable">
                        <Columns>
                            <telerik:GridClientSelectColumn FilterControlAltText="Filter selected column" UniqueName="selected">
                            </telerik:GridClientSelectColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <br />
                <asp:Button ID="btnNewValue" runat="server" OnClick="btnNewValue_Click" Text="Create New Value" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel runat="server" ID="pnlEditValue" HorizontalAlign="Center" style="display: inline">
                    <asp:Button ID="btnEdit" runat="server" Text="Edit Selected Value" OnClick="btnEdit_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDuplicate" runat="server" OnClick="btnDuplicate_Click" Text="Duplicate Selected Value" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDeleteValue" runat="server" OnClick="btnDeleteValue_Click" Text="Delete Selected Value" />
                </asp:Panel>

                <asp:SqlDataSource ID="FilteredUsersTable" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase %>" SelectCommand="FilteredRoles" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchID" DefaultValue="" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchRoleName" Name="roleName" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="PersonalFilteredUsersTable" runat="server" CancelSelectOnNullParameter="False" ConnectionString="<%$ ConnectionStrings:TestDatabase2 %>" SelectCommand="FilteredRolesTrue" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchID" ConvertEmptyStringToNull="true" DefaultValue="" Name="id" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchRoleName" ConvertEmptyStringToNull="true" Name="roleName" PropertyName="Text" Type="String" />
                        <asp:SessionParameter Name="dbId" SessionField="currentDB" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </div>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


