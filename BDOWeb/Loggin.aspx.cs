using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public partial class Loggin : Page
    {
        private static string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;

        public class UserDetails
        {
            private string userID;
            private string username;
            private string password;
            private int roleID;
            public UserDetails(string userID, string username, string password, int roleID)
            {
                this.userID = userID;
                this.username = username;
                this.password = password;
                this.roleID = roleID;
            }
            public UserDetails() { }
            public string UserID()
            {
                return userID;
            }
            public string Username()
            {
                return username;
            }
            public string Password()
            {
                return password;
            }
            public int RoleID()
            {
                return roleID;
            }
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            string value = Session["username"] as string;
            if(!String.IsNullOrEmpty(value))
            {
                Page.Response.Redirect(Utils.RedirectHome());
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            UserDetails user;
            LoginStuff loginMethods = new LoginStuff();
            bool setCookies = RememberMe.Checked;
            try
            {
                string username = Email.Text;
                string password = Utils.Hash(Password.Text);
                user = ExecuteLogin(username, password);
                if (String.IsNullOrEmpty(user.Username()))
                    throw new ArgumentException();

                else
                {
                    SetLoginSession(user);
                    if (setCookies)
                        SetLoginCookies(user);
                }
                Page.Response.Redirect(Utils.RedirectHome());
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Errore nel login: L'utente " + Email.Text + " non esiste oppure la password è errata.";
                dberror.Visible = true;
            }
            catch (SqlException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Errore su database. Controllare il file di log.";
                dberror.Visible = true;
            }

        }

        public void LoginWithCookies(string username, string password)
        {
            try
            {
                UserDetails usr = ExecuteLogin(username, password);
                if (usr.Username() == "")
                    throw new ArgumentException();
                SetLoginSession(usr);
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
            }
            catch (SqlException ex)
            {
                Errors.LogError(ex);
            }
        }

        public static UserDetails ExecuteLogin(string username, string password)
        {
            UserDetails user = new UserDetails();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand loginChecker = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT * FROM websiteusers WHERE username = @param1 AND password = @param2"
                };
                loginChecker.Parameters.AddWithValue("@param1", username);
                loginChecker.Parameters.AddWithValue("@param2", password);
                using (SqlDataReader reader = loginChecker.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new UserDetails(Convert.ToString(reader.GetInt32(0)), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
                    }
                }
            }

            return user;
        }

        public void SetLoginSession(UserDetails usr)
        {
            Session["userID"] = usr.UserID();
            Session["username"] = usr.Username();
            Session["roleID"] = usr.RoleID();
        }
        public void SetLoginCookies(UserDetails usr)
        {
            HttpCookie myCookie = new HttpCookie("myCookie");
            myCookie.Values.Add("username", usr.Username());
            myCookie.Values.Add("password", usr.Password());
            myCookie.Expires = DateTime.Now.AddYears(12);
            Response.Cookies.Add(myCookie);
        }
    }
}