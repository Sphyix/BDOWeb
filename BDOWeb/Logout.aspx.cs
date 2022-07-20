using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteLogout();
        }
        public void ExecuteLogout()
        {
            Session.Clear();
            Response.Cookies["myCookie"].Expires = DateTime.Now.AddDays(-1);
            Page.Response.Redirect(Utils.RedirectHome());
        }
    }
}