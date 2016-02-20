using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using UserCreationScreen.SQLInterfaces;

namespace UserCreationScreen.RolePages
{
    public partial class ViewRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // If we are logged in and we don't have access to this page (create)
                if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("create_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    btnNewValue.Visible = false;
                }

                // If we are logged in and we don't have access to this page (edit)
                if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("edit_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    pnlEditValue.Visible = false;
                }

                // If we aren't logged in
                if (!Master.isLoggedIn)
                {
                    Master.GoToLoginPage();
                }
                else if ((Master.currentUser.CanAccessPage("create_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation) && (Master.currentUser.CanAccessPage("edit_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    Master.GoToUsersPage();
                }

                if (Master.isInPersonalDB)
                {
                    UpdateGrid();
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextboxes();
            udpSearchBar.Update();
            UpdateGrid();
        }

        private bool AreTextboxesClear()
        {
            bool toReturn = true;

            if ((txtSearchID != null) && (txtSearchID.Text != ""))
            {
                toReturn = false;
            }

            if ((txtSearchRoleName != null) && (txtSearchRoleName.Text != ""))
            {
                toReturn = false;
            }

            return toReturn;
        }

        private void ClearTextboxes()
        {
            txtSearchID.Text = "";
            txtSearchRoleName.Text = "";
        }

        protected void btnNewValue_Click(object sender, EventArgs e)
        {
            // Generate the requestion
            string toGoTo = Request.Url.AbsoluteUri;
            toGoTo = (new System.Text.RegularExpressions.Regex(@"(.+\/).+").Match(toGoTo).Groups[1].Value);
            toGoTo += "ModifyRole";

            // Send Request
            Response.Redirect(toGoTo);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (rdgDisplayGrid.SelectedItems.Count > 0)
            {
                // Get the currently selected ID
                GridDataItem gdi = rdgDisplayGrid.SelectedItems[0] as GridDataItem;
                int selectedID = int.Parse(gdi["role_id"].Text);

                // Generate the http request
                string destination = Request.Url.AbsoluteUri;
                destination = new Regex(@"(.+\/).+").Match(destination).Groups[1].Value;
                destination += "ModifyRole?role_id=" + selectedID.ToString();

                // Transfer the page
                Response.Redirect(destination);
            }
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (rdgDisplayGrid.SelectedItems.Count > 0)
            {
                // Get the currently selected ID
                GridDataItem gdi = rdgDisplayGrid.SelectedItems[0] as GridDataItem;
                int selectedID = int.Parse(gdi["role_id"].Text);
                string columns = "";

                // Calculate the columns
                object[,] itemsToPopulate = new SQLInterface().GetQuery("EXEC [TestDatabase].[dbo].[GetColumnNames] @TableName = N'Roles'");

                for (int i = 1; i < itemsToPopulate.GetLength(0); i++)
                {
                    columns += itemsToPopulate[i, 0] as string;

                    if (i != (itemsToPopulate.GetLength(0) - 1))
                        columns += ", ";
                }

                if (!Master.isInPersonalDB) new SQLInterface().SendCommand("INSERT INTO TestDatabase.dbo.Roles SELECT " + columns + " FROM TestDatabase.dbo.Roles WHERE role_id=" + selectedID);
                else new SQLInterface().SendCommand("INSERT INTO TestDatabase2.dbo.Roles" + Master.currentDB + " SELECT " + columns + " FROM TestDatabase2.dbo.Roles" + Master.currentDB + " WHERE role_id=" + selectedID);
            }

            UpdateGrid();
        }

        protected void btnDeleteValue_Click(object sender, EventArgs e)
        {
            if (rdgDisplayGrid.SelectedItems.Count > 0)
            {
                // Get the currently selected ID
                GridDataItem gdi = rdgDisplayGrid.SelectedItems[0] as GridDataItem;
                int selectedID = int.Parse(gdi["role_id"].Text);

                if (!Master.isInPersonalDB) new SQLInterfaces.SQLInterface().SendCommand("DELETE FROM TestDatabase.dbo.Roles WHERE role_id=" + selectedID);
                else new SQLInterfaces.SQLInterface().SendCommand("DELETE FROM TestDatabase2.dbo.Roles" + Master.currentDB + " WHERE role_id=" + selectedID);
            }

            UpdateGrid();
        }

        protected void txtSearchID_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchRoleName_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            rdgDisplayGrid.DataSourceID = null;

            if (Master.isInPersonalDB)
                rdgDisplayGrid.DataSource = PersonalFilteredUsersTable;
            else
                rdgDisplayGrid.DataSource = FilteredUsersTable;

            rdgDisplayGrid.Rebind();
            udpGrid.Update();
        }
    }
}