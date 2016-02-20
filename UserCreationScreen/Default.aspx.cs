using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCreationScreen.SQLInterfaces;
using UserCreationScreen.SQLInterfaces.Storage;
using Telerik.Web.UI;

// NOTE: The textboxes, after having their text set through SetToExistingRecord, will not update their Text member when typed into...

namespace UserCreationScreen
{
    public partial class _Default : Page
    {
        // Public Constants

        // Public Variables
        public SQLInterface sqlInterface = new SQLInterface();

        // Private Constants

        // Private Variables
        private int usr_id = -1;
        private bool isRegisterMode = false;

        private bool isPasswordValidated
        {
            get { return (txtPassword.Text == txtPasswordValidate.Text); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // When we are passed a number through the uri, edit the value
            if (Request.QueryString["usr_id"] != null)
            {
                if (!this.IsPostBack) // Make sure that I don't overwrite previous data
                    SetToExistingRecord(int.Parse(Request.QueryString["usr_id"]));
                else
                    usr_id = int.Parse(Request.QueryString["usr_id"]); // Ensure that the page remembers that it is update and not new entry

                lblTitle.Text = "Edit User " + usr_id;
                btnClear.Text = "Reset";
            }
            else if (Request.QueryString["register_user"] != null)
            {
                isRegisterMode = true;
                lblTitle.Text = "Welcome New User, Input Your Information Below";
            }
            else
                lblTitle.Text = "Create New User Entry";

            if (!this.IsPostBack)
            {
                // If we aren't logged in
                if (!Master.isLoggedIn && !isRegisterMode)
                {
                    Master.GoToLoginPage();
                }
                else if (!Master.isInPersonalDB)
                {
                    // If we are logged in and we don't have access to this page (creation)
                    if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("create_user", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation) && (usr_id <= -1))
                    {
                        Master.GoToUsersPage();
                    }

                    // If we are logged in and we don't have access to this page (editing)
                    if (Master.isLoggedIn && (Master.currentUser.CanAccessPage("edit_user", Master.isInPersonalDB) != SQLInterfaces.Permissions.PageAccessValidation.PassedValidation) && (usr_id >= 0))
                    {
                        Master.GoToUsersPage();
                    }
                }                

                if (usr_id >= 0)
                {
                    string[,] usernamePassword = GetUsernamePassword();

                    // Check if we should display the username and password
                    if ((usernamePassword == null))
                    {
                        pnlUsernamePassword.Visible = true;
                        pnlUsernamePassword.Style.Add("display", "inline");
                    }
                    else if ((usernamePassword.GetLength(0) <= 0))
                    {
                        pnlUsernamePassword.Visible = true;
                        pnlUsernamePassword.Style.Add("display", "inline");
                    }
                    else if ((Master.currentUser.usr_id == usr_id))
                    {
                        pnlUsernamePassword.Visible = true;
                        pnlUsernamePassword.Style.Add("display", "inline");
                        txtUsername.Text = usernamePassword[0, 1];
                    }
                    else
                    {
                        pnlUsernamePassword.Visible = false;
                    }

                    if (Master.isInPersonalDB)
                    {
                        pnlUsernamePassword.Visible = false;
                    }
                }
                else if (!Master.isInPersonalDB)
                {
                    pnlUsernamePassword.Visible = true;
                    pnlUsernamePassword.Style.Add("display", "inline");
                }
            }

            Page.Title = lblTitle.Text;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UserData usrData = new UserData();

            if (!pnlUsernamePassword.Visible || isPasswordValidated)
            {
                // When we aren't passed a number, create new user
                if (usr_id < 0)
                {
                    if ((usrData.ValidatePhoneNumber(txtPhoneNumber.TextWithLiterals)) && (usrData.ValidateEmailAddress(txtEmailAddress.Text))) // Validate the phone number and email
                    {
                        usrData = SetUserToPage();

                        sqlInterface.SendCommand(usrData.AddToUsersTableString(Master.isInPersonalDB, Master.currentDB), 30);

                        // Get the freshly created usr_id
                        object[,] values = sqlInterface.GetQuery("SELECT usr_id FROM TestDatabase.dbo.Users");
                        usr_id = Convert.ToInt32(values[(values.GetLength(0)-1), 0]);

                        if ((txtPassword.Text != null) && (txtPassword.Text != "") && !Master.isInPersonalDB)
                            sqlInterface.SendCommand(new string[] { "INSERT INTO TestDatabase.dbo.LoginInformation VALUES (@0, @1, @2)", usr_id.ToString(), txtUsername.Text, txtPassword.Text });

                        sqlInterface.SendCommand("INSERT INTO TestDatabase.dbo.UsersRoles VALUES ( " + usr_id + ", 2)");

                        lblTesting.Text = "You have successfully submitted a new user!";
                        CreatePersonalArea();
                        GoToSelectUser();
                    }
                    else
                    {
                        string toDisplay = "You have improperly formatted input in ";

                        if ((!usrData.ValidatePhoneNumber(txtPhoneNumber.Text)) && (!usrData.ValidateEmailAddress(txtEmailAddress.Text)))
                            toDisplay += "Phone Number and Email Address";
                        else if (!usrData.ValidatePhoneNumber(txtPhoneNumber.Text))
                            toDisplay += "Phone number";
                        else if (!usrData.ValidateEmailAddress(txtEmailAddress.Text))
                            toDisplay += "Email Address";

                        lblTesting.Text = toDisplay;
                    }
                }
                // When we are passed a number, edit the user
                else
                {
                    if ((usrData.ValidatePhoneNumber(txtPhoneNumber.TextWithLiterals)) && (usrData.ValidateEmailAddress(txtEmailAddress.Text))) // Validate the phone number and email
                    {
                        usrData = SetUserToPage();

                        sqlInterface.SendCommand(usrData.UpdateUserTableString(Master.isInPersonalDB, Master.currentDB), 30);
                        if (!Master.isInPersonalDB)
                        {
                            if (pnlUsernamePassword.Visible && (GetUsernamePassword() == null)) sqlInterface.SendCommand(new string[] { "INSERT INTO TestDatabase.dbo.LoginInformation VALUES (@0, @1, @2)", usr_id.ToString(), txtUsername.Text, txtPassword.Text });
                            else if (pnlUsernamePassword.Visible && (txtPassword.Text != null) && (txtPassword.Text != "")) sqlInterface.SendCommand(new string[] { "UPDATE TestDatabase.dbo.LoginInformation SET usr_id=@0, username=@1, password=@2 WHERE usr_id=@0", usr_id.ToString(), txtUsername.Text, txtPassword.Text });
                            else if (pnlUsernamePassword.Visible && (txtUsername != null) && (txtUsername.Text != "")) sqlInterface.SendCommand(new string[] { "UPDATE TestDatabase.dbo.LoginInformation SET usr_id=@0, username=@1 WHERE usr_id=@0", usr_id.ToString(), txtUsername.Text });
                        }

                        CreatePersonalArea();
                        GoToSelectUser();
                    }
                    else
                    {
                        string toDisplay = "You have improperly formatted input in ";

                        if ((!usrData.ValidatePhoneNumber(txtPhoneNumber.Text)) && (!usrData.ValidateEmailAddress(txtEmailAddress.Text)))
                            toDisplay += "Phone Number and Email Address";
                        else if (!usrData.ValidatePhoneNumber(txtPhoneNumber.Text))
                            toDisplay += "Phone number";
                        else if (!usrData.ValidateEmailAddress(txtEmailAddress.Text))
                            toDisplay += "Email Address";

                        lblTesting.Text = toDisplay;
                    }
                }
            }
            else
            {
                lblTesting.Text = "Your password did not validate. Please ensure that both your password and validate password are equal.";
                txtPassword.Text = "";
                txtPasswordValidate.Text = "";
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if (usr_id < 0)
            {
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtPhoneNumber.Text = "";
                txtEmailAddress.Text = "";
                txtCity.Text = "";
                txtState.Text = "";
                txtZip.Text = "";
                txtCountry.Text = "";

                if (pnlUsernamePassword.Visible)
                {
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    txtPasswordValidate.Text = "";
                }
            }
            else
                SetToExistingRecord(usr_id);

            udpContent.Update();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtPhoneNumber.Text = "";
            txtEmailAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtZip.Text = "";
            txtCountry.Text = "";

            lblTesting.Text = "";

            // Send back to the view page
            GoToSelectUser();
        }

        private void SetToExistingRecord(int userId)
        {
            UserData usr = new UserData(userId, Master.isInPersonalDB, Master.currentDB);
            usr_id = usr.id;
            txtFirstName.Text = usr.firstName;
            txtLastName.Text = usr.lastName;
            txtAddress1.Text = usr.address1;
            txtAddress2.Text = usr.address2;
            txtAddress3.Text = usr.address3;
            txtPhoneNumber.TextWithLiterals = usr.phoneNumber;
            txtEmailAddress.Text = usr.emailAddress;
            txtCity.Text = usr.city;
            txtState.Text = usr.state;
            txtZip.Text = usr.zipCode;
            txtCountry.Text = usr.country;

            if (pnlUsernamePassword.Visible)
            {
                string[,] usernamePassword = GetUsernamePassword();
                txtUsername.Text = usernamePassword[0, 1];
                txtPassword.Text = usernamePassword[0, 2];
                txtPasswordValidate.Text = usernamePassword[0, 2];
            }
        }

        private UserData SetUserToPage()
        {
            UserData toReturn = new UserData();

            if (usr_id >= 0) toReturn.id = usr_id; // When the id number is passed to us, pass it to the new UserData
            toReturn.firstName = txtFirstName.Text;
            toReturn.lastName = txtLastName.Text;
            toReturn.address1 = txtAddress1.Text;
            toReturn.address2 = ((txtAddress2.Text == "") || (txtAddress2.Text == null)) ? txtAddress2.Text : null;
            toReturn.address3 = ((txtAddress3.Text == "") || (txtAddress3.Text == null)) ? txtAddress3.Text : null;
            toReturn.phoneNumber = txtPhoneNumber.TextWithLiterals;
            toReturn.emailAddress = txtEmailAddress.Text;
            toReturn.city = txtCity.Text;
            toReturn.state = txtState.Text;
            toReturn.zipCode = txtZip.Text;
            toReturn.country = txtCountry.Text;

            return toReturn;
        }

        private void GoToSelectUser(string addToEnd = "")
        {
            // Generate the http request
            string destination = Request.Url.AbsoluteUri;
            destination = new System.Text.RegularExpressions.Regex(@"(.+\/).+").Match(destination).Groups[1].Value;
            destination += "SelectUser.aspx" + addToEnd;

            // Tell the browser to send us to our destination
            Response.Redirect(destination, false);
        }

        private string[,] GetUsernamePassword()
        {
            object[,] incoming;

            if (Master.isInPersonalDB)
                incoming = sqlInterface.GetQuery("SELECT * FROM TestDatabase2.dbo.LoginInformation" + Master.currentDB + " WHERE usr_id=" + usr_id);
            else
                incoming = sqlInterface.GetQuery("SELECT * FROM TestDatabase.dbo.LoginInformation WHERE usr_id=" + usr_id);

            string[,] toReturn = null;

            if (incoming != null)
            {
                toReturn = new string[incoming.GetLength(0), incoming.GetLength(1)];

                for (int i = 0; i < incoming.GetLength(0); i++)
                {
                    for (int j = 0; j < incoming.GetLength(1); j++)
                    {
                        toReturn[i, j] = incoming[i, j] as string;
                    }
                }

                return toReturn;
            }
            else
            {
                return null;
            }
        }

        private void CreatePersonalArea()
        {
            if (!Master.isInPersonalDB) sqlInterface.SendCommand("EXEC TestDatabase2.[dbo].[sp_generateUserDb] @username=" + usr_id);
        }
    }
}