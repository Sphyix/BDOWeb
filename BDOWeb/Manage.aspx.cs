using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public partial class Manage : System.Web.UI.Page
    {
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

            if (!Page.IsPostBack)
            {
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
        protected void gvstatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvstatus.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("lblID");
            string userid = row.Cells[0].Text;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"delete from users where userid = @param1"
                };
                cmd.Parameters.AddWithValue("@param1", userid);
                conn.Open();
                cmd.ExecuteNonQuery();
                
            }
            gvBind();
        }
        protected void gvstatus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvstatus.EditIndex = e.NewEditIndex;
            gvBind();
        }
        protected void gvstatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)gvstatus.Rows[e.RowIndex];
            Label lblID = (Label)row.FindControl("lblID");
            //TextBox txtname=(TextBox)gr.cell[].control[];  
            TextBox userid = (TextBox)row.Cells[0].Controls[0];
            TextBox textName = (TextBox)row.Cells[1].Controls[0];
            gvstatus.EditIndex = -1;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"update users set username = @param1 where userid = @param2"
                };
                cmd.Parameters.AddWithValue("@param1", textName.Text);
                cmd.Parameters.AddWithValue("@param2", userid.Text);
                cmd.ExecuteNonQuery();
            }
            gvBind();
        }
        protected void gvstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvstatus.PageIndex = e.NewPageIndex;
            gvBind();
        }
        protected void gvstatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvstatus.EditIndex = -1;
            gvBind();
        }
        public void BtnClick_AddUser(Object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand addUser = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"insert into users values(@param1)"
                };
                conn.Open();
                addUser.Parameters.AddWithValue("@param1", addusername.Text);
                addUser.ExecuteNonQuery();
            }
            gvBind();
            addusername.Text = "";        }
    }
}