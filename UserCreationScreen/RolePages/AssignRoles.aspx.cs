using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using UserCreationScreen.SQLInterfaces;

namespace UserCreationScreen.RolePages
{
    public partial class AssignRoles : System.Web.UI.Page
    {
        // Public Variables
        
        // Private Variables
        private bool isInAddMode = true;
        private SQLInterface sqlInterface = new SQLInterface();

        private int usr_id
        {
            get { return (ViewState["usr_id"] != null) ? (Convert.ToInt32(ViewState["usr_id"])) : -1; }
            set { ViewState["usr_id"] = value; }
        }

        private int role_id
        {
            get { return (ViewState["role_id"] != null) ? (Convert.ToInt32(ViewState["role_id"])) : -1; }
            set { ViewState["role_id"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((usr_id >= 0) && (role_id >= 0))
            {
                isInAddMode = false;
            }
            else
            {
                isInAddMode = true;
            }

            if (!this.IsPostBack)
            {
                btnNewValue_Click(new object(), new EventArgs());
                UpdateDropDownLists();

                // If we are logged in and we don't have access to this page
                if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("assign_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    Master.GoToUsersPage();
                }
                // If we aren't logged in
                else if (!Master.isLoggedIn)
                {
                    Master.GoToLoginPage();
                }

                if (Master.isInPersonalDB)
                {
                    ddlName.DataSourceID = null;
                    ddlName.DataSource = PersonalUsers;
                    ddlRole.DataSourceID = null;
                    ddlRole.DataSource = PersonalRoles;

                    UpdateGrid();
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextboxes();
            udpSearchBar.Update();
            UpdateDropDownLists();
        }

        protected void btnNewValue_Click(object sender, EventArgs e)
        {
            usr_id = -1;
            role_id = -1;

            btnSubmit.Text = "Add New Entry";
            lblMode.Text = "Create Value:";
        }

        protected void txtSearchUserID_TextChanged(object sender, EventArgs e)
        {
            UpdateDropDownLists();
        }

        protected void txtSearchFirstName_TextChanged(object sender, EventArgs e)
        {
            UpdateDropDownLists();
        }

        protected void txtSearchLastName_TextChanged(object sender, EventArgs e)
        {
            UpdateDropDownLists();
        }

        protected void txtSearchRoleID_TextChanged(object sender, EventArgs e)
        {
            UpdateDropDownLists();
        }

        protected void txtSearchRoleName_TextChanged(object sender, EventArgs e)
        {
            UpdateDropDownLists();
        }

        protected void rdgDisplayGrid_Click(object sender, GridCommandEventArgs e)
        {
            //lblTesting.Text = Master.currentUser.canAssignRole.ToString();
            if (e.CommandName == "UpdateDropDownLists")
            {
                GridDataItem gdi = e.Item as GridDataItem;
                usr_id = int.Parse(gdi["usr_id"].Text);
                role_id = int.Parse(gdi["role_id"].Text);

                btnSubmit.Text = "Submit Edit";
                lblMode.Text = "Edit Value:";

                ddlName.SelectedValue = usr_id.ToString();
                ddlRole.SelectedValue = role_id.ToString();
            }
            else if (e.CommandName == "DeleteElement")
            {
                GridDataItem gdi = e.Item as GridDataItem;
                int userID = int.Parse(gdi["usr_id"].Text);
                int roleID = int.Parse(gdi["role_id"].Text);
                string toSend;

                if (!Master.isInPersonalDB) toSend = "DELETE FROM TestDatabase.dbo.UsersRoles WHERE usr_id=" + userID + " AND role_id=" + roleID;
                else toSend = "DELETE FROM TestDatabase2.dbo.UsersRoles" + Master.currentDB + " WHERE usr_id=" + userID + " AND role_id=" + roleID;

                sqlInterface.SendCommand(toSend);
            }

            UpdateGrid();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (isInAddMode)
            {
                string command;
                if (!Master.isInPersonalDB) command = "INSERT INTO TestDatabase.dbo.UsersRoles VALUES (" + ddlName.SelectedValue + ", " + ddlRole.SelectedValue + ")";
                else command = "INSERT INTO TestDatabase2.dbo.UsersRoles" + Master.currentDB + " VALUES (" + ddlName.SelectedValue + ", " + ddlRole.SelectedValue + ")";
                sqlInterface.SendCommand(command);
            }
            else
            {
                string command;
                if (!Master.isInPersonalDB) command = "UPDATE TestDatabase.dbo.UsersRoles SET usr_id=" + ddlName.SelectedValue + ", role_id=" + ddlRole.SelectedValue + " WHERE usr_id=" + usr_id + " AND role_id=" + role_id;
                else command = "UPDATE TestDatabase2.dbo.UsersRoles" + Master.currentDB + " SET usr_id=" + ddlName.SelectedValue + ", role_id=" + ddlRole.SelectedValue + " WHERE usr_id=" + usr_id + " AND role_id=" + role_id;
                sqlInterface.SendCommand(command);
            }

            UpdateGrid();
        }

        private void ClearTextboxes()
        {
            txtSearchFirstName.Text = "";
            txtSearchLastName.Text = "";
            txtSearchRoleID.Text = "";
            txtSearchRoleName.Text = "";
            txtSearchUserID.Text = "";
        }

        private void UpdateDropDownLists()
        {
            // Update the view
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            if (Master.isInPersonalDB)
            {
                rdgDisplayGrid.DataSourceID = null;
                rdgDisplayGrid.DataSource = PersonalFilteredUserTable;
                rdgDisplayGrid.Rebind();
            }
            else
            {
                rdgDisplayGrid.DataSourceID = null;
                rdgDisplayGrid.DataSource = FilteredUsersTable;
                rdgDisplayGrid.Rebind();
            }

            udpGrid.Update();
        }
    }
}