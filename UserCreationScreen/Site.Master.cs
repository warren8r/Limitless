using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCreationScreen.SQLInterfaces;
using UserCreationScreen.SQLInterfaces.Permissions;

namespace UserCreationScreen
{
    public partial class SiteMaster : MasterPage
    {
        public const string LOGIN_PAGE = "~/CustomAccountPages/Login";
        public const string USER_PAGE = "~/CustomAccountPages/UserPage";

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        public Permissions currentUser
        {
            get { return (Session["currentUser"] != null) ? (Session["currentUser"] as Permissions) : new Permissions(); }
            set 
            { 
                Session["currentUser"] = value;
                Session["currentDB"] = (value != null) ? (value.usr_id as int?) : null;
            }
        }

        public int currentDB
        {
            get { return (Session["currentDB"] != null) ? (Convert.ToInt32(Session["currentDB"])) : -1; }
        }

        public bool isLoggedIn
        {
            get { return (Session["isLoggedIn"] != null) ? (Convert.ToBoolean(Session["isLoggedIn"])) : false; }
            set { Session["isLoggedIn"] = value; }
        }

        public bool isInPersonalDB
        {
            get { return (Session["isInPersonalDB"] != null) ? (Convert.ToBoolean(Session["isInPersonalDB"])) : false; }
            set { Session["isInPersonalDB"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (isLoggedIn)
                {
                    lblLoggedIn.Text = "Welcome " + currentUser.first_name;
                    lblLoggedIn.Visible = true;
                    btnLogout.Visible = true;
                    btnLogin.Visible = false;
                    btnEnterPrivateZone.Visible = true;

                    rdmNavigate_UpdateVisibility();
                }
                else
                {
                    lblLoggedIn.Visible = false;
                    btnLogout.Visible = false;
                    btnLogin.Visible = true;
                    rdmNavigate.Visible = false;
                    btnEnterPrivateZone.Visible = false;
                }

                if (isInPersonalDB)
                {
                    btnEnterPrivateZone.Text = "Enter Main Database";
                }
                else
                {
                    btnEnterPrivateZone.Text = "Enter Private Database";
                }
            }
        }

        void rdmNavigate_UpdateVisibility()
        {
            if (!isInPersonalDB)
            {
                rdmNavigate.Items[0].Items[0].Visible = (currentUser.CanAccessPage("create_user", isInPersonalDB) == PageAccessValidation.PassedValidation); // Create User
                rdmNavigate.Items[0].Items[1].Visible = (currentUser.CanAccessPage("edit_user", isInPersonalDB) == PageAccessValidation.PassedValidation); // View Users
                rdmNavigate.Items[0].Items[2].Visible = (currentUser.CanAccessPage("edit_user", isInPersonalDB) == PageAccessValidation.PassedValidation); // Edit Users

                rdmNavigate.Items[1].Items[0].Visible = (currentUser.CanAccessPage("create_role", isInPersonalDB) == PageAccessValidation.PassedValidation); // Create Role
                rdmNavigate.Items[1].Items[1].Visible = (currentUser.CanAccessPage("edit_role", isInPersonalDB) == PageAccessValidation.PassedValidation); // Edit Role
                rdmNavigate.Items[1].Items[2].Visible = (currentUser.CanAccessPage("assign_role", isInPersonalDB) == PageAccessValidation.PassedValidation); // Assign Role
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            GoToLoginPage();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            currentUser = null;
            isLoggedIn = false;
            isInPersonalDB = false;
            GoToLoginPage();
        }

        public void GoToLoginPage()
        {
            Response.Redirect(LOGIN_PAGE, true);
        }

        public void GoToUsersPage()
        {
            Response.Redirect(USER_PAGE, true);
        }

        protected void btnEnterPrivateZone_Click(object sender, EventArgs e)
        {
            isInPersonalDB = !isInPersonalDB;
            Response.Redirect(Request.RawUrl, false);
        }
    }
}