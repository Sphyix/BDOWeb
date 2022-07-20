using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public class Utils : SiteMaster
    {
        private static string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;

        public static string RedirectHome()
        {
            return "/AddDrops";
        }

        public static string RedirectDenied()
        {
            return "/AccessDenied";
        }

        public static string RecoverUserIdFromName(string username)
        {
            string userid = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand recoverNameId = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT userid FROM USERS WHERE username = @param1"
                };
                recoverNameId.Parameters.AddWithValue("@param1", username);

                using (SqlDataReader reader = recoverNameId.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userid = Convert.ToString(reader.GetInt32(0));
                    }
                }
            }
            return userid;
        }

        public static int RecoverLastNodewar(SqlConnection conn)
        {
            int nodewarId = -1;
            using (conn)
            {

                SqlCommand recoverNameId = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT TOP 1 nodewarid FROM nodewars ORDER BY nodewarid DESC"
                };

                using (SqlDataReader reader = recoverNameId.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nodewarId = reader.GetInt32(0);
                    }
                }
            }
            if (nodewarId > 0)
            {
                return nodewarId;
            }
            else
            {
                throw new ArgumentException("L'ultima nodewar non è stata trovata");
            }
        }

        public static bool GetPermissions(int permissionLevel, int pageRequiredLevel)
        {
            try
            {
                if (pageRequiredLevel < permissionLevel || permissionLevel == 0)
                {
                    throw new UnauthorizedAccessException("Non hai i permessi per visualizzare questa pagina.");
                }
                else if (pageRequiredLevel >= permissionLevel)
                {
                    return true;
                }
                return false;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Errors.LogNoPerms("Ip: " + GetIpAddress() + " " + ex);
                return false;
            }
        }

        public static string GetIpAddress()  // Get IP Address
        {
            string ip = "";
            IPHostEntry ipEntry = Dns.GetHostEntry(GetCompCode());
            IPAddress[] addr = ipEntry.AddressList;
            ip = addr[1].ToString();
            return ip;
        }

        public static string GetCompCode()  // Get Computer Name
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }

        public static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public static int GetNumberOfUsers()
        {
            int result = 0;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"SELECT COUNT(*) FROM users"
                };
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return result;
        }

        public void ShowError(string error)
        {
            Label lblMaster = (Label)Master.FindControl("errorLabel");
            lblMaster.Text = error;
            ((System.Web.UI.HtmlControls.HtmlGenericControl)Page.Master.FindControl("dberror")).Visible = true;
        }



        public void ShowSuccess(string success)
        {
            Label lblMaster = (Label)Master.FindControl("successLabel");
            
        }
    }
}