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
    public partial class Male_Female : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

        public static string chartTitle1 = "A2. Age and Gender Member Under 20";
        public static string chartTitle2 = "A1. Age and Gender Member In Group of 5";

        public string Filter_Data = "";
        public static string Filter_District = "NULL";
        public static string Filter_Supervisor = "-1";
        public static string Filter_Enumerator = "-1";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else
            {
                logFile.log("Male Female Charts", Session["username"].ToString());

                Session["lblHeadingName"] =  "<b>" + Session["reg"] + "</b> -Male_Female";

                if (!IsPostBack)
                {

                    load_Filters_Data();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateChartAge()", true);



                }
            }
        }



        private void load_Filters_Data()
        {

            //For 1st Filter Districts

            load_Filter_Districts();
            lstDistricts.SelectedIndex = 0;

            //For 2st Filter Supervisors

            load_Filter_SurveyTeam("");
            lstSurveyTeam.SelectedIndex = 0;
            //For 3rd Filter Cluster



            //For 4th Filter Enumerator

            load_Filter_Enumerator("");


        }

        private void load_Filter_Districts()
        {
            lstDistricts.Items.Clear();
            lstDistricts.Items.Add("All");
            lstDistricts.SelectedIndex = 0;


            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT district FROM [Main] " + Filter_Data + " order by 1"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstDistricts.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }

        }

        private void load_Filter_SurveyTeam(string Filter_Data)
        {
            lstSurveyTeam.Items.Clear();

            lstSurveyTeam.Items.Add("All");
            lstSurveyTeam.SelectedIndex = 0;
            Filter_Supervisor = "-1";

            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT G3_Supervisor_1_code ,G3_Supervisor_1 FROM [Main] " + Filter_Data + " order by 1"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)

                            if (!dt.Rows[i].IsNull("G3_Supervisor_1_code"))
                            {
                                ListItem Item = new ListItem();
                                string ID = dt.Rows[i].Field<object>("G3_Supervisor_1_code").ToString();
                                Item.Value = ID;
                                Item.Text = dt.Rows[i].Field<string>(1);

                                lstSurveyTeam.Items.Add(Item);
                            }

                    }
                }
            }
        }
        private void load_Filter_Enumerator(string Filter_Data)
        {
            lstEnumerator.Items.Clear();
            lstEnumerator.Items.Add("All");
            lstEnumerator.SelectedIndex = 0;
            Filter_Enumerator = "-1";




            using (SqlCommand cmd = new SqlCommand(
                       "SELECT DISTINCT G2_Interviewer_1_code, G2_Interviewer_1 FROM [Main] " + Filter_Data + " order by 1"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;



                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (!dt.Rows[i].IsNull("G2_Interviewer_1_code"))
                            {
                                ListItem Item = new ListItem();
                               string ID = dt.Rows[i].Field<object>("G2_Interviewer_1_code").ToString();
                                Item.Value = ID;
                                Item.Text = dt.Rows[i].Field<string>(1);

                                lstEnumerator.Items.Add(Item);
                            }

                        }


                    }
                }
            }
        }
        protected void lstDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDistricts.SelectedItem.Text == "All")
            {
                Filter_Data = Filter_Data + "";
                Filter_District = "NULL";

            }
            else
            {
                if (Filter_Data == "")
                {
                    Filter_Data = "WHERE [district] = '" + lstDistricts.SelectedItem.Text + "'";
                    Filter_District = lstDistricts.SelectedItem.Text;
                }
                else
                {
                    Filter_Data = Filter_Data + " and [district] = '" + lstDistricts.SelectedItem.Text + "'";
                    Filter_District = lstDistricts.SelectedItem.Text;
                }

            }

            logFile.log("Filter-------"+Filter_Data, Session["username"].ToString());
            load_Filter_SurveyTeam(Filter_Data);
            load_Filter_Enumerator(Filter_Data);


            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateChartAge()", true);



        }

        protected void lstSurveyTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSurveyTeam.SelectedItem.Text == "All")
            {
                Filter_Data = Filter_Data + "";
                Filter_Supervisor = "-1";

            }
            else
            {
                if (Filter_Data == "")
                {
                    Filter_Data = " Where [G3_Supervisor_1_code] = '" + lstSurveyTeam.SelectedValue.ToString() + "'";
                    Filter_Supervisor = lstSurveyTeam.SelectedValue.ToString();
                }
                else
                {

                    Filter_Data = Filter_Data + " and [G3_Supervisor_1_code] = '" + lstSurveyTeam.SelectedValue.ToString() + "'";
                    Filter_Supervisor = lstSurveyTeam.SelectedValue.ToString();
                }

            }
        
            load_Filter_Enumerator(Filter_Data);


            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateChartAge()", true);


        }


        protected void lstEnumerator_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstEnumerator.SelectedItem.Text == "All")
            {
                Filter_Data = Filter_Data + "";
                Filter_Enumerator = "-1";
            }
            else
            {
                if (Filter_Data == "")
                {
                    Filter_Data = " Where [G2_Interviewer_1_code] = " + lstEnumerator.SelectedValue.ToString() + "";
                    Filter_Enumerator = lstEnumerator.SelectedValue.ToString();
                }
                else
                {
                    Filter_Data = Filter_Data + " and [G2_Interviewer_1_code] = " + lstEnumerator.SelectedValue.ToString() + "";
                    Filter_Enumerator = lstEnumerator.SelectedValue.ToString();
                }

            }


            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateChartAge()", true);


        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData_Dashboard_MemberUnder20()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("");

            if (Filter_District == "NULL" && Filter_Supervisor == "-1" && Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

            else if (Filter_District == "NULL" && Filter_Supervisor == "-1")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

            else if (Filter_District == "NULL" && Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

            else if (Filter_District == "NULL")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }


            else if (Filter_Supervisor == "-1" && Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
                sql.Parameters.AddWithValue("@district", Filter_District);


            }
            else if (Filter_Supervisor == "-1")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@district", Filter_District);
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
            }
            else if (Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@district", Filter_District);
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
            }
            else
            {
                sql = new SqlCommand("MemberUnder20", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@district", Filter_District);
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
            }

            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);

            string Categories = "['0', '1', '2', '3', '4', '5', '6', '7', '8','9','10','11', '12', '13','14','15','16','17','18','19','20']";
            var Male = new StringBuilder();
            var Female = new StringBuilder();



            Male.Append("[");
            Female.Append("[");


            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                Male.Append(string.Format("{0},", row["Total Male % (0-20)"].ToString()));
                Female.Append(string.Format("-{0},", row["Total Female % (0-20)"].ToString()));

            }


            Male.Length--;
            Male.Append("]");

            Female.Length--;
            Female.Append("]");

            returnData.Add(Categories);
            returnData.Add(Male.ToString());
            returnData.Add(Female.ToString());

            if (returnData[1] == "]" && returnData[2] == "]")
            {
                returnData.Clear();
            }

            return returnData;
        }


        [System.Web.Services.WebMethod]
        public static List<string> getChartData_Dashboard_MemberInGroupof5()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("");


            if (Filter_District == "NULL" && Filter_Supervisor == "-1" && Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

            else if (Filter_District == "NULL" && Filter_Supervisor == "-1")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

            else if (Filter_District == "NULL" && Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

           

            else if (Filter_District == "NULL")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
                sql.Parameters.AddWithValue("@district", DBNull.Value);


            }

            else if (Filter_Supervisor == "-1" && Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
                sql.Parameters.AddWithValue("@district", Filter_District);


            }
            else if (Filter_Supervisor == "-1")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@district", Filter_District);
                sql.Parameters.AddWithValue("@Supervisor", DBNull.Value);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
            }
            else if (Filter_Enumerator == "-1")
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@district", Filter_District);
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", DBNull.Value);
            }
            else
            {
                sql = new SqlCommand("MemberInGroupof5", con);
                sql.CommandType = CommandType.StoredProcedure;
                sql.Parameters.AddWithValue("@district", Filter_District);
                sql.Parameters.AddWithValue("@Supervisor", Filter_Supervisor);
                sql.Parameters.AddWithValue("@Interviewer", Filter_Enumerator);
            }

            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);

            string Categories = "['0-4', '5-9', '10-14', '15-19', '20-24', '25-29', '30-34', '35-39','40-44','45-49','50-54', '55-59', '60-64','65-69','70-74','75-79','80-84','85-89','90-94','95-99']";
            var Male = new StringBuilder();
            var Female = new StringBuilder();

            object[] Age_Categories = new object[100];

            Male.Append("[");
            Female.Append("[");


            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                Male.Append(string.Format("{0},", row["Total Male %"].ToString()));
                Female.Append(string.Format("-{0},", row["Total Female %"].ToString()));

            }


            //foreach (DataRow row in dataset.Tables[0].Rows)
            //{

            //    Male.Append(string.Format("{0},", row["Total Male %"].ToString()));
            //    Female.Append(string.Format("-{0},", row["Total Female %"].ToString()));

            //}


            Male.Length--;
            Male.Append("]");

            Female.Length--;
            Female.Append("]");

            returnData.Add(Categories);
            returnData.Add(Male.ToString());
            returnData.Add(Female.ToString());

            if (returnData[1] == "]" && returnData[2] == "]")
            {
                returnData.Clear();
            }

            return returnData;
        }


    }
}