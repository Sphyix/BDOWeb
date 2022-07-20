using NLog;
using System;
using System.Web;
using System.Web.UI;

namespace BDOWeb
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try {
                HttpCookie myCookie = Request.Cookies["myCookie"];
                string username = myCookie.Values["username"].ToString();
                string password = myCookie.Values["password"].ToString();
                LoginStuff loginMethods = new LoginStuff();
                Loggin login = new Loggin();
                login.LoginWithCookies(username, password);
            }
            catch { }

            string value = Session["username"] as string;
            if (String.IsNullOrEmpty(value))
            {
                loggedIn.Visible = false;
                logout.Visible = false;
                addNodewar.Visible = false;
            }
            else
            {
                navbarUsername.Text = value;
                login.Visible = false;
            }
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;

        }
    }
}