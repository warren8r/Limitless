using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserCreationScreen.CustomAccountPages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master.isLoggedIn)
            {

            }

            if (!this.IsPostBack)
                txtUsername.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string[] toSend = new string[3];
            toSend[0] = "EXEC TestDatabase.dbo.sp_DoesLoginExist @username=@0, @password=@1";
            toSend[1] = txtUsername.Text;
            toSend[2] = txtPassword.Text;

            int result = Convert.ToInt32(new SQLInterfaces.SQLInterface().GetQuery(toSend)[0,0]);

            if (result > 0)
            {
                Master.currentUser = SQLInterfaces.Permissions.Permissions.GetPermissionsFromUser(result);
                Master.isLoggedIn = true;
                Response.Redirect("~/CustomAccountPages/UserPage", false);
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/?register_user=1", false);
        }
    }
}