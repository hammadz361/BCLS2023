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
    public partial class Dashboard1 : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

        public static string chartTitle1 = "O1. Overall Survey Progress";
        public static string chartTitle2 = "C2-1. Survey Progress at the Cluster Initation Level";
        public static string chartTitle21 = "C2-2. Survey Progress at the Cluster Finilization Level";
        public static string chartTitle22 = "C2-3. Survey Progress at the Cluster Revisit Level";
        public static string chartTitle3 = "C3. Cluster Target vs Achievement";
        public static string chartTitle4 = "O2. Completed vs Remaining HHs";
        public static string chartTitle5 = "H2. Enumeration Progress (Initiated vs Remaining Households)";
        public static string chartTitle6 = "H3. No. Of HH visited: interviewed vs. Not completed";
        public static string chartTitle7 = "S1-1. Clusters assigned: initiated, completed and remaining";
        public static string chartTitle77 = "S1-2. Clusters assigned: initiated, completed and remaining";
        public static string chartTitle8 = "S2-1. Clusters Completed Percentage (by Team Supervisors)";
        public static string chartTitle88 = "S2-2. Clusters Completed Percentage (by Team Supervisors)";

        public static string chartTitle9 = "CH1. Children Statistics as per HH Listing, HH Roster and Child Interviews";
        public static string chartTitle10 = "CH2. Total vs Working Children";
        public static string chartTitle11 = "C1. Overall evolution: Target vs. Completed clusters (%)";
        public static string chartTitle12 = "H1. Overall evolution: Target vs. completed households (%)";

        //public static string chartTitle123 = "H3. Kohistan Comppleted VS Pending Listing (%)";
        public static int Target_HH = 0;
        public static string Filter_Data = "";
        public static string Filter = "All";
        public static string QueryFilter = "";
        public static string chart_SurveyProgress_ClusterLevel = "";
        public static string chart_SurveyProgress_ClusterLevel11 = "";
        public static string chart_SurveyProgress_ClusterLevel22 = "";
        public static string chart_ClusterTarget_Achievement = "";
        public static string chart_Completed_RemainingHHs = "";
        public static string Piechart_Overall_evolution = "";
        public static string chart_Enumeration_Progress = "";
        public static string chart_HH_Visited_Interview_not_Completed = "";
        public static string chart_Children_Statistics = "";
        public static string chart_Total_Working_Childern = "";
        public static string chart_district = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else if (Session["username"] == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else
            {
                
                logFile.log("Dashboard", Session["username"].ToString());

                Session["lblHeadingName"] = "<b>" + Session["reg"] + "</b>-Dashboard";

                if (!string.IsNullOrEmpty(Request.Params["District"]))
                {
                    string DistrictsGet = Request["District"];
                    string[] Fields = DistrictsGet.Split(',');
                    string whereAnd = "";
                    int totalcommas = Fields.Count();
                    totalcommas--;
                    for (int i = 0; i < totalcommas; i++)
                    {
                        if (Fields[i] == "All")
                        {
                            whereAnd += " 1=1";
                        }
                        else
                        {
                            if (i == 0)
                                whereAnd += "[Name of District]='";
                            else
                                whereAnd += " OR [Name of District]='";
                            whereAnd += Fields[i];
                            whereAnd += "'";
                        }
                    }
                    QueryFilter = "WHERE AND " + whereAnd;
                }
                else
                {
                    QueryFilter = "WHERE 1=1";
                }
                //Response.Write(QueryFilter) ;
                if (QueryFilter.Equals("WHERE"))
                    QueryFilter = "";
                Key_Indicators();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateCharts()", true);
                load_Filter_Districts();
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
                        {
                            lstDistricts.Items.Add(dt.Rows[i].Field<string>(0));
                            var ii = dt.Rows[i].Field<string>(0);
                        }


                    }
                }
            }

        }

        private void Key_Indicators()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand("Key_Indicators", con);
            sql.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            foreach (DataRow row in dataset.Tables[0].Rows)
            {


                //chartLabel.Append(string.Format("'{0}',", row["Date"].ToString()));
                lbl_Indicator_District.Text = row["Districts"].ToString();
                lbl_Indicator_Enumerator.Text = row["Enumerators"].ToString();
                lbl_Indicator_Supervisor.Text = row["Supervisors"].ToString();
                lbl_Indicator_HH.Text = row["Households"].ToString();
                lbl_Indicator_Childern.Text = row["Children"].ToString();
                LastSubmissionDate.Text = row["LastSubmissionDate"].ToString();






            }
        }



        [System.Web.Services.WebMethod]
        public static List<string> getChartData1()
        {
            // System.Threading.Thread.Sleep(5000);

            var returnData = new List<string>();

            // Hasnain Machine
            //var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            // AHMAD Machine 

            var sql = new SqlCommand("ChartLine", con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var TotalFormSubmitted = new StringBuilder();
            var HH_Covered_on_the_day = new StringBuilder();
            var RequiredDays = new StringBuilder();
            var Line_C = new StringBuilder();

            //chartLabel.Append("[");
            TotalFormSubmitted.Append("[");
            HH_Covered_on_the_day.Append("[");
            RequiredDays.Append("[");
            Line_C.Append("[");

            int TotalHHs_Interviewed = 0;
            int TotalDays = 0;
            int Average_per_Day = 0;
            int Total_Work_days_req = 0;
            int c = 0;


            //draw 1st Line Green

            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                TotalHHs_Interviewed = TotalHHs_Interviewed + Convert.ToInt32(row["TotalForm"]);

                //chartLabel.Append(string.Format("'{0}',", row["Date"].ToString()));
                TotalFormSubmitted.Append(string.Format("{0},", TotalHHs_Interviewed.ToString()));

                HH_Covered_on_the_day.Append(string.Format("'{0}',", row["SubmissionDate"].ToString()));




                if (TotalDays == dataset.Tables[0].Rows.Count - 1)
                {
                    RequiredDays.Append(string.Format("{0},", TotalHHs_Interviewed.ToString()));
                }

                else
                {
                    RequiredDays.Append(string.Format("{0},", "null"));
                }

                TotalDays++;
            }


            //draw 2nd Line blue

            // Average_per_Day = TotalHHs_Interviewed / TotalDays;
            if (con.State != ConnectionState.Closed) { con.Close(); }
            con.Open();
            sql = new SqlCommand("  Select count(distinct(cluster_code))*[Count of HH_ID] from ReferenceTable group by [Count of HH_ID]", con);
            Target_HH = (int)sql.ExecuteScalar();
            con.Close();
            con.Open();
            sql = new SqlCommand("SELECT TOp 1 CAST(Frequency as int) as Frequency FROM Bos_Frequency", con);
            Average_per_Day = (int)sql.ExecuteScalar();
            con.Close();


            int DaysCount = 0;
            int i = TotalHHs_Interviewed;
            int Target_Per_day = 0;
            int Total_Days_Req = 0;


            DateTime dt = DateTime.Now;

            con.Open();
            sql = new SqlCommand("SELECT COUNT(DISTINCT G2_Interviewer_1_code) FROM [Main] WHERE G2_Interviewer_1 != 'NULL' order by 1", con);

            int TotalEnumerator = (int)sql.ExecuteScalar();
            Target_Per_day = TotalEnumerator * 3;

            ////Asad 
            if (Target_Per_day == 0)
                Target_Per_day = 1;
            // Target_HH = 100;  //new line added from code
            Total_Work_days_req = Target_HH / Target_Per_day;
            Total_Days_Req = Total_Work_days_req;
            c = Total_Days_Req;

          //  Total_Days_Req = Target_HH / Total_Days_Req;


            while (i < Target_HH)
            {
                i = i + Target_Per_day;

                if (i < Target_HH)
                {
                    RequiredDays.Append(string.Format("{0},", i.ToString()));
                    DaysCount++;
                    dt = dt.AddDays(1);
                    HH_Covered_on_the_day.Append(string.Format("'{0}',", dt.ToString("dd-MMM-yyyy")));
                }

                else if (i > Target_HH)
                {
                    RequiredDays.Append(string.Format("{0},", Target_HH.ToString()));
                    DaysCount++;
                    dt = dt.AddDays(1);
                    HH_Covered_on_the_day.Append(string.Format("'{0}',", dt.ToString("dd-MMM-yyyy")));
                }


            }

            //if (i <= Target_HH)
            //{

            //    RequiredDays.Append(string.Format("{0},", Target_HH.ToString()));
            //    DaysCount++;
            //    dt = dt.AddDays(DaysCount);
            //    HH_Covered_on_the_day.Append(string.Format("'{0}',", dt.ToString("dd-MMM-yyyy")));
            //}

            //draw 3rd Line Orange

            int CheckTotalDaysReq_EqualToTarget = 0;


            for (int a = 1; a <= c; a++)
            {
                CheckTotalDaysReq_EqualToTarget = (Target_Per_day * a);
               
                if (CheckTotalDaysReq_EqualToTarget >= Target_HH)
                {
                    a = c;
                }
                else
                {
                    Line_C.Append(string.Format("{0},", CheckTotalDaysReq_EqualToTarget.ToString()));
                }

            }

            if (CheckTotalDaysReq_EqualToTarget < Target_HH)
            {
                Line_C.Append(string.Format("{0},", Target_HH.ToString()));
            }

            else if (CheckTotalDaysReq_EqualToTarget > Target_HH)
            {
               // Line_C.Append(string.Format("{0},", Target_HH.ToString()));
            }

            TotalFormSubmitted.Length--; //For removing ','  
            TotalFormSubmitted.Append("]");
            HH_Covered_on_the_day.Length--; //For removing ','  
            HH_Covered_on_the_day.Append("]");
            RequiredDays.Length--; //For removing ','  
            RequiredDays.Append("]");
            Line_C.Length--; //For removing ','  
            Line_C.Append("]");



            //returnData.Add(chartLabel.ToString());
            returnData.Add(TotalFormSubmitted.ToString());
            returnData.Add(HH_Covered_on_the_day.ToString());
            returnData.Add(RequiredDays.ToString());
            returnData.Add(Line_C.ToString());

            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static void getChartData23()
        {
            chart_SurveyProgress_ClusterLevel = "";
            if (QueryFilter.Equals("Where"))
            {
                QueryFilter = "";
            }

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query2 = "Select * from [dbo].m_Chart_C2 order by District asc";
            var sql = new SqlCommand(Query2, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);



            var District_Name = new object[1000];
            var District_Data = new object[1000];


            int i = 0;

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District_Name[i] = "'" + row["District"].ToString() + "'";

                District_Data[i] = "[" + ((Convert.ToDouble(row[3]) / Convert.ToDouble(row[2])) * 100).ToString()+ "]";
                //District_Data[i + 1] = ((Convert.ToDouble(row[5]) / Convert.ToDouble(row[2])) * 100).ToString() + ",";
                //District_Data[i + 2] = ((Convert.ToDouble(row[4]) / Convert.ToDouble(row[2])) * 100).ToString() + "]";
                i = i + 1;
            }




            i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                chart_SurveyProgress_ClusterLevel = chart_SurveyProgress_ClusterLevel + " { name: " + District_Name[i].ToString() + ", data: " + District_Data[i] + " } ,";

                i = i + 1;
            }


            chart_SurveyProgress_ClusterLevel = chart_SurveyProgress_ClusterLevel.Substring(0, chart_SurveyProgress_ClusterLevel.Length - 1);

        }
        [System.Web.Services.WebMethod]
        public static void getChartData231()
        {
            chart_SurveyProgress_ClusterLevel11 = "";
            if (QueryFilter.Equals("Where"))
            {
                QueryFilter = "";
            }

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query2 = "Select * from [dbo].m_Chart_C2 order by District asc";
            var sql = new SqlCommand(Query2, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);



            var District_Name = new object[1000];
            var District_Data = new object[1000];


            int i = 0;

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District_Name[i] = "'" + row["District"].ToString() + "'";

                District_Data[i] = "[" + ((Convert.ToDouble(row[5]) / Convert.ToDouble(row[2])) * 100).ToString() + "]";
                //District_Data[i + 1] = ((Convert.ToDouble(row[5]) / Convert.ToDouble(row[2])) * 100).ToString() + ",";
                //District_Data[i + 2] = ((Convert.ToDouble(row[4]) / Convert.ToDouble(row[2])) * 100).ToString() + "]";
                i = i + 1;
            }




            i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                chart_SurveyProgress_ClusterLevel11 = chart_SurveyProgress_ClusterLevel11 + " { name: " + District_Name[i].ToString() + ", data: " + District_Data[i] + " } ,";

                i = i + 1;
            }


            chart_SurveyProgress_ClusterLevel11 = chart_SurveyProgress_ClusterLevel11.Substring(0, chart_SurveyProgress_ClusterLevel11.Length - 1);

        }


        [System.Web.Services.WebMethod]
        public static void getChartData2311()
        {
            chart_SurveyProgress_ClusterLevel22 = "";
            if (QueryFilter.Equals("Where"))
            {
                QueryFilter = "";
            }

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query2 = "Select * from [dbo].m_Chart_C2 order by District asc";
            var sql = new SqlCommand(Query2, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);



            var District_Name = new object[1000];
            var District_Data = new object[1000];


            int i = 0;

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District_Name[i] = "'" + row["District"].ToString() + "'";

                District_Data[i] = "[" + ((Convert.ToDouble(row[4]) / Convert.ToDouble(row[2])) * 100).ToString() + "]";
                //District_Data[i + 1] = ((Convert.ToDouble(row[5]) / Convert.ToDouble(row[2])) * 100).ToString() + ",";
                //District_Data[i + 2] = ((Convert.ToDouble(row[4]) / Convert.ToDouble(row[2])) * 100).ToString() + "]";
                i = i + 1;
            }




            i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                chart_SurveyProgress_ClusterLevel22 = chart_SurveyProgress_ClusterLevel22 + " { name: " + District_Name[i].ToString() + ", data: " + District_Data[i] + " } ,";

                i = i + 1;
            }


            chart_SurveyProgress_ClusterLevel22 = chart_SurveyProgress_ClusterLevel22.Substring(0, chart_SurveyProgress_ClusterLevel22.Length - 1);

        }

        [System.Web.Services.WebMethod]
        public static void getChartData3()
        {

            string Query3 = "Select * from [dbo].m_Chart_C3 " + QueryFilter.Replace("Name of ", "") + "order by District asc";
            chart_Completed_RemainingHHs = "";

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand(Query3, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            int i = 0;

            var District = new StringBuilder();
            var Target_Clusters = new StringBuilder();
            var Cluster_Completed = new StringBuilder();

            //  int OverallTarget = 0;
            //    int OverallCompleted = 0;


            Target_Clusters.Append("[");
            Cluster_Completed.Append("[");


            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District.Append(string.Format("'{0}', ", row["District"].ToString()));

                Target_Clusters.Append(string.Format("{0},", row["TotalClusters"].ToString()));
                Cluster_Completed.Append(string.Format("{0},", row["Completed"].ToString()));


                //   OverallTarget = OverallTarget + Convert.ToInt32(row["Number of Clusters"]);
                //   OverallCompleted = OverallCompleted + Convert.ToInt32(row["Number of Clusters Completed"]);



                i++;
            }


            //     Target_Clusters.Append(string.Format("{0},", OverallTarget.ToString()));
            //     Cluster_Completed.Append(string.Format("{0},", OverallCompleted.ToString()));

            //      District.Append(string.Format("'{0}',", "Overall"));
            District.Length--; //For removing ','  

            Target_Clusters.Length--; //For removing ','
            Cluster_Completed.Length--; //For removing ','


            Target_Clusters.Append("]");
            Cluster_Completed.Append("]");


            chart_Completed_RemainingHHs = chart_Completed_RemainingHHs + " { name: " + "'Target Clusters'" + ", data: " + Target_Clusters + " } ,";
            chart_Completed_RemainingHHs = chart_Completed_RemainingHHs + " { name: " + "'Finalised', color: 'orange'" + ", data: " + Cluster_Completed + ", pointPadding: 0.3 " + " } ,";




            chart_Completed_RemainingHHs = chart_Completed_RemainingHHs.Substring(0, chart_Completed_RemainingHHs.Length - 1);

            chart_district = District.ToString();

        }
        [System.Web.Services.WebMethod]
        public static List<string> getChartData4()
        {
            var returnData = new List<string>();

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query4 = "Select * from [dbo].[m_TargetVsCompletedHH]";
            var sql = new SqlCommand(Query4, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var PieChartData = new StringBuilder();




            //chartLabel.Append("[");
            // District.Append("[");
            PieChartData.Append("[");


            float Total_HouseHolds = 0;
            float Total_Remaining = 0;

            int i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                Total_HouseHolds = Total_HouseHolds + Convert.ToInt32(row["Number of Target Households"]);
                Total_Remaining = Total_Remaining + Convert.ToInt32(row["Total Number Of HH Interviews Completed"]);



                i++;
            }


            // Total_Remaining = Total_Cluster - Total_Remaining;
            float remaining_percentage = ((Total_Remaining * 100) / Total_HouseHolds) ;
            float total_percentage = 100 - remaining_percentage;
            PieChartData.Append(string.Format("{0},", +Math.Round(remaining_percentage,2) + "," + Math.Round(total_percentage,2)).ToString());

            District.Append(string.Format("{0},", "Submitted households "));
            District.Append(string.Format("{0},", "Remaning households "));
            District.Length--; //For removing ','  
            //District.Append("]");

            PieChartData.Length--;

            PieChartData.Append("]");




            returnData.Add(District.ToString());
            returnData.Add(PieChartData.ToString());



            return returnData;
        }


        //[System.Web.Services.WebMethod]
        //public static List<string> getChartData4listing()
        //{
        //    var returnData = new List<string>();

        //    var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        //    String Query4 = "Select * from [dbo].[Kohistan_listing_Progress]";
        //    var sql = new SqlCommand(Query4, Con);
        //    var dataAdapter = new SqlDataAdapter(sql);
        //    var dataset = new DataSet();
        //    dataAdapter.Fill(dataset);


        //    var District = new StringBuilder();


        //    var PieChartData = new StringBuilder();




        //    //chartLabel.Append("[");
        //    // District.Append("[");
        //    PieChartData.Append("[");


        //    float Total_HouseHolds = 0;
        //    float Total_Remaining = 0;

        //    int i = 0;
        //    foreach (DataRow row in dataset.Tables[0].Rows)
        //    {

        //        Total_HouseHolds = Total_HouseHolds + Convert.ToInt32(row["Targeted_Clusters"]);
        //        Total_Remaining = Total_Remaining + Convert.ToInt32(row["Synced_Clusters"]);



        //        i++;
        //    }


        //    // Total_Remaining = Total_Cluster - Total_Remaining;
        //    float remaining_percentage = ((Total_Remaining * 100) / Total_HouseHolds);
        //    float total_percentage = 100 - remaining_percentage;
        //    PieChartData.Append(string.Format("{0},", +Math.Round(remaining_percentage, 2) + "," + Math.Round(total_percentage, 2)).ToString());

        //    District.Append(string.Format("{0},", "Submitted Custers "));
        //    District.Append(string.Format("{0},", "Remaning Remaning "));
        //    District.Length--; //For removing ','  
        //    //District.Append("]");

        //    PieChartData.Length--;

        //    PieChartData.Append("]");




        //    returnData.Add(District.ToString());
        //    returnData.Add(PieChartData.ToString());



        //    return returnData;
        //}

        [System.Web.Services.WebMethod]
        public static List<string> getChartData5()
        {
            chart_Enumeration_Progress = "";
            var returnData = new List<string>();

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query5 = "Select * from m_Added_Report_New2_District " + QueryFilter;
            var sql = new SqlCommand(Query5, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var RemainingHH = new StringBuilder();
            var InitiatedHH = new StringBuilder();



            //chartLabel.Append("[");
            District.Append("[");
            RemainingHH.Append("[");
            InitiatedHH.Append("[");


            int BothHH1 = 0;
            int BothHH2 = 0;

            int i = 0;
            if (dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                    RemainingHH.Append(string.Format("{0},", (Convert.ToInt32(row["Target HHs"]) - Convert.ToInt32(row["HHs Interviewed"])).ToString()));
                    InitiatedHH.Append(string.Format("{0},", Convert.ToInt32(row["HHs Interviewed"].ToString())));

                    //   BothHH1 = BothHH1 + Convert.ToInt32(row["Target HHs"]) - Convert.ToInt32(row["HHs Interviewed"]);
                    //  BothHH2 = BothHH2 + Convert.ToInt32(row["HHs Interviewed"]);

                    i++;
                }



                //   District.Append(string.Format("'{0}',", "Overall"));
                District.Length--; //For removing ','  
            }
            else
            {
                District.Append(string.Format("'{0}', ", ""));

                RemainingHH.Append(string.Format("{0},", ""));
                InitiatedHH.Append(string.Format("{0},", ""));
            }
            District.Append("]");

            //  RemainingHH.Append(string.Format("{0},", BothHH1.ToString()));
            RemainingHH.Length--; //For removing ','  
            RemainingHH.Append("]");

            //  InitiatedHH.Append(string.Format("{0},", BothHH2.ToString()));
            InitiatedHH.Length--; //For removing ','  
            InitiatedHH.Append("]");


            chart_Enumeration_Progress = chart_Enumeration_Progress + " { name: " + "'Remaining Households', color: 'rgba(165,170,217,1)'" + ", data: " + RemainingHH + " } ,";
            chart_Enumeration_Progress = chart_Enumeration_Progress + " { name: " + "'Households Submitted', color: 'rgba(126,86,134,.9)'" + ", data: " + InitiatedHH + " } ,";

            chart_Enumeration_Progress = chart_Enumeration_Progress.Substring(0, chart_Enumeration_Progress.Length - 1);

            returnData.Add(District.ToString());



            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData6()
        {
            chart_HH_Visited_Interview_not_Completed = "";
            var returnData = new List<string>();

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand("Select * from m_Chart_H3  " + QueryFilter.Replace("Name of ", "") + "order by District asc", Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var HHVisited = new StringBuilder();
            var HHCompleted = new StringBuilder();
            var HHRevisit = new StringBuilder();



            //chartLabel.Append("[");
            District.Append("[");
            HHVisited.Append("[");
            HHCompleted.Append("[");
            HHRevisit.Append("[");


            int BothHH1 = 0;
            int BothHH2 = 0;

            int i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                HHVisited.Append(string.Format("{0},", row["VisitNoComplete"]).ToString());
                HHCompleted.Append(string.Format("{0},", row["VisitCompleted"].ToString()));
                HHRevisit.Append(string.Format("{0},", row["Revisit"].ToString()));

                i++;
            }



            // District.Append(string.Format("'{0}',", "Overall"));
            District.Length--; //For removing ','  
            District.Append("]");

            //RemainingHH.Append(string.Format("{0},", BothHH1.ToString()));
            HHVisited.Length--; //For removing ','  
            HHVisited.Append("]");

            //InitiatedHH.Append(string.Format("{0},", BothHH2.ToString()));
            HHCompleted.Length--; //For removing ','  
            HHCompleted.Append("]");
            //InitiatedHH.Append(string.Format("{0},", BothHH2.ToString()));
            HHRevisit.Length--; //For removing ','  
            HHRevisit.Append("]");

            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed + " { name: " + "'HH visited but not completed', color: 'rgba(231, 163, 39, 0.7)'" + ", data: " + HHVisited + " } ,";
            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed + " { name: " + "'HH completed (successfully interviewed)', color: 'rgba(140, 201, 27)'" + ", data: " + HHCompleted + " } ,";
            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed + " { name: " + "'HH to be revisited', color: 'rgba(165,170,217,1)'" + ", data: " + HHRevisit + " } ,";

            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed.Substring(0, chart_HH_Visited_Interview_not_Completed.Length - 1);

            returnData.Add(District.ToString());


            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData7()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("SELECT TOP(41) * FROM [m_Chart_S1] " + QueryFilter.Replace("Name of ", "")+" Order by District asc", con);

            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
           dataAdapter.Fill(dataset);



            var Team_Supervisor = new StringBuilder();
            var Clusters_Revisit = new StringBuilder();
            var Clusters_In_Progress = new StringBuilder();
            var Clusters_Completed = new StringBuilder();
            var Clusters_Assigned = new StringBuilder();

            //Team_Supervisor
            //Clusters_Initiated
            //Clusters_In_Progress
            //Clusters_Completed

            Team_Supervisor.Append("[");
            Clusters_Revisit.Append("[");
            Clusters_In_Progress.Append("[");
            Clusters_Completed.Append("[");
            Clusters_Assigned.Append("[");


            int pendingclusters = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                pendingclusters = Convert.ToInt32(row["Initiated"]) - Convert.ToInt32(row["Completed"]);
                //chartLabel.Append(string.Format("'{0}',", row["Date"].ToString()));
                Team_Supervisor.Append(string.Format("'{0}',", row["Supervisor_Name"].ToString()));
                if(Convert.ToInt32(row["Initiated"])==0)
                {
                    Clusters_Revisit.Append(string.Format("{0},", row["TotalClusters"].ToString()));
                }
                else if ((Convert.ToInt32(row["TotalClusters"])-pendingclusters) > 0)
                {
                    Clusters_Revisit.Append(string.Format("{0},", (Convert.ToInt32(row["TotalClusters"]) - Convert.ToInt32(row["Initiated"])).ToString()));
                }
                else
                {
                    Clusters_Revisit.Append(string.Format("{0},", row["Revisit"].ToString()));
                }
                Clusters_In_Progress.Append(string.Format("{0},", pendingclusters.ToString()));
                Clusters_Completed.Append(string.Format("{0},", row["Completed"].ToString()));
                Clusters_Assigned.Append(string.Format("{0},", row["TotalClusters"].ToString()));




            }
            Team_Supervisor.Length--;
            Team_Supervisor.Append("]");

            Clusters_Revisit.Length--;
            Clusters_Revisit.Append("]");

            Clusters_In_Progress.Length--;
            Clusters_In_Progress.Append("]");

            Clusters_Completed.Length--;
            Clusters_Completed.Append("]");

            Clusters_Assigned.Length--;
            Clusters_Assigned.Append("]");







            //returnData.Add(chartLabel.ToString());
            returnData.Add(Team_Supervisor.ToString());
            returnData.Add(Clusters_Revisit.ToString());
            returnData.Add(Clusters_In_Progress.ToString());
            returnData.Add(Clusters_Completed.ToString());
            returnData.Add(Clusters_Assigned.ToString());


            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData77()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("with cte as ("+
 "SELECT *, "+
  "ROW_NUMBER() OVER(ORDER BY district) as rn "+
 "FROM[m_Chart_S1]) "+
"SELECT* FROM cte "+
"WHERE rn BETWEEN 42 and 100; ", con);

            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);



            var Team_Supervisor = new StringBuilder();
            var Clusters_Revisit = new StringBuilder();
            var Clusters_In_Progress = new StringBuilder();
            var Clusters_Completed = new StringBuilder();
            var Clusters_Assigned = new StringBuilder();

            //Team_Supervisor
            //Clusters_Initiated
            //Clusters_In_Progress
            //Clusters_Completed

            Team_Supervisor.Append("[");
            Clusters_Revisit.Append("[");
            Clusters_In_Progress.Append("[");
            Clusters_Completed.Append("[");
            Clusters_Assigned.Append("[");


            int pendingclusters = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                pendingclusters = Convert.ToInt32(row["Initiated"]) - Convert.ToInt32(row["Completed"]);
                //chartLabel.Append(string.Format("'{0}',", row["Date"].ToString()));
                Team_Supervisor.Append(string.Format("'{0}',", row["Supervisor_Name"].ToString()));
                if (Convert.ToInt32(row["Initiated"]) == 0)
                {
                    Clusters_Revisit.Append(string.Format("{0},", row["TotalClusters"].ToString()));
                }
                else if ((Convert.ToInt32(row["TotalClusters"]) - pendingclusters) > 0)
                {
                    Clusters_Revisit.Append(string.Format("{0},", (Convert.ToInt32(row["TotalClusters"]) - Convert.ToInt32(row["Initiated"])).ToString()));
                }
                else
                {
                    Clusters_Revisit.Append(string.Format("{0},", row["Revisit"].ToString()));
                }
                Clusters_In_Progress.Append(string.Format("{0},", pendingclusters.ToString()));
                Clusters_Completed.Append(string.Format("{0},", row["Completed"].ToString()));
                Clusters_Assigned.Append(string.Format("{0},", row["TotalClusters"].ToString()));




            }
            Team_Supervisor.Length--;
            Team_Supervisor.Append("]");

            Clusters_Revisit.Length--;
            Clusters_Revisit.Append("]");

            Clusters_In_Progress.Length--;
            Clusters_In_Progress.Append("]");

            Clusters_Completed.Length--;
            Clusters_Completed.Append("]");

            Clusters_Assigned.Length--;
            Clusters_Assigned.Append("]");







            //returnData.Add(chartLabel.ToString());
            returnData.Add(Team_Supervisor.ToString());
            returnData.Add(Clusters_Revisit.ToString());
            returnData.Add(Clusters_In_Progress.ToString());
            returnData.Add(Clusters_Completed.ToString());
            returnData.Add(Clusters_Assigned.ToString());


            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData8()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("");

            // if (Filter_Data == "")
            sql = new SqlCommand("SELECT TOP(41) * FROM [m_Report1_chart]  " + QueryFilter, con);
            // else
            //   sql = new SqlCommand("SELECT * FROM [Report1] " + Filter_Data + " ", con);


            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);

            var Team_Supervisor = new StringBuilder();
            var Completed_Percentage = new StringBuilder();
            var Progress_Percentage = new StringBuilder();
            var Revisit = new StringBuilder();

            //Team_Supervisor
            //Clusters_Initiated
            //Clusters_In_Progress
            //Clusters_Completed

            Team_Supervisor.Append("[");
            Completed_Percentage.Append("[");
            Progress_Percentage.Append("[");
            Revisit.Append("[");
           // Int32 pending = 0;

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Team_Supervisor.Append(string.Format("'{0}',", row["Team Supervisor"].ToString()));
                Completed_Percentage.Append(string.Format("{0},", row["Percentage Complete"].ToString()));
                Progress_Percentage.Append(string.Format("{0},", row["inProgressPer"].ToString()));
                Revisit.Append(string.Format("{0},", row["Percentage InProgress"].ToString()));
            }
            Team_Supervisor.Length--;
            Team_Supervisor.Append("]");

            Completed_Percentage.Length--;
            Completed_Percentage.Append("]");

            Progress_Percentage.Length--;
            Progress_Percentage.Append("]");

            Revisit.Length--;
            Revisit.Append("]");

            //returnData.Add(chartLabel.ToString());
            returnData.Add(Team_Supervisor.ToString());
            returnData.Add(Completed_Percentage.ToString());
            returnData.Add(Progress_Percentage.ToString());
            returnData.Add(Revisit.ToString());


            return returnData;
        }


        [System.Web.Services.WebMethod]
        public static List<string> getChartData88()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("");

            // if (Filter_Data == "")
            sql = new SqlCommand("with cte as ("+
                     "SELECT *, "+
                      "ROW_NUMBER() OVER(ORDER BY[Name of District]) as rn "+
                     "FROM m_Report1_chart) "+
                    "SELECT* FROM cte "+
                    QueryFilter+" and rn BETWEEN 42 and 100; ", con);
            // else
            //   sql = new SqlCommand("SELECT * FROM [Report1] " + Filter_Data + " ", con);


            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);

            var Team_Supervisor = new StringBuilder();
            var Completed_Percentage = new StringBuilder();
            var Progress_Percentage = new StringBuilder();
            var Revisit = new StringBuilder();

            //Team_Supervisor
            //Clusters_Initiated
            //Clusters_In_Progress
            //Clusters_Completed

            Team_Supervisor.Append("[");
            Completed_Percentage.Append("[");
            Progress_Percentage.Append("[");
            Revisit.Append("[");
            // Int32 pending = 0;

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Team_Supervisor.Append(string.Format("'{0}',", row["Team Supervisor"].ToString()));
                Completed_Percentage.Append(string.Format("{0},", row["Percentage Complete"].ToString()));
                Progress_Percentage.Append(string.Format("{0},", row["inProgressPer"].ToString()));
                Revisit.Append(string.Format("{0},", row["Percentage InProgress"].ToString()));
            }
            Team_Supervisor.Length--;
            Team_Supervisor.Append("]");

            Completed_Percentage.Length--;
            Completed_Percentage.Append("]");

            Progress_Percentage.Length--;
            Progress_Percentage.Append("]");

            Revisit.Length--;
            Revisit.Append("]");

            //returnData.Add(chartLabel.ToString());
            returnData.Add(Team_Supervisor.ToString());
            returnData.Add(Completed_Percentage.ToString());
            returnData.Add(Progress_Percentage.ToString());
            returnData.Add(Revisit.ToString());


            return returnData;
        }


        [System.Web.Services.WebMethod]
        public static List<string> getChartData9()
        {
            var returnData = new List<string>();
            chart_Children_Statistics = "";

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand("Select * from m_Added_Report_New2_District  " + QueryFilter, con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var Children_Listing_Data = new StringBuilder();
            var Children_HH_Interview = new StringBuilder();
            var Children_Interviewed = new StringBuilder();



            //chartLabel.Append("[");
            District.Append("[");
            Children_Listing_Data.Append("[");
            Children_HH_Interview.Append("[");
            Children_Interviewed.Append("[");


            //int BothHH1 = 0;
            //int BothHH2 = 0;
            //int BothHH3 = 0;

            int i = 0;
            if (dataset.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                    Children_Listing_Data.Append(string.Format("{0},", row["No. of Children as per Listing"]).ToString());
                    Children_HH_Interview.Append(string.Format("{0},", row["No. of Children as per HH Interview (Roster)"].ToString()));
                    Children_Interviewed.Append(string.Format("{0},", row["No. of Children Interviewed"].ToString()));

                    //BothHH1 = BothHH1 + Convert.ToInt32(row["No. of Children as per Listing"]);
                    //  BothHH2 = BothHH2 + Convert.ToInt32(row["No. of Children as per HH Interview (Roster)"]);
                    //    BothHH3 = BothHH3 + Convert.ToInt32(row["No. of Children Interviewed"]);

                    i++;
                }

                //  Children_Listing_Data.Append(string.Format("{0},", BothHH1.ToString()));
                //     Children_HH_Interview.Append(string.Format("{0},", BothHH2.ToString()));
                //    Children_Interviewed.Append(string.Format("{0},", BothHH3.ToString()));



                //    District.Append(string.Format("'{0}',", "Overall"));
                District.Length--; //For removing ','  
            }
            else
            {
                District.Append(string.Format("'{0}', ", ""));

                Children_Listing_Data.Append(string.Format("{0},", ""));
                Children_HH_Interview.Append(string.Format("{0},", ""));
                Children_Interviewed.Append(string.Format("{0},", ""));

            }
            District.Append("]");

            Children_Listing_Data.Length--; //For removing ','  
            Children_Listing_Data.Append("]");

            Children_HH_Interview.Length--; //For removing ','  
            Children_HH_Interview.Append("]");

            Children_Interviewed.Length--; //For removing ','  
            Children_Interviewed.Append("]");

            chart_Children_Statistics = chart_Children_Statistics + " { name: " + "'Number of Children as per Lisiting Data', color: 'rgba(244, 67, 54, 0.6)'" + ", data: " + Children_Listing_Data + " } ,";
            chart_Children_Statistics = chart_Children_Statistics + " { name: " + "'Number of Children as per HH Interview (Roster)'" + ", data: " + Children_HH_Interview + " } ,";
            chart_Children_Statistics = chart_Children_Statistics + " { name: " + "'Number of Children Interviewed'" + ", data: " + Children_Interviewed + " } ,";


            chart_Children_Statistics = chart_Children_Statistics.Substring(0, chart_Children_Statistics.Length - 1);



            returnData.Add(District.ToString());



            return returnData;
        }



        [System.Web.Services.WebMethod]
        public static List<string> getChartData10()
        {
            var returnData = new List<string>();
            chart_Total_Working_Childern = "";

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand("Select * from m_ChildrenRosterVsWorking  " + QueryFilter, con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var Children = new StringBuilder();
            var WorkingChildren = new StringBuilder();




            //chartLabel.Append("[");
            District.Append("[");
            Children.Append("[");
            WorkingChildren.Append("[");

            int i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                Children.Append(string.Format("{0},", row["Children"]).ToString());
                WorkingChildren.Append(string.Format("{0},", row["WorkingChildren"].ToString()));

                i++;
            }


            District.Length--; //For removing ','  
            District.Append("]");

            Children.Length--; //For removing ','  
            Children.Append("]");

            WorkingChildren.Length--; //For removing ','  
            WorkingChildren.Append("]");

            chart_Total_Working_Childern = chart_Total_Working_Childern + " { name: " + "'Children', color: 'rgba(244, 67, 54, 0.6)'" + ", data: " + Children + " } ,";
            chart_Total_Working_Childern = chart_Total_Working_Childern + " { name: " + "'Working Children'" + ", data: " + WorkingChildren + " } ,";

            chart_Total_Working_Childern = chart_Total_Working_Childern.Substring(0, chart_Total_Working_Childern.Length - 1);


            returnData.Add(District.ToString());

            return returnData;
        }


        [System.Web.Services.WebMethod]
        public static List<string> getChartData11()
        {
            var returnData = new List<string>();

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand("Select * from [dbo].m_Chart_C1", Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var PieChartData = new StringBuilder();




            //chartLabel.Append("[");
            // District.Append("[");
            PieChartData.Append("[");


            float Total_Cluster = 0;
            float Total_Completed = 0;

            int i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Total_Cluster = Total_Cluster + Convert.ToInt32(row["Target"]);
                Total_Completed = Total_Completed + Convert.ToInt32(row["Completed"]);
                i++;
            }
            // Total_Remaining = Total_Cluster - Total_Remaining;
            float total_percentage = (Total_Completed / Total_Cluster) * 100;
            float remaining_percentage = 100 - total_percentage;
            PieChartData.Append(string.Format("{0},", + Math.Round(total_percentage,2) + "," + Math.Round(remaining_percentage,2)));

            // PieChartData.Append(string.Format("{0},", + Total_Remaining + "," + Total_Cluster).ToString());

            District.Append(string.Format("{0},", "Finalised Percentage"));
            District.Append(string.Format("{0},", "Pending Percentage"));
            District.Length--; //For removing ','  
            //District.Append("]");

            PieChartData.Length--;

            PieChartData.Append("]");




            returnData.Add(District.ToString());
            returnData.Add(PieChartData.ToString());



            return returnData;
        }


        private static DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }



    }
}