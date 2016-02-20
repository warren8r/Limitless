using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UserCreationScreen
{
    public partial class Site_Mobile : System.Web.UI.MasterPage
    {
        public static Site_Mobile instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            instance = this;
            
        }
    }
}