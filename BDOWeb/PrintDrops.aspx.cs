using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public partial class PrintDrops : Page
    {
        const int PAGE_SECURITY_LEVEL = 2;
        DataTable dt = new DataTable();
        private string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            dberror.Visible = false;
            if (!IsPostBack)
            {
                PopulateTable();
            }
        }
        protected void grd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int permissionLevel = 0;
            try
            {
                Int32.TryParse(Session["roleID"].ToString(), out permissionLevel);
            }
            catch (Exception)
            {

            }
            if (Utils.GetPermissions(permissionLevel, PAGE_SECURITY_LEVEL))
            {
                GridViewRow row = (GridViewRow)grdContent.Rows[e.RowIndex];
                Label lbldeleteid = (Label)row.FindControl("lblID");
                string dropid = row.Cells[0].Text;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = @"delete from drops where dropid = @param1"
                    };
                    cmd.Parameters.AddWithValue("@param1", dropid);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                PopulateTable();
            }
            else
            {
                errorLabel.Text = "Non hai i permessi per eliminare i drop. Esegui il login.";
                dberror.Visible = true;
            }
        }
        public void SendDropsBtn_Click(Object sender, EventArgs e)
        {
            PopulateTable();
        }
        private void PopulateTable()
        {
            
            DateTime baseDate = DateTime.Today;

            var today = baseDate;
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(1);
            if (isBackup.Checked)
                thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(8);

            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = @"SELECT dropid, users.username as nome, infocrono, screenlink, neidan, goldcoin, otherdrops, 
                            FORMAT(SUM((neidan+goldcoin)*100000 + (otherdrops*100000000)), '#,#') as totale 
                            FROM drops, users WHERE infocrono BETWEEN @startweek AND @endweek
                            AND users.userid = drops.userid "
                    };
                    string userId = Utils.RecoverUserIdFromName(username.Text);
                    if(!String.IsNullOrEmpty(username.Text))
                    {
                        cmd.CommandText += " AND userid = @userID";

                    }
                    cmd.CommandText += " GROUP BY dropid, users.username, infocrono, screenlink, neidan, goldcoin, otherdrops";

                    cmd.Parameters.AddWithValue("@userID", userId);
                    cmd.Parameters.AddWithValue("@startweek", thisWeekStart);
                    cmd.Parameters.AddWithValue("@endweek", thisWeekEnd);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    grdContent.DataSource = dt;
                    grdContent.DataBind();
                }
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
            }
            catch (SqlException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Errore nel database. Controllare i file di log.";
                dberror.Visible = true;
            }
        }
    }
}