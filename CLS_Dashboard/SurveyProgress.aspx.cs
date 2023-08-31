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
using System.Text.RegularExpressions;

namespace CLS_Dashboard
{
    public partial class SurveyProgress : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        private static string CS = ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString;
        private SqlConnection con = new SqlConnection(CS);
        public string Filter_Data = "";
        public string Report_ID = "";
        public string Report_Name = "";



        public string Report_1 = "1_survey_Progress";
        public string Report_2 = "2_Household_Completion";
        public string Report_21 = "21_Household_Completion";
        public string Report_3 = "3_Enumerators_Progress";
        public string Report_4 = "4_Survey_Coverage";
        public string Report_5 = "5_Percent_Children_Interviewed";
        public string Report_6 = "6_District_Wise_Enumerators_Collection";
        public string Report_7 = "7_Supervisor_time_spent";
        public string Report_8 = "8_Monitoring_Enumerator";
        public string Report_9 = "9_Monitoring_Supervisor";
        public string Report_10 = "10_Monitoring_Observer";
        public string Report_11 = "11_Observers_Checklists";
        public string Report_101 = "17_non_applicable";
        public string Report_12 = "12_Observers's_Interviewer_evaluation_weighted";
        public string Report_13 = "13_Observers's_Interviewer_evaluation_Absolute";
        public string Report_14 = "14_District_Wise_Progress";
        public string Report_15 = "15_Projected_Completion";
        public string Report_16 = "16_Progress_Field_Monitors";
        public string Report_17 = "17_Observers_Progress";
        public string Report_18 = "18_Assigned_Queries";
        public string Report_19 = "19_Summary_Correction";
        public string Report_1001 = "1001_Text_Review";
        public string Report_1002 = "1002_Others";
        public string Report_1003 = "1003_Dont_Know";
        public string Report_1004 = "1004_Mistakes_Summarized_Overall";
        public string Report_10044 = "10044_Mistakes_Summarized_Overall";
        public string Report_1005 = "1005_Mistakes_Numbers";
        public string Report_1006 = "1006_Mistakes_Outlier";
        public string Report_1007 = "1007";
        public string Report_1008 = "1008";
        public string Report_1009 = "1009";
        public string Report_1010 = "1010";
        public string Report_1020 = "1020";
        public string Report_1021 = "1021";
        public string Report_1022 = "1022";
        public string Report_1023 = "1023";
        public string Report_191 = "191_Enumerator_Wise_Covid";
        public string Report_192 = "192_Supervisor_Wise_Covid";
        public string Report_193 = "193_District_Wise_Covid";
        public string Report_194 = "194_Cluster_Wise_Covid";


        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else
            {
                Site1.active = "active";
                GridView1.PageIndex = 0;
                GridView2.PageIndex = 0;
                //  Session["lblHeadingName"] = Session["lblHeadingName"].ToString().Remove(Session["lblHeadingName"].ToString().IndexOf('-'));
                Session["lblHeadingName"] = Session["reg"] + "-Survey Progress";
                Report_ID = Request.QueryString["id"];
                Report_Name = Request.QueryString["name"];

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StaticHeaderRow()", true);

                if (!IsPostBack)
                {
                    if (Report_ID != null && Report_ID == "1004_Mistakes_Summarized_Overall")
                    {
                        chksummaryOrderAsc.Visible = true;
                    }
                    if (Session["summaryOrderAsc"] != null && Session["summaryOrderAsc"].ToString() == "True")
                        chksummaryOrderAsc.Checked = true;
                    else
                        chksummaryOrderAsc.Checked = false;

                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + Session["summaryOrderAsc"] + "')", true);

                    if (Report_ID != "" && Report_ID != null)
                    {
                        logFile.log("Survey Progress Reports" + "----Report_Name--------" + Report_Name, Session["username"].ToString());
                        Visible_Filters();
                        load_Filters_Data();
                        gv_Bind_Data(" ", Report_ID, Report_Name);
                        using (SqlCommand cmdu = new SqlCommand("Exec LastUpdates"))
                        {
                            using (SqlDataAdapter sdau = new SqlDataAdapter())
                            {
                                cmdu.Connection = con;
                                sdau.SelectCommand = cmdu;

                                using (DataTable dtu = new DataTable())
                                {
                                    sdau.Fill(dtu);
                                    Last_Updates.Text = "Last Form Submission: ";
                                    Last_Updates.Text += dtu.Rows[0]["LastSubmission"].ToString();
                                    Last_Updates.Text += "<br>Last Observer Submission: ";
                                    Last_Updates.Text += dtu.Rows[0]["Observer"].ToString();
                                    Last_Updates.Text += "<Br>Last Monitoring Submission: ";
                                    Last_Updates.Text += dtu.Rows[0]["Monitoring"].ToString();
                                    Last_Updates.Text += "<br>Last Listing Submission: ";
                                    Last_Updates.Text += dtu.Rows[0]["Listing"].ToString();
                                }
                            }
                        }

                    }
                    else
                    {
                        Response.Redirect("Dashboard.aspx");
                    }
                }
            }
        }

        protected void chksummaryOrderAsc_CheckedChanged(object sender, EventArgs e)
        {
            // store value into session
            //if(chksummaryOrderAsc.Checked)
            Session["summaryOrderAsc"] = chksummaryOrderAsc.Checked;
            Response.Redirect(Request.RawUrl);
            //else
            //   Session["autorefresh"] = chksummaryOrderAsc.Checked;
            // call method where you enable/disable auto refresh
            //summaryOrderAscSite();
        }




        private void Visible_Filters()
        {
            if ((Report_ID == Report_1002) || (Report_ID == Report_1023) || (Report_ID == Report_1003) || (Report_ID == Report_1021) || (Report_ID == Report_1010) || (Report_ID == Report_1022))
            {
                lstDistricts.Visible = false; lblDistricts.Visible = false;
                lstTypes.Visible = true; lblTypes.Visible = true;
            }
            if ((Report_ID == Report_1004) || (Report_ID == Report_10044) || (Report_ID == Report_1005) || (Report_ID == Report_1007) || (Report_ID == Report_1008))
            {
                lstDistricts.Visible = true; lblDistricts.Visible = true;
                lstEnumerator.Visible = true; lblEnumerator.Visible = true;
            }
            if ((Report_ID == Report_1001) || (Report_ID == Report_1020) || (Report_ID == Report_1006) || (Report_ID == Report_1009) || (Report_ID == Report_1010) || (Report_ID == Report_18) || (Report_ID == Report_19) || (Report_ID == Report_101))
            {
                lstDistricts.Visible = false; lblDistricts.Visible = false;
                lstEnumerator.Visible = false; lblEnumerator.Visible = false;
            }
            if (Report_ID == Report_1 || Report_ID == Report_2 || Report_ID == Report_21 || Report_ID == Report_8 || Report_ID == Report_3 || Report_ID == Report_5 || Report_ID == Report_7 || Report_ID == Report_9 || Report_ID == Report_11 || Report_ID == Report_12 || Report_ID == Report_13 || Report_ID == Report_16 || Report_ID == Report_10 || Report_ID == Report_17 || Report_ID == Report_1006) { lstSurveyTeam.Visible = true; lblSurveyTeam.Visible = true; }
            if (Report_ID == Report_2 || Report_ID == Report_21 || Report_ID == Report_5) { lstClusterCodes.Visible = true; lblClusterCode.Visible = true; }
            if (Report_ID == Report_3 || Report_ID == Report_6 || Report_ID == Report_8 || Report_ID == Report_11 || Report_ID == Report_12 || Report_ID == Report_13 || Report_ID == Report_1006) { lstEnumerator.Visible = true; lblEnumerator.Visible = true; }
            if (Report_ID == Report_1004 || Report_ID == Report_10044)
            {
                lstMonitors.Visible = true; lblMonitors.Visible = true;
            }
        }

        private void load_Filter_Districts()
        {
            lstDistricts.Items.Clear();
            lstDistricts.Items.Add("All");


            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT district FROM [Main] order by 1"))
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
        private void load_Filter_Monitors()
        {
            lstMonitors.Items.Clear();
            lstMonitors.Items.Add("All");


            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT [Monitor_Name] FROM [DeskMonitors] order by 1"))
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
                            lstMonitors.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }

        }


        private void load_Filter_SurveyTeam(string Filter_Data)
        {
            lstSurveyTeam.Items.Clear();

            lstSurveyTeam.Items.Add("All");
            lstSurveyTeam.SelectedIndex = 0;

            if (Filter_Data.Contains("[Name of District]")) { Filter_Data = Filter_Data.Replace("[Name of District]", "district"); }

            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT G3_Supervisor_1 FROM [Main] " + Filter_Data + " order by 1"))
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
                            lstSurveyTeam.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }
        }

        private void load_Filter_ClusterCodes(string Filter_Data)
        {
            lstClusterCodes.Items.Clear();
            lstClusterCodes.Items.Add("All");
            lstClusterCodes.SelectedIndex = 0;

            if (Filter_Data.Contains("[Name of District]")) { Filter_Data = Filter_Data.Replace("[Name of District]", "district"); }
            if (Filter_Data.Contains("[Team Supervisor]")) { Filter_Data = Filter_Data.Replace("[Team Supervisor]", "G3_Supervisor_1"); }

            //using (SqlCommand cmd = new SqlCommand("  SELECT  DISTINCT CAST(cluster_code AS numeric) AS CCode FROM [Main] " + Filter_Data + " order by 1"))
            //{
            //    using (SqlDataAdapter sda = new SqlDataAdapter())
            //    {
            //        cmd.Connection = con;
            //        sda.SelectCommand = cmd;

            //        using (DataTable dt = new DataTable())
            //        {
            //            sda.Fill(dt);
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //                lstClusterCodes.Items.Add(dt.Rows[i].ItemArray[0].ToString());


            //        }
            //    }
            //}
        }

        private void load_Filter_Enumerator(string Filter_Data)
        {
            lstEnumerator.Items.Clear();
            lstEnumerator.Items.Add("All");
            lstEnumerator.SelectedIndex = 0;

            if (Filter_Data.Contains("[Name of District]")) { Filter_Data = Filter_Data.Replace("[Name of District]", "district"); }
            if (Filter_Data.Contains("[Team Supervisor]")) { Filter_Data = Filter_Data.Replace("[Team Supervisor]", "G3_Supervisor_1"); }
            if (Filter_Data.Contains("[Cluster Code]")) { Filter_Data = Filter_Data.Replace("[Cluster Code]", "Cluster_Code"); }

            using (SqlCommand cmd = new SqlCommand(
                       "SELECT DISTINCT G2_Interviewer_1 FROM [Main] " + Filter_Data + " order by 1"))
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
                            lstEnumerator.Items.Add(dt.Rows[i].Field<string>(0));


                    }
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

            load_Filter_ClusterCodes("");

            //For 4th Filter Enumerator

            load_Filter_Enumerator("");
            lstClusterCodes.SelectedIndex = 0;

            //For Monitors Filter
            load_Filter_Monitors();
            lstMonitors.SelectedIndex = 0;

            //For 5th Filter Types


            //   if (Report_ID == Report_1001)
            // {
            //
            //   lstTypes.Items.Add(new ListItem("Industries", "1"));
            // lstTypes.Items.Add(new ListItem("Occupation", "2"));
            //lstTypes.Items.Add(new ListItem("Tools", "3"));

            //lstTypes.SelectedIndex = 0;
            //}
            if (Report_ID == Report_1010 || Report_ID == Report_1022)
            {

                lstTypes.Items.Add(new ListItem("Incidence", "101"));
                lstTypes.Items.Add(new ListItem("Magnitude", "100"));
                if (Request["type"] == "100")
                    lstTypes.SelectedIndex = 1;
                else
                    lstTypes.SelectedIndex = 0;
            }
            if (Report_ID == Report_1023)
            {

                lstTypes.Items.Add(new ListItem("Monitors", "100"));
                lstTypes.Items.Add(new ListItem("Supervisors", "101"));
                lstTypes.Items.Add(new ListItem("Operators", "102"));
                if (Request["type"] == "100")
                    lstTypes.SelectedIndex = 0;
                else if (Request["type"] == "101")
                    lstTypes.SelectedIndex = 1;
                else if (Request["type"] == "102")
                    lstTypes.SelectedIndex = 2;
            }
            if (Report_ID == Report_1002)
            {

                lstTypes.Items.Add(new ListItem("OtherQuery1_A6a", "4"));
                lstTypes.Items.Add(new ListItem("OtherQuery2_A17", "5"));
                lstTypes.Items.Add(new ListItem("OtherQuery3_A23", "6"));
                lstTypes.Items.Add(new ListItem("OtherQuery4_A24", "7"));
                lstTypes.Items.Add(new ListItem("OtherQuery5_A29", "8"));
                lstTypes.Items.Add(new ListItem("OtherQuery6_A14", "9"));
                lstTypes.Items.Add(new ListItem("OtherQuery7_A36", "10"));
                lstTypes.Items.Add(new ListItem("OtherQuery7_A45", "11"));
                lstTypes.Items.Add(new ListItem("OtherQuery9_A49", "12"));
                lstTypes.Items.Add(new ListItem("OtherQuery10_A16", "13"));
                lstTypes.Items.Add(new ListItem("OtherQuery11_A39", "14"));
                lstTypes.Items.Add(new ListItem("OtherQuery12_A34", "15"));
                lstTypes.Items.Add(new ListItem("OtherQuery13_A47", "16"));
                lstTypes.Items.Add(new ListItem("OtherQuery14_C13", "17"));
                lstTypes.Items.Add(new ListItem("OtherQuery15_C33", "18"));
                lstTypes.Items.Add(new ListItem("OtherQuery16_C27", "19"));
                lstTypes.Items.Add(new ListItem("OtherQuery17_C41", "20"));
                lstTypes.Items.Add(new ListItem("OtherQuery18_C30", "21"));
                lstTypes.Items.Add(new ListItem("OtherQuery19_C25", "22"));
                lstTypes.Items.Add(new ListItem("OtherQuery20_C29", "23"));
                lstTypes.Items.Add(new ListItem("OtherQuery21_C7", "24"));
                lstTypes.Items.Add(new ListItem("OtherQuery22_G10b", "25"));
                lstTypes.Items.Add(new ListItem("OtherQuery23_G10a", "26"));
                lstTypes.Items.Add(new ListItem("OtherQuery24_i7", "27"));
                lstTypes.Items.Add(new ListItem("OtherQuery25_B18", "28"));
                lstTypes.Items.Add(new ListItem("OtherQuery26_B22", "29"));
                lstTypes.Items.Add(new ListItem("OtherQuery27_B11", "30"));
                lstTypes.Items.Add(new ListItem("OtherQuery28_B14", "31"));
                lstTypes.Items.Add(new ListItem("OtherQuery29_B19", "32"));
                lstTypes.Items.Add(new ListItem("OtherQuery30_B23", "33"));
                lstTypes.Items.Add(new ListItem("OtherQuery31_B20", "34"));
                lstTypes.Items.Add(new ListItem("OtherQuery32_B1a", "35"));
                lstTypes.Items.Add(new ListItem("OtherQuery33_B1b", "36"));
                lstTypes.Items.Add(new ListItem("OtherQuery34_C40", "37"));

                lstTypes.SelectedIndex = 0;
            }
            if (Report_ID == Report_1003)
            {

                lstTypes.Items.Add(new ListItem("TotalQuestions", "DkQuery1_Count"));
                lstTypes.Items.Add(new ListItem("Percentage", "DkQuery2_Per"));
                lstTypes.Items.Add(new ListItem("PercentagePerQuestion", "DkQuery2_PerQues"));
                lstTypes.Items.Add(new ListItem("NumberPerQuestion", "DkQuery2_NumberPerQues"));

                lstTypes.SelectedIndex = 0;
            }
            if (Report_ID == Report_1021)
            {

                lstTypes.Items.Add(new ListItem("TotalQuestions", "Others_Count"));
                lstTypes.Items.Add(new ListItem("Percentage", "Others_Per"));
                lstTypes.Items.Add(new ListItem("PercentagePerQuestion", "Others_PerQues"));
                lstTypes.Items.Add(new ListItem("NumberPerQuestion", "Others_NumberPerQues"));

                lstTypes.SelectedIndex = 0;
            }

        }


        private void gv_Bind_Data(string Filter_Data, string Report_ID, string Report_Name)
        {

            lblTableHeading.Text = Report_Name;
            string query = "";


            if (Report_ID == Report_1)
            {
                Console.WriteLine(Filter_Data);
                query = "SELECT top 100 * FROM [Report1]" + Filter_Data;
                Data_Source.Text = "Data Source: Supervisors app<br>";
                Data_Source.Visible = true;
            }

            else if (Report_ID == Report_2)
            {
                query = @"select top 100  [Name of District]
                        ,[Team Supervisor]
                        ,[Cluster Code]
                        ,[Completed]
                        ,[Postponed/ No respondent available]
                        ,[Dwelling Vacant]
                        ,[Refused]
                        ,[Partially complete children not found children missing]
                        ,[Incomplete]
                        ,[No eligible children in HH]
                        ,[Number of HHs Interviewed Excluded No Eligible Children]
                        ,[Total Number of HHs Interviewed] FROM [Report2]" + Filter_Data;
                Data_Source.Text = "Data Source: Interviewers submissions<br>";
                Data_Source.Visible = true;
            }
            else if (Report_ID == Report_21)
            {
                query = "select top 100  * FROM [Report2B]" + Filter_Data;
                Data_Source.Text = "Data Source: Supervisors app<br>";
                Data_Source.Visible = true;
            }

            else if (Report_ID == Report_3)
            {
                query = @"SELECT TOP 1000[Name of District]
      ,[Team Supervisor]
      ,[Name Of Enumerator]
      ,[Total Interviews Completed] as [Total Interviews submitted]
      ,[Average Number of Interviews Per Day]
      ,[Minimum Duration]
      ,[Average HH Size]
      ,[Min HH Size]
      ,[Maximum HH Size]
      ,[Average Number Of Children]
      ,[Min Number Of Children]
      ,[Maximum Number Of Children]
        FROM [Report4]" + Filter_Data;
                Data_Source.Text = "Data Source: Interviewers submissions<br>";
                Data_Source.Visible = true;
            }

            else if (Report_ID == Report_4)
            {

                query = "select top 100  * FROM [Report1_District]" + Filter_Data;
            }

            else if (Report_ID == Report_5)
            {

                query = "select top 100  [Name of District],[Team Supervisor],[Cluster Code],[Target HHs],[HHs Interviewed] as [HHs Submitted] ,[Percent Covered],[No. of Children as per Listing],[No. of Children as per HH Interview (Roster)],[No. of Children Interviewed],[Percent of children interviewd over roster] as [% of children interviewed over roster I/H],[%children listed vs. children roster] as [% of children in roster over children listed H/G] FROM [Added_Report_New2]" + Filter_Data + "order by 2";
            }

            else if (Report_ID == Report_6)
            {

                query = "select top 100  * FROM [Added_Report_New3]" + Filter_Data;
            }


            else if (Report_ID == Report_7)
            {

                query = "select top 100  * FROM [Supervisor_Hr_In_Field]" + Filter_Data;
            }

            else if (Report_ID == Report_8)
            {

                query = "select top 100  * FROM [Monitoring_Of_Enumerator]" + Filter_Data;
            }
            else if (Report_ID == Report_101)
            {
                query = "select top 100  * from [Non-applicable Percentage & Number]" + Filter_Data;
            }

            else if (Report_ID == Report_9)
            {

                query = "select top 100  * FROM [Monitoring_Of_Supervisors]" + Filter_Data;
            }

            else if (Report_ID == Report_10)
            {

                query = "select top 100  * FROM [Monitoring_Of_Observers]" + Filter_Data;
            }

            else if (Report_ID == Report_11)
            {

                query = "select top 100  * FROM [Observers_Report_Affirmatives]" + Filter_Data;
            }

            else if (Report_ID == Report_12)
            {

                query = "select top 100  * FROM [Observers_Report_WeightedIssues]" + Filter_Data;
            }

            else if (Report_ID == Report_13)
            {

                query = "select top 100  * FROM [Observers_Report_Absolute_Issues_Percent]" + Filter_Data;
            }

            else if (Report_ID == Report_14)
            {

                query = "select top 100  * FROM [Report3]" + Filter_Data;
            }

            else if (Report_ID == Report_15)
            {
                query = @"select top 100  [Name of District]
      ,[Target HHs]
      ,[HHs Interviewed] as [HHs submitted]
      ,[Percent Covered] as [Percent Covered C/B]
      ,[Start Date]
      ,[Number of Days Required]
      ,[Projected completion date (submitted data)]
      ,[Projected completion date (Bos)]
        FROM [Added_Report_New1]" + Filter_Data;
                Data_Source.Text = "Data Source: Interviewers submissions<br>";
                Data_Source.Visible = true;
            }
            else if (Report_ID == Report_16)
            {
                query = "select top 100  * FROM [Progress_of_field_monitor]" + Filter_Data;
            }
            else if (Report_ID == Report_1023)
            {
                if (Request["type"] == "100")
                {
                    query = "select top 100  * FROM [Summary_Progress_Monitor]";
                }
                else if (Request["type"] == "101")
                {
                    query = "select top 100  * FROM [Summary_Progress_Supervisor]";
                }
                else if (Request["type"] == "102")
                {
                    query = "select top 100  * FROM [Summary_Progress_Operator]";
                }
            }
            else if (Report_ID == Report_17)
            {
                query = "select top 100  * FROM [Progress_of_observers]" + Filter_Data;
            }
            else if (Report_ID == Report_1004)
            {

                string filter = "";
                try
                {
                    if (Session["Role"].ToString().Contains("DeskMonitor"))
                    {
                        filter = "where [Supervisor Code] in ( select top 100  [Supervisor_ID] from Supervisor where DeskMonitor_ID = '" + Session["Profile_ID"] + "') ";
                    }
                }
                catch (Exception)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                if (Session["Role"].ToString().Contains("DeskMonitor"))
                {
                    filter = "where [Supervisor Code] in ( select top 100  [Supervisor_ID] from Supervisor where DeskMonitor_ID = '" + Session["Profile_ID"] + "') ";
                    performance();
                }
                else if (Session["Role"].ToString().Contains("DataValidator"))
                {
                    filter = "where [Supervisor Code] in ( select top 100  [Supervisor_ID] from Supervisor where DeskMonitor_ID in ( select top 100  Monitor_ID from DeskMonitors where Operator_ID = '" + Session["Profile_ID"] + "')) ";

                }
                else if (Session["Role"].ToString().Contains("Supervisor"))
                {
                    filter = "where [Supervisor Code] = '" + Session["Profile_ID"] + "' ";

                }
                else
                    filter = "";

                if (Filter_Data == " ")
                {
                    if (Request["Type"] == "Corrected")
                        query = "select top 100   * FROM [SummaryR_Corrected] " + filter;
                    else if (Session["Role"].ToString().Contains("Supervisor"))
                        query = "select top 100   * FROM [SummaryR_SupervisorFilter] " + filter;
                    else if (Session["Role"].ToString().Contains("DataValidator"))
                        query = "select top 100   * FROM [SummaryR_ValidatorFilter] " + filter;
                    else
                    {
                        if (Session["summaryOrderAsc"] != null && Session["summaryOrderAsc"].ToString() == "True")
                            query = "select top 100   * FROM [SummaryR] " + filter + "  order by SubmissionDate asc ";
                        else
                            query = "select top 100   * FROM [SummaryR] " + filter + "  order by SubmissionDate desc ";
                    }

                }
                else
                {
                    if (Request["Type"] == "Corrected")
                        query = "select top 100  * from (select top 100  * FROM [SummaryR_Corrected] " + filter + ") as X " + Filter_Data;
                    else if (Session["Role"].ToString().Contains("Supervisor"))
                        query = "select top 100  * from (select top 100  * FROM [SummaryR_SupervisorFilter] " + filter + ") as X " + Filter_Data;
                    else if (Session["Role"].ToString().Contains("DataValidator"))
                        query = "select top 100  * from (select top 100  * FROM [SummaryR_ValidatorFilter] " + filter + ") as X " + Filter_Data;
                    else
                    {
                        if (Session["summaryOrderAsc"] != null && Session["summaryOrderAsc"].ToString() == "True")
                            query = "select top 100  * from (select top 100  * FROM [SummaryR] " + filter + ") as X " + Filter_Data + "  order by SubmissionDate asc ";
                        else
                            query = "select top 100  * from (select top 100  * FROM [SummaryR] " + filter + ") as X " + Filter_Data + "  order by SubmissionDate desc ";
                    }

                }
            }
            else if (Report_ID == Report_10044)
            {
                if (Filter_Data == " ")
                {
                    if (Request["Type"] == "Corrected")
                        query = "SELECT top 100 [HouseHold Key] ,[Cluster Code],[Household ID],[Name of District],[Name Of Enumerator],[Enumerator Code],[Name of Supervisor],[Supervisor Code],[Monitor_Name],[Tehsil],[Section1],[Section2],[Section3],[Section5],[Section7],[Section8],[Section9],[Section10],[Section12],[Others] FROM [SummaryR_Corrected] ";
                    else
                    {
                        if (Session["summaryOrderAsc"] != null && Session["summaryOrderAsc"].ToString() == "True")
                            query = "SELECT top 100 [HouseHold Key] ,[Cluster Code],[Household ID],[Name of District],[Name Of Enumerator],[Enumerator Code],[Name of Supervisor],[Supervisor Code],[Monitor_Name],[Tehsil],[Section1],[Section2],[Section3],[Section5],[Section7],[Section8],[Section9],[Section10],[Section12],[Others] FROM [SummaryR]   order by SubmissionDate asc ";
                        else
                            query = "SELECT top 100 [HouseHold Key] ,[Cluster Code],[Household ID],[Name of District],[Name Of Enumerator],[Enumerator Code],[Name of Supervisor],[Supervisor Code],[Monitor_Name],[Tehsil],[Section1],[Section2],[Section3],[Section5],[Section7],[Section8],[Section9],[Section10],[Section12],[Others] FROM [SummaryR]  order by SubmissionDate desc ";
                    }

                }
                else
                {
                    if (Request["Type"] == "Corrected")
                        query = "select top 100 [HouseHold Key] ,[Cluster Code],[Household ID],[Name of District],[Name Of Enumerator],[Enumerator Code],[Name of Supervisor],[Supervisor Code],[Monitor_Name],[Tehsil],[Section1],[Section2],[Section3],[Section5],[Section7],[Section8],[Section9],[Section10],[Section12],[Others] from (SELECT * FROM [SummaryR_Corrected] " + ") as X " + Filter_Data;
                    else
                    {
                        if (Session["summaryOrderAsc"] != null && Session["summaryOrderAsc"].ToString() == "True")
                            query = "select top 100 [HouseHold Key] ,[Cluster Code],[Household ID],[Name of District],[Name Of Enumerator],[Enumerator Code],[Name of Supervisor],[Supervisor Code],[Monitor_Name],[Tehsil],[Section1],[Section2],[Section3],[Section5],[Section7],[Section8],[Section9],[Section10],[Section12],[Others] from (SELECT * FROM [SummaryR] " + ") as X " + Filter_Data + " order by SubmissionDate asc ";
                        else
                            query = "select top 100 [HouseHold Key] ,[Cluster Code],[Household ID],[Name of District],[Name Of Enumerator],[Enumerator Code],[Name of Supervisor],[Supervisor Code],[Monitor_Name],[Tehsil],[Section1],[Section2],[Section3],[Section5],[Section7],[Section8],[Section9],[Section10],[Section12],[Others] from (SELECT * FROM [SummaryR] " + ") as X " + Filter_Data + " order by SubmissionDate desc ";
                    }

                }
            }
            else if (Report_ID == Report_1005)
            {

                query = "select top 100  * FROM [Enumerator_Mistakes_Count]" + Filter_Data;
            }
            else if (Report_ID == Report_1006)
            {

                query = "select top 100  * FROM [OutlierMistakes]" + Filter_Data;
            }
            else if (Report_ID == Report_1007)
            {

                query = "select top 100  * FROM [Others_Percentage_FieldWise]" + Filter_Data;
            }
            else if (Report_ID == Report_1008)
            {

                query = "select top 100  * FROM [Others_Question_percentage]" + Filter_Data;
            }
            else if (Report_ID == Report_1009)
            {

                query = "exec [Report_Others_Outlier]";
            }
            else if ((Report_ID == Report_1010))
            {
                if (Request["type"] == "100")
                {
                    query = "exec [Report_HH_Size_Magnitude]";
                }
                else
                {
                    query = "exec [Report_HH_Size]";
                }
            }
            else if (Report_ID == Report_1020)
            {

                query = "select top 100  * FROM [Others_Percentage_FieldWise]" + Filter_Data;
            }
            else if ((Report_ID == Report_1022))
            {
                if (Request["type"] == "100")
                {
                    query = "exec [Report_HH_Size_Child_Magnitude]";
                }
                else
                {
                    query = "exec [Report_HH_Size_Child]";
                }
            }
            else if (Report_ID == Report_1021)
            {
                if (!IsPostBack)
                    query = "Others_Count";
                else
                {
                    query = Filter_Data;
                }
            }
            else if (Report_ID == Report_1001)
            {
                //  if (Filter_Data == "Tools")
                query = "exec [Report_Progress_Coding] @type=3";
                // else if (Filter_Data == "Occupation")
                //   query = "SELECT * FROM [Occupation_Text_Review]";
                //else
                //  query = "SELECT * FROM [Industries_Text_Review]";

            }
            else if (Report_ID == Report_191)
            {
                query = "select top 100  * from [COVID19 Risk Assmnt Enumerator]";
            }
            else if (Report_ID == Report_192)
            {
                query = "select top 100  * from [COVID-19 Risk Assmnt Supervisor]";
            }
            else if (Report_ID == Report_193)
            {
                query = "select top 100  * from [COVID19 Risk Assmnt District]";
            }
            else if (Report_ID == Report_194)
            {
                query = "select top 100  * from [COVID19 Risk Assmnt Cluster Code]";
            }

            else if (Report_ID == Report_1002)
            {
                if (!IsPostBack)
                    query = "OtherQuery2_A17";
                else
                {

                    query = Filter_Data;
                }
            }

            else if (Report_ID == Report_18)
            {
                query = "select top 100  * from [Number of assigned queries]";
            }
            else if (Report_ID == Report_19)
            {
                query = "select top 100  * from [Summary of correction and revision of queries]";
            }
            else if (Report_ID == Report_1003)
            {
                if (!IsPostBack)
                    query = "DkQuery1_Count";
                else
                {

                    query = Filter_Data;
                }
            }

            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = con;

            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (Report_ID == Report_1004)
            {
                GridView2.DataSource = dt;
                GridView2.DataBind();
                GridView1.Visible = false;

            }
            GridView1.DataSource = dt;
            GridView1.DataBind();


            if (Report_ID == Report_1001 || Report_ID == Report_10044 || Report_ID == Report_1002 || Report_ID == Report_1003 || Report_ID == Report_1004 || (Report_ID == Report_1005) || (Report_ID == Report_1006) || (Report_ID == Report_1007) || (Report_ID == Report_1008) || (Report_ID == Report_1009) || (Report_ID == Report_1010) || (Report_ID == Report_18))
            {

            }

            else
            {
                GirdView_Footer_Total();
            }


        }
        protected void lstMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMonitors.SelectedItem.Text == "All")
            {
                Filter_Data = "";

            }
            else
            {
                Filter_Data = "WHERE [Monitor_Name] = '" + lstMonitors.SelectedItem.Text + "'";

            }

            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);
        }
        protected void lstDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstDistricts.SelectedItem.Text == "All")
            {
                Filter_Data = "";

            }
            else
            {
                Filter_Data = "WHERE [Name of District] = '" + lstDistricts.SelectedItem.Text + "'";

            }


            logFile.log("Filter------" + Filter_Data, Session["username"].ToString());
            load_Filter_SurveyTeam(Filter_Data);
            load_Filter_ClusterCodes(Filter_Data);
            load_Filter_Enumerator(Filter_Data);



            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);
        }

        protected void lstSurveyTeam_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lstSurveyTeam.SelectedItem.Text == "All")
            {
                Filter_Data = Filter_Data + "";

            }
            else
            {
                if (Filter_Data == "")
                {
                    Filter_Data = " Where [Team Supervisor] = '" + lstSurveyTeam.SelectedItem.Text + "'";

                }
                else
                {
                    Filter_Data = Filter_Data + " and [Team Supervisor] = '" + lstSurveyTeam.SelectedItem.Text + "'";
                }

            }



            if (lblClusterCode.Visible == true)
            {
                load_Filter_ClusterCodes(Filter_Data);
            }

            else if (lstEnumerator.Visible == true)
            {
                load_Filter_Enumerator(Filter_Data);
            }

            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);
        }

        protected void lstClusterCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstClusterCodes.SelectedItem.Text == "All")
            {
                Filter_Data = Filter_Data + "";

            }
            else
            {
                if (Filter_Data == "")
                {
                    Filter_Data = " Where [Cluster Code] = '" + lstClusterCodes.SelectedItem.Text + "'";

                }
                else
                {
                    Filter_Data = Filter_Data + " and [Cluster Code] = '" + lstClusterCodes.SelectedItem.Text + "'";
                }

            }


            load_Filter_Enumerator(Filter_Data);
            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);
        }

        protected void performance()
        {
            progress.Visible = true;
            string query = "select [Total Assigned],[Reviewed & Resolved] + [Reviewed & Closed] as [Resolved & Corrected] ,[Reviewed & Reassigned] from [Summary_Progress_Monitor] where Monitor_Name = ( SELECT [Username] FROM [dbo].[users] where UserId =  '" + Session["UserID"] + "')";
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = con;

            cmd.CommandTimeout = 0;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                assigned.InnerText = dt.Rows[0]["Total Assigned"].ToString();
                resolved.InnerText = dt.Rows[0]["Resolved & Corrected"].ToString();
                reassigned.InnerText = dt.Rows[0]["Reviewed & Reassigned"].ToString();
            }


        }

        protected void lstEnumerator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstEnumerator.SelectedItem.Text == "All")
            {
                Filter_Data = Filter_Data + "";

            }
            else
            {
                if (Filter_Data == "")
                {
                    Filter_Data = " Where [Name Of Enumerator] = '" + lstEnumerator.SelectedItem.Text + "'";

                }
                else
                {
                    Filter_Data = Filter_Data + " and [Name Of Enumerator] = '" + lstEnumerator.SelectedItem.Text + "'";
                }

            }

            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);

        }
        protected void lstTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Report_ID == Report_1010)
                Response.Redirect("surveyProgress.aspx?id=1010&type=" + lstTypes.SelectedItem.Value + "&name=Household Size Report");
            else if (Report_ID == Report_1022)
                Response.Redirect("surveyProgress.aspx?id=1022&type=" + lstTypes.SelectedItem.Value + "&name=Number of children");
            else if (Report_ID == Report_1023)
                Response.Redirect("surveyProgress.aspx?id=1023&type=" + lstTypes.SelectedItem.Value + "&name=D10%20Monitoring%20Correction%20Progress");

            else
                Filter_All_Selected_Items();
        }

        protected void Filter_All_Selected_Items()
        {



            if (lstTypes.SelectedItem != null)
            {
                if (lstTypes.SelectedItem.Value == "1")
                {

                    Filter_Data = "Industries_Text_Review";
                }

                else
                {
                    if (Filter_Data == "2")
                    {
                        Filter_Data = "Occupation_Text_Review";

                    }
                    else if (Filter_Data == "3")
                    {
                        Filter_Data = "Tools_Text_Review";
                    }
                    else
                    {
                        if (Report_ID == Report_1003 || Report_ID == Report_1021)
                            Filter_Data = lstTypes.SelectedItem.Value;
                        else if (lstTypes.SelectedItem != null)
                            Filter_Data = lstTypes.SelectedItem.Text;

                    }

                }
            }

            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);


        }

        private void GirdView_Footer_Total()
        {
            string[] columntypes = new string[100];
            string columnName;

            float totalHH = 0;
            float riskHH = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                string type = "Int32";
                int count = 1;
                int count2 = 0;

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    columnName = dt.Columns[j].ColumnName.ToString();


                    if (type == dt.Columns[j].DataType.Name.ToString())
                    {
                        if (count2 == 0)
                        {
                            GridView1.FooterRow.Cells[0].ColumnSpan = j;
                            GridView1.FooterRow.Cells[0].Text = "Total";
                            if (Report_ID == Report_3)
                                GridView1.FooterRow.Cells[0].Text = "Total <font color=red>(Average for columns E-L)</font>";
                            int c = (dt.Columns.Count / 2) - 1;

                            if ((dt.Columns.Count & 1) != 0)
                            {
                                c = c + 1;
                            }


                            int a = 1;

                            for (int i = 1; i < j; i++)
                            {
                                if (a == c)
                                {
                                    a = 1;
                                }

                                GridView1.FooterRow.Cells.RemoveAt(a);
                                a++;
                            }

                            count2++;

                        }


                        if (((count == 3) && (Report_ID == Report_5)) || ((count == 3) && (Report_ID == Report_3)) || ((count == 4) && (Report_ID == Report_3)) || ((count == 5) && (Report_ID == Report_3)) || ((count == 6) && (Report_ID == Report_3)) || ((count == 7) && (Report_ID == Report_3)) || ((count == 8) && (Report_ID == Report_3)) || ((count == 9) && (Report_ID == Report_3)) || ((count == 8) && (Report_ID == Report_5)) || ((count == 3) && (Report_ID == Report_15)))
                        {
                            float total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            float total0 = dt.Rows.Count;
                            float total2 = total1 / total0;
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].Text = total2.ToString();
                        }
                        else if ((count == 5) && (Report_ID == Report_15))
                        {

                        }
                        else if (columnName != "Average no. of children(5-17) in HH with at least 1 working child" && columnName != "Average Number of Interviews Per Day" && columnName != "Average number of working children in HH with at least 1 working children" && (Report_ID == Report_18))
                        {
                            Int32 total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].Text = total1.ToString();
                        }
                        else
                        {

                            float total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            float total0 = dt.Rows.Count;
                            float total2 = total1;/// total0;
                            if (columnName == "Total no. of households initiated ")
                            {
                                totalHH = total2;
                            }
                            if (columnName == "Total no. of households that failed the risk assessment")
                            {
                                riskHH = total2;
                            }
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            if (columnName == "Percentage of households that failed the risk assessment (F/E)" || columnName == "Percentage of households that failed the risk assessment (C/B)" || columnName == "Percentage of households that failed the risk assessment (D/C)")
                            {
                                GridView1.FooterRow.Cells[count].Text = Math.Round(((riskHH * 100) / totalHH)) + "%".ToString();
                            }

                            else
                            {
                                GridView1.FooterRow.Cells[count].Text = total2.ToString();
                            }
                        }
                    }
                    else if ((count == 7) && (Report_ID == Report_5))
                    {
                        int Column_I = Convert.ToInt32(GridView1.FooterRow.Cells[6].Text.ToString());
                        int Column_H = Convert.ToInt32(GridView1.FooterRow.Cells[5].Text.ToString());
                        float Avg = Column_I == 0 || Column_H == 0 ? 0 : (Column_I / Column_H) * 100;
                        decimal m = Convert.ToDecimal(Avg);
                        decimal d = Math.Round(m, 2);
                        GridView1.FooterRow.Cells[count].Text = d.ToString();
                    }
                    else if ((count == 8) && (Report_ID == Report_5))
                    {
                        int Column_H = Convert.ToInt32(GridView1.FooterRow.Cells[5].Text.ToString());
                        int Column_G = Convert.ToInt32(GridView1.FooterRow.Cells[4].Text.ToString());
                        float Avg = Column_H == 0 || Column_G == 0 ? 0 : (Column_H / Column_G) * 100;
                        decimal m = Convert.ToDecimal(Avg);
                        decimal d = Math.Round(m, 2);
                        GridView1.FooterRow.Cells[count].Text = d.ToString();
                    }
                    else if ((count == 5) && (Report_ID == Report_1))
                    {
                        int Column_F = Convert.ToInt32(GridView1.FooterRow.Cells[4].Text.ToString());
                        int Column_C = Convert.ToInt32(GridView1.FooterRow.Cells[1].Text.ToString());
                        float Avg = Column_F == 0 || Column_C == 0 ? 0 : (Column_F / Column_C) * 100;
                        decimal m = Convert.ToDecimal(Avg);
                        decimal d = Math.Round(m, 2);
                        GridView1.FooterRow.Cells[count].Text = d.ToString();
                    }
                    else if ((count == 6) && (Report_ID == Report_1))
                    {
                        int Column_D = Convert.ToInt32(GridView1.FooterRow.Cells[2].Text.ToString());
                        int Column_C = Convert.ToInt32(GridView1.FooterRow.Cells[1].Text.ToString());
                        float Avg = Column_D == 0 || Column_C == 0 ? 0 : (Column_D / Column_C) * 100;
                        decimal m = Convert.ToDecimal(Avg);
                        decimal d = Math.Round(m, 2);
                        GridView1.FooterRow.Cells[count].Text = d.ToString();
                    }
                    else if ((count == 3) && (Report_ID == Report_15))
                    {
                        int Column_B = Convert.ToInt32(GridView1.FooterRow.Cells[1].Text.ToString());
                        int Column_C = Convert.ToInt32(GridView1.FooterRow.Cells[2].Text.ToString());
                        float Avg = Column_B == 0 || Column_C == 0 ? 0 : (Column_B / Column_C) * 100;
                        decimal m = Convert.ToDecimal(Avg);
                        decimal d = Math.Round(m, 2);
                        GridView1.FooterRow.Cells[count].Text = d.ToString();
                    }


                    if (count2 > 0)
                        count++;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            lstSurveyTeam_SelectedIndexChanged(this, EventArgs.Empty);
        }
        public string CheckColor(string key_, int section)
        {
            string color = "";
            using (SqlCommand cmd = new SqlCommand("exec [CheckColor] @key_='" + key_ + "', @Section='" + section + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        color = dt.Rows[0]["Color"].ToString();
                    }
                }
            }
            return color;
        }
        public string CheckAssigned(string key_, int section, string role)
        {
            string color = "";
            using (SqlCommand cmd = new SqlCommand("exec [Check_assgined] @key_='" + key_ + "', @Section='" + section + "', @role='" + role + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;

                    cmd.CommandTimeout = 0;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        color = dt.Rows[0]["Assigned"].ToString();
                    }
                }
            }
            return color;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                char c = 'A';

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (c < 'Z')
                        e.Row.Cells[i].Text = c + "<br />" + e.Row.Cells[i].Text;
                    e.Row.Cells[i].Style.Add("vertical-align", "Top");


                    c++;

                    if (e.Row.Cells[i].Text.Contains("Percent of Children"))
                    {
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text + "<br />" + "(I / H) * 100";
                    }

                    if (e.Row.Cells[i].Text.Contains("Children listed vs. Children in HH Roster"))
                    {
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text + "<br />" + "(G - H / H) * 100";
                    }
                }



            }
            else if (Report_ID != Report_1004)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //loop all cells in the row
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        //set the tooltip text to be the header text
                        e.Row.Cells[i].ToolTip = GridView1.HeaderRow.Cells[i].Text;
                    }
                }
            }

            string ulTagOpen = "<ul style='margin-left: 0%; color:black;  padding-left:1em; font-size:smaller; text-align: justify; text-justify:inter-word; width:90%;  list-style-type:disc ;color:black'>";
            string ulTagClose = "</ul>";
            string liRedTagOpen = "<li style='margin-top:10px; '><span style=' background-color:Coral;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liRedTagClose = "</li>";
            string liYellowTagOpen = "<li style='margin-top:10px; '><span style=' background-color:Yellow;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liYellowTagClose = "</li>";
            string liOrangeTagOpen = "<li style='margin-top:10px; '><span style=' background-color:Orange;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liOrangeTagClose = "</li>";
            string liWhiteTagOpen = "<li style='margin-top:10px; '><span style=' background-color:White;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liWhiteTagClose = "</li>";
            string liGreenTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:YellowGreen;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liGreenTagClose = "</li>";
            string liKhakiTagOpen = "<li style='margin-top: 10px;'><span style='background-color:khaki;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liKhakiTagClose = "</li>";
            string liMediumAquamarineTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:MediumAquamarine;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liMediumAquamarineTagClose = "</li>";



            if (e.Row.RowIndex >= 0)
            {
                if (Report_ID == Report_1010)
                {
                    if (Convert.ToInt32(e.Row.Cells[7].Text) > 0)
                    {
                        e.Row.Cells[7].BackColor = System.Drawing.Color.Coral;
                    }
                }
                if (Report_ID == Report_1022)
                {
                    if (Convert.ToInt32(e.Row.Cells[7].Text) > 0)
                    {
                        e.Row.Cells[7].BackColor = System.Drawing.Color.Coral;
                    }
                }
                if (Report_ID == Report_1)
                {
                    e.Row.Cells[7].Text = e.Row.Cells[7].Text + "%";
                    e.Row.Cells[6].Text = e.Row.Cells[6].Text + "%";

                    if (Convert.ToInt32(e.Row.Cells[3].Text) > 6)
                    {
                        e.Row.Cells[3].BackColor = System.Drawing.Color.Coral;
                    }

                    else if (Convert.ToInt32(e.Row.Cells[3].Text) <= 6)
                    {
                        e.Row.Cells[3].BackColor = System.Drawing.Color.YellowGreen;
                    }

                    lblLegendsHeading.Visible = false;
                    lblLegendsText.Visible = false;

                    //lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Number of Enumeration Blocks in Progress is more than 6 " + liRedTagClose + liGreenTagOpen + "Number of Enumeration Blocks in Progress’ is less than or equal to 6" + liGreenTagClose + ulTagClose;

                }
                else if (Report_ID == Report_1003)
                {

                    if (Filter_Data == "DkQuery2_PerQues")
                    {
                        for (int i = 1; i <= 41; i++)
                        {
                            if (Convert.ToDecimal(e.Row.Cells[i].Text) > 0)
                            {
                                e.Row.Cells[i].BackColor = System.Drawing.Color.Coral;
                            }
                        }
                    }
                    else if (Convert.ToDecimal(e.Row.Cells[1].Text) > 0)
                    {
                        e.Row.Cells[1].BackColor = System.Drawing.Color.Coral;
                    }



                    lblLegendsHeading.Visible = false;
                    lblLegendsText.Visible = false;

                    //lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Number of Enumeration Blocks in Progress is more than 6 " + liRedTagClose + liGreenTagOpen + "Number of Enumeration Blocks in Progress’ is less than or equal to 6" + liGreenTagClose + ulTagClose;

                }
                /* else if (Report_ID == Report_2)
                 {
                     if (Convert.ToInt32(e.Row.Cells[10].Text) > 6)
                     {
                         e.Row.Cells[10].BackColor = System.Drawing.Color.Coral;
                     }

                     else if (Convert.ToInt32(e.Row.Cells[10].Text) == 0)
                     {
                         e.Row.Cells[10].BackColor = System.Drawing.Color.YellowGreen;
                     }

                     else if (Convert.ToInt32(e.Row.Cells[10].Text) < 0)
                     {
                         e.Row.Cells[10].BackColor = System.Drawing.Color.Black;
                         e.Row.Cells[10].ForeColor = System.Drawing.Color.White;
                     }

                     else
                     {
                         e.Row.Cells[10].BackColor = System.Drawing.Color.Khaki;
                     }

                     lblLegendsHeading.Visible = true;
                     lblLegendsText.Visible = true;

                     lblLegendsText.Text = ulTagOpen + liRedTagOpen + "If the 'Final Balance’ is greater than 6" + liRedTagClose + liGreenTagOpen + "If the 'Final Balance’ is 0" + liGreenTagClose + liKhakiTagOpen + "1<=x<=6" + liKhakiTagClose + ulTagClose;


                 }*/
                else if (Report_ID == Report_1006)
                {
                    for (int i = 4; i <= 13; i++)
                    {
                        if (Convert.ToInt32(e.Row.Cells[i].Text) < 0)
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Coral;
                            int value = Convert.ToInt32(e.Row.Cells[i].Text) * -1;
                            e.Row.Cells[i].Text = value.ToString();
                        }
                    }
                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;
                    lblLegendsText.Text = ulTagOpen + liOrangeTagOpen + "Outlier according to the 2std rule" + liOrangeTagClose + ulTagClose;

                }
                else if (Report_ID == Report_1004 || Report_ID == Report_10044)
                {

                    for (int i = 10; i <= 17; i++)
                    {

                        int section = i - 9;
                        if (i == 13)
                        {
                            section++;
                        }
                        else if (i == 14 || i == 17 || i == 16)
                        {
                            section += 2;
                        }
                        else if (i == 18)
                        {
                            section = 12;
                        }
                        if (section == 6)
                        {
                            section = 8;
                        }
                        if (CheckColor(e.Row.Cells[0].Text, section) == "0")
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Orange;
                        }
                        else if (CheckColor(e.Row.Cells[0].Text, section) == "1")
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.GreenYellow;
                        }
                        else if (CheckColor(e.Row.Cells[0].Text, section) == "2")
                        {
                            e.Row.Cells[i].BackColor = System.Drawing.Color.Yellow;
                        }
                    }
                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;
                    lblLegendsText.Text = ulTagOpen + liWhiteTagOpen + "All or at least one query is pending to be revised by the desk monitor" + liWhiteTagClose + liGreenTagOpen + "All queries have been resolved" + liGreenTagClose + liOrangeTagOpen + "At least on query has been closed and the rest are resolved" + liOrangeTagClose + liYellowTagOpen + "At least one query has been assigned to the Data validator or Field Monitor" + liYellowTagClose + ulTagClose;


                }
                else if (Report_ID == Report_1021)
                {
                    /*  int i = 3;
                          if (Convert.ToInt32(e.Row.Cells[i].Text) < 0)
                          {
                              e.Row.Cells[i].BackColor = System.Drawing.Color.Coral;
                              int value = Convert.ToInt32(e.Row.Cells[i].Text) * -1;
                              e.Row.Cells[i].Text = value.ToString();
                          }*/

                }
                else if (Report_ID == Report_3)
                {
                    int minHHSize = 1000, maxHHSize = 0;

                    int cellValue = 0;
                    string cellDistrict = "";
                    int minDuration = 0;
                    int minHHSize_value = 0, maxHHSize_value = 0;
                    try
                    {
                        cellDistrict = e.Row.Cells[0].Text.ToString();
                        cellValue = Convert.ToInt32(e.Row.Cells[6].Text.ToString());
                        minDuration = Convert.ToInt32(e.Row.Cells[5].Text.ToString());

                        minHHSize_value = Convert.ToInt32(e.Row.Cells[7].Text.ToString());
                        maxHHSize_value = Convert.ToInt32(e.Row.Cells[8].Text.ToString());
                    }
                    catch (Exception) // Null Value
                    {

                    }

                    //HH Size min/max
                    //if (minHHSize_value == minHHSize)
                    //  e.Row.Cells[7].BackColor = System.Drawing.Color.Khaki;
                    //if (maxHHSize == maxHHSize_value)
                    //e.Row.Cells[8].BackColor = System.Drawing.Color.Khaki;


                    //Duration
                    if (minDuration <= 30)
                        e.Row.Cells[5].BackColor = System.Drawing.Color.Coral;
                    //else if (minDuration <= 40)
                    //  e.Row.Cells[5].BackColor = System.Drawing.Color.Khaki;


                    //todo
                    //if (cellDistrict.Equals("Chakwal"))
                    //{
                    //    if (cellValue >= 5.7)
                    //        e.Row.Cells[6].BackColor = System.Drawing.Color.YellowGreen;
                    //    else if (cellValue >= 4.7)
                    //        e.Row.Cells[6].BackColor = System.Drawing.Color.Khaki;
                    //    else
                    //        e.Row.Cells[6].BackColor = System.Drawing.Color.Coral;
                    //}
                    //else if (cellDistrict.Equals("Muzaffargarh"))
                    //{

                    //    if (cellValue >= 7.4)
                    //        e.Row.Cells[6].BackColor = System.Drawing.Color.YellowGreen;
                    //    else if (cellValue >= 6.4)
                    //        e.Row.Cells[6].BackColor = System.Drawing.Color.Khaki;
                    //    else
                    //        e.Row.Cells[6].BackColor = System.Drawing.Color.Coral;
                    //}

                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;



                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "MIN DURATION: Less than 30 Minutes | Avg HH SIZE: If the HH size is less than 'District avg HH size' minus one" + liRedTagClose + ulTagClose;
                    //+liGreenTagOpen + "Avg HH SIZE: If the average HH size equal to or greater than the 'District given average HH size" + liGreenTagClose + liKhakiTagOpen + "MIN DURATION: Between 31 & 40 Minutes | Avg HH SIZE: If the HH size is less than the given 'District avg HH size' and the difference is less than 1" + liKhakiTagClose + ulTagClose;


                }

                else if (Report_ID == Report_4)
                {


                    try
                    {

                        e.Row.Cells[6].Text = e.Row.Cells[6].Text + "%";
                    }
                    catch (Exception) // Null Value
                    {

                    }
                }

                else if (Report_ID == Report_5)
                {
                    double cellValue = 0, cellValue2 = 0;

                    try
                    {
                        cellValue = Convert.ToDouble(e.Row.Cells[9].Text.ToString());

                        cellValue2 = Convert.ToDouble(e.Row.Cells[10].Text.ToString());

                        e.Row.Cells[9].Text = cellValue + "%";
                        e.Row.Cells[10].Text = cellValue2 + "%";

                    }
                    catch (Exception) // Null Value
                    {

                    }

                    if (cellValue >= 90)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.YellowGreen;
                    else if (cellValue >= 80 && cellValue < 90)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.Khaki;
                    else if (cellValue < 80)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.Coral;


                    if (cellValue2 >= 90 && cellValue2 <= 100)
                        e.Row.Cells[10].BackColor = System.Drawing.Color.YellowGreen;
                    else if (cellValue2 >= 80 && cellValue2 < 90)
                        e.Row.Cells[10].BackColor = System.Drawing.Color.Khaki;
                    else if (cellValue2 < 80)
                        e.Row.Cells[10].BackColor = System.Drawing.Color.Coral;

                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;

                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Efficency: Target # of children as per listing <80%" + liRedTagClose + liKhakiTagOpen + "Efficency: Target # of children as per listing <90% & >80%" + liKhakiTagClose + liGreenTagOpen + "Efficency: Target # of children as per listing >=90% <=100%" + liGreenTagClose + ulTagClose;


                }

                else if (Report_ID == Report_6)
                {
                    int cellValue = 0;

                    try
                    {
                        cellValue = Convert.ToInt32(e.Row.Cells[4].Text.ToString());
                    }
                    catch (Exception) // Null Value
                    {

                    }

                    if (cellValue <= 3)
                        e.Row.Cells[4].BackColor = System.Drawing.Color.Coral;

                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;

                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Efficency: Less than 3 forms submitted" + liRedTagClose + ulTagClose;

                }

                else if (Report_ID == Report_7)
                {
                    double cellValue = 0;
                    int cellValue2 = 0;

                    try
                    {
                        cellValue = Convert.ToDouble(e.Row.Cells[5].Text.ToString());
                        cellValue2 = Convert.ToInt32(e.Row.Cells[6].Text.ToString());
                    }
                    catch (Exception) // Null Value
                    {

                    }

                    //    if (cellValue <= 5)
                    //      e.Row.Cells[5].BackColor = System.Drawing.Color.Khaki;
                    //else if (cellValue >= 10)
                    //  e.Row.Cells[5].BackColor = System.Drawing.Color.Coral;

                    if (cellValue2 <= 3)
                        e.Row.Cells[6].BackColor = System.Drawing.Color.Coral;

                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;

                    // lblLegendsText.Text = "<span style=' padding: 2px ; border: 1px solid #000; display: inline; float: left;margin-left: 15%;'> Hour in Field " + ulTagOpen + liRedTagOpen + "Greater than or equal to 10 hours" + liRedTagClose + liKhakiTagOpen + "Less than or equal to 5 hours" + liKhakiTagClose + ulTagClose + "</span>";
                    lblLegendsText.Text = "<span style=' padding: 14px ; border: 1px solid #000; display: inline; float: left;margin-left: 5px;'>Total Collections" + ulTagOpen + liRedTagOpen + "Less than or equal to 3 Collections" + liRedTagClose + ulTagClose + "&nbsp;&nbsp;&nbsp;&nbsp;</span>";

                }

                else if (Report_ID == Report_8 || Report_ID == Report_9 || Report_ID == Report_10)
                {
                    int cellValue = 0;
                    int columnumber = 6;
                    if (Report_ID == Report_9)
                        columnumber = 5;
                    try
                    {

                        cellValue = Convert.ToInt32(e.Row.Cells[columnumber].Text.ToString());

                    }
                    catch (Exception) // Null Value
                    {

                    }

                    if (cellValue < 85)
                        e.Row.Cells[columnumber].BackColor = System.Drawing.Color.Coral;
                    else if (cellValue >= 85 && cellValue < 100)
                        e.Row.Cells[columnumber].BackColor = System.Drawing.Color.Khaki;
                    else
                        e.Row.Cells[columnumber].BackColor = System.Drawing.Color.YellowGreen;


                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;

                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Affirmative Answers less than 80%" + liRedTagClose + liGreenTagOpen + "Affirmative Answers 90%-100%" + liGreenTagClose + liKhakiTagOpen + " Affirmative Answers 80%-90%" + liKhakiTagClose + ulTagClose;

                }

                else if (Report_ID == Report_6)
                {
                    double cellValue = 0;

                    try
                    {
                        cellValue = Convert.ToDouble(e.Row.Cells[5].Text.ToString());


                    }
                    catch (Exception) // Null Value
                    {

                    }

                    if (cellValue >= 100)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.YellowGreen;
                    else if (cellValue <= 5)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.Khaki;
                    else if (cellValue >= 10)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.Coral;




                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;

                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Efficency: Target # of children as per listing <-20%" + liRedTagClose + liMediumAquamarineTagOpen + "Efficency: Target # of children as per listing <-10% & > 0%" + liMediumAquamarineTagClose + liGreenTagOpen + "Efficency: Target # of children as per listing >= 0%" + liGreenTagClose + liKhakiTagOpen + "Efficency: Target # of children as per listing <-20% & > -10%" + liKhakiTagClose + ulTagClose;


                }
            }
        }

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            //Filter_All_Selected_Items();
            lstSurveyTeam_SelectedIndexChanged(this, EventArgs.Empty);
        }
    }
}