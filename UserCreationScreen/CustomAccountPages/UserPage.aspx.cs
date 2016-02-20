using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserCreationScreen.CustomAccountPages
{
    public partial class UserPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Master.isLoggedIn)
                Master.GoToLoginPage();

            if (Master.isInPersonalDB)
            {
                if (!lblInUserDb.Visible)
                {
                    lblInUserDb.Visible = true;
                }
            }
            else
                lblInUserDb.Visible = false;
        }
    }
}