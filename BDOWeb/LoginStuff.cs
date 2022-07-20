using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace BDOWeb
{
    public class LoginStuff : SiteMaster
    {

        

        private static string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;

    }
}