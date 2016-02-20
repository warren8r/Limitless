<%@ Page Title="View and Assign Roles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignRoles.aspx.cs" Inherits="UserCreationScreen.RolePages.AssignRoles"%>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" runat="server" contentplaceholderid="FeaturedContent">
    <div style="text-align: center">
        <asp:UpdatePanel ID="udpHeader" runat="server">
            <ContentTemplate>
                <h1 aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Aharoni" Font-Size="XX-Large" ForeColor="Blue" Text="View and Assign Roles" CssClass="page-title"></asp:Label>
                </h1>
                <p aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="User Creation">
                    <asp:Label ID="lblTesting" runat="server"></asp:Label>
                </p>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="MainContent">
    <asp:UpdatePanel ID="udpSearchBar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="search-table">
                <caption>Filter</caption>
                <tr>
                    <th>User ID</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Role ID</th>
                    <th>Role Name</th>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updUserID" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchUserID" runat="server" Width="100px" TabIndex="1" AutoPostBack="true" OnTextChanged="txtSearchUserID_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updFirstName" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchFirstName" runat="server" Width="100px" TabIndex="2" AutoPostBack="true" OnTextChanged="txtSearchFirstName_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updLastName" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchLastName" runat="server" Width="100px" TabIndex="3" AutoPostBack="true" OnTextChanged="txtSearchLastName_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updRoleID" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchRoleID" runat="server" Width="100px" TabIndex="4" AutoPostBack="true" OnTextChanged="txtSearchRoleID_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updRoleName" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchRoleName" runat="server" Width="100px" TabIndex="5" AutoPostBack="true" OnTextChanged="txtSearchRoleName_TextChanged"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5"><asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="9" /></td>
                </tr>
            </table>
                
                <br /><br /><br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
        
    <asp:UpdatePanel ID="udpGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="text-align: center; margin-right: auto; margin-left: auto">
                <asp:Panel ID="pnlCreateEditValues" runat="server">
                    <asp:Label ID="lblMode" runat="server" Font-Size="Medium" Text="Edit Value:"></asp:Label>
                    &nbsp;(NOTE: Edit column that sets this to Edit Mode)<br />
                    <asp:DropDownList ID="ddlName" runat="server" DataSourceID="Names" DataTextField="Name" DataValueField="usr_id">
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlRole" runat="server" DataSourceID="Roles" DataTextField="role_name" DataValueField="role_id">
                        <asp:ListItem>BlankItem</asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSubmit" runat="server" Text="New" OnClick="btnSubmit_Click" />
                </asp:Panel>
                <br />
                <br />
                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Users (by ID number, with roles):"></asp:Label>
                <br />
                <telerik:RadGrid runat="server" ID="rdgDisplayGrid" GridLines="Both" DataSourceID="FilteredUsersTable" AllowSorting="True" TabIndex="10" ClientSettings-EnablePostBackOnRowClick="true" onitemcommand="rdgDisplayGrid_Click">
                    <GroupingSettings MainTableCaption="Users" />
                    <ClientSettings ActiveRowIndex="0">
                    </ClientSettings>
                    <MasterTableView DataKeyNames="usr_id" DataSourceID="FilteredUsersTable">
                        <Columns>
                            <telerik:GridButtonColumn CommandName="UpdateDropDownLists" FilterControlAltText="Filter edit column" Text="Edit" UniqueName="edit">
                            </telerik:GridButtonColumn>
                            
                            <telerik:GridButtonColumn CommandName="DeleteElement" FilterControlAltText="Filter delete column" Text="Delete" UniqueName="delete">
                            </telerik:GridButtonColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <br />
                <asp:Button ID="btnNewValue" runat="server" OnClick="btnNewValue_Click" Text="Create New Value" />

                <asp:SqlDataSource ID="FilteredUsersTable" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase %>" SelectCommand="FilteredUsersWithRoles" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchUserID" Name="userId" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchFirstName" Name="firstName" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchLastName" Name="lastName" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchRoleID" Name="roleId" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchRoleName" Name="roleName" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="Users" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase %>" SelectCommand="filteredNames" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchUserID" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchFirstName" Name="firstName" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchLastName" Name="lastName" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="Roles" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase %>" SelectCommand="FilteredRoles" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchRoleID" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchRoleName" Name="roleName" PropertyName="Text" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="PersonalFilteredUserTable" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase2 %>" SelectCommand="FilteredUsersWithRolesTrue" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchUserID" Name="userId" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchFirstName" Name="firstName" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchLastName" Name="lastName" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchRoleID" Name="roleId" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchRoleName" Name="roleName" PropertyName="Text" Type="String" />
                        <asp:SessionParameter Name="dbOwnerID" SessionField="currentDB" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="PersonalUsers" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase2 %>" SelectCommand="filteredNamesTrue" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchUserID" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchFirstName" Name="firstName" PropertyName="Text" Type="String" />
                        <asp:ControlParameter ControlID="txtSearchLastName" Name="lastName" PropertyName="Text" Type="String" />
                        <asp:SessionParameter Name="dbId" SessionField="currentDB" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="PersonalRoles" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase2 %>" SelectCommand="FilteredRolesTrue" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="False">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchRoleID" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchRoleName" Name="roleName" PropertyName="Text" Type="String" />
                        <asp:SessionParameter Name="dbId" SessionField="currentDB" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </div>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>