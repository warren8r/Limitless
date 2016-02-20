using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCreationScreen.SQLInterfaces.Permissions;

namespace UserCreationScreen.RolePages
{
    public partial class ModifyRole : System.Web.UI.Page
    {
        // Public Variables

        // Private Variables
        private int permission_id = -1;
        private SQLInterfaces.SQLInterface sqlInterface = new SQLInterfaces.SQLInterface();

        protected void Page_Load(object sender, EventArgs e)
        {
            // If we are on a bad page, don't do anything
            if (cblPermissions != null)
            {
                if (!this.IsPostBack)
                {
                    // If we are logged in and we don't have access to this page (create)
                    if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("create_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation) && (permission_id <= -1))
                    {
                        Master.GoToUsersPage();
                    }
                    // If we are logged in and we don't have access to this page (edit)
                    else if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("edit_role", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation) && (permission_id >= 0))
                    {
                        Master.GoToUsersPage();
                    }
                    // If we aren't logged in
                    else if (!Master.isLoggedIn)
                    {
                        Master.GoToLoginPage();
                    }

                    // Populate the CheckBoxList
                    object[,] itemsToPopulate = sqlInterface.GetQuery("EXEC [TestDatabase].[dbo].[GetColumnNames] @TableName = N'Roles'");

                    for (int i = 2; i < itemsToPopulate.GetLength(0); i++)
                    {
                        if (itemsToPopulate[i, 0] as string != null)
                            cblPermissions.Items.Add(itemsToPopulate[i, 0] as string);
                        else
                            cblPermissions.Items.Add("");
                    }
                }

                // Determine if we are editing or creating
                if (Request.QueryString["role_id"] != null)
                {
                    permission_id = int.Parse(Request.QueryString["role_id"]);
                    this.Title = "Edit Role " + permission_id;
                    btnClear.Text = "Reset";
                    if (!this.IsPostBack) btnClear_Click(btnClear, new EventArgs());
                }
                else
                {
                    this.Title = "New Role";
                }

                lblTitle.Text = this.Title;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (permission_id == -1)
            {
                // Generate the INSERT INTO command
                string[] query = new string[2];
                if (!Master.isInPersonalDB) query[0] = "INSERT INTO TestDatabase.dbo.Roles VALUES(@0,";
                else query[0] = "INSERT INTO TestDatabase2.dbo.Roles" + Master.currentDB + " VALUES(@0,";
                query[1] = txtRoleName.Text;

                for (int i = 0; i < cblPermissions.Items.Count; i++)
                {
                    if (cblPermissions.Items[i].Selected)
                    {
                        query[0] += "1";
                    }
                    else
                    {
                        query[0] += "0";
                    }

                    if (i != (cblPermissions.Items.Count - 1))
                        query[0] += ",";
                }

                query[0] += ")";

                // Send the request
                sqlInterface.SendCommand(query);
            }
            else
            {
                string[] query = new string[2];
                if (!Master.isInPersonalDB) query[0] = "UPDATE TestDatabase.dbo.Roles SET role_name=@0,";
                else query[0] = "UPDATE TestDatabase2.dbo.Roles" + Master.currentDB + " SET role_name=@0,";
                query[1] = txtRoleName.Text;

                for (int i = 0; i < cblPermissions.Items.Count; i++)
                {
                    query[0] += cblPermissions.Items[i].Value + "=";

                    if (cblPermissions.Items[i].Selected)
                    {
                        query[0] += "1";
                    }
                    else
                    {
                        query[0] += "0";
                    }

                    if (i != (cblPermissions.Items.Count - 1))
                        query[0] += ",";
                }

                query[0] += " WHERE role_id=" + permission_id;

                // Send the request
                sqlInterface.SendCommand(query);
            }

            GoToViewRoles();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (permission_id == -1)
            {
                txtRoleName.Text = "";

                for (int i = 0; i < cblPermissions.Items.Count; i++)
                {
                    cblPermissions.Items[i].Selected = false;
                }
            }
            else
            {
                if (!Master.isInPersonalDB) SetPageToRole(sqlInterface.GetPermissionsQuery("SELECT * FROM TestDatabase.dbo.roles WHERE role_id=" + permission_id)[0]);
                else SetPageToRole(sqlInterface.GetPermissionsQuery("SELECT * FROM TestDatabase2.dbo.roles" + Master.currentDB + " WHERE role_id=" + permission_id)[0]);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            GoToViewRoles();
        }

        public void SetPageToRole(Permissions role)
        {
            txtRoleName.Text = role.role_name;

            for (int i = 0; i < cblPermissions.Items.Count; i++)
            {
                cblPermissions.Items[i].Selected = role.pagePermissions[i].canAccess;
            }
        }

        private void GoToViewRoles()
        {
            // Generate the requestion
            string toGoTo = Request.Url.AbsoluteUri;
            toGoTo = (new System.Text.RegularExpressions.Regex(@"(.+\/).+").Match(toGoTo).Groups[1].Value);
            toGoTo += "ViewRoles";

            // Send Request
            Response.Redirect(toGoTo);
        }
    }
}