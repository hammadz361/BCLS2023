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
    public partial class SearchListing : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string HHID = HH_ID.Text;
                string cluster = Cluster_Code.Text;
                logFile.log("Search listing Data", Session["username"].ToString());
                logFile.log("Household------"+HHID+"---------Cluster---------"+cluster, Session["username"].ToString());
                SqlCommand cmd = new SqlCommand("Search_Listing");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HHID", HHID);
                cmd.Parameters.AddWithValue("@cluster", cluster);
                cmd.Connection = con;
                con.Open();
                SqlDataAdapter myCommand = new SqlDataAdapter(cmd);
                myCommand.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    Label1.Text = "No Record Found!";
                    Label1.Visible = true;
                }
                else
                {
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                con.Close();
            }
        }
    }
}