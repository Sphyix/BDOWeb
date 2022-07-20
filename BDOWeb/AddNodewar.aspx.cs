using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public partial class AddNodewar : System.Web.UI.Page
    {
        SiteMaster master = new SiteMaster();
        const int PAGE_SECURITY_LEVEL = 2;
        private string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            int permissionLevel = 0;
            try
            {
                Int32.TryParse(Session["roleID"].ToString(), out permissionLevel);
            }
            catch (Exception)
            {

            }
            if (!Utils.GetPermissions(permissionLevel, PAGE_SECURITY_LEVEL))
            {
                Page.Response.Redirect(Utils.RedirectDenied());
            }
            if (!IsPostBack)
            {
                dberror.Visible = false;
                success.Visible = false;
                gvBind();
            }
        }

        public void gvBind()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlDataAdapter dap = new SqlDataAdapter("select * from users", conn);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    gvstatus.DataSource = ds;
                    gvstatus.DataBind();
                }
            }
            catch (Exception) { }
        }
        public void AddNodeBtn_Click(Object sender, EventArgs e)
        {
            dberror.Visible = false;
            try
            {
                string tier = GetTier();

                

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = CreateInsertNewNode(conn, tier);
                    int queryRes = cmd.ExecuteNonQuery();
                    QuerySuccess(queryRes);
                    int nodewarId = Utils.RecoverLastNodewar(conn);
                    AddPartecipatingUsers(nodewarId);

                }
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Selezionare un tier";
                dberror.Visible = true;
            }
            catch (SqlException ex)
            {
                Errors.LogError(ex);
                errorLabel.Text = "Errore nel database. Controllare i file di log.";
                dberror.Visible = true;
            }
        }

        private string GetTier()
        {
            string tier = "-1";
            if (Convert.ToInt32(nodeTier.SelectedValue) != -1)
            {
                tier = nodeTier.SelectedValue;
            }
            else
            {
                errorLabel.Text = "Selezionare il tier della nodewar";
                dberror.Visible = true;
                throw new ArgumentException("Nessun tier selezionato");
            }
            return tier;
        }
        private void QuerySuccess(int queryRes)
        {
            Errors.LogQuery(queryRes);
            success.Visible = true;

        }
        private SqlCommand CreateInsertNewNode(SqlConnection conn, string tier)
        {
            string datetime = DateTime.Now.ToString("yyyy-MM-dd");
            SqlCommand cmd = new SqlCommand
            {
                Connection = conn,
                CommandText = "INSERT INTO nodewars VALUES(@param1, null, @param2)",
            };
            cmd.Parameters.AddWithValue("@param1", tier);
            cmd.Parameters.AddWithValue("@param2", datetime);
            return cmd;
        }
        private void AddPartecipatingUsers( int nodewarId)
        {
            foreach (GridViewRow row in gvstatus.Rows)
            {
                CheckBox chk = row.Cells[2].Controls[1] as CheckBox;
                string userid = row.Cells[0].Text;
                if (chk != null && chk.Checked)
                {
                    bool partecipated = true;
                    InsertPartecipationOnDb( userid, nodewarId, partecipated);
                }
                else if (chk != null && !chk.Checked)
                {
                    bool partecipated = false;
                    InsertPartecipationOnDb( userid, nodewarId, partecipated);
                }
            }
        }
        private void InsertPartecipationOnDb( string userid, int nodewarId, bool partecipated)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"INSERT INTO nodewarattendance VALUES(@nodeID, @userID, @partecipation)",
                };
                cmd.Parameters.AddWithValue("@nodeID", nodewarId);
                cmd.Parameters.AddWithValue("@userID", userid);
                if (partecipated)
                {
                    cmd.Parameters.AddWithValue("@partecipation", 1);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@partecipation", 0);
                }
                int queryRes = cmd.ExecuteNonQuery();
                QuerySuccess(queryRes);
            }
        }
        private bool IsAtLeastOneChecked()
        {
            foreach (GridViewRow row in gvstatus.Rows)
            {
                CheckBox chk = row.Cells[4].Controls[1] as CheckBox;
                if (chk != null && chk.Checked)
                {
                    return true;
                }
            }
            return false;
        }
    }
}