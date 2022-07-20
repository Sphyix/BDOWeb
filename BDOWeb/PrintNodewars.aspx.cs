using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;

namespace BDOWeb
{
    public partial class PrintNodewars : System.Web.UI.Page
    {
        const int PAGE_SECURITY_LEVEL = 2;
        private string connString = ConfigurationManager.ConnectionStrings["myConn"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dberror.Visible = false;
                success.Visible = false;
                gvBindNodes();
                if (!Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
                {
                    editPartecipations.Visible = false;
                    showResults.CssClass = "confirmButton";
                }
            }
        }
        public void gvBindNodes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlDataAdapter dap = new SqlDataAdapter(@"select nodewarid, nodewartier, FORMAT(money, '#,#') as money, FORMAT(data, 'dd-MM-yyyy') 
                                                        as data from nodewars", conn);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    gvstatus.DataSource = ds;
                    gvstatus.DataBind();
                }
            }
            catch (Exception ex)
            {
                Errors.LogError(ex);
            }
        }
        public void gvBindEditPartecipations()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandText = @"select users.username, nodewarattendance.ispartecipate
                                                from users, nodewarattendance
                                                where users.userid = nodewarattendance.userid
                                                AND nodewarattendance.nodewarid = @nodeID"
                };
                string nodeID = GetSelectedNodes();
                nodeID = nodeID.Remove(nodeID.Length - 2);
                cmd.Parameters.AddWithValue("@nodeID", nodeID);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dap.Fill(ds);
                gvpartecipationsEditor.DataSource = ds;
                gvpartecipationsEditor.DataBind();
                foreach (GridViewRow row in gvpartecipationsEditor.Rows)
                {
                    CheckBox chk = row.Cells[2].Controls[1] as CheckBox;
                    if (row.Cells[1].Text == "True")
                    {
                        chk.Checked = true;
                    }

                }
            }

        }

        public void gvBindUserNodes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = @"SELECT users.username, 
                            SUM(case when nodewarattendance.ispartecipate = 1 then 1 else 0 end) as partecipations
                            FROM nodewarattendance, users
                            WHERE nodewarattendance.userid = users.userid
                            AND nodewarattendance.nodewarid IN"
                    };
                    string nodeIds = GetSelectedNodes();
                    nodeIds = nodeIds.Remove(nodeIds.Length - 2);
                    cmd.CommandText += "(" + nodeIds + ")";
                    cmd.CommandText += " GROUP BY users.username";

                    SqlCommand partecipations = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = @"SELECT SUM(case when nodewarattendance.ispartecipate = 1 then 1 else 0 end) as partecipations
                                FROM nodewarattendance
                                WHERE nodewarattendance.nodewarid IN "
                    };
                    partecipations.CommandText += "(" + nodeIds + ")";
                    int totalPartecipations = Convert.ToInt32(partecipations.ExecuteScalar());
                    
                    SqlCommand money = new SqlCommand
                    {
                        Connection = conn,
                        CommandText = @"SELECT SUM(nodewars.money) as money
                                FROM nodewars
                                WHERE nodewars.nodewarid IN "
                    };
                    money.CommandText += "(" + nodeIds + ")";
                    int totalMoney = Convert.ToInt32(money.ExecuteScalar());

                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    dap.Fill(ds);
                    ds.Tables[0].Columns.Add("money", typeof(string));

                    int numberOfUsers = Utils.GetNumberOfUsers();
                    for(int i = 0; i<numberOfUsers; i++)
                    {
                        int userPartecipations = Convert.ToInt32(ds.Tables[0].Rows[i][1]);
                        int userMoney = userPartecipations * (totalMoney / totalPartecipations);
                        ds.Tables[0].Rows[i][2] = String.Format("{0:n0}", userMoney);
                    }

                    usersResult.DataSource = ds;
                    usersResult.DataBind();
                }
            }
            catch (Exception ex)
            {
                Errors.LogError(ex);
            }
        }

        public void ShowPartecipationsBtn_Click(Object sender, EventArgs e)
        {
            try
            {
                if (!IsAtLeastOneChecked())
                {
                    throw new ArgumentException("Nessuna nodewar selezionata");
                }
                dberror.Visible = false;
                gvstatus.Visible = false;
                showResults.Visible = false;
                editPartecipations.Visible = false;
                usersResult.Visible = true;
                goBack.Visible = true;
                gvBindUserNodes();
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
                Errore("Selezionare almeno una nodewar.");
            }
        }

        public void EditPartecipationsBtn_Click(Object sender, EventArgs e)
        {
            try
            {
                if (!IsAtLeastOneChecked())
                {
                    throw new ArgumentException("Selezionare una nodewar");
                }
                if (!IsOnlyOneChecked())
                {
                    throw new ArgumentException("Selezionare soltanto una nodewar per modificare le presenze");
                }
                if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
                {
                    dberror.Visible = false;
                    gvstatus.Visible = false;
                    showResults.Visible = false;
                    editPartecipations.Visible = false;
                    usersResult.Visible = false;
                    goBack.Visible = false;
                    goBackEdit.Visible = true;
                    gobackCancelEdit.Visible = true;
                    gvpartecipationsEditor.Visible = true;
                    gvBindEditPartecipations();
                }
                else
                    throw new ArgumentException("Non hai i permessi per eseguire questa azione. Esegui il login.");
            }
            catch (ArgumentException ex)
            {
                Errors.LogError(ex);
                Errore(ex.Message);
            }
            catch (Exception ex)
            {
                Errors.LogError(ex);
            }
        }

        public void RefreshPageBtn_Click(Object sender, EventArgs e)
        {
            gvstatus.Visible = true;
            gvpartecipationsEditor.Visible = false;
            showResults.Visible = true;
            
            usersResult.Visible = false;
            goBack.Visible = false;
            goBackEdit.Visible = false;
            gobackCancelEdit.Visible = false;
            if(Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
            {
                editPartecipations.Visible = true;
            }
            gvBindNodes();
        }
        
        public void SaveAndRefreshBtn_Click(Object sender, EventArgs e)
        {
            SaveUpdatedPartecipations();
            gvstatus.Visible = true;
            gvpartecipationsEditor.Visible = false;
            showResults.Visible = true;
            if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
            {
                editPartecipations.Visible = true;
            }
            usersResult.Visible = false;
            goBack.Visible = false;
            goBackEdit.Visible = false;
            gobackCancelEdit.Visible = false;
            gvBindNodes();
        }

        public void SaveUpdatedPartecipations()
        {

            try
            {
                if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        string[] UserIds = GetUserIds();
                        string nodeID = GetSelectedNodes();
                        UserIds[0] = UserIds[0].Remove(UserIds[0].Length - 2);
                        UserIds[1] = UserIds[1].Remove(UserIds[1].Length - 2);
                        nodeID = nodeID.Remove(nodeID.Length - 2);


                        SqlCommand addPartecipatingUsers = new SqlCommand
                        {
                            Connection = conn,
                            CommandText = @"update nodewarattendance set ispartecipate = 1 
                                where nodewarid = @nodeID
                                and userid IN "
                        };
                        addPartecipatingUsers.CommandText += "(" + UserIds[0] + ")";
                        addPartecipatingUsers.Parameters.AddWithValue("@nodeID", nodeID);


                        SqlCommand removeNonPartecipatingUsers = new SqlCommand
                        {
                            Connection = conn,
                            CommandText = @"update nodewarattendance set ispartecipate = 0 
                                where nodewarid = @nodeID
                                and userid IN "
                        };
                        removeNonPartecipatingUsers.CommandText += "(" + UserIds[1] + ")";
                        removeNonPartecipatingUsers.Parameters.AddWithValue("@nodeID", nodeID);
                        addPartecipatingUsers.ExecuteNonQuery();
                        removeNonPartecipatingUsers.ExecuteNonQuery();
                    }
                }
                else
                    throw new ArgumentException("Non hai i permessi per eseguire questa azione. Esegui il login.");

            }
            catch (SqlException ex)
            {
                Errors.LogError(ex);
                if (!dberror.Visible)
                {
                    Errore("Selezionare almeno una nodewar.");
                }
            }
        }

        public void CancelAndRefreshBtn_Click(Object sender, EventArgs e)
        {
            gvstatus.Visible = true;
            gvpartecipationsEditor.Visible = false;
            showResults.Visible = true;
            if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
            {
                editPartecipations.Visible = true;
            }
            usersResult.Visible = false;
            goBack.Visible = false;
            goBackEdit.Visible = false;
            gobackCancelEdit.Visible = false;
            gvBindNodes();
        }
        

        private void QuerySuccess(int queryRes)
        {
            Errors.LogQuery(queryRes);
            success.Visible = true;

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

        private bool IsOnlyOneChecked()
        {
            int onlyOne = 0;
            foreach (GridViewRow row in gvstatus.Rows)
            {
                CheckBox chk = row.Cells[4].Controls[1] as CheckBox;
                if (chk != null && chk.Checked)
                {
                    onlyOne++;
                }
            }
            if (onlyOne > 1)
                return false;
            else
                return true;

        }

        private string GetSelectedNodes()
        {
            string result = "";
            foreach (GridViewRow row in gvstatus.Rows)
            {
                CheckBox chk = row.Cells[4].Controls[1] as CheckBox;
                if (chk != null && chk.Checked)
                {
                    result += row.Cells[1].Text + ", ";
                }
            }
            return result;
        }

        private string[] GetUserIds()
        {
            string[] userIDs = new string[2];
            foreach (GridViewRow row in gvpartecipationsEditor.Rows)
            {
                CheckBox chk = row.Cells[2].Controls[1] as CheckBox;
                if (chk.Checked)
                {
                    userIDs[0] += Utils.RecoverUserIdFromName(row.Cells[0].Text) + ", ";
                }
                else
                {
                    userIDs[1] += Utils.RecoverUserIdFromName(row.Cells[0].Text) + ", ";
                }
            }
            return userIDs;
        }

        protected void gvstatus_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dberror.Visible = false;
            if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
            {
                gvstatus.EditIndex = e.NewEditIndex;
                gvBindNodes();
                GridViewRow row = gvstatus.Rows[e.NewEditIndex];
                TextBox nodeId = (TextBox)row.Cells[1].Controls[0];
                nodeId.Enabled = false;
            }
            else
            {
                Errore("Non hai i permessi per eseguire questa operazione. Esegui il login.");
            }
        }

        protected void gvstatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
                {
                    GridViewRow row = (GridViewRow)gvstatus.Rows[e.RowIndex];
                    string nodeId = row.Cells[1].Text;
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlCommand clearAttendances = new SqlCommand
                        {
                            Connection = conn,
                            CommandText = @"delete from nodewarattendance where nodewarid = @nodeID"
                        };
                        clearAttendances.Parameters.AddWithValue("@nodeID", nodeId);
                        SqlCommand cmd = new SqlCommand
                        {
                            Connection = conn,
                            CommandText = @"delete from nodewars where nodewarid = @nodeID"
                        };
                        cmd.Parameters.AddWithValue("@nodeID", nodeId);
                        conn.Open();
                        clearAttendances.ExecuteNonQuery();
                        cmd.ExecuteNonQuery();

                    }
                    gvBindNodes();
                }
                else
                {
                    throw new ArgumentException("Non hai i permessi per eseguire questa azione. Esegui il login.");
                }
            }
            catch (ArgumentException ex)
            {
                Errore(ex.Message);
            }
            catch (SqlException)
            {
                Errore("Errore sul database. Controllare file di log");
            }
        }

        protected void gvstatus_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            dberror.Visible = false;
            GridViewRow row = gvstatus.Rows[e.RowIndex];
            //TextBox txtname = (TextBox)row.Cells[].Controls[];  
            TextBox data = (TextBox)row.Cells[0].Controls[0];
            TextBox nodeId = (TextBox)row.Cells[1].Controls[0];
            TextBox nodeTier = (TextBox)row.Cells[2].Controls[0];
            TextBox money = (TextBox)row.Cells[3].Controls[0];
            gvstatus.EditIndex = -1;
            if (Utils.GetPermissions(RecoverPermissionLevel(), PAGE_SECURITY_LEVEL))
            {

                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        SqlCommand cmd = new SqlCommand
                        {
                            Connection = conn,
                            CommandText = @"update nodewars set nodewartier = @nodeTier, money = @money, data = @data where nodewarid = @nodeID"
                        };
                        conn.Open();
                        string fixedMoney = money.Text.Replace(",", "");
                        cmd.Parameters.AddWithValue("@nodeTier", nodeTier.Text);
                        cmd.Parameters.AddWithValue("@money", fixedMoney);
                        CultureInfo ci = new CultureInfo("it-IT");
                        string dateFormat = "dd-MM-yyyy";
                        DateTime formattedDate;
                        formattedDate = DateTime.ParseExact(data.Text, dateFormat, ci);
                        cmd.Parameters.AddWithValue("@data", formattedDate);
                        cmd.Parameters.AddWithValue("@nodeID", nodeId.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    Errors.LogError(ex);
                    Errore("Errore nel database. Controllare i file di log.");
                }
                gvBindNodes();
            }
            else
            {
                Errore("Non hai i permessi per eseguire questa operazione. Esegui il login.");
            }
        }
        protected void gvstatus_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dberror.Visible = false;
            gvstatus.EditIndex = -1;
            gvBindNodes();
        }
        private int RecoverPermissionLevel()
        {
            int permissionLevel = 0;
            try
            {
                Int32.TryParse(Session["roleID"].ToString(), out permissionLevel);
            }
            catch (Exception)
            {

            }
            return permissionLevel;
        }
        private void Errore(string errore)
        {
            errorLabel.Text = errore;
            dberror.Visible = true;
        }
 
    }
}