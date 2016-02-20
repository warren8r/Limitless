<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectUser.aspx.cs" Inherits="UserCreationScreen.About" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <div style="text-align: center">
    <asp:UpdatePanel ID="udpHeader" runat="server">
        <ContentTemplate>
            <h1 aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="Select User">
                <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Font-Names="Aharoni" Font-Size="XX-Large" ForeColor="Blue" Text="Select User" CssClass="page-title"></asp:Label>
            </h1>
            <p aria-multiselectable="True" style="margin-left: auto; margin-right:auto" title="Select User">
                <asp:Label ID="lblTesting" runat="server" Visible="false"></asp:Label>
            </p>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>    
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="udpSearchBar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table class="search-table">
                <caption>Filter</caption>
                <tr>
                    <th>ID</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Email Address</th>
                    <th>City</th>
                    <th>State</th>
                    <th>Country</th>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="updRoleName" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchID" runat="server" Width="100px" OnTextChanged="txtSearchID_TextChanged" TabIndex="1" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchFirstName" runat="server" Width="100px" OnTextChanged="txtSearchFirstName_TextChanged" TabIndex="2" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchLastName" runat="server" Width="100px" OnTextChanged="txtSearchLastName_TextChanged" TabIndex="3" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchEmailAddress" runat="server" Width="100px" OnTextChanged="txtSearchEmailAddress_TextChanged" TabIndex="4" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchCity" runat="server" Width="100px" OnTextChanged="txtSearchCity_TextChanged" TabIndex="5" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchState" runat="server" Width="100px" OnTextChanged="txtSearchState_TextChanged" TabIndex="6" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel6" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSearchCountry" runat="server" Width="100px" OnTextChanged="txtSearchCountry_TextChanged" TabIndex="7" AutoPostBack="true"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="7"><asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" TabIndex="9" /></td>
                </tr>
            </table>
            <div style="vertical-align: central;">
                
                <br /><br /><br />
                <div style="text-align: center; width: auto; float: left; height: auto;">
                    &nbsp;&nbsp;
                    
                </div>
            </div>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
        
    <asp:UpdatePanel ID="udpGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="text-align: center; margin-right: auto; margin-left: auto">
                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Users (by ID number):"></asp:Label>
                <br />
                <telerik:RadGrid runat="server" ID="rdgDisplayGrid" GridLines="Both" DataSourceID="FilteredUsersTable" AllowSorting="True" OnSelectedIndexChanged="rdgDisplayGrid_SelectedIndexChanged" TabIndex="10" ClientSettings-EnablePostBackOnRowClick="true">
                    <GroupingSettings MainTableCaption="Users" />
                    <ClientSettings ActiveRowIndex="0">
                        <Selecting AllowRowSelect="True" />
                    </ClientSettings>
                    <MasterTableView DataKeyNames="usr_id" DataSourceID="FilteredUsersTable" AllowMultiColumnSorting="true" AutoGenerateColumns="False">
                        <Columns>
                            <telerik:GridClientSelectColumn FilterControlAltText="Filter selected column" UniqueName="selected">
                            </telerik:GridClientSelectColumn>
                            <telerik:GridBoundColumn DataField="usr_id" DataType="System.Int32" FilterControlAltText="Filter usr_id column" HeaderText="usr_id" ReadOnly="True" SortExpression="usr_id" UniqueName="usr_id">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="first_name" FilterControlAltText="Filter first_name column" HeaderText="first_name" SortExpression="first_name" UniqueName="first_name">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="last_name" FilterControlAltText="Filter last_name column" HeaderText="last_name" SortExpression="last_name" UniqueName="last_name">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="email_address" FilterControlAltText="Filter email_address column" HeaderText="email_address" SortExpression="email_address" UniqueName="email_address">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="city" FilterControlAltText="Filter city column" HeaderText="city" SortExpression="city" UniqueName="city">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="state" FilterControlAltText="Filter state column" HeaderText="state" SortExpression="state" UniqueName="state">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="country" FilterControlAltText="Filter country column" HeaderText="country" SortExpression="country" UniqueName="country">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
                <br />
                <asp:Button ID="btnNewValue" runat="server" OnClick="btnNewValue_Click" Text="Create New Value" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Panel runat="server" style="display: inline" ID="pnlEditValue">
                    <asp:Button ID="btnEdit" runat="server" Text="Edit Selected Value" OnClick="btnEdit_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDuplicate" runat="server" OnClick="btnDuplicate_Click" Text="Duplicate Selected Value" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDeleteValue" runat="server" OnClick="btnDeleteValue_Click" Text="Delete Selected Value" />
                </asp:Panel>

                <asp:SqlDataSource ID="FilteredUsersTable" runat="server" ConnectionString="<%$ ConnectionStrings:TestDatabase %>" SelectCommand="filteredusertable" SelectCommandType="StoredProcedure" CancelSelectOnNullParameter="false">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchID" DefaultValue="" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchFirstName" Name="firstName" PropertyName="Text" Type="String" DefaultValue="" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchLastName" DefaultValue="" Name="lastName" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchEmailAddress" DefaultValue="" Name="emailAddress" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchCity" DefaultValue="" Name="city" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchState" DefaultValue="" Name="state" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchCountry" DefaultValue="" Name="country" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="PersonalFilteredUsersTable" runat="server" CancelSelectOnNullParameter="False" ConnectionString="<%$ ConnectionStrings:TestDatabase2 %>" SelectCommand="filteredusertableTrue" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="txtSearchID" DefaultValue="" Name="id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchFirstName" Name="firstName" PropertyName="Text" Type="String" DefaultValue="" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchLastName" DefaultValue="" Name="lastName" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchEmailAddress" DefaultValue="" Name="emailAddress" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchCity" DefaultValue="" Name="city" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchState" DefaultValue="" Name="state" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:ControlParameter ControlID="txtSearchCountry" DefaultValue="" Name="country" PropertyName="Text" Type="String" ConvertEmptyStringToNull="true"/>
                        <asp:SessionParameter Name="dbId" SessionField="currentDB" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </div>
            </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Content>