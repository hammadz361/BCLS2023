using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace CLS_Dashboard
{
    public partial class Login : System.Web.UI.Page
    {
        private static string CS = ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString;
        private SqlConnection con = new SqlConnection(CS);
        
        protected void ValidateUser(object sender, EventArgs e)
        {
            int userId = 0;
           
           
                using (SqlCommand cmd = new SqlCommand("Validate_User"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", Login1.UserName);
                    cmd.Parameters.AddWithValue("@Password", Login1.Password);
                    cmd.Connection = con;

                cmd.CommandTimeout = 0;

                con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
                switch (userId)
                {
                    case -1:
                        Login1.FailureText = "Username and/or password is incorrect.";
                        break;
                    case -2:
                        Login1.FailureText = "Account has not been activated.";
                        break;
                    default:
                        FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);

                       SqlCommand cmdAssign = new SqlCommand("Select Assign, Role, Profile_ID, username, ReportGroup from Users where UserId = '" + userId + "' ", con);
                                con.Open();
                                var r1 = cmdAssign.ExecuteReader();

                                string Assign = "";
                               
                                string Role = "";

                                string Profile_ID = "";

                                string username = "";

                                string ReportGroup = "";
                       

                                while (r1.Read())
                                {
                                    Assign = r1[0].ToString();
                                    
                                    Role = r1[1].ToString();
                                    Profile_ID = r1[2].ToString();
                                    username = r1[3].ToString();
                                    ReportGroup = r1[4].ToString();
                                   

                                }
                                Session["UserID"] = userId.ToString();
                                Session["Assign"] = Assign;
                                Session["Role"] = Role;
                                Session["Profile_ID"] = Profile_ID;
                                Session["username"] = username;
                                Session["ReportGroup"] = ReportGroup;
                        

                                String query = "select code, '" + ReportGroup + "' from users_report_group '";

                                logFile.log("Login", username);

                                using (SqlConnection conn = new SqlConnection(CS))
                                {
                                    conn.Open();
                                    SqlCommand sql = new SqlCommand("select id, code, description, " + ReportGroup + " from users_report_group where " + ReportGroup + " = '1'", conn);
                                    DataTable dt = new DataTable();
                                    SqlDataAdapter sda = new SqlDataAdapter(sql);
                                    sda.Fill(dt);
                                    Session["dt_report_allow"] = dt;
                                }
                                con.Close();

                                using(SqlConnection conn = new SqlConnection(CS))
                                {
                                    conn.Open();
                                    SqlCommand sql = new SqlCommand("SELECT [Bos_Name] FROM [Bos_Frequency]", conn);
                                    Session["reg"]  = (string)sql.ExecuteScalar();
                                }
                                con.Close();
                        break;
                }
        }
    }
}