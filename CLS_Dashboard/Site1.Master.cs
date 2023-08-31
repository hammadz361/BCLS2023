using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CLS_Dashboard
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        public static string strQuery = "";
        public static string active = "";
        public static string active2 = "active";
        public string[] reports = {"Dashboard", "surveyProgress", "Female","main"};
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
                            
            else
            {
                try
                {
                    string HeadingName = Session["lblHeadingName"] as string;
                    string sessionUserId = Session["UserID"] as string;
                    string Assign = Session["Assign"] as string;
                    string Role = Session["Role"] as string;


                    if (!string.IsNullOrEmpty(HeadingName))
                    {
                        if (!string.IsNullOrEmpty(sessionUserId))
                        {
                            if (!string.IsNullOrEmpty(Assign))
                            {
                                if (!string.IsNullOrEmpty(Role))
                                {

                                  

                                     lblHeadingName.Text = HeadingName;

                                     int UserID = Convert.ToInt32(sessionUserId);

                                    get_LastLogin(UserID);


                                    if (Assign.Contains("Disable")) { SB_Download.Style.Add("display", "none"); Li2.Attributes.Add("style", "display: none;"); } else if (Assign.Contains("Allow")) { SB_Download.Style.Add("display", "block"); Li2.Attributes.Add("style", "display: block;"); }

                                    if (Role.Contains("Admin")) { ManagementAllow.Style.Add("display", "block"); } else { ManagementAllow.Style.Add("display", "none"); }
                                    string path = HttpContext.Current.Request.Url.AbsolutePath.ToString();
                                    if(reports.Any(path.Contains))
                                    {
                                        menu1.Attributes.Remove("class");
                                        menu1.Attributes.Add("class", "tab-pane fade in active");
                                        menu2.Attributes.Remove("class");
                                        menu2.Attributes.Add("class", "tab-pane fade in");

                                    }
                                    else
                                    {
                                        menu2.Attributes.Remove("class");
                                        menu2.Attributes.Add("class", "tab-pane fade in active");
                                        menu1.Attributes.Remove("class");
                                        menu1.Attributes.Add("class", "tab-pane fade in");
                                    }
                                    using (SqlCommand cmdu = new SqlCommand("Exec LastUpdates"))
                                    {
                                        using (SqlDataAdapter sdau = new SqlDataAdapter())
                                        {
                                            cmdu.Connection = con;
                                            sdau.SelectCommand = cmdu;

                                            using (DataTable dtu = new DataTable())
                                            {
                                                sdau.Fill(dtu);
                                                Last_Updates.Text = "<table style='width:100%'><tr><td>Last Form Submission: ";
                                                Last_Updates.Text += dtu.Rows[0]["LastSubmission"].ToString();
                                                Last_Updates.Text += "</td><td>Last Observer Submission: ";
                                                Last_Updates.Text += dtu.Rows[0]["Observer"].ToString();
                                                Last_Updates.Text += "</td></tr><tr><td>Last Monitoring Submission: ";
                                                Last_Updates.Text += dtu.Rows[0]["Monitoring"].ToString();
                                                Last_Updates.Text += "</td><td>Last Listing Submission: ";
                                                Last_Updates.Text += dtu.Rows[0]["Listing"].ToString() + "</td></tr></table>";




                                            }
                                        }
                                    }
                                    Assign = "";
                                    Role = "";
                                }

                                else
                                {

                                    Show("Your Role Session has been expire ..");


                                }
                            }
                            else
                            {

                                Show("Your Assign Session has been expire ..");


                            }
                        }

                        else
                        {
                           // FormsAuthentication.RedirectToLoginPage();
                            Show("Your User Session has been expire ..");
                            
                        }
                    }

                    else
                    {
                        Show("Your Heading Session has been expire ..");
                        

                    }
                }

                catch (Exception ex)
                {
                    Show(ex.ToString());
                    
                }

                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }


        }

        public  void Show(string message)
        {
            string cleanMessage = message.Replace("'", "\'");
            Page page = HttpContext.Current.CurrentHandler as Page;
            string script = string.Format("alert('{0}');", cleanMessage);
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('" + cleanMessage + " ');window.location ='login.aspx';</script>");
            }


        } 

        private void get_LastLogin(int userId)
        {
            strQuery = "Select Username, LastLoginDate from Users where UserId = " + userId + " ";

            object UserLogin = "";
            object LastLogin = "";

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
                    UserLogin = rdr.GetValue(0);
                    LastLogin = rdr.GetValue(1);


                    c++;
                }

                con.Close();

                lblUserLogin.Text = "Loggin as : " + UserLogin.ToString();
                lblLastLogin.Text = "Last login : " + LastLogin.ToString();
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


        }

        public void Logout(object sender, EventArgs e)
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        public void surveyProgress(object sender, EventArgs e)
        {
            active = "active";
            active2 = "";
        }



    }
}