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

                                     Assign = "";
                                     Role = "";

                                     lblHeadingName.Text = HeadingName;

                                     int UserID = Convert.ToInt32(sessionUserId);

                                    get_LastLogin(UserID);


                                    if (Assign.Contains("Disable")) { SB_Download.Style.Add("display", "none"); } else if (Assign.Contains("Allow")) { SB_Download.Style.Add("display", "block"); }

                                    if (Role.Contains("User")) { ManagementAllow.Style.Add("display", "none"); } else if (Role.Contains("Admin")) { ManagementAllow.Style.Add("display", "block"); }


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