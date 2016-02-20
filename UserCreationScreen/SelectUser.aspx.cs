using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
using UserCreationScreen.SQLInterfaces.Storage;

namespace UserCreationScreen
{
    public partial class About : Page
    {
        // Public Constants

        // Public Variables

        // Private Constants

        // Private Variables
        

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "View User Table";

            if (!this.IsPostBack)
            {
                // If we are logged in and we don't have access to this page (create)
                if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("create_user", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    btnNewValue.Visible = false;
                }

                // If we are logged in and we don't have access to this page (edit)
                if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("edit_user", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    pnlEditValue.Visible = false;
                }

                // If we aren't logged in
                if (!Master.isLoggedIn)
                {
                    Master.GoToLoginPage();
                }
                else if ((Master.currentUser.CanAccessPage("create_user", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation) && (Master.currentUser.CanAccessPage("edit_user", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation))
                {
                    Master.GoToUsersPage();
                }

                if (Master.isInPersonalDB)
                {
                    UpdateGrid();
                }
            }
        }

        protected void rdgDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LogDebug(toDisplay); // Show that we have stuff coming in (UserData)
        }

        protected void txtSearchID_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchFirstName_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchLastName_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchEmailAddress_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchCity_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchState_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void txtSearchCountry_TextChanged(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        public bool CheckIfAllTextboxesClear()
        {
            bool toReturn = true;

            // Check if not clear
            if ((txtSearchID.Text != "") && (txtSearchID != null))
            {
                toReturn = false;
            }
            else if ((txtSearchFirstName.Text != "") && (txtSearchFirstName != null))
            {
                toReturn = false;
            }
            else if ((txtSearchLastName.Text != "") && (txtSearchLastName != null))
            {
                toReturn = false;
            }
            else if ((txtSearchEmailAddress.Text != "") && (txtSearchEmailAddress != null))
            {
                toReturn = false;
            }
            else if ((txtSearchCity.Text != "") && (txtSearchCity != null))
            {
                toReturn = false;
            }
            else if ((txtSearchState.Text != "") && (txtSearchState != null))
            {
                toReturn = false;
            }
            else if ((txtSearchCountry.Text != "") && (txtSearchCountry != null))
            {
                toReturn = false;
            }

            return toReturn;
        }

        public void ClearTextboxes()
        {
            txtSearchID.Text = "";
            txtSearchFirstName.Text = "";
            txtSearchLastName.Text = "";
            txtSearchEmailAddress.Text = "";
            txtSearchCity.Text = "";
            txtSearchState.Text = "";
            txtSearchCountry.Text = "";
        }

        public void LogDebug(string toDisplay)
        {
            lblTesting.Text = toDisplay;
            lblTesting.Visible = true;
        }

        void UpdateFilter()
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextboxes();
            udpSearchBar.Update();
            udpGrid.Update();
            lblTesting.Visible = false;
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            UpdateGrid();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (rdgDisplayGrid.SelectedItems.Count > 0)
            {
                // Get the currently selected ID
                GridDataItem gdi = rdgDisplayGrid.SelectedItems[0] as GridDataItem;
                int selectedID = int.Parse(gdi["usr_id"].Text);

                // Generate the http request
                string destination = Request.Url.AbsoluteUri;
                destination = new Regex(@"(.+\/).+").Match(destination).Groups[1].Value;
                destination += "?usr_id=" + selectedID.ToString();

                // Transfer the page
                Response.Redirect(destination, false);
            }
        }

        protected void btnNewValue_Click(object sender, EventArgs e)
        {
            // Generate the http request
            string destination = Request.Url.AbsoluteUri;
            destination = new Regex(@"(.+\/).+").Match(destination).Groups[1].Value;

            // Transfer the page
            Response.Redirect(destination, false);
        }

        protected void btnDeleteValue_Click(object sender, EventArgs e)
        {
            if (rdgDisplayGrid.SelectedItems.Count > 0)
            {
                // Get the currently selected ID
                GridDataItem gdi = rdgDisplayGrid.SelectedItems[0] as GridDataItem;
                int selectedID = int.Parse(gdi["usr_id"].Text);

                new SQLInterfaces.SQLInterface().SendCommand(UserData.DeleteRecordString(selectedID, Master.isInPersonalDB, Master.currentDB));
            }

            UpdateGrid();
        }

        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (rdgDisplayGrid.SelectedItems.Count > 0)
            {
                // Get the currently selected ID
                GridDataItem gdi = rdgDisplayGrid.SelectedItems[0] as GridDataItem;
                int selectedID = int.Parse(gdi["usr_id"].Text);

                new SQLInterfaces.SQLInterface().SendCommand(new UserData(selectedID, Master.isInPersonalDB, Master.currentDB).AddToUsersTableString(Master.isInPersonalDB, Master.currentDB));
            }

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