using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;


namespace CLS_Dashboard
{
    public partial class Management : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        public static string strQuery = "";
        public static string active1 = "";
        public static string active2 = "";
        public static string active3 = "";
        public static string active_id = "";
        public string sessionUserId = "";
        public string district = "";
      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else if ((string)(Session["Role"]) != "Admin")
            {
                Response.Redirect("Dashboard.aspx");
            }

            else
            {
                sessionUserId = Session["UserID"] as string;
                Session["lblHeadingName"] = "<b>CLS</b>-Admin Panel";
                

                if (!IsPostBack)
                {
                    active1 = "active";
                    active2 = "";

                    //Div_Hide.Style.Add("display", "none");
                    GetDownloadData();
                    SQL_Query();
                    LoadDataDrop();

                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Detail"), new DataColumn("Code") });
                    dt.Rows.Add(1, "Roster Details", "A");
                    dt.Rows.Add(2, "Family tree", "A1");
                    dt.Rows.Add(3, "Household Asset details", "B");
                    dt.Rows.Add(4, "Adult Questionnaire", "Main");
                    dt.Rows.Add(5, "Child questionnaire", "C");
                    dt.Rows.Add(6, "Monitoring Data", "Monitoring");
                    dt.Rows.Add(7, "Observers data", "Observer");
                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
            }

        }


        private int generateAutonumber()
        {
            if (con.State != ConnectionState.Closed)
                con.Close();

            con.Open();
            SqlCommand cmd = new SqlCommand("Select Count(Monitor_ID) AS Monitor_ID from DeskMonitors", con);
            cmd.CommandType = CommandType.Text;
            int i = (int)cmd.ExecuteScalar();
            if (con.State != ConnectionState.Closed)
                con.Close();
            i++;

           return (i);


        }

        private string get_Login(int userId)
        {
            strQuery = "Select Username from Users where UserId = " + userId + " ";

            string UserLogin = "";
           

            try
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                con.Open();
                SqlCommand cmd = new SqlCommand(strQuery);
                cmd.Connection = con;
                SqlDataReader rdr = cmd.ExecuteReader();


                int c = 0;
                while (rdr.Read())
                {
                    UserLogin = Convert.ToString(rdr.GetValue(0));
                    


                    c++;
                }

                con.Close();

                

                
                
            }

            catch
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

                
            }

            return UserLogin;
        }
        private void LoadDataDrop()
        {
            DropDownList ddUsername = (GridView1.Rows[0].FindControl("ddUsername") as DropDownList);
            ddUsername.DataSource = GetData("SELECT UserId, Username FROM Users");
            ddUsername.DataTextField = "Username";
            ddUsername.DataValueField = "UserId";
            ddUsername.DataBind();

            DropDownList ddlRoles = (GridView1.Rows[0].FindControl("ddlRoles") as DropDownList);
            ddlRoles.DataSource = GetData("SELECT distinct Role FROM Users");
            ddlRoles.DataTextField = "Role";
            
            ddlRoles.DataBind();



            ddSupervisors.DataSource = GetData("select distinct G3_Supervisor_1_code As Supervisor_code, G3_Supervisor_1 As Supervisor_Name from main");
            ddSupervisors.DataTextField = "Supervisor_Name";
            ddSupervisors.DataValueField = "Supervisor_code";
            ddSupervisors.DataBind();


            ddRolesMonitors.DataSource = GetData("select distinct Monitor_ID, Monitor_Name from DeskMonitors");
            ddRolesMonitors.DataTextField = "Monitor_Name";
            ddRolesMonitors.DataValueField = "Monitor_ID";
            ddRolesMonitors.DataBind();

            

            //for (int i = 1; i < GridView1.Rows.Count; i++)
            //{
            //    GridView1.Rows;
            //}
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            active1 = "";
            active2 = "active";
            active3 = "";
            
            
            DropDownList ddlRoles = (GridView1.Rows[0].FindControl("ddlRoles") as DropDownList);

            if (ddlRoles.SelectedItem.Text == "User")
            GridView2.Visible = true;
            else
                GridView2.Visible = false;

          

        } 


        private void SQL_Query()
        {

            strQuery = "SELECT UserId,Username,Role,Assign,Email,CreatedDate,LastLoginDate from Users";

            DataTable dt2 = GetData(strQuery);
            
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            ds.Tables.Add(dt);
            dt.Columns.Add("UserId", typeof(Int32));
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            

            GridView1.DataSource = ds;
            GridView1.DataBind();

            GridView3.DataSource = dt2;
            GridView3.DataBind();

            if (GridView3.Columns.Count > 0)
                GridView3.Columns[1].Visible = false;
            else
            {
                GridView3.HeaderRow.Cells[1].Visible = false;
                foreach (GridViewRow gvr in GridView3.Rows)
                {
                    gvr.Cells[1].Visible = false;
                }
            }

            LoadDataDrop();
        }

        private DataTable GetData(string query)
        {
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    return dt;
                }
            }
        }

        protected void GridView3_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)GridView3.Rows[e.RowIndex];
            
            con.Open();
            SqlCommand cmd = new SqlCommand("delete FROM Users where UserId='" + Convert.ToInt32(GridView3.DataKeys[e.RowIndex].Value.ToString()) + "'");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            SQL_Query();
        }
        protected void GridView3_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView3.EditIndex = e.NewEditIndex;
            SQL_Query();
            active1 = "active";
            active2 = "";
        }
        protected void GridView3_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userid = Convert.ToInt32(GridView3.DataKeys[e.RowIndex].Value.ToString());
            GridViewRow row = (GridViewRow)GridView3.Rows[e.RowIndex];
           
            //TextBox txtname=(TextBox)gr.cell[].control[];  
            string Username = ((TextBox)GridView3.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
           
         // string Password = ((TextBox)GridView1.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string Role = ((TextBox)GridView3.Rows[e.RowIndex].Cells[3].Controls[0]).Text;
            string Assign = ((TextBox)GridView3.Rows[e.RowIndex].Cells[4].Controls[0]).Text;
            string Email = ((TextBox)GridView3.Rows[e.RowIndex].Cells[5].Controls[0]).Text;
            string CreateDate = ((TextBox)GridView3.Rows[e.RowIndex].Cells[6].Controls[0]).Text;

            //TextBox textadd = (TextBox)row.FindControl("txtadd");  
            //TextBox textc = (TextBox)row.FindControl("txtc");  
            GridView3.EditIndex = -1;
            con.Open();
            //SqlCommand cmd = new SqlCommand("SELECT * FROM detail", con);  
            SqlCommand cmd = new SqlCommand("update Users set Username='" + Username + "',Role='" + Role + "',Assign='" + Assign + "',Email='" + Email + "',CreatedDate='" + CreateDate + "'where UserId=" + userid + "");
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();
            SQL_Query();
            GridView3.DataBind();

            active1 = "active";
            active2 = "";
        }
       
        protected void GridView3_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView3.EditIndex = -1;
            SQL_Query();
        }  
       

        protected void UpdateRole(object sender, EventArgs e)
        {
            DropDownList ddUsername = (GridView1.Rows[0].FindControl("ddUsername") as DropDownList);
            DropDownList ddlRoles = (GridView1.Rows[0].FindControl("ddlRoles") as DropDownList);

            int UserId = Convert.ToInt32(ddUsername.SelectedItem.Value);
            string Code = "";
            int count = 0;
           

            for (int c = 0; c < GridView2.Rows.Count; c++)
            {

                GridViewRow row = GridView2.Rows[c];

                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                    if (chkRow.Checked)
                    {
                        count = 1;
      
                        Code = Code + row.Cells[1].Text + " , ";


                        using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Assign = @Assign WHERE UserId = @UserId"))
                        {
                            cmd.Parameters.AddWithValue("@Assign", Code);
                            cmd.Parameters.AddWithValue("@UserId", UserId);
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        
                    }

                }

                

            }

            if (count == 0 && ddlRoles.SelectedItem.Text == "User")
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Assign = @Assign WHERE UserId = @UserId"))
                {
                    Code = "Disable";

                    cmd.Parameters.AddWithValue("@Assign", Code);
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            else if (count == 0 && ddlRoles.SelectedItem.Text == "Admin")
            {
                
                
                    using (SqlCommand cmd = new SqlCommand("UPDATE Users SET Assign = @Assign WHERE UserId = @UserId"))
                    {
                        Code = "Allow";

                        cmd.Parameters.AddWithValue("@Assign", Code);
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                
            }

            Session["Assign"] = Code;

            GridView2.Visible = false;

            active1 = "";
            active2 = "active";
            active3 = "";

            SQL_Query();
            
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "UpdateMessage();", true);
            
        }

        protected void GetDownloadData()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Detail"), new DataColumn("Code") });
            dt.Rows.Add(1, "Roster Details", "A");
            dt.Rows.Add(2, "Family tree", "A1");
            dt.Rows.Add(3, "Household Asset details", "B");
            dt.Rows.Add(4, "Adult Questionnaire", "Main");
            dt.Rows.Add(5, "Child questionnaire", "C");
            dt.Rows.Add(6, "Monitoring Data", "Monitorin");
            dt.Rows.Add(7, "Observers data", "Observer");
            GridView2.DataSource = dt;
            GridView2.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "" || txtPassword.Text == "" || txtEmail.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert2", "MessageUsersWarning();", true);
            }

            else
            {
                string AssignRole = "";

                if (ddRoles.SelectedItem.Text == "Admin")
                {
                    AssignRole = "Allow";
                }
                else
                {
                    AssignRole = "Disable";
                }

                try
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES ('" + txtUser.Text + "','" + txtPassword.Text + "','" + ddRoles.SelectedItem.Text + "','" + AssignRole + "','" + txtEmail.Text + "','" + DateTime.Now.ToString() + "','" + null + "')"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();

                        txtUser.Text = "";
                        txtPassword.Text = "";
                        ddRoles.SelectedIndex = 0;
                        txtEmail.Text = "";

                        active1 = "active";
                        active2 = "";
                        active3 = "";

                        SQL_Query();

                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert2", "MessageUsers();", true);

                    }
                }

                catch
                {

                }

                finally
                {
                    con.Close();
                }
            }

            
        }

        protected void ddRolesMonitorSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddRolesMonitors.Items.Count > 0)
            {
                if (ddRolesMonitorSupervisor.SelectedItem.Text == "Supervisor")
                {
                    div_ddSupervisors.Style.Add("display", "block");
                    div_Monitors.Style.Add("display", "block");
                    div_dduser.Style.Add("display", "none");
                }

                else
                {
                    div_dduser.Style.Add("display", "block");
                    div_ddSupervisors.Style.Add("display", "none");
                    div_Monitors.Style.Add("display", "none");
                }

                active1 = "active";
                active2 = "";
                active3 = "";
            }

            else
            {

              ScriptManager.RegisterStartupScript(this,typeof(Page),"Alert", "<script>alert('" + "Please add Monitor first .." + "');</script>",false);
              ddRolesMonitorSupervisor.SelectedIndex = 0;
            }
        }
        protected void btnSubmit2_MonitorSupervisor_Click(object sender, EventArgs e)
        {
            if (txtUserMonitorSupervisor.Text == "" && ddRolesMonitorSupervisor.SelectedItem.Text == "DeskMonitors")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert2", "MessageUsersWarning();", true);
            }

            else
            {
                string Type = "";
                string query = "";
                object i = 0;

                if (ddRolesMonitorSupervisor.SelectedItem.Text == "DeskMonitors")
                {
                    Type = "DeskMonitors";
                    query = "INSERT INTO " + Type + " VALUES ('" + generateAutonumber() + "','" + txtUserMonitorSupervisor.Text + "','" + txtEmailMonitorSupervisor.Text + "','" + txtEmailCCMonitorSupervisor.Text + "','" + txtEmailBCCMonitorSupervisor.Text + "','" + DateTime.Now.ToString() + "','" + get_Login(Convert.ToInt32(sessionUserId)) + "')";
                }
                else
                {
                    Type = "Supervisor";
                    query = "INSERT INTO " + Type + " VALUES ('" + ddSupervisors.SelectedItem.Value + "','" + ddRolesMonitorSupervisor.SelectedItem.Text + "','" + "" + "','" + ddRolesMonitors.SelectedItem.Value + "','" + txtEmailMonitorSupervisor.Text + "','" + txtEmailCCMonitorSupervisor.Text + "','" + txtEmailBCCMonitorSupervisor.Text + "','" + DateTime.Now.ToString() + "','" + get_Login(Convert.ToInt32(sessionUserId)) + "')";

                    using (SqlCommand cmd = new SqlCommand("select  case when count(*) > 0 then -1 else  1 end from Supervisor where Supervisor_ID = @Supervisor_ID"))
                    {
                        cmd.Parameters.AddWithValue("@Supervisor_ID", ddSupervisors.SelectedItem.Value);

                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        i = cmd.ExecuteScalar();
                        con.Close();
                    }

                }

                try
                {
                   

                    
                    if (Convert.ToInt32(i) == -1)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "<script>alert('" + "Supervisor already exists .." + "');</script>", false);
                    }

                    else
                    {

                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();

                            txtUserMonitorSupervisor.Text = "";
                            ddRolesMonitorSupervisor.SelectedIndex = 0;
                            txtEmailMonitorSupervisor.Text = "";
                            txtEmailBCCMonitorSupervisor.Text = "";
                            txtEmailCCMonitorSupervisor.Text = "";

                            div_dduser.Style.Add("display", "block");
                            div_ddSupervisors.Style.Add("display", "none");
                            div_Monitors.Style.Add("display", "none");

                            ddRolesMonitors.SelectedIndex = 0;
                            ddSupervisors.SelectedIndex = 0;
                            ddRoles.SelectedIndex = 0;
                            txtEmail.Text = "";

                            active1 = "active";
                            active2 = "";
                            active3 = "";

                            SQL_Query();

                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert2", "MessageUsers();", true);

                        }
                    }
                }

                catch
                {

                }

                finally
                {
                    con.Close();
                }
            }


        }
    }
}