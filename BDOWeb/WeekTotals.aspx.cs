using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BDOWeb
{
    public partial class WeekTotals : Page
    {
        private string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dberror.Visible = false;
                PopulateTable();
            }
        }

        public void SendDropsBtn_Click(Object sender, EventArgs e)
        {
            PopulateTable();
        }

        private void PopulateTable()
        {

            DateTime baseDate = DateTime.Today;

            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(1);
            if (isBackup.Checked)
            {
                thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(8);
            }
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            string startUserDate = startdate.Text;
            string endUserDate = enddate.Text;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = @"SELECT users.username as nome, FORMAT(SUM(neidan*100000), '#,#') as neidan, FORMAT(SUM(goldcoin*100000), '#,#') as goldcoin, 
                                    FORMAT(SUM(otherdrops*100000000), '#,#') as otherdrops, FORMAT(SUM((neidan+goldcoin)*100000 + (otherdrops*100000000)), '#,#') as totale
                                    FROM drops, users
                                    WHERE infocrono BETWEEN @startweek AND @endweek
                                    AND users.userid = drops.userid"
                    };
                    cmd.CommandText += " GROUP BY users.username ORDER BY users.username ASC";
                    if (!String.IsNullOrEmpty(startUserDate))
                    {
                        cmd.Parameters.AddWithValue("@startweek", startUserDate + " 00:00:00");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@startweek", thisWeekStart);
                    }

                    if (!String.IsNullOrEmpty(endUserDate))
                    {
                        cmd.Parameters.AddWithValue("@endweek", endUserDate + " 23:59:59");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@endweek", thisWeekEnd);
                    }

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