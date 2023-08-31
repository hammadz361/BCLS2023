using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.Security;

namespace CLS_Dashboard
{
    public partial class Change_Password : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        public static int UserID = -1;
        public static string message = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void MessageAlert(string alertType, string messageContent)
        {
            if (alertType == "Error")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert2", "MessageUsersWarning();", true);
            }

            else if (alertType == "Ok")
            {

                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert2", "MessageUsers();", true);
            }

            message = messageContent;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtOldPass.Text == "" || txtPassword.Text == "" || txtConfPassword.Text == "")
            {
                MessageAlert("Error", "One of the fields are empty please fill the complete fields..");
            }

            else if (txtPassword.Text.Trim() != txtConfPassword.Text.Trim())
            {
                MessageAlert("Error", "Password does not match .. !");
            }


            else
            {

                try
                {
                    UserID = (int)(Session["UserID"]);

                    SqlCommand cmd = new SqlCommand("");

                    using (cmd = new SqlCommand("select Password from Users where UserId=" + UserID + ""))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        object OldPass = cmd.ExecuteScalar();
                        con.Close();

                        if (OldPass.ToString() != txtOldPass.Text.Trim())
                        {
                            MessageAlert("Error", "Old Password is incorrect .. !");

                            txtOldPass.Text = "";
                            txtPassword.Text = "";
                            txtConfPassword.Text = "";
                        }

                        else
                        {





                            using (cmd = new SqlCommand("update Users set Password=" + txtPassword.Text + " where UserId=" + UserID + ""))
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();

                                txtOldPass.Text = "";
                                txtPassword.Text = "";
                                txtConfPassword.Text = "";

                                MessageAlert("Ok", "Password Updated Successfully .. ");


                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageAlert("Error", ex.ToString());
                }

                finally
                {
                    con.Close();
                }
            }
        }
    }
}