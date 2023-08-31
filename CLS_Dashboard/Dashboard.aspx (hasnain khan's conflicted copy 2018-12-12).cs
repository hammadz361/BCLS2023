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
        public static string chartTitle2 = "C2. Survey Progress at the Cluster Level";
        public static string chartTitle3 = "C3. Cluster Target vs Achievement";
        public static string chartTitle4 = "O2. Completed vs Remaining HHs";
        public static string chartTitle5 = "H1. Enumeration Progress (Initiated vs Remaining Households)";
        public static string chartTitle6 = "H2. No. Of HH visited: interviewed vs. Not completed";
        public static string chartTitle7 = "S1. Clusters assigned: initiated, completed and remaining";
        public static string chartTitle8 = "S2. Clusters Completed Percentage (by Team Supervisors)";
        public static string chartTitle9 = "CH1. Children Statistics as per HH Listing, HH Roster and Child Interviews";
        public static string chartTitle10 = "CH2. Total vs Working Children";
        public static string chartTitle11 = "C1. Overall evolution: Target vs. Completed clusters (%)";
        public static string chartTitle12 = "H1. Overall evolution: Target vs. completed households (%)";
        public static int Target_HH = 0;
        public static string Filter_Data = "";
        public static string Filter = "All";
        public static string QueryFilter = "";
        public static string chart_SurveyProgress_ClusterLevel = "";
        public static string chart_ClusterTarget_Achievement = "";
        public static string chart_Completed_RemainingHHs = "";
        public static string Piechart_Overall_evolution  = "";
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

            else
            {
                Session["lblHeadingName"] = "<b>CLS</b>-Dashboard";

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
                    QueryFilter = "WHERE " + whereAnd;
                }
                else
                {
                    QueryFilter = "WHERE 1=1";
                }
                //Response.Write(QueryFilter) ;
                
                    Key_Indicators();
                    load_Filter_Districts();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "UpdateCharts()", true);
              
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

            Average_per_Day = TotalHHs_Interviewed / TotalDays;
            if (con.State != ConnectionState.Closed) { con.Close(); }
            con.Open();
            sql = new SqlCommand("Select SUM(CAST([Count of HH_ID] AS INT)) from ReferenceTable", con);
            Target_HH = (int)sql.ExecuteScalar();
            con.Close();


            int DaysCount = 0;
            int i = TotalHHs_Interviewed;
            int Target_Per_day = 0;
            int Total_Days_Req = 0;


            DateTime dt = DateTime.Now;

            con.Open();
            sql = new SqlCommand("SELECT COUNT(DISTINCT G2_Interviewer_1_code) FROM Main WHERE G2_Interviewer_1 != 'NULL' order by 1", con);

            int TotalEnumerator = (int)sql.ExecuteScalar();
            Target_Per_day = TotalEnumerator * 3;

            Total_Work_days_req = Target_HH / Target_Per_day;
            Total_Days_Req = Total_Work_days_req * 140 / 100;
            c = Total_Days_Req;

            Total_Days_Req = Target_HH / Total_Days_Req;


            while (i < Target_HH)
            {
                i = i + Average_per_Day;

                if (i < Target_HH)
                {
                    RequiredDays.Append(string.Format("{0},", i.ToString()));
                    DaysCount++;
                    dt = dt.AddDays(DaysCount);
                    HH_Covered_on_the_day.Append(string.Format("'{0}',", dt.ToString("dd-MMM-yyyy")));
                }

                else if (i > Target_HH)
                {
                    RequiredDays.Append(string.Format("{0},", Target_HH.ToString()));
                    DaysCount++;
                    dt = dt.AddDays(DaysCount);
                    HH_Covered_on_the_day.Append(string.Format("'{0}',", dt.ToString("dd-MMM-yyyy")));
                }


            }

            if (i <= Target_HH)
            {

                RequiredDays.Append(string.Format("{0},", Target_HH.ToString()));
                DaysCount++;
                dt = dt.AddDays(DaysCount);
                HH_Covered_on_the_day.Append(string.Format("'{0}',", dt.ToString("dd-MMM-yyyy")));
            }

            //draw 3rd Line Orange

            int CheckTotalDaysReq_EqualToTarget = 0;


            for (int a = 1; a <= c; a++)
            {
                CheckTotalDaysReq_EqualToTarget = (Total_Days_Req * a);
                Line_C.Append(string.Format("{0},", CheckTotalDaysReq_EqualToTarget.ToString()));

            }

            if (CheckTotalDaysReq_EqualToTarget < Target_HH)
            {
                Line_C.Append(string.Format("{0},", Target_HH.ToString()));
            }

            else if (CheckTotalDaysReq_EqualToTarget > Target_HH)
            {
                Line_C.Append(string.Format("{0},", Target_HH.ToString()));
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

           
            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query2 = "Select * from [dbo].Report1_District " + QueryFilter;
            var sql = new SqlCommand(Query2, Con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);

           

            var District_Name = new object[1000];
            var District_Data = new object[1000];


            int i =0;

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District_Name[i] = "'" + row["Name of District"].ToString() + "'" ;

                District_Data[i] = "[" + ((Convert.ToDouble(row["Number of Clusters Initiated"]) / Convert.ToDouble(row["Number of Clusters"])) * 100).ToString() + "," ;
                District_Data[i + 1] = ((Convert.ToDouble(row["Number of Clusters Completed"]) / Convert.ToDouble(row["Number of Clusters"])) * 100).ToString() + ",";
                District_Data[i + 2] = ((Convert.ToDouble(row["Number of Clusters To Be Revisted"]) / Convert.ToDouble(row["Number of Clusters"])) * 100).ToString() + "]";
                i = i + 3;
            }

            
           

            i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                chart_SurveyProgress_ClusterLevel = chart_SurveyProgress_ClusterLevel + " { name: " + District_Name[i].ToString() + ", data: " + District_Data[i] + District_Data[i + 1] + District_Data[i + 2] + " } ,";
                                                
                i = i + 3;
            }


            chart_SurveyProgress_ClusterLevel = chart_SurveyProgress_ClusterLevel.Substring(0, chart_SurveyProgress_ClusterLevel.Length - 1);
            
        }

        [System.Web.Services.WebMethod]
        public static void getChartData3()
        {
           
            chart_Completed_RemainingHHs = "";

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query3 = "Select * from [dbo].Report1_District "+ QueryFilter;
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
                District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                Target_Clusters.Append(string.Format("{0},",row["Number of Clusters"].ToString()));
                Cluster_Completed.Append(string.Format("{0},", row["Number of Clusters Completed"].ToString()));
              
                
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
            chart_Completed_RemainingHHs = chart_Completed_RemainingHHs + " { name: " + "'No of Cluster Completed', color: 'orange'" + ", data: " + Cluster_Completed + ", pointPadding: 0.3 " + " } ,";


           

            chart_Completed_RemainingHHs = chart_Completed_RemainingHHs.Substring(0, chart_Completed_RemainingHHs.Length - 1);

            chart_district = District.ToString();
           
        }
        [System.Web.Services.WebMethod]
        public static List<string> getChartData4()
        {
            var returnData = new List<string>();

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query4  = "Select * from [dbo].[TargetVsCompletedHH]";
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
            float remaining_percentage=(Total_Remaining/Total_HouseHolds)*100;
            float total_percentage = 100 - remaining_percentage;
            PieChartData.Append(string.Format("{0},", +remaining_percentage + "," + total_percentage).ToString());

            District.Append(string.Format("{0},", "Completed households"));
            District.Append(string.Format("{0},", "Target households"));
            District.Length--; //For removing ','  
            //District.Append("]");

            PieChartData.Length--;

            PieChartData.Append("]");




            returnData.Add(District.ToString());
            returnData.Add(PieChartData.ToString());



            return returnData;
        }
      
        [System.Web.Services.WebMethod]
        public static List<string> getChartData5()
        {
            chart_Enumeration_Progress = "";
            var returnData = new List<string>();

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            String Query5  = "Select * from Added_Report_New2_District " + QueryFilter;
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
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                RemainingHH.Append(string.Format("{0},", (Convert.ToInt32(row["Target HHs"]) - Convert.ToInt32(row["HHs Interviewed"])).ToString()));
                InitiatedHH.Append(string.Format("{0},", Convert.ToInt32(row["HHs Interviewed"].ToString())));

                BothHH1 = BothHH1 + Convert.ToInt32(row["Target HHs"]) - Convert.ToInt32(row["HHs Interviewed"]);
                BothHH2 = BothHH2 + Convert.ToInt32(row["HHs Interviewed"]);

                i++;
            }

           

            District.Append(string.Format("'{0}',", "Overall"));
            District.Length--; //For removing ','  
            District.Append("]");

            RemainingHH.Append(string.Format("{0},", BothHH1.ToString()));
            RemainingHH.Length--; //For removing ','  
            RemainingHH.Append("]");

            InitiatedHH.Append(string.Format("{0},", BothHH2.ToString()));
            InitiatedHH.Length--; //For removing ','  
            InitiatedHH.Append("]");


            chart_Enumeration_Progress = chart_Enumeration_Progress + " { name: " + "'Remaining Households', color: 'rgba(165,170,217,1)'" + ", data: " + RemainingHH + " } ,";
            chart_Enumeration_Progress = chart_Enumeration_Progress + " { name: " + "'Households Initiated', color: 'rgba(126,86,134,.9)'" + ", data: " + InitiatedHH + " } ,";

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
            var sql = new SqlCommand("Select * from Report2_Chart " + QueryFilter, Con);
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
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                District.Append(string.Format("'{0}', ", row["Name of District"].ToString()));

                RemainingHH.Append(string.Format("{0},", row["Number of HHs To Be Revisited"]).ToString());
                InitiatedHH.Append(string.Format("{0},", row["Number of HHs Initiated"].ToString()));

                BothHH1 = BothHH1 + Convert.ToInt32(row["Number of HHs To Be Revisited"]);
                BothHH2 = BothHH2 + Convert.ToInt32(row["Number of HHs Initiated"]);

                i++;
            }

           

            District.Append(string.Format("'{0}',", "Overall"));
            District.Length--; //For removing ','  
            District.Append("]");

            RemainingHH.Append(string.Format("{0},", BothHH1.ToString()));
            RemainingHH.Length--; //For removing ','  
            RemainingHH.Append("]");

            InitiatedHH.Append(string.Format("{0},", BothHH2.ToString()));
            InitiatedHH.Length--; //For removing ','  
            InitiatedHH.Append("]");


            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed + " { name: " + "'Number of HHs To Be Revisited', color: 'rgba(231, 163, 39, 0.7)'" + ", data: " + RemainingHH + " } ,";
            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed + " { name: " + "'HH Interviewed', color: 'rgba(140, 201, 27)'" + ", data: " + InitiatedHH + " } ,";

            chart_HH_Visited_Interview_not_Completed = chart_HH_Visited_Interview_not_Completed.Substring(0, chart_HH_Visited_Interview_not_Completed.Length - 1);

            returnData.Add(District.ToString());


            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData7()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            
          var sql = new SqlCommand("SELECT * FROM [Report1]  " + QueryFilter, con);

            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);



            var Team_Supervisor = new StringBuilder();
            var Clusters_Initiated = new StringBuilder();
            var Clusters_In_Progress = new StringBuilder();
            var Clusters_Completed = new StringBuilder();

            //Team_Supervisor
            //Clusters_Initiated
            //Clusters_In_Progress
            //Clusters_Completed

            Team_Supervisor.Append("[");
            Clusters_Initiated.Append("[");
            Clusters_In_Progress.Append("[");
            Clusters_Completed.Append("[");


            int pendingclusters = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {

                pendingclusters = Convert.ToInt32(row["Number of Clusters Initiated"])-Convert.ToInt32(row["Number of Clusters Completed"]);
                //chartLabel.Append(string.Format("'{0}',", row["Date"].ToString()));
                Team_Supervisor.Append(string.Format("'{0}',", row["Team Supervisor"].ToString()));
                Clusters_Initiated.Append(string.Format("{0},", row["Number of Clusters Initiated"].ToString()));
                Clusters_In_Progress.Append(string.Format("{0},", pendingclusters.ToString()));
                Clusters_Completed.Append(string.Format("{0},", row["Number of Clusters Completed"].ToString()));




            }
            Team_Supervisor.Length--;
            Team_Supervisor.Append("]");

            Clusters_Initiated.Length--;
            Clusters_Initiated.Append("]");

            Clusters_In_Progress.Length--;
            Clusters_In_Progress.Append("]");

            Clusters_Completed.Length--;
            Clusters_Completed.Append("]");







            //returnData.Add(chartLabel.ToString());
            returnData.Add(Team_Supervisor.ToString());
            returnData.Add(Clusters_Initiated.ToString());
            returnData.Add(Clusters_In_Progress.ToString());
            returnData.Add(Clusters_Completed.ToString());


            return returnData;
        }

        [System.Web.Services.WebMethod]
        public static List<string> getChartData8()
        {
            var returnData = new List<string>();

            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

            var sql = new SqlCommand("");

           // if (Filter_Data == "")
                sql = new SqlCommand("SELECT * FROM [Report1]  " + QueryFilter, con);
            // else
            //   sql = new SqlCommand("SELECT * FROM [Report1] " + Filter_Data + " ", con);


            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);

            var Team_Supervisor = new StringBuilder();
            var Completed_Percentage = new StringBuilder();

            //Team_Supervisor
            //Clusters_Initiated
            //Clusters_In_Progress
            //Clusters_Completed

            Team_Supervisor.Append("[");
            Completed_Percentage.Append("[");



            foreach (DataRow row in dataset.Tables[0].Rows)
            {


                //chartLabel.Append(string.Format("'{0}',", row["Date"].ToString()));
                Team_Supervisor.Append(string.Format("'{0}',", row["Team Supervisor"].ToString()));
                Completed_Percentage.Append(string.Format("{0},", row["Percentage Complete"].ToString()));

            }
            Team_Supervisor.Length--;
            Team_Supervisor.Append("]");

            Completed_Percentage.Length--;
            Completed_Percentage.Append("]");

            //returnData.Add(chartLabel.ToString());
            returnData.Add(Team_Supervisor.ToString());
            returnData.Add(Completed_Percentage.ToString());


            return returnData;
        }


        [System.Web.Services.WebMethod]
        public static List<string> getChartData9()
        {
            var returnData = new List<string>();
            chart_Children_Statistics = "";

            var Con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
            var sql = new SqlCommand("Select * from Added_Report_New2_District  " + QueryFilter, con);
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
            var sql = new SqlCommand("Select * from ChildrenRosterVsWorking  " + QueryFilter, con);
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
            var sql = new SqlCommand("Select * from [dbo].Report1_District ", con);
            var dataAdapter = new SqlDataAdapter(sql);
            var dataset = new DataSet();
            dataAdapter.Fill(dataset);


            var District = new StringBuilder();


            var PieChartData = new StringBuilder();




            //chartLabel.Append("[");
            // District.Append("[");
            PieChartData.Append("[");


            float Total_Cluster = 0;
            float Total_Remaining = 0;

            int i = 0;
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                
                Total_Cluster = Total_Cluster + Convert.ToInt32(row["Number of Clusters Initiated"]);
                Total_Remaining = Total_Remaining + Convert.ToInt32(row["Number of Clusters Completed"]);
                
                

                i++;
            }


            // Total_Remaining = Total_Cluster - Total_Remaining;
            float remaining_percentage = (Total_Remaining / Total_Cluster) * 100;
            float total_percentage = 100 - remaining_percentage;
            PieChartData.Append(string.Format("{0},", +remaining_percentage + "," + total_percentage).ToString());

           // PieChartData.Append(string.Format("{0},", + Total_Remaining + "," + Total_Cluster).ToString());

            District.Append(string.Format("{0},", "Completed clusters"));
            District.Append(string.Format("{0},", "Target clusters"));
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