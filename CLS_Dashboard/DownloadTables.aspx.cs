using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;

namespace CLS_Dashboard
{
    public partial class DownloadTables : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        public static string code = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        public static string strQuery = "";
        public static string map = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }



            else
            {
                Session["lblHeadingName"] = "<b>CLS</b>-Download";


                if (!this.IsPostBack)
                {
                    //Div_Download.Attributes["class"] = "display:none";  // For Hide
                    string Assign = (string)(Session["Assign"]);
                    string ReportGroup = (string)(Session["ReportGroup"]);
                    string Role = (string)(Session["Role"]);
                    DataTable dt_report_allow = new DataTable();

                    if (Session["dt_report_allow"] != null)
                    {
                        dt_report_allow = (DataTable)Session["dt_report_allow"];
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }

                    map = Request.QueryString["id"];
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Detail"), new DataColumn("Code") });

                    if (Role == "Admin")
                    {
                        foreach (DataRow row in dt_report_allow.Rows)
                        {
                            dt.Rows.Add(Convert.ToInt32(row["id"].ToString()), row["description"].ToString(), row["code"].ToString());
                        }
                    }
                    else if (map.Contains("main"))
                    {
                        foreach (DataRow row in dt_report_allow.Rows)
                        {
                            String report_name = row["code"].ToString();
                            if (!report_name.Contains("Listing") && report_name != "GPS_Collected")
                            {
                                dt.Rows.Add(Convert.ToInt32(row["id"].ToString()), row["description"].ToString(), row["code"].ToString());
                            }

                        }


                        //if (Assign.Contains("A") || Assign.Contains("Allow")) { dt.Rows.Add(1, "Roster Details", "A"); }
                        //if (Assign.Contains("A1") || Assign.Contains("Allow")) { dt.Rows.Add(2, "Family tree", "A1"); }

                        //if (Assign.Contains("B") || Assign.Contains("Allow")) { dt.Rows.Add(3, "Household Asset details", "B"); }
                        //if (Assign.Contains("Main") || Assign.Contains("Allow")) { dt.Rows.Add(4, "Adult Questionnaire", "Main"); }
                        //if (Assign.Contains("C") || Assign.Contains("Allow")) { dt.Rows.Add(5, "Child questionnaire", "C"); }
                        //if (Assign.Contains("QCR") || Assign.Contains("Allow")) { dt.Rows.Add(5, "Quick Count Record", "QCR"); }
                        //if (Assign.Contains("Monitoring") || Assign.Contains("Allow")) { dt.Rows.Add(6, "Monitoring Data", "Monitoring"); }
                        //if (Assign.Contains("Observer") || Assign.Contains("Allow")) { dt.Rows.Add(7, "Observers data", "Observer"); }
                        //if (Assign.Contains("IndustriesTextReview") || Assign.Contains("Allow")) { dt.Rows.Add(8, "Industries TextReview", "IndustriesTextReview"); }
                        //if (Assign.Contains("ProfessionTextReview") || Assign.Contains("Allow")) { dt.Rows.Add(9, "Occupation TextReview", "ProfessionTextReview"); }
                        //if (Assign.Contains("ToolsTextReview") || Assign.Contains("Allow")) { dt.Rows.Add(10, "Tools TextReview", "ToolsTextReview"); }
                        //if (Assign.Contains("SummaryReport") || Assign.Contains("Allow")) { dt.Rows.Add(11, "Summarized Report", "SummaryReport"); }
                        //if (Assign.Contains("MistakesCount") || Assign.Contains("Allow")) { dt.Rows.Add(12, "Mistake Count Report", "MistakesCount"); }
                        //if (Assign.Contains("MistakeOutlier") || Assign.Contains("Allow")) { dt.Rows.Add(13, "Mistake Outlier Report", "MistakeOutlier"); }
                        //if (Assign.Contains("OthersFieldSummary") || Assign.Contains("Allow")) { dt.Rows.Add(14, "Others Field Wise Report", "OthersFieldSummary"); }
                        //if (Assign.Contains("OthersSummary") || Assign.Contains("Allow")) { dt.Rows.Add(15, "Others Enumerators Report", "OthersSummary"); }
                        //if (Assign.Contains("HHSummary") || Assign.Contains("Allow")) { dt.Rows.Add(16, "Household Size Report", "HHSummary"); }
                        //if (Assign.Contains("OthersCodes") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Other Coding", "OthersCodes"); }
                        //if (Assign.Contains("AcceptedCodesInd") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Accepted Codes Industries", "AcceptedCodesInd"); }
                        //if (Assign.Contains("AcceptedCodesOcc") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Accepted Codes Occupations", "AcceptedCodesOcc"); }
                        //if (Assign.Contains("AcceptedCodesTools") || Assign.Contains("Allow")) { dt.Rows.Add(18, "Accepted Codes Tools", "AcceptedCodesTools"); }
                        //if (Assign.Contains("Deskmonitor_correction") || Assign.Contains("Allow")) { dt.Rows.Add(18, "Corrections of Deskmonitor", "Deskmonitor_correction"); }
                        //if (Assign.Contains("Orig_A") || Assign.Contains("Allow")) { dt.Rows.Add(1, "Original Roster Details", "Orig_A"); }
                        //if (Assign.Contains("Orig_A1") || Assign.Contains("Allow")) { dt.Rows.Add(2, "Original Family tree", "Orig_A1"); }
                        //if (Assign.Contains("Orig_B") || Assign.Contains("Allow")) { dt.Rows.Add(3, "Original Household Asset details", "Orig_B"); }
                        //if (Assign.Contains("Orig_Main") || Assign.Contains("Allow")) { dt.Rows.Add(4, "Original Adult Questionnaire", "Orig_Main"); }
                        //if (Assign.Contains("Orig_C") || Assign.Contains("Allow")) { dt.Rows.Add(5, "Original Child questionnaire", "Orig_C"); }
                        //if (Assign.Contains("Template_data") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Template of ODK Form", "Template_data"); }

                        //if (Assign.Contains("Super_data") || Assign.Contains("Allow")) { dt.Rows.Add(18, "Supervisor App Data", "Super_data"); }

                        //if (Assign.Contains("Kohistan_listing") || Assign.Contains("Allow")) { dt.Rows.Add(19, "Kohistan Listing", "Kohistan_listing"); }
                    }
                    else if (map.Contains("listing"))
                    {
                        //  if (Assign.Contains("listing_monitoring") || Assign.Contains("Allow")) { dt.Rows.Add(22, "Cluster for Monitoring", "listing_monitoring"); }

                        foreach (DataRow row in dt_report_allow.Rows)
                        {
                            String report_name = row["code"].ToString();
                            if (report_name.Contains("Listing") || report_name == "GPS_Collected")
                            {
                                dt.Rows.Add(Convert.ToInt32(row["id"].ToString()), row["description"].ToString(), row["code"].ToString());
                            }
                        }

                        //if (Assign.Contains("Listing_data_raw") || Assign.Contains("Allow")) { dt.Rows.Add(23, "Listing Data (RAW)", "Listing_data_raw"); }
                        //if (Assign.Contains("Listing_data") || Assign.Contains("Allow")) { dt.Rows.Add(23, "Listing Data", "Listing_data"); }

                        //if (Assign.Contains("Listing_listers") || Assign.Contains("Allow")) { dt.Rows.Add(23, "Listers Collected Addresses", "Listing_listers"); }
                        //if (Assign.Contains("GPS_Collected") || Assign.Contains("Allow")) { dt.Rows.Add(23, "GPS Collected Below 10th Percentile", "GPS_Collected"); }
                    }
                    //dt.re
                    GridView1.DataSource = dt;
                    GridView1.DataBind();



                    //if (Assign.Contains("A")) { GridView1.Rows[1].Visible = true; }

                }
            }
        }



        private void SQL_Query(string code)
        {
            //      if (code == "Kohistan_listing")
            //      {
            //          logFile.log("Downloading Listing", Session["username"].ToString());
            //          strQuery = "SELECT [PARENT_KEY]"+
            //",[_CREATION_DATE]"+
            //",[_SUBMISSION_DATE]" +
            //",[DISTRICT]" +
            //",[NOTABLE_PERSON_CELL]" +
            //",[TODAY_VISIT1]" +
            //",[EBCODE]" +
            //",[DEVICE_ID]" +
            //",[CLUSTER_CODE]" +
            //",[LOCALITY_ADDRESS]" +
            //",[NOTABLE_PERSON]" +
            //",[LISTER_NAME]" +
            //",[LISTER_CODE]" +
            //",[LOCALITY]" +
            //",[AREA_TYPE]" +
            //",[TEHSIL]" +
            //",[ENDTIME_VISIT]" +
            //",[STARTTIME_VISIT1]" +
            //",[DEVICE_PHONE_NUMBER]" +
            //",[SIM_ID]" +
            //",[KEY_]" +
            //",[SR_NO_HOUSEHOLD]" +
            //",[STRUCTURE_TYPE]" +
            //",[STRUCTURE_NO]" +
            //",[REMARKS]" +
            //",[ADDRESS_OF_HOUSEHOLD]" +
            //",[CHILD_PRESENT_STATUS]" +
            //",[HHMEMBERS_COUNT]" +
            //",[HH_HEAD_NAME]" +
            //",[HH_FATHER_NAME]" +
            //",[CHILD_SCHOOL_COUNT]" +
            //",[CHILDREN]" +
            //",[BOYS_COUNT]" +
            //",[GIRLSL_COUNT]" +
            //",[HHSELECTED]" +
            //",[SELECTED_DATE]"+
            //"  FROM [dbo].[Listing_HH_Kohistan]";
            //      }
            if (code == "A")
            {
                logFile.log("Downloading A", Session["username"].ToString());
                strQuery = "select [KEY_],[PARENT_KEY],[_URI],[A1_1],'A2_1' as [A2_1],[A4_1],[A5_1],[A6_1],[A6a_other_1],[A6c_1]," +
                               "CASE when[A6d_1] is not null then 1 else 0 end as A6d_1" +
                            ",[A6e_1],[A7_1],[A8_1],[A8_PolygamousWives],[A6a_1_a],[A6a_1_b],[A6a_1_c],[A6a_1_d],[A6a_1_e],[A6a_1_f],[A6a_1_g],[A6a_1_h]," +
                            "[A6a_1_i],[A6b_1_a],[A6b_1_b],[A6b_1_c],[A6b_1_d],[A6b_1_e],[A6b_1_f],[A6b_1_g],[A6b_1_h],[A6b_1_i],[Logical_Check],A8B,[A6f_1_1],[A6f_1_2],[A6f_1_3],[A6f_1_4],[A6f_1_5],[A6f_1_6],[A6f_1_7],[A6f_1_8],[A6g_1_1],[A6g_1_2],[A6g_1_3],[A6g_1_4],[A6g_1_5],[A6g_1_6],[A6g_1_7],[A6g_1_8],[Comments_Logical],[Telephone],[Comments_Telephone],[Field],[Comments_Field]" +
                             "FROM [PART_1_A]";
            }
            else if (code == "Super_data")
            {
                strQuery = "SELECT [HH_id]" +
      ",[HH_status]" +
      ",[HH_sampled]" +
      ",[HH_result]" +
      ",[Enum]" +
     " ,[Cluster]" +
     " ,[Timestamp]" +
      ",[Revist]" +
      ",[HH_Sr]" +
       " FROM[Supervisor_App].[dbo].[HH_App]";
            }
            else if (code == "Listing_data_raw")
            {
                logFile.log("Downloading listing data", Session["username"].ToString());

                strQuery = @"SELECT cast([_SUBMISSION_DATE] as date) as [_SUBMISSION_DATE]
                           ,[DISTRICT]
	                       ,[TEHSIL]
	                       ,[LOCALITY]
	                       ,[CLUSTER_CODE]
                          ,[EBCODE]
                         ,[LISTER_CODE]
                         ,[LISTER_NAME]
                         ,[STRUCTURE_NO]
                          ,[SR_NO_HOUSEHOLD]
                          ,[STRUCTURE_TYPE]
                          ,[HHMEMBERS_COUNT]
                          ,[CHILD_PRESENT_STATUS]
                          ,[CHILDREN]
                          ,[BOYS_COUNT]
                          ,[GIRLSL_COUNT]
                          FROM [Listing_HH_Raw]";
            }
            else if (code == "A1")
            {
                logFile.log("Downloading A1", Session["username"].ToString());

                strQuery = "SELECT * from  PART_1_A1";
            }
            else if (code == "B")
            {
                logFile.log("Downloading B", Session["username"].ToString());

                strQuery = "SELECT * from  PART_1_B";
            }
            else if (code == "C")
            {
                logFile.log("Downloading C", Session["username"].ToString());

                strQuery = "SELECT  [KEY_],[PARENT_KEY],[_URI],[mChildAge],'mChildName' as mChildName ,[C0_9],[C0A_9_59],[C0B_9_59],[C1_9],[C10_9],[C10_A_9],[C10_A_other_9],[C11_9],[C12_9],[C13_9],[C13_9_other],[C14_9],[C15_9],[C16_9],[C17_10],[C19_9],[C2_9],[C20a_10],[C20b_10],[C21a_10],[C21b_10],[C22_10],[C23Ma_10],[C23Mb_10],[C23Mc_10],[C23Md_10],[C23Me_10],[C23Mf_10],[C23Mg_10],[C23Oa_10],[C23Ob_10],[C23Oc_10],[C23Od_10],[C23Oe_10],[C23Of_10],[C23Og_10],[C23_total],[C24_10A_a],[C24_10A_b],[C24_10A_c],[C24_10A_d],[C24_10B_a],[C24_10B_b],[C24_10B_c],[C24_10B_d],[C24_10B],[i2_12_a],[i2_12_b],[i2_12_c],[i2_12_d],[i2_12_e],[C25_10],[C25_10_other],[C26_10],[C27_10_other],[C27_C27_10],[C28_10],[C28A_10],[C29_10_other],[C3_9],[C3_A_9],[C3_A_Other_9],[C31_10],[C33_A_11],[C34_A_11],[C34_C34_11],[C35_11],[C36_11],[C37_11],[C39_Other_11],[C4_9],[C40_11_1],[C40_11_2],[C40_11_3],[C40_11_4],[C40_11_5],[C40_11_6],[C40_11_OTHER],[C44_12],[C5_9],[C6_9],[C7_9_other],[C8_9],[C9_9],[C9_9_OTHER],[visit_1],[visit_2_date],[visit_2_time],[C18a_10],[C18b_10],[C18c_10],[C18d_10],[C18e_10],[C18f_10],[C32_10],[C32_a_1_10],[C32_a_2_10],[C32_a_3_10],[C32_a_4_10],[C32_a_5_10],[C32_a_6_10],[C32_a_7_10],[C32_a_8_10],[C32_a_9_10],[C41_1_12],[C41_2_12],[C41_3_12],[C41_4_12],[C41_5_12],[C41_6_12],[C41_7_12],[C42_01_12],[C42_02_12],[C42_03_12],[C42_04_12],[C42_05_12],[C42_06_12],[C42_07_12],[C42_total],[C43_12A_a],[C43_12A_b],[C43_12A_c],[C43_12A_d],[C43_12B_a],[C43_12B_b],[C43_12B_c],[C43_12B_d],[C33_1_11],[C33_10_11],[C33_11_11],[C33_12_11],[C33_2_11],[C33_3_11],[C33_4_11],[C33_5_11],[C33_6_11],[C33_7_11],[C33_8_11],[C33_9_11],[C38_1_11],[C38_2_11],[C38_2_11_2nd],[C38_3_11],[C38_3_11_2nd],[C38_3a_11],[C38_3a_11_2nd],[C38_3b_11],[C38_3b_11_2nd],[C38_3c_11],[C38_3c_11_2nd],[C38_3d_11],[C38_3d_11_2nd],[C38_3e_11],[C38_3e_11_2nd],[C39_1_11],[C39_10_11],[C39_11_11],[C39_12_11],[C39_13_11],[C39_14_11],[C39_15_11],[C39_16_11],[C39_2_11],[C39_3_11],[C39_4_11],[C39_5_11],[C39_6_11],[C39_7_11],[C39_8_11],[C39_9_11],[visit_1_G12],[visit_2_G12a],[visit_2_G12c],[V_S9],[V_S10],[V_S11],[V_S12],[C38_2ndEquipment_11],[C7_9_a],[C7_9_b],[C7_9_c],[C7_9_d],[C7_9_e],[C7_9_f],[C7_9_g],[C7_9_h],[C7_9_i],[C7_9_j],[C7_9_k],[C7_9_l],[C7_9_m],[C7_9_n],[C7_9_o],[C7_9_p],[C7_9_q],[C7_9_r],[C7_9_s],[C29_10_a],[C29_10_b],[C29_10_c],[C29_10_d],[C29_10_e],[C29_10_f],[C29_10_g],[C29_10_i],[C29_10_h],[C30_10_a],[C30_10_b],[C30_10_c],[C30_10_d],[C30_10_e],[C30_10_f],[C30_10_g],[C30_10_h],[C30_10_i],[C30_10_J],[C30_10_k],[C30_10_l],[C30_10_m],[C30_10_n],[C30_10_o],[C30_10_p],[C30_10_q],[C30_10_r],[C30_10_s],[visit_2_a],[visit_2_b],[visit_2_c],[visit_2_d],[i3_12],[i4_12],[C26_10_Other]  FROM [C]";
            }
            else if (code == "QCR")
            {
                logFile.log("Downloading QCR", Session["username"].ToString());

                strQuery = "SELECT * from  QCR_listing";
            }
            else if (code == "Main")
            {
                logFile.log("Downloading Main", Session["username"].ToString());

                strQuery = "select * from [DownloadMain]";
                //  strQuery = "
            }
            else if (code == "Orig_A")
            {
                logFile.log("Downloading A original", Session["username"].ToString());

                strQuery = "select [KEY_],[PARENT_KEY],[_URI],[A1_1],'A2_1' as [A2_1],[A4_1],[A5_1],[A6_1],[A6a_other_1],[A6c_1]," +
                               "CASE when[A6d_1] is not null then 1 else 0 end as A6d_1" +
                            ",[A6e_1],[A7_1],[A8_1],[A8_PolygamousWives],[A6a_1_a],[A6a_1_b],[A6a_1_c],[A6a_1_d],[A6a_1_e],[A6a_1_f],[A6a_1_g],[A6a_1_h]," +
                            "[A6a_1_i],[A6b_1_a],[A6b_1_b],[A6b_1_c],[A6b_1_d],[A6b_1_e],[A6b_1_f],[A6b_1_g],[A6b_1_h],[A6b_1_i],[Logical_Check],A8B,[A6f_1_1],[A6f_1_2],[A6f_1_3],[A6f_1_4],[A6f_1_5],[A6f_1_6],[A6f_1_7],[A6f_1_8],[A6g_1_1],[A6g_1_2],[A6g_1_3],[A6g_1_4],[A6g_1_5],[A6g_1_6],[A6g_1_7],[A6g_1_8],[Comments_Logical],[Telephone],[Comments_Telephone],[Field],[Comments_Field]" +
                             "FROM [PART_1_A_original]";
            }
            else if (code == "Orig_A1")
            {
                logFile.log("Downloading original A1", Session["username"].ToString());

                strQuery = "SELECT * from  PART_1_A1_original";
            }
            else if (code == "Orig_B")
            {
                logFile.log("Downloading original B", Session["username"].ToString());

                strQuery = "SELECT * from  PART_1_B_original";
            }
            else if (code == "Orig_C")
            {
                logFile.log("Downloading original C", Session["username"].ToString());

                strQuery = "SELECT  [KEY_],[PARENT_KEY],[_URI],[mChildAge],'mChildName' as mChildName ,[C0_9],[C0A_9_59],[C0B_9_59],[C1_9],[C10_9],[C10_A_9],[C10_A_other_9],[C11_9],[C12_9],[C13_9],[C13_9_other],[C14_9],[C15_9],[C16_9],[C17_10],[C19_9],[C2_9],[C20a_10],[C20b_10],[C21a_10],[C21b_10],[C22_10],[C23Ma_10],[C23Mb_10],[C23Mc_10],[C23Md_10],[C23Me_10],[C23Mf_10],[C23Mg_10],[C23Oa_10],[C23Ob_10],[C23Oc_10],[C23Od_10],[C23Oe_10],[C23Of_10],[C23Og_10],[C23_total],[C24_10A_a],[C24_10A_b],[C24_10A_c],[C24_10A_d],[C24_10B_a],[C24_10B_b],[C24_10B_c],[C24_10B_d],[C24_10B],[i2_12_a],[i2_12_b],[i2_12_c],[i2_12_d],[i2_12_e],[C25_10],[C25_10_other],[C26_10],[C27_10_other],[C27_C27_10],[C28_10],[C28A_10],[C29_10_other],[C3_9],[C3_A_9],[C3_A_Other_9],[C31_10],[C33_A_11],[C34_A_11],[C34_C34_11],[C35_11],[C36_11],[C37_11],[C39_Other_11],[C4_9],[C40_11_1],[C40_11_2],[C40_11_3],[C40_11_4],[C40_11_5],[C40_11_6],[C40_11_OTHER],[C44_12],[C5_9],[C6_9],[C7_9_other],[C8_9],[C9_9],[C9_9_OTHER],[visit_1],[visit_2_date],[visit_2_time],[C18a_10],[C18b_10],[C18c_10],[C18d_10],[C18e_10],[C18f_10],[C32_10],[C32_a_1_10],[C32_a_2_10],[C32_a_3_10],[C32_a_4_10],[C32_a_5_10],[C32_a_6_10],[C32_a_7_10],[C32_a_8_10],[C32_a_9_10],[C41_1_12],[C41_2_12],[C41_3_12],[C41_4_12],[C41_5_12],[C41_6_12],[C41_7_12],[C42_01_12],[C42_02_12],[C42_03_12],[C42_04_12],[C42_05_12],[C42_06_12],[C42_07_12],[C42_total],[C43_12A_a],[C43_12A_b],[C43_12A_c],[C43_12A_d],[C43_12B_a],[C43_12B_b],[C43_12B_c],[C43_12B_d],[C33_1_11],[C33_10_11],[C33_11_11],[C33_12_11],[C33_2_11],[C33_3_11],[C33_4_11],[C33_5_11],[C33_6_11],[C33_7_11],[C33_8_11],[C33_9_11],[C38_1_11],[C38_2_11],[C38_2_11_2nd],[C38_3_11],[C38_3_11_2nd],[C38_3a_11],[C38_3a_11_2nd],[C38_3b_11],[C38_3b_11_2nd],[C38_3c_11],[C38_3c_11_2nd],[C38_3d_11],[C38_3d_11_2nd],[C38_3e_11],[C38_3e_11_2nd],[C39_1_11],[C39_10_11],[C39_11_11],[C39_12_11],[C39_13_11],[C39_14_11],[C39_15_11],[C39_16_11],[C39_2_11],[C39_3_11],[C39_4_11],[C39_5_11],[C39_6_11],[C39_7_11],[C39_8_11],[C39_9_11],[visit_1_G12],[visit_2_G12a],[visit_2_G12c],[V_S9],[V_S10],[V_S11],[V_S12],[C38_2ndEquipment_11],[C7_9_a],[C7_9_b],[C7_9_c],[C7_9_d],[C7_9_e],[C7_9_f],[C7_9_g],[C7_9_h],[C7_9_i],[C7_9_j],[C7_9_k],[C7_9_l],[C7_9_m],[C7_9_n],[C7_9_o],[C7_9_p],[C7_9_q],[C7_9_r],[C7_9_s],[C7_9_t],[C29_10_a],[C29_10_b],[C29_10_c],[C29_10_d],[C29_10_e],[C29_10_f],[C29_10_g],[C29_10_i],[C29_10_h],[C30_10_a],[C30_10_b],[C30_10_c],[C30_10_d],[C30_10_e],[C30_10_f],[C30_10_g],[C30_10_h],[C30_10_i],[C30_10_J],[C30_10_k],[C30_10_l],[C30_10_m],[C30_10_n],[C30_10_o],[C30_10_p],[C30_10_q],[C30_10_r],[C30_10_s],[visit_2_a],[visit_2_b],[visit_2_c],[visit_2_d],[i3_12],[i4_12],[C26_10_Other]  FROM [C_original]";
            }
            else if (code == "Orig_Main")
            {
                logFile.log("Downloading original Main", Session["username"].ToString());

                strQuery = "select * from [DownloadMain_orig]";
                //  strQuery = "
            }
            else if (code == "Monitoring")
            {
                logFile.log("Downloading Monitoring", Session["username"].ToString());

                strQuery = "Exec [DownloadMonitoring]";
            }
            else if (code == "Observer")
            {
                logFile.log("Downloading Observer", Session["username"].ToString());

                strQuery = "Exec [DownloadObserver]";
            }
            else if (code == "IndustriesTextReview")
            {
                logFile.log("Downloading industrues Text review", Session["username"].ToString());

                strQuery = "select * from  Industries_Text_Review";
            }
            else if (code == "ProfessionTextReview")
            {
                logFile.log("Downloading professional Text review", Session["username"].ToString());

                strQuery = "select * from  Occupation_Text_Review";
            }
            else if (code == "ToolsTextReview")
            {
                logFile.log("Downloading tools Text review", Session["username"].ToString());

                strQuery = "select * from  Tools_Text_Review";
            }
            else if (code == "SummaryReport")
            {
                logFile.log("Downloading SummaryR", Session["username"].ToString());

                strQuery = "select * from  SummaryR";
            }
            else if (code == "HHSummary")
            {
                logFile.log("Downloading Houshold Summary", Session["username"].ToString());

                strQuery = "EXEC [Report_HH_Size]";
            }
            else if (code == "OthersFieldSummary")
            {
                logFile.log("Downloading others field summary", Session["username"].ToString());

                strQuery = "select * from  Others_Percentage_FieldWise";
            }
            else if (code == "OthersSummary")
            {
                logFile.log("Downloading others summary", Session["username"].ToString());

                strQuery = "exec Others_NumberPerQues";
            }
            else if (code == "MistakesCount")
            {
                logFile.log("Downloading Mistake count", Session["username"].ToString());

                strQuery = "select * from  Enumerator_Mistakes_Count";
            }
            else if (code == "MistakeOutlier")
            {
                logFile.log("Downloading Mistake outlier", Session["username"].ToString());

                strQuery = "exec  [Report_Outlier]";
            }
            else if (code == "OthersCodes")
            {
                logFile.log("Downloading Others Code", Session["username"].ToString());

                strQuery = "exec  [Report_Others]";
            }
            else if (code == "AcceptedCodesInd")
            {
                logFile.log("Downloading Accepted Codes Industries", Session["username"].ToString());

                strQuery = "SELECT * FROM [All_Accepted_Industries]";
            }
            else if (code == "AcceptedCodesOcc")
            {
                logFile.log("Downloading Accepted Codes Professionals", Session["username"].ToString());

                strQuery = "SELECT * FROM [All_Accepted_Occupations]";
            }
            else if (code == "AcceptedCodesTools")
            {
                logFile.log("Downloading Accepted Codes Tools", Session["username"].ToString());

                strQuery = "SELECT * FROM [All_Accepted_Tools]";
            } //Deskmonitor_correction
            else if (code == "Deskmonitor_correction")
            {
                logFile.log("Downloading Deskmonitors correction", Session["username"].ToString());

                strQuery = "SELECT * FROM [Corrections Of DeskMonitor]";
            }
            else if (code == "GPS_Collected")
            {
                logFile.log("Downloading GPS collected", Session["username"].ToString());

                strQuery = @"With Table1 As
                            (
                            SELECT DISTRICT,CLUSTER_CODE, LISTER_NAME

                            ,count(distinct(STRUCTURE_NO)) AS TOTAL_STRUCTURES,count(distinct(RECORDGPS_FIRST_LAT)) as GPS_COLLECTED
                            ,count(distinct(RECORDGPS_FIRST_LAT))*100 / count(distinct(STRUCTURE_NO)) AS COVERAGE
                            from Listing_HH_Old
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
            else if (code == "Listing_listers")
            {
                logFile.log("Downloading listing listers", Session["username"].ToString());

                strQuery = @"
                  select cast(A.[_SUBMISSION_DATE] as date) as [_SUBMISSION_DATE],A.CLUSTER_CODE,A.EBCODE, A.LISTER_NAME, A.LISTER_CODE,A.ADDRESS_OF_HOUSEHOLD,A.STRUCTURE_TYPE,'Same_'+cast(B.countt as varchar) as same_group from Listing_HH_Old A,
                  (SELECT distinct(KEY_), cast(COUNT(*) as varchar) + '_' + cast (Row_Number() OVER (Partition By COUNT(*)  order by key_) As varchar) as countt
	              ,cast(COUNT(*) as varchar) +  cast(Row_Number() OVER (Partition By COUNT(*)  order by key_) As varchar) as num
	               FROM [Listing_HH_Old]
	               GROUP BY KEY_) B 
                   where A.KEY_=B.KEY_ 
                   order by cast(num as int)";
            }
            else if (code == "Listing_data" || code == "listing_monitoring")
            {
                logFile.log("Downloading listing data", Session["username"].ToString());

                strQuery = @"SELECT cast([SUBMISSION_DATE] as date) as [_SUBMISSION_DATE]
                           ,[DISTRICT]
	                       ,[TEHSIL]
	                       ,[LOCALITY]
	                       ,[CLUSTER_CODE]
                          ,[EBCODE]
                         ,[LISTER_CODE]
                         ,[LISTER_NAME]
                         ,[STRUCTURE_NO]
                          ,[SR_NO_HOUSEHOLD]
                          ,[STRUCTURE_TYPE]
                          ,[HHMEMBERS_COUNT]
                          ,[CHILD_PRESENT_STATUS]
                          ,[CHILDREN]
                          ,[BOYS_COUNT]
                          ,[GIRLSL_COUNT]
                          FROM [Listing_HH]";
            }
            else if (code == "Template_data")
            {
                logFile.log("Downloading Template data", Session["username"].ToString());

                strQuery = @"select list_name, name, label, HHID, X.CUSTER_CODE, DISTRICT, supervisorcode_sel, tehsil_sel, district_sel, supervisor_sel, ObserverCode_sel
                            ,enumeratorCode_sel, monitorCode_sel, EBCode_sel, Province_sel
                            from
                            (select  distinct(Y.CLUSTER_CODE) as name,'CCode' as list_name, Y.CLUSTER_CODE as label, '' as HHID, '' as CUSTER_CODE,
                            Y.DISTRICT as DISTRICT, X.Supervisor_ID as supervisorcode_sel
                            , '' as tehsil_sel, Y.DISTRICT as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, Y.CLUSTER_CODE as EBCode_sel, '' as Province_sel
                            from Final_Listing_HH as Y inner join Supervisor as X on Y.DISTRICT=X.District_Name


                            union all        
                            select  distinct(DISTRICT) as name,'district_list' as list_name, DISTRICT as label, '' as HHID,'' as CUSTER_CODE, '' as DISTRICT, 
                            '' as supervisorcode_sel, '' as tehsil_sel, '' as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Final_Listing_HH

                            union all        
                            select  Supervisor_Name as name,'Supervisor_list' as list_name, Supervisor_Name as label, '' as HHID, '' as CUSTER_CODE,
                            '' as DISTRICT, Supervisor_ID as supervisorcode_sel
                            , '' as tehsil_sel, '' as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Supervisor

                            union all        select  Supervisor_ID as name,'SupervisorCode_list' as list_name, Supervisor_ID as label, '' as HHID, '' as CUSTER_CODE,
                            '' as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, District_Name as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Supervisor


                            union all        
                            select distinct(LOCALITY) as name,'tehsil_list' as list_name, LOCALITY as label, '' as HHID,'' as CUSTER_CODE,
                             DISTRICT as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, DISTRICT as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Final_Listing_HH

                            union all        select  Code as name,'EnumeratorCode_list' as list_name, code as label, '' as HHID,'' as CUSTER_CODE,
                             '' as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, '' as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Enumerators


                            union all        
                            select  Name as name,'Enumerator_list' as list_name, Name as label, '' as HHID,'' as CUSTER_CODE,
                             '' as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, '' as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , code as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Enumerators

                            union all        
                            select  SR_NO_HOUSEHOLD as name,'HHIDs' as list_name, SR_NO_HOUSEHOLD as label, '' as HHID,CLUSTER_CODE as CUSTER_CODE,
                             DISTRICT as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, DISTRICT as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Final_Listing_HH

                            union all        
                            select  CHILDREN as name,'Childern' as list_name, CHILDREN as label, SR_NO_HOUSEHOLD as HHID,CLUSTER_CODE as CUSTER_CODE,
                             '' as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, '' as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Final_Listing_HH

                            union all        
                            select  LOCALITY_ADDRESS as name,'Addresses' as list_name, LOCALITY_ADDRESS as label, SR_NO_HOUSEHOLD as HHID,CLUSTER_CODE as CUSTER_CODE,
                             '' as DISTRICT, '' as supervisorcode_sel
                            , '' as tehsil_sel, '' as district_sel, '' as supervisor_sel, '' as ObserverCode_sel
                            , '' as enumeratorCode_sel, '' as monitorCode_sel, '' as EBCode_sel, '' as Province_sel
                            from Final_Listing_HH) as X
                ";
            }
            else if (code == "Listing_Relist_data")
            {
                logFile.log("Downloading Relisting Data", Session["username"].ToString());

                strQuery = "SELECT * from  Download_relisting";
            }

            else if (code == "ReListing_listers_address")
            {
                logFile.log("Downloading Relisting listers", Session["username"].ToString());

                strQuery = @"
                select cast(A.[_SUBMISSION_DATE] as date) as [_SUBMISSION_DATE],A.CLUSTER_CODE,A.EBCODE, A.LISTER_NAME, A.LISTER_CODE,A.ADDRESS_OF_HOUSEHOLD,A.STRUCTURE_TYPE,'Same_'+cast(B.countt as varchar) as same_group from Listing_HH A,
                  (SELECT distinct(KEY_), cast(COUNT(*) as varchar) + '_' + cast (Row_Number() OVER (Partition By COUNT(*)  order by key_) As varchar) as countt
             ,cast(COUNT(*) as varchar) +  cast(Row_Number() OVER (Partition By COUNT(*)  order by key_) As varchar) as num
              FROM [Listing_HH]
              GROUP BY KEY_) B
                   where A.KEY_=B.KEY_
                   order by cast(num as int)";
            }

            else if (code == "ReListing_GPS_Collected")
            {
                logFile.log("Downloading Relisting GPS collected", Session["username"].ToString());

                strQuery = @" With Table1 As
                            (
                            SELECT DISTRICT,CLUSTER_CODE, LISTER_NAME

                            ,count(distinct(STRUCTURE_NO)) AS TOTAL_STRUCTURES,count(distinct(RECORDGPS_FIRST_LAT)) as GPS_COLLECTED
                            ,count(distinct(RECORDGPS_FIRST_LAT))*100 / nullif(count(distinct(STRUCTURE_NO)),0) AS COVERAGE
                            from Listing_HH
                            group by DISTRICT,CLUSTER_CODE,LISTER_NAME

                            )
                            ,TABLE2 AS
                            (select DISTRICT,CLUSTER_CODE,LISTER_NAME,TOTAL_STRUCTURES,GPS_COLLECTED,CONCAT(COVERAGE,'%') AS COVERAGE
                            ,CASE WHEN COVERAGE<=(PERCENTILE_CONT(0.1) WITHIN GROUP (ORDER BY COVERAGE)  
                            OVER (PARTITION BY NULL)) THEN 1 ELSE 0 END AS [FLAG BELOW 10TH PERCENTILE]
                            from Table1)

                            SELECT TABLE2.* FROM TABLE2 INNER JOIN  Table1 ON Table1.DISTRICT=TABLE2.DISTRICT AND Table1.CLUSTER_CODE=TABLE2.CLUSTER_CODE and table1.Lister_name=TABLE2.Lister_name
                            AND [FLAG BELOW 10TH PERCENTILE]=1
                            ORDER BY TABLE1.COVERAGE";
            }







        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            //else if (e.CommandName == "Excel")
            //{
            //    //Determine the RowIndex of the Row whose LinkButton was clicked.
            //    int rowIndex = Convert.ToInt32(e.CommandArgument);

            //    //Reference the GridView Row.
            //    GridViewRow row = GridView1.Rows[rowIndex];

            //    //Fetch value of Name.
            //    //string name = (row.FindControl("txtName") as TextBox).Text;

            //    //Fetch value of Code
            //    string code = row.Cells[1].Text;

            //    SQL_Query(code);

            //    ExportToExcel(sender, e);

            //}

            if (e.CommandName == "request")
            {
                //Determine the RowIndex of the Row whose LinkButton was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GridView1.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Code
                string code = row.Cells[1].Text;
                string role = Session["Role"].ToString();
                string Assign = (string)(Session["Assign"]);
                // SQL_Query(code);

                //string code = row.Cells[1].Text;
                if (code == "listing_monitoring")
                {
                    if (role.Contains("listmonitor"))
                    {
                        Regex regex = new Regex("[1-6]");

                        if (Assign.Contains("6"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Request for Download access initiated already')", true);

                        }
                        else if (Assign.Contains("0") && role.Contains("listmonitor"))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Download limit has reached')", true);

                            var imageButton = (ImageButton)row.FindControl("ImageButton1");
                            imageButton.Visible = false;

                            row.BackColor = System.Drawing.Color.Red;
                        }
                        else if (!regex.IsMatch(Assign) && role.Contains("listmonitor"))
                        {



                            var imageButton = (ImageButton)row.FindControl("ImageButton1");
                            imageButton.Visible = false;
                            var btn = (Button)row.FindControl("request_btn");
                            btn.Visible = true;
                            btn.Text = "Requested";
                            row.BackColor = System.Drawing.Color.Yellow;
                            string updateSql =
                                               @"UPDATE users
                                                SET Assign = @assign
                                                WHERE Role = @Id";

                            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                            {
                                connection.Open();
                                var command = new SqlCommand(updateSql, connection);
                                var args = command.Parameters;

                                args.Add("@assign", "Allow,6");
                                args.Add("@Id", "listmonitor");

                                command.ExecuteNonQuery();
                                Session["Assign"] = "Allow,6";
                            }
                        }


                        else
                        {
                            var imageButton = (ImageButton)row.FindControl("ImageButton1");
                            imageButton.Visible = false;
                            var btn = (Button)row.FindControl("request_btn");
                            btn.Visible = true;
                            row.BackColor = System.Drawing.Color.Yellow;
                        }

                    }
                    else if (role.Contains("listadmin"))
                    {
                        string updateSql =
                                                @"UPDATE users
                                                SET Assign = @assign
                                                WHERE Role = @Id";

                        Regex regex1 = new Regex("[1-5]");
                        SqlCommand cmdd = new SqlCommand("select Assign from users where Role = 'listmonitor'");
                        DataTable dt = GetData(cmdd);
                        string bol = dt.Rows[0].ItemArray[0].ToString();
                        var btn = (Button)row.FindControl("request_btn");

                        if (btn.Text.Contains("Revoke"))
                        {
                            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                            {
                                connection.Open();
                                var command = new SqlCommand(updateSql, connection);
                                var args = command.Parameters;

                                args.Add("@assign", "Allow");
                                args.Add("@Id", "listmonitor");

                                command.ExecuteNonQuery();

                            }
                            btn.Visible = false;

                        }
                        if (bol.Contains("6"))
                        {
                            //SqlCommand cmd = new SqlCommand("update users set Assign = ' where Role = ''");
                            //DataTable dtr = GetData(cmd);


                            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                            {
                                connection.Open();
                                var command = new SqlCommand(updateSql, connection);
                                var args = command.Parameters;

                                args.Add("@assign", "Allow,5");
                                args.Add("@Id", "listmonitor");

                                command.ExecuteNonQuery();

                            }
                            //cmd.CommandText = "update users set Assign = 'Allow,1' where Role = 'listmonitor'";//UPDATE Summarized_Report SET Assigned = @assigned Where Report_Name = @rn and key_ = @key_";



                            // var btn = (Button)row.FindControl("request_btn");
                            btn.Text = "Revoke";
                        }


                        else if (regex1.IsMatch(bol))
                        {

                            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                            {
                                connection.Open();
                                var command = new SqlCommand(updateSql, connection);
                                var args = command.Parameters;

                                args.Add("@assign", "Allow");
                                args.Add("@Id", "listmonitor");

                                command.ExecuteNonQuery();

                            }
                            //var btn = (Button)row.FindControl("request_btn");
                            btn.Visible = false;
                        }


                    }

                    //imageButton.ImageUrl = "";
                    //imageButton.ImageUrl =Page.ResolveUrl("~/assets/img/CSV.png");
                    // imageButton.DataBind();
                    //row.Cells[0].Text = imageButton.ImageUrl;
                    // set the path of image

                    //  row.Cells[2].Attributes.Add("src", "../assets/img/CSV_1.gif");
                }

            }

            if (e.CommandName == "CSV")
            {
                string role = Session["Role"].ToString();
                string Assign = (string)(Session["Assign"]);
                // string 
                //Determine the RowIndex of the Row whose LinkButton was clicked.
                // int rowIndex = Convert.ToInt32(e.Row.RowIndex);

                //Reference the GridView Row.

                //if()
                //Determine the RowIndex of the Row whose LinkButton was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GridView1.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Code
                string code = row.Cells[1].Text;

                SQL_Query(code);
                if (role.Contains("listmonitor") && code.Contains("listing_monitoring"))
                {
                    Regex reg = new Regex("[1-5]");
                    if (reg.IsMatch(Assign))
                    {
                        Response.Redirect("Maps.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Download limit reached.')", true);
                    }

                }
                else if (role.Contains("listadmin") && code.Contains("listing_monitoring"))
                {

                    Response.Redirect("Maps.aspx");
                }
                else
                    ExportToCSV(code, sender, e);


            }


        }



        private DataTable GetData(SqlCommand cmd)
        {
            DataTable dt = new DataTable();


            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            cmd.CommandTimeout = 0;
            try
            {
                con.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        //protected void ExportToWord(object sender, EventArgs e)
        //{
        //    //Get the data from database into datatable

        //    SqlCommand cmd = new SqlCommand(strQuery);
        //    DataTable dt = GetData(cmd);

        //    //Create a dummy GridView
        //    GridView GridView5 = new GridView();
        //    GridView5.AllowPaging = false;
        //    GridView5.DataSource = dt;
        //    GridView5.DataBind();

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition",
        //     "attachment;filename=" + code + " - " + DateTime.Now.ToString("o").Replace(':', '-') + ".xls");
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.ms-excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    for (int i = 0; i < GridView5.Rows.Count; i++)
        //    {
        //        //Apply text style to each Row
        //        GridView5.Rows[i].Attributes.Add("class", "textmode");
        //    }
        //    GridView5.RenderControl(hw);

        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.Close();
        //    Response.End();
        //}

        //protected void ExportToExcel(object sender, EventArgs e)
        //{
        //    //Get the data from database into datatable

        //    SqlCommand cmd = new SqlCommand(strQuery);
        //    DataTable dt = GetData(cmd);

        //    //Create a dummy GridView
        //    GridView GridView5 = new GridView();
        //    GridView5.AllowPaging = false;
        //    GridView5.DataSource = dt;
        //    GridView5.DataBind();

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition",
        //     "attachment;filename=" + code + " - " + DateTime.Now.ToString("o").Replace(':', '-') + ".xls");
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.ms-excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    for (int i = 0; i < GridView5.Rows.Count; i++)
        //    {
        //        //Apply text style to each Row
        //        GridView5.Rows[i].Attributes.Add("class", "textmode");
        //    }
        //    GridView5.RenderControl(hw);

        //    //Div_Download.Attributes["display"] = "block";  // For Show
        //    string script = "window.onload = function() { toggle_visibility(); };";
        //    ClientScript.RegisterStartupScript(this.GetType(), "UpdateTime", "script", true);
        //    //style to format numbers to string
        //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //    Response.Write(style);
        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();





        //}



        //protected void ExportToPDF(object sender, EventArgs e)
        //{


        //    //Get the data from database into datatable

        //    SqlCommand cmd = new SqlCommand(strQuery);
        //    DataTable dt = GetData(cmd);

        //    //Create a dummy GridView
        //    GridView GridView5 = new GridView();
        //    GridView5.AllowPaging = false;
        //    GridView5.DataSource = dt;
        //    GridView5.DataBind();

        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition",
        //        "attachment;filename=" + code +" - " + DateTime.Now.ToString("o").Replace(':', '-') + ".pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    GridView1.RenderControl(hw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    Response.Write(pdfDoc);
        //    Response.End();          


        //}
        protected void ExportToCSV(string filename, object sender, EventArgs e)
        {
            //Get the data from database into datatable

            SqlCommand cmd = new SqlCommand(strQuery);


            DataTable dt = GetData(cmd);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=" + filename + " - " + DateTime.Now.ToString("o").Replace(':', '-') + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/text";


            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                //add separator
                sb.Append(dt.Columns[k].ColumnName + ',');
            }
            //append new line
            sb.Append("\r\n");
            Response.Output.Write(sb.ToString());

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StringBuilder row = new StringBuilder();
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    string myString = dt.Rows[i][k].ToString();
                    myString = myString.Replace(System.Environment.NewLine, " ");
                    myString = Regex.Replace(myString, @"<(.|\n)*?>", " ");
                    myString = myString.Replace("\r", " ");
                    myString = myString.Replace("/g", " ");
                    myString = myString.Replace(",", " ");
                    myString = myString.Replace("\n", " ");
                    myString = Regex.Replace(myString, @"\t|\n|\r", " ");
                    row.Append(myString + ',');
                }
                //append new line
                row.Append("\r\n");

                Response.Output.Write(row.ToString());
                //  Response.Flush();
            }
            // sb.Append(row);
            // Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string role = Session["Role"].ToString();
            string Assign = (string)(Session["Assign"]);

            if (GridView1.Rows.Count > 0)//if (role == "BOS")
            {
                //Determine the RowIndex of the Row whose LinkButton was clicked.
                int rowIndex = Convert.ToInt32(e.Row.RowIndex);

                //Reference the GridView Row.
                if (rowIndex > 0)
                {
                    GridViewRow row = GridView1.Rows[rowIndex - 1];


                    //Fetch value of Code
                    string code = row.Cells[1].Text;
                    if (code == "listing_monitoring")
                    {
                        if (role.Contains("listmonitor"))
                        {

                            if (Assign.Contains("0"))
                            {
                                var imageButton = (ImageButton)row.FindControl("ImageButton1");
                                imageButton.Visible = false;
                                var btn = (Button)row.FindControl("request_btn");
                                btn.Text = "Limit Exceeded";
                                btn.Visible = true;
                                row.BackColor = System.Drawing.Color.Red;
                            }
                            else if (Assign.Contains("1") || Assign.Contains("2") || Assign.Contains("3") || Assign.Contains("4") || Assign.Contains("5"))
                            {
                                row.BackColor = System.Drawing.Color.Green;

                            }
                            else if (Assign.Contains("6"))
                            {
                                var imageButton = (ImageButton)row.FindControl("ImageButton1");
                                imageButton.Visible = false;
                                var btn = (Button)row.FindControl("request_btn");
                                btn.Text = "Requested";
                                btn.Visible = true;
                                row.BackColor = System.Drawing.Color.Yellow;
                            }
                            else
                            {
                                var imageButton = (ImageButton)row.FindControl("ImageButton1");
                                imageButton.Visible = false;
                                var btn = (Button)row.FindControl("request_btn");
                                btn.Visible = true;
                                row.BackColor = System.Drawing.Color.Yellow;
                            }
                        }
                        else if (role.Contains("listadmin"))
                        {
                            Regex regex2 = new Regex("[0-5]");
                            SqlCommand cmdd = new SqlCommand("select Assign from users where Role = 'listmonitor'");
                            DataTable dt = GetData(cmdd);
                            string bol = dt.Rows[0].ItemArray[0].ToString();
                            if (bol.Contains("6"))
                            {
                                var btn = (Button)row.FindControl("request_btn");
                                btn.Visible = true;
                                btn.Text = "Allow";
                                row.BackColor = System.Drawing.Color.Yellow;
                            }
                            else if (regex2.IsMatch(bol))
                            {
                                var btn = (Button)row.FindControl("request_btn");
                                btn.Visible = true;
                                btn.Text = "Revoke";
                                row.BackColor = System.Drawing.Color.Yellow;
                            }
                        }
                    }
                }
            }
        }



    }


}