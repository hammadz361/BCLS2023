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
    public partial class Listing_Progress : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        private static string CS = ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString;
        private SqlConnection con = new SqlConnection(CS);
        public string Filter_Data = "";
        public static string Report_ID = "";
        public string Report_Name = "";
        public static string  cc = "";



        public string Report_1 = "1_listing_Progress";
        public string Report_2 = "2_size_of_clusters";
        public string Report_3 = "3_Short_addresses";
        public string Report_4 = "4_number_and_percentage_non_dwelling";
        public string Report_5 = "5_number_and_percentage_HH_childern";
        public string Report_55 = "55_GPS_Collected_Below_10th_Percentile";
        public string Report_6 = "6_HH_per_structure";
        public string Report_7 = "C_1";
        public string Report_8 = "C_2";
        public string Report_9 = "C_3";
        public string Report_10 = "C_4";
        public string Report_11 = "C_5";


       

        DataTable dt = new DataTable();
        DataTable ddt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else
            {
                Site1.active = "active";

                Session["lblHeadingName"] = "<b>KP CLS</b>-Survey Progress";
                Report_ID = Request.QueryString["id"];
                Report_Name = Request.QueryString["name"];



            

                if (!IsPostBack)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StaticHeaderRow()", true);

                    if (Report_ID != "")
                    {
                        Visible_Filters();
                        load_Filters_Data();
                        gv_Bind_Data(" ", Report_ID, Report_Name);
                        if(Report_ID==Report_1)
                        {
                            tickers.Visible = true;
                        }
                      

                        //using (SqlCommand cmdu = new SqlCommand("Exec LastUpdates"))
                        //{
                        //    using (SqlDataAdapter sdau = new SqlDataAdapter())
                        //    {
                        //        cmdu.Connection = con;
                        //        sdau.SelectCommand = cmdu;

                        //        using (DataTable dtu = new DataTable())
                        //        {
                        //            sdau.Fill(dtu);
                        //            Last_Updates.Text = "Last Form Submission: ";
                        //            Last_Updates.Text += dtu.Rows[0]["LastSubmission"].ToString();
                        //            Last_Updates.Text += "<br>Last Observer Submission: ";
                        //            Last_Updates.Text += dtu.Rows[0]["Observer"].ToString();
                        //            Last_Updates.Text += "<Br>Last Monitoring Submission: ";
                        //            Last_Updates.Text += dtu.Rows[0]["Monitoring"].ToString();
                        //            Last_Updates.Text += "<br>Last Listing Submission: ";
                        //            Last_Updates.Text += dtu.Rows[0]["Listing"].ToString();




                        //        }
                        //    }
                        //}
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM [li_tickers]"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.Connection = con;
                                sda.SelectCommand = cmd;

                                using (DataTable dt = new DataTable())
                                {
                                    sda.Fill(dt);

                                    lbl_Indicator_District.Text =dt.Rows[0].ItemArray[0].ToString();
                                    lbl_Indicator_Enumerator.Text = dt.Rows[0].ItemArray[1].ToString();
                                    lbl_Indicator_HH.Text = dt.Rows[0].ItemArray[2].ToString();
                                    lbl_Indicator_Childern.Text =dt.Rows[0].ItemArray[3].ToString();
                                    Boys.Text = dt.Rows[0].ItemArray[4].ToString();
                                    Girls.Text = dt.Rows[0].ItemArray[5].ToString();

                                }
                            }
                        }

                    }


                }
            }



        }


        private void Visible_Filters()
        {
            if (Report_ID == Report_7 || Report_ID == Report_8 || Report_ID == Report_9 || Report_ID == Report_10 || Report_ID == Report_11)
            {
               // lstDistricts.Visible = true; lblDistricts.Visible = true;
                lblcriteria.Visible = true;  criteria_list.Visible = true;
                //lstTypes.Visible = true; lblTypes.Visible = true;
            }
            if(Report_ID == Report_1)
            {
                lstDistricts.Visible = true; lblDistricts.Visible = true;
            }
            if ((Report_ID == Report_2) || (Report_ID == Report_3) || (Report_ID == Report_4) || (Report_ID == Report_5) || (Report_ID == Report_6))
            {
                lstDistricts.Visible = true; lblDistricts.Visible = true;
                lstEnumerator.Visible = true; lblEnumerator.Visible = true;
            }

        }

        private void load_Filter_Districts()
        {
            lstDistricts.Items.Clear();
            lstDistricts.Items.Add("All");


            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT District FROM Listing_HH order by 1"))
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





        private void load_Filter_ClusterCodes(string Filter_Data)
        {
            lstClusterCodes.Items.Clear();
            lstClusterCodes.Items.Add("All");
            //lstClusterCodes.SelectedIndex = 0;

            if (Filter_Data.Contains("[Name of District]")) { Filter_Data = Filter_Data.Replace("[Name of District]", "district"); }
            if (Filter_Data.Contains("[Team Supervisor]")) { Filter_Data = Filter_Data.Replace("[Team Supervisor]", "G3_Supervisor_1"); }

            using (SqlCommand cmd = new SqlCommand("  SELECT  DISTINCT CAST(cluster_code AS numeric) AS CCode FROM [Main] " + Filter_Data + " order by 1"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstClusterCodes.Items.Add(dt.Rows[i].ItemArray[0].ToString());


                    }
                }
            }
        }

        //Enumerators are refered to be listers here
        private void load_Filter_Enumerator(string Filter_Data)
        {
            lstEnumerator.Items.Clear();
            lstEnumerator.Items.Add("All");
            lstEnumerator.SelectedIndex = 0;

            if (Filter_Data.Contains("[Name of District]")) { Filter_Data = Filter_Data.Replace("[Name of District]", "district"); }
            if (Filter_Data.Contains("[Cluster Code]")) { Filter_Data = Filter_Data.Replace("[Cluster Code]", "Cluster_Code"); }

            using (SqlCommand cmd = new SqlCommand(
                       "SELECT DISTINCT LISTER_NAME FROM Listing_HH " + Filter_Data + " order by 1"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
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

            // load_Filter_SurveyTeam("");
            // lstSurveyTeam.SelectedIndex = 0;
            //For 3rd Filter Cluster

            load_Filter_ClusterCodes("");

            //For 4th Filter Enumerator

            load_Filter_Enumerator("");
            lstClusterCodes.SelectedIndex = 0;

            //For Monitors Filter
            //  load_Filter_Monitors();
            //  lstMonitors.SelectedIndex = 0;

            //For 5th Filter Types


            //   if (Report_ID == Report_1001)
            // {
            //
            //   lstTypes.Items.Add(new ListItem("Industries", "1"));
            // lstTypes.Items.Add(new ListItem("Occupation", "2"));
            //lstTypes.Items.Add(new ListItem("Tools", "3"));

            //lstTypes.SelectedIndex = 0;
            //}
            //if (Report_ID == Report_1010 || Report_ID == Report_1022)
            //{

            //    lstTypes.Items.Add(new ListItem("Incidence", "101"));
            //    lstTypes.Items.Add(new ListItem("Magnitude", "100"));
            //    if (Request["type"] == "100")
            //        lstTypes.SelectedIndex = 1;
            //    else
            //        lstTypes.SelectedIndex = 0;
            //}
            //if (Report_ID == Report_1002)
            //{

            //    lstTypes.Items.Add(new ListItem("OtherQuery1_A6a", "4"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery2_A17", "5"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery3_A23", "6"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery4_A24", "7"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery5_A29", "8"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery6_A14", "9"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery7_A36", "10"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery7_A45", "11"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery9_A49", "12"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery10_A16", "13"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery11_A39", "14"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery12_A34", "15"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery13_A47", "16"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery14_C13", "17"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery15_C33", "18"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery16_C27", "19"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery17_C41", "20"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery18_C30", "21"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery19_C25", "22"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery20_C29", "23"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery21_C7", "24"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery22_G10b", "25"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery23_G10a", "26"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery24_i7", "27"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery25_B18", "28"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery26_B22", "29"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery27_B11", "30"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery28_B14", "31"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery29_B19", "32"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery30_B23", "33"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery31_B20", "34"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery32_B1a", "35"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery33_B1b", "36"));
            //    lstTypes.Items.Add(new ListItem("OtherQuery34_C40", "37"));

            //    lstTypes.SelectedIndex = 0;
            //}
            if (Report_ID == Report_7 || Report_ID == Report_8 || Report_ID == Report_9 || Report_ID == Report_10 || Report_ID == Report_11)
            {
                lstTypes.Items.Clear();
                lstTypes.Items.Add(new ListItem("All", "All"));
                lstTypes.Items.Add(new ListItem("Rural", "Rural"));
                lstTypes.Items.Add(new ListItem("Urban", "Urban"));

                criteria_list.Items.Clear();
                criteria_list.Items.Add(new ListItem("TOO FEW HOUSEHOLDS WITH CHILDERN IN A CLUSTER", "C_1"));
                criteria_list.Items.Add(new ListItem("TOO BIG HOUSES", "C_2"));
                criteria_list.Items.Add(new ListItem("VERY SMALL HOUSES", "C_3"));
                criteria_list.Items.Add(new ListItem("TOO MANY HOUSEHOLDS PER STRUCTURE", "C_4"));
                criteria_list.Items.Add(new ListItem("SMALL CLUSTERS", "C_5"));

                //criteria_list.SelectedIndex = 0;
            }

        }


        private void gv_Bind_Data(string Filter_Data, string Report_ID, string Report_Name)
        {

            lblTableHeading.Text = Report_Name;
            string query = "";
            string q2 = "";


            if (Report_ID == Report_1)
            {
                query = "SELECT * FROM [li_listing_progress] " + Filter_Data + " order by cast(REPLACE([Percent Covered],'%','') as int)  ASC";
                
            }

            else if (Report_ID == Report_2)
            {

                query = "SELECT * FROM [li_size_of_cluster] " + Filter_Data + "  order by  [Number of HH per cluster H=G/D] desc";
            }

            else if (Report_ID == Report_3)
            {

                query = "SELECT * FROM [li_short_addresses]" + Filter_Data + " order by [Number of  structures with short addresses (<=6)] desc ";
            }

            else if (Report_ID == Report_4)
            {

                query = "SELECT * FROM [li_percent_non_dwelling]" + Filter_Data + "  order by cast(REPLACE([Percentage of Non-dwelling  structures],'%','') as int) desc";
            }

            else if (Report_ID == Report_5)
            {

                query = "SELECT * from  [li_percent_HH_childern]" + Filter_Data + " order by cast(REPLACE([Percentage of households with children 5-17],'%','') as int) desc";
            }
            else if (Report_ID == Report_55)
            {

                query = @"With Table1 As
                            (
                            SELECT DISTRICT,CLUSTER_CODE, LISTER_NAME

                            ,count(distinct(STRUCTURE_NO)) AS TOTAL_STRUCTURES,count(distinct(RECORDGPS_FIRST_LAT)) as GPS_COLLECTED
                            ,count(distinct(RECORDGPS_FIRST_LAT))*100 / count(distinct(STRUCTURE_NO)) AS COVERAGE
                            from Listing_HH
                            group by DISTRICT,CLUSTER_CODE,LISTER_NAME

                            )
                            ,TABLE2 AS
                            (select DISTRICT,CLUSTER_CODE,LISTER_NAME,TOTAL_STRUCTURES,GPS_COLLECTED,CONCAT(COVERAGE,'%') AS COVERAGE
                            ,CASE WHEN COVERAGE<=(PERCENTILE_CONT(0.1) WITHIN GROUP (ORDER BY COVERAGE)   
                            OVER (PARTITION BY NULL)) THEN 1 ELSE 0 END AS [FLAG BELOW 10TH PERCENTILE] 
                            from Table1)

                            SELECT TABLE2.* FROM TABLE2 INNER JOIN  Table1 ON Table1.DISTRICT=TABLE2.DISTRICT AND Table1.CLUSTER_CODE=TABLE2.CLUSTER_CODE
                            AND [FLAG BELOW 10TH PERCENTILE]=1
                            ORDER BY TABLE1.COVERAGE";
            }

            else if (Report_ID == Report_6)
            {

                query = "SELECT * FROM [li_HH_per_structure]" + Filter_Data + "  order by [Mean number of households per structure] desc";
            }


            else if (Report_ID == "C_1" || Report_ID == Report_7)
            {
                lstTypes.Visible = false; lblTypes.Visible = false;
                GridView1.Visible = true;
                // Filter_Data = Filter_Data.Replace("WHERE", "And");
                query = @"With Table1 as
                 (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District]
                            ,LISTER_NAME,
							(Select count(cast(L1.CHILDREN as int )) as ST FROM   Listing_HH L1
                            WHERE    CHILDREN is not null and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE)*100/(Select count(cast(L1.HHMEMBERS_COUNT as int )) as ST FROM   Listing_HH L1
                            WHERE    L1.HHMEMBERS_COUNT > 0 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) AS [HH_Childern]

                            , ROW_NUMBER() OVER(ORDER BY (Select count(cast(L1.CHILDREN as int )) as ST FROM   Listing_HH L1
                            WHERE    CHILDREN is not null and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE)*100/(Select count(cast(L1.HHMEMBERS_COUNT as int )) as ST FROM   Listing_HH L1
                            WHERE    L1.HHMEMBERS_COUNT > 0 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) ASC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select row# as Ranking,CLUSTER_CODE as [Cluster Code],[Name of District],LISTER_NAME,[Rural/ Urban] as [Area Type],
							CAST([HH_Childern] AS varchar)+'%' as [HH_childern] from Table1 
							inner join Listing_2018 l2  on Table1.CLUSTER_CODE=l2.Pcode and Table1.[Name of District]=l2.District 
                            where Row# between 1 and 10 order by Ranking asc  ";
            }

            else if (Report_ID == "C_2")
            {

                // Filter_Data = Filter_Data.Replace("WHERE", "And");
                query = @" With Table1 as  (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District],LISTER_NAME
                            ,(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban' 
                             and  l1.HHMEMBERS_COUNT >10 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 
                            inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban' 
                            and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            ) as Percentage_HHMembers

                            , ROW_NUMBER() OVER(ORDER BY (select count(l1.CLUSTER_CODE)  from Listing_HH l1
                            inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban' 
                             and l1.HHMEMBERS_COUNT >10 
                            and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban' 
                            and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100 DESC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select Row# as Ranking,CLUSTER_CODE as [Cluster Code],[Name of District],LISTER_NAME,[Rural/ Urban]  AS [Area Type],
                            cast(Percentage_HHMembers as varchar)+'%' as [More10_HH_members] from Table1
                            inner join  Listing_2018 L2 on table1.[Name of District]=l2.District and Table1.CLUSTER_CODE=l2.Pcode
                            --and l2.[Rural/ Urban]='Rural'
                            and  Row# between 1 and 10
                             order by Ranking ASC ";

                q2 = @"With Table1 as  (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District],LISTER_NAME
                            ,(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                             and  l1.HHMEMBERS_COUNT >10 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 
                            inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                            and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            ) as Percentage_HHMembers

                            , ROW_NUMBER() OVER(ORDER BY (select count(l1.CLUSTER_CODE)  from Listing_HH l1
                            inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                             and l1.HHMEMBERS_COUNT >10 
                            and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                            and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100 DESC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select Row# as Ranking,CLUSTER_CODE as [Cluster Code],[Name of District],LISTER_NAME,[Rural/ Urban]  AS [Area Type],
                            cast(Percentage_HHMembers as varchar)+'%' as [More10_HH_members] from Table1
                            inner join  Listing_2018 L2 on table1.[Name of District]=l2.District and Table1.CLUSTER_CODE=l2.Pcode
                            --and l2.[Rural/ Urban]='Rural'
                            and  Row# between 1 and 10
                              order by Ranking ASC";
            }

            else if (Report_ID == "C_3")
            {

                Filter_Data = Filter_Data.Replace("WHERE", "And");
                query = @"With Table1 as  (
                        sELECT        CLUSTER_CODE  , DISTRICT as [Name of District],LISTER_NAME
                        ,(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban'
                         and  l1.HHMEMBERS_COUNT <3 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 
                        inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban'
                        and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        ) as Percentage_HHMembers

                        , ROW_NUMBER() OVER(ORDER BY (select count(l1.CLUSTER_CODE)  from Listing_HH l1
                        inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban'
                         and l1.HHMEMBERS_COUNT <3 
                        and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban'
                        and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        )*100 DESC) AS Row#

                        FROM            Listing_HH
                        WHERE      LISTER_CODE is not null
                        GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                        )
                        select Row# as Ranking,CLUSTER_CODE as [Cluster Code],[Name of District],LISTER_NAME,[Rural/ Urban]  AS [Area Type],
                        cast(Percentage_HHMembers as varchar)+'%' as [Fewer3_HH_members] from Table1
                        inner join  Listing_2018 L2 on table1.[Name of District]=l2.District and Table1.CLUSTER_CODE=l2.Pcode
                        and  Row# between 1 and 10
                         order by Ranking ASC ";

                q2 = @"
                        With Table1 as  (
                        sELECT        CLUSTER_CODE  , DISTRICT as [Name of District],LISTER_NAME
                        ,(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                         and  l1.HHMEMBERS_COUNT <3 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 
                        inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                        and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        ) as Percentage_HHMembers

                        , ROW_NUMBER() OVER(ORDER BY (select count(l1.CLUSTER_CODE)  from Listing_HH l1
                        inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                         and l1.HHMEMBERS_COUNT <3 
                        and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                        and l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                        )*100 DESC) AS Row#

                        FROM            Listing_HH
                        WHERE      LISTER_CODE is not null
                        GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                        )
                        select Row# as Ranking,CLUSTER_CODE as [Cluster Code],[Name of District],LISTER_NAME,[Rural/ Urban]  AS [Area Type],
                        cast(Percentage_HHMembers as varchar)+'%' as [Fewer3_HH_members] from Table1
                        inner join  Listing_2018 L2 on table1.[Name of District]=l2.District and Table1.CLUSTER_CODE=l2.Pcode
                        and  Row# between 1 and 10
                         order by Ranking ASC ";
            }

            else if (Report_ID == "C_4")
            {

                Filter_Data = Filter_Data.Replace("WHERE", "And");
                query = @"  With Table2 as
                        (
                        select  CLUSTER_CODE,[Name of District],max(HH) AS [Max HH Per Structure],LISTER_NAME,Area_type,
                        ROW_NUMBER() OVER(ORDER BY (Select max(hh)as ST FROM   Listing_HH L1
                        WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=table1.CLUSTER_CODE 
                        GROUP BY L1.CLUSTER_CODE) DESC) AS Row#
                            from 
                        (
                        Select l1.DISTRICT as [Name of District],CLUSTER_CODE,STRUCTURE_NO,count(DISTINCT (L1.SR_NO_HOUSEHOLD ))as HH
                        ,LISTER_NAME,[Rural/ Urban] as Area_type
                        FROM   Listing_HH L1
                        inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Urban'
                        WHERE    STRUCTURE_TYPE=1 
                        GROUP BY l1.DISTRICT,L1.CLUSTER_CODE,STRUCTURE_NO,LISTER_NAME,[Rural/ Urban])Table1
                        group by CLUSTER_CODE,[Name of District],LISTER_NAME,Area_type
                        )
                        select row# as Ranking,CLUSTER_CODE,[Name of District],LISTER_NAME,Area_type,[Max HH Per Structure] from Table2
                        where Row# between 1 and 10  order by Ranking ASC";

                q2 = @"
                        With Table2 as
                        (
                        select  CLUSTER_CODE,[Name of District],max(HH) AS [Max HH Per Structure],LISTER_NAME,Area_type,
                        ROW_NUMBER() OVER(ORDER BY (Select max(hh)as ST FROM   Listing_HH L1
                        WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=table1.CLUSTER_CODE 
                        GROUP BY L1.CLUSTER_CODE) DESC) AS Row#
                            from 
                        (
                        Select l1.DISTRICT as [Name of District],CLUSTER_CODE,STRUCTURE_NO,count(DISTINCT (L1.SR_NO_HOUSEHOLD ))as HH
                        ,LISTER_NAME,[Rural/ Urban] as Area_type
                        FROM   Listing_HH L1
                        inner join Listing_2018 l2  on l1.CLUSTER_CODE=l2.Pcode and l1.DISTRICT=l2.District  and [Rural/ Urban]='Rural'
                        WHERE    STRUCTURE_TYPE=1 
                        GROUP BY l1.DISTRICT,L1.CLUSTER_CODE,STRUCTURE_NO,LISTER_NAME,[Rural/ Urban])Table1
                        group by CLUSTER_CODE,[Name of District],LISTER_NAME,Area_type
                        )
                        select row# as Ranking,CLUSTER_CODE,[Name of District],LISTER_NAME,Area_type,[Max HH Per Structure] from Table2
                        where Row# between 1 and 10  order by Ranking ASC";
            }

            else if (Report_ID == "C_5")
            {
                lstTypes.Visible = false; lblTypes.Visible = false;
                GridView1.Visible = true;

                //Filter_Data = Filter_Data.Replace("WHERE", "And");
                query = @" With Table1 as
	                        (
	                        sELECT        CLUSTER_CODE  , DISTRICT as [Name of District],LISTER_NAME
		                        ,(Select count(DISTINCT (L1.SR_NO_HOUSEHOLD ))as ST FROM   Listing_HH L1
	                        WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
	                        GROUP BY L1.CLUSTER_CODE) AS [Total_HH]

	                        , ROW_NUMBER() OVER(ORDER BY (Select count(DISTINCT (L1.SR_NO_HOUSEHOLD ))as ST FROM   Listing_HH L1
	                        WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
	                        GROUP BY L1.CLUSTER_CODE) ASC) AS Row#

	                        FROM            Listing_HH
	                        WHERE      LISTER_CODE is not null
	                        GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
	                        )
	                        select row# as Ranking,CLUSTER_CODE as [Cluster Code],[Name of District],
	                        [Rural/ Urban] as [Area Type],
	                        LISTER_NAME,	[Total_HH] from Table1 
	                        inner join Listing_2018 l2  on Table1.CLUSTER_CODE=l2.Pcode and Table1.[Name of District]=l2.District 
	                        where Row# between 1 and 10  order by Ranking ASC";
            }

            if (Report_ID == "C_2" || Report_ID == "C_3" || Report_ID == "C_4")
            {
                SqlCommand cmdd = new SqlCommand(q2);
                cmdd.Connection = con;


                SqlDataAdapter dda = new SqlDataAdapter(cmdd);
                dda.Fill(ddt);

                GridView2.DataSource = ddt;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {
                GridView2.Visible = false;
            }

            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = con;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
           

                if (Report_ID == Report_1 || Report_ID == Report_2 || Report_ID == Report_4 || (Report_ID == Report_5) || (Report_ID == Report_6))
                    {

               
                             GirdView_Footer_Total();
                    }
            //}


        }

        protected void lstDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load_Filters_Data();
            if (lstDistricts.SelectedItem.Text == "All")
            {
                Filter_Data = "";
            }
            else
            {
                Filter_Data = "WHERE [Name of District] = '" + lstDistricts.SelectedItem.Text + "'";
            }
            //load_Filter_SurveyTeam(Filter_Data);
            load_Filter_ClusterCodes(Filter_Data);
            load_Filter_Enumerator(Filter_Data);

            if (Report_ID == Report_7) //cc
            {
                
                //criteria_list.SelectedValue.Contains(lblcriteria.Text);
                gv_Bind_Data(Filter_Data, cc, criteria_list.Items.FindByValue(cc).Text.ToString());
            }
            else
            {
                gv_Bind_Data(Filter_Data, Report_ID, Report_Name);
            }
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
                    Filter_Data = " Where [Lister_Name] = '" + lstEnumerator.SelectedItem.Text + "'";

                }
                else
                {
                    Filter_Data = Filter_Data + " and [Lister_Name] = '" + lstEnumerator.SelectedItem.Text + "'";
                }

            }

            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);

        }
        protected void lstTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTypes.SelectedItem.Text.Equals("Urban"))
            {
                GridView2.Visible = false;
                GridView1.Visible = true;
            }
            else if (lstTypes.SelectedItem.Text.Equals("Rural"))
            {
                GridView1.Visible = false;
                GridView2.Visible = true;
            }
            else
            {
                GridView1.Visible = true;
                GridView2.Visible = true;
            }
           // Filter_All_Selected_Items();
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
                        //if (Report_ID == Report_1003)
                        //    Filter_Data = lstTypes.SelectedItem.Value;
                        //else if (lstTypes.SelectedItem != null)
                        //    Filter_Data = lstTypes.SelectedItem.Text;

                    }

                }
            }

            gv_Bind_Data(Filter_Data, Report_ID, Report_Name);


        }

        private void GirdView_Footer_Total()
        {
            string[] columntypes = new string[100];
            string columnName;

            if (dt != null && dt.Rows.Count > 0)
            {
                string type = "Int32";
                int count = 1;
                int count2 = 0;
                //string type2 = "Decimal";

                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    columnName = dt.Columns[j].ColumnName.ToString();


                    if (type == dt.Columns[j].DataType.Name.ToString())
                    {
                        if (count2 == 0)
                        {
                            GridView1.FooterRow.Cells[0].ColumnSpan = j;
                            GridView1.FooterRow.Cells[0].Text = "Total";

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


                        if (((count >= 3) && (Report_ID == Report_1)) || ((count >= 3) && (Report_ID == Report_3)) || ((count == 4) && (Report_ID == Report_5)) || ((count == 7) && (Report_ID == Report_1)) || ((count == 3) && (Report_ID == Report_2)) || ((count == 5) && (Report_ID == Report_2))) //!columnName.Equals("Number of households with children 5-17"))
                        {
                            float total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            float total0 = dt.Rows.Count;
                            float total2 = total1 / total0;
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].Text = total2.ToString();
                            //GridView1.FooterRow.Cells[count].Text = "sdsdsd";
                        }
                        else if (columnName != "Average number of working children in HH with at least 1 working children")
                        {

                            Int32 total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].Text = total1.ToString();
                            if(columnName == "Median of the number of households per structure (unit)")
                            {
                                GridView1.FooterRow.Cells[count].Text = "";
                            }

                        }
                        else if (columnName == "Percentage of households with children 5-17")
                        {

                            float total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            float total0 = dt.Rows.Count;
                            float total2 = total1 / total0;
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].Text = total2.ToString();

                        }

                        else
                        {
                            Int32 total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                            Int32 total0 = dt.Rows.Count;
                            float total2 = total1 / total0;
                            GridView1.FooterRow.Font.Bold = true;
                            GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                            GridView1.FooterRow.Cells[count].Text = total2.ToString();

                        }



                    }
                    else if (Report_ID == Report_5)
                    {
                        //  Int32 total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                        //Int32 total0 = dt.Rows.Count;
                        int total0 = dt.AsEnumerable().Sum(row => row.Field<Int32?>("Number of Households") ?? 0);
                        int total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>("Number of households with children 5-17") ?? 0);
                        float total2 = (total1 *100 / total0);
                        GridView1.FooterRow.Font.Bold = true;
                        GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                        GridView1.FooterRow.Cells[count].Text = total2.ToString() + "%";
                    }
                    else if (Report_ID == Report_1 && count == 3)
                    {
                        //  Int32 total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                        //Int32 total0 = dt.Rows.Count;
                        int total0 = dt.AsEnumerable().Sum(row => row.Field<Int32?>("Number of Clusters Covered") ?? 0);
                        int total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>("Target Clusters") ?? 0);
                        float total2 = (total0 * 100 / total1);
                        GridView1.FooterRow.Font.Bold = true;
                        GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                        GridView1.FooterRow.Cells[count].Text = total2.ToString() + "%";
                    }
                    else if (Report_ID == Report_4 && count == 4)
                    {
                        //  Int32 total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>(columnName) ?? 0);
                        //Int32 total0 = dt.Rows.Count;
                        int total0 = dt.AsEnumerable().Sum(row => row.Field<Int32?>("Number of Non-dwelling  structures ") ?? 0);
                        int total1 = dt.AsEnumerable().Sum(row => row.Field<Int32?>("Number of structures covered") ?? 0);
                        float total2 = (total0 * 100 / total1);
                        GridView1.FooterRow.Font.Bold = true;
                        GridView1.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GridView1.FooterRow.Cells[count].HorizontalAlign = HorizontalAlign.Center;
                        GridView1.FooterRow.Cells[count].Text = total2.ToString() + "%";
                    }
                    

                    if (count2 > 0)
                        count++;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Filter_All_Selected_Items();
        }
        public string CheckColor(string key_, int section)
        {
            string color = "";
            using (SqlCommand cmd = new SqlCommand("exec [CheckColor] @key_='" + key_ + "', @Section='" + section + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
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
            if (Report_ID == Report_2)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                  

                    //loop all cells in the row
                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        //set the tooltip text to be the header text
                        //e.Row.Cells[i].ToolTip = GridView2.HeaderRow.Cells[i].Text;
                    }
                }
            }
            else if (Report_ID != Report_2)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Report_ID == Report_7 || Report_ID == Report_8 || Report_ID == Report_9 || Report_ID == Report_10 || Report_ID == Report_11)
                    {
                       // e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                       // e.Row.Attributes["style"] = "cursor:pointer";
                    }
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
            string liGreenTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:YellowGreen;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liGreenTagClose = "</li>";
            string liKhakiTagOpen = "<li style='margin-top: 10px;'><span style='background-color:khaki;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liKhakiTagClose = "</li>";
            string liMediumAquamarineTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:MediumAquamarine;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liMediumAquamarineTagClose = "</li>";
            string liBlackTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:Black;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liBlackClose = "</li>";



            if (e.Row.RowIndex >= 0)
            {

                if (Report_ID == "")
                {
                    //e.Row.Cells[5].Text = e.Row.Cells[5].Text + "%";
                    // e.Row.Cells[5].Text = e.Row.Cells[6].Text + "%";

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
                else if (Report_ID == "")
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



                else if (Report_ID == "")
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

                else if (Report_ID == "")
                {


                    try
                    {

                        e.Row.Cells[6].Text = e.Row.Cells[6].Text + "%";
                    }
                    catch (Exception) // Null Value
                    {

                    }
                }

                else if (Report_ID == "")
                {
                    double cellValue = 0, cellValue2 = 0;

                    try
                    {
                        cellValue = Convert.ToDouble(e.Row.Cells[9].Text.ToString());

                        cellValue2 = Convert.ToDouble(e.Row.Cells[10].Text.ToString());

                        e.Row.Cells[9].Text = e.Row.Cells[9].Text + "%";
                        e.Row.Cells[10].Text = e.Row.Cells[10].Text + "%";
                    }
                    catch (Exception) // Null Value
                    {

                    }

                    if (cellValue >= 90 && cellValue <= 100)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.YellowGreen;
                    else if (cellValue >= 80 && cellValue < 90)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.Khaki;
                    else if (cellValue < 80)
                        e.Row.Cells[9].BackColor = System.Drawing.Color.Coral;


                    if (cellValue2 >= 0)
                        e.Row.Cells[10].BackColor = System.Drawing.Color.YellowGreen;
                    else if (cellValue2 >= -10)
                        e.Row.Cells[10].BackColor = System.Drawing.Color.Khaki;
                    else if (cellValue2 <= -20)
                        e.Row.Cells[10].BackColor = System.Drawing.Color.Coral;

                    lblLegendsHeading.Visible = true;
                    lblLegendsText.Visible = true;

                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Efficency: Target # of children as per listing <80%" + liRedTagClose + liKhakiTagOpen + "Efficency: Target # of children as per listing <90% & >80%" + liKhakiTagClose + liGreenTagOpen + "Efficency: Target # of children as per listing >=90% <=100%" + liGreenTagClose + ulTagClose;


                }

                else if (Report_ID == "")
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

                    lblLegendsText.Text = ulTagOpen + liRedTagOpen + "Efficency: Average collection more than 3 per day" + liRedTagClose + ulTagClose;

                }





                else if (Report_ID == "")
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

        //protected void c1_Click(object sender, EventArgs e)
        //{
        //    Visible_Filters();
        //    load_Filters_Data();
        //    gv_Bind_Data(" ", "c11", "Too few households with children in a cluster");
        //}

        //protected void c2_Click(object sender, EventArgs e)
        //{
        //    Visible_Filters();
        //    load_Filters_Data();
        //    gv_Bind_Data(" ", "c21", "Too big Houses");
        //}

        //protected void c3_Click(object sender, EventArgs e)
        //{
        //    Visible_Filters();
        //    load_Filters_Data();
        //    gv_Bind_Data(" ", "c31", "Very small houses");
        //}

        //protected void c4_Click(object sender, EventArgs e)
        //{
        //    Visible_Filters();
        //    load_Filters_Data();
        //    gv_Bind_Data(" ", "c41", "Too many households per structure");
        //}

        //protected void c5_Click(object sender, EventArgs e)
        //{
        //    Visible_Filters();
        //    load_Filters_Data();
        //    gv_Bind_Data(" ", "c51", "Small clusters");
        //}

        protected void criteria_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            cc = criteria_list.SelectedItem.Value;
            if (cc == "C_1")
            {
                lstTypes.Visible = false; lblTypes.Visible = false;
                footnote.Visible = false;
                Filter_Data = Filter_Data.Replace("WHERE", "And");
                Visible_Filters();
                load_Filters_Data();
                Report_ID = "C_1";
                gv_Bind_Data(" ", "C_1", "Too few households with children in a cluster");
            }

            else if (cc == "C_2")
            {
                lstTypes.Visible = true; lblTypes.Visible = true;
                footnote.Text = "Sindh: 90th percentile is 10 members for rural and 10 members for urban (both are the same)";
                footnote.Visible = true;
                Filter_Data = Filter_Data.Replace("WHERE", "And");
                Visible_Filters();
                load_Filters_Data();
                Report_ID = "C_2";
                gv_Bind_Data(" ", "C_2", "Too big Houses");

            }

            else if (cc == "C_3")
            {
                lstTypes.Visible = true; lblTypes.Visible = true;
                footnote.Text = "Sindh: 10th percentile is 3 members for rural and 3 members for urban (both are the same)";
                footnote.Visible = true;
                Filter_Data = Filter_Data.Replace("WHERE", "And");
                Visible_Filters();
                load_Filters_Data();
                Report_ID = "C_3";
                gv_Bind_Data(" ", "C_3", "Very small houses");

            }

            else if (cc == "C_4")
            {
                lstTypes.Visible = true; lblTypes.Visible = true;
                footnote.Visible = false;
                Filter_Data = Filter_Data.Replace("WHERE", "And");
                Visible_Filters();
                load_Filters_Data();
                Report_ID = "C_4";
                gv_Bind_Data(" ", "C_4", "Too many households per structure");


            }

            else if (cc == "C_5")
            {
                lstTypes.Visible = false; lblTypes.Visible = false;
                footnote.Visible = false;
                Filter_Data = Filter_Data.Replace("WHERE", "And");
                Visible_Filters();
                load_Filters_Data();
                Report_ID = "C_5";
                gv_Bind_Data(" ", "C_5", "Small clusters");

            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {


            GridViewRow row = GridView1.SelectedRow;
            string district = row.Cells[1].Text;
            if (cc == "")
                cc = "C_1";
            //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + criteria_list.SelectedValue + "');", true);
            modal_heading.Text = lblTableHeading.Text;
            modal_binding(cc, district);
            //   ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('"+cc+"');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
        }
        public void modal_binding(string report, string district)
        {
            string sql_query = "";
            if (report == "C_1")
            {

                sql_query = @"  With Table1 as
                 (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District]
                            ,
							(Select count(cast(L1.CHILDREN as int )) as ST FROM   Listing_HH L1
                            WHERE    CHILDREN is not null and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE)*100/(Select count(cast(L1.HHMEMBERS_COUNT as int )) as ST FROM   Listing_HH L1
                            WHERE    L1.HHMEMBERS_COUNT > 0 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) AS [Number of Childern]

                            , RANK() OVER(PARTITION BY DISTRICT ORDER BY (Select count(cast(L1.CHILDREN as int ))as ST FROM   Listing_HH L1
                            WHERE    CHILDREN is not null and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) ASC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select CLUSTER_CODE as [Cluster Code],[Name of District],CAST([Number of Childern] AS varchar)+'%' as [Few HH with childern] from Table1
                            where [Name of District] = '" + district + "' order by [Number of Childern] asc  ";
            }

            else if (report == "C_2")
            {


                sql_query = @"With Table1 as  (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District]
                            ,(select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT >15 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            ) as Total

                            , RANK() OVER(PARTITION BY DISTRICT ORDER BY (select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT >15 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100 DESC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select CLUSTER_CODE as [Cluster Code],[Name of District],cast(Total as varchar)+'%' as [Highest Percentage] from Table1
                          where [Name of District] =  '" + district + "' order by Total desc ";
            }

            else if (report == "C_3")
            {


                sql_query = @"With Table1 as  (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District]
                            ,(select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT <5  and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            ) as Total

                            , RANK() OVER(PARTITION BY DISTRICT ORDER BY (select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT <5  and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100/(select count(l1.CLUSTER_CODE)  from Listing_HH l1 where l1.HHMEMBERS_COUNT >0 and Listing_HH.CLUSTER_CODE=l1.CLUSTER_CODE group by  CLUSTER_CODE
                            )*100 DESC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select CLUSTER_CODE as [Cluster Code],[Name of District],cast(Total as varchar)+'%' as [Highest Percentage] from Table1
                            where [Name of District] =  '" + district + "' ORDER BY Total DESC ";
            }

            else if (report == "C_4")
            {


                sql_query = @" With Table2 as
                            (
                            select  CLUSTER_CODE,[Name of District],max(HH) AS [Max HH Per Structure],
                            RANK() OVER(PARTITION BY [Name of District] ORDER BY (Select max(hh)as ST FROM   Listing_HH L1
                            WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=table1.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) DESC) AS Row#
                             from 
                            (
                            Select DISTRICT as [Name of District],CLUSTER_CODE,STRUCTURE_NO,count(DISTINCT (L1.SR_NO_HOUSEHOLD ))as HH
                            FROM   Listing_HH L1
                            WHERE    STRUCTURE_TYPE=1 
                            GROUP BY DISTRICT,L1.CLUSTER_CODE,STRUCTURE_NO)Table1
                            group by CLUSTER_CODE,[Name of District]
                            )
                            select CLUSTER_CODE,[Name of District],[Max HH Per Structure] from Table2
                            where [Name of District] = '" + district + "' order by [Max HH Per Structure] DESC ";

            }

            else if (report == "C_5")
            {


                sql_query = @" With Table1 as
                            (
                            sELECT        CLUSTER_CODE  , DISTRICT as [Name of District]
                            ,(Select count(DISTINCT (L1.KEY_ ))as ST FROM   Listing_HH L1
                            WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) AS [Number of units interviewed]

                            , RANK() OVER(PARTITION BY DISTRICT ORDER BY (Select count(DISTINCT (L1.KEY_ ))as ST FROM   Listing_HH L1
                            WHERE    STRUCTURE_TYPE=1 and   L1.CLUSTER_CODE=Listing_HH.CLUSTER_CODE 
                            GROUP BY L1.CLUSTER_CODE) ASC) AS Row#

                            FROM            Listing_HH
                            WHERE      LISTER_CODE is not null
                            GROUP BY CLUSTER_CODE, LISTER_NAME, DISTRICT 
                            )
                            select CLUSTER_CODE as [Cluster Code],[Name of District],[Number of units interviewed] from Table1
                           where [Name of District] =  '" + district + "' ORDER BY [Number of units interviewed] ASC ";
            }
            SqlCommand cmd = new SqlCommand(sql_query);
            cmd.Connection = con;


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            modal_content.DataSource = dt;
            modal_content.DataBind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {


                //using (SqlCommand cmd = new SqlCommand("DELETE FROM Listing_HH WHERE PARENT_KEY = @PARENT_KEY", con))
                //{

                //    connection();
                //    cmd.Connection = con;
                //    cmd.Parameters.AddWithValue("@PARENT_KEY", lblParentKey.Text);
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //    ShowMessage("Duplicate Record deleted successfully", MessageType.Success);
                //    rep_bind();

                //}


            }
            catch (Exception ex)
            {
              //  ShowMessage("Error: Unable to delete Record .. ", MessageType.Error);
            }
        }



    }


}
