using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace BDOWeb
{
    public partial class AddDrops : Page
    {
        SiteMaster master = new SiteMaster();
        private string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dberror.Visible = false;
                success.Visible = false;
            }

        }
        public void SendDropsBtn_Click(Object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string userid = Utils.RecoverUserIdFromName(username.Text);
                    SqlCommand cmd = CreateInsertNewDrop(conn, userid);
                    int queryRes = cmd.ExecuteNonQuery();
                    QuerySuccess(queryRes);
                }
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Errore nell'inserimento: l'utente " + username.Text + " non esiste.";
                dberror.Visible = true;
            }
            catch (SqlException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Errore nel database. Controllare i file di log.";
                dberror.Visible = true;
            }
        }
        private void QuerySuccess(int queryRes)
        {
            Errors.LogQuery(queryRes);
            success.Visible = true;
            username.Text = "";
            screenshotlink.Text = "";
            neidan.Text = "";
            goldcoins.Text = "";
            otherdrops.Text = "";
        }
        private SqlCommand CreateInsertNewDrop(SqlConnection conn, string userid)
        {
            SqlCommand cmd = new SqlCommand
            {
                Connection = conn,
                CommandText = "INSERT INTO DROPS (infocrono, screenlink, neidan, goldcoin, otherdrops, isbackup, userid) " +
                                     "VALUES (@infocrono, @screenlink, @neidan, @goldcoins, @otherdrops, @isbackup, @userid)",
            };
            cmd.Parameters.AddWithValue("@screenlink", screenshotlink.Text);
            cmd.Parameters.AddWithValue("@neidan", neidan.Text);
            cmd.Parameters.AddWithValue("@goldcoins", goldcoins.Text);
            cmd.Parameters.AddWithValue("@otherdrops", otherdrops.Text);
            if (isBackup.Checked)
            {
                cmd.Parameters.AddWithValue("@isbackup", 1);
                cmd.Parameters.AddWithValue("@infocrono", DateTime.Now.AddDays(8));
            }
            else
            {
                cmd.Parameters.AddWithValue("@isbackup", 0);
                cmd.Parameters.AddWithValue("@infocrono", DateTime.Now);
            }
            if (userid != "")
            {
                cmd.Parameters.AddWithValue("@userid", userid);
            }
            else
            {
                throw new ArgumentException();
            }
            return cmd;
        }
    }
}
