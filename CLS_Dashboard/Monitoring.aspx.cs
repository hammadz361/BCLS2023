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
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;

namespace CLS_Dashboard
{
    public partial class Monitoring : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        public static string strQuery = "";
        public static string active1 = "";
        public static string active2 = "";
        public static string active3 = "";
        public static string active_id = "";
        public string sessionUserId = "";
        public string sessionUserRole = "";
        public string district = "";
        public  String[] correction_field = new String[14];
        public  int[] correction_type = new int[15];
        public  DataTable final_data = new DataTable();
        public static string proc_name = "";
        public string HH_Key = "";
        public  string report_ID = "";
        public  string gotpid = "";
        public static string Data_validator = "0";
        public static string Supervisor = "0";
        public static DataTable data_verification = new DataTable();
        public static DataTable data_description = new DataTable();
        public static DataTable error_data = new DataTable();
        public static int correction_index = 0;
        public static DataTable ques_table = new DataTable();
        public static SqlCommand query_command = new SqlCommand();
     
        protected void Page_Load(object sender, EventArgs e)
        {
            correction_field = new String[14];
            correction_type = new int[15];
            string gotkey = "";
            //string gotpid = "";
            string report_id = "";
            string ulTagOpen = "<ul style='margin-left: 0%; color:black;  padding-left:1em; font-size:smaller; text-align: justify; text-justify:inter-word; width:90%;  list-style-type:disc ;color:black'>";
            string ulTagClose = "</ul>";
            string liYellowTagOpen = "<li style='margin-top:10px; '><span style=' background-color:Yellow;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liYellowTagClose = "</li>";
            string liOrangeTagOpen = "<li style='margin-top:10px; '><span style=' background-color:Orange;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liOrangeTagClose = "</li>";
            string liWhiteTagOpen = "<li style='margin-top:10px; '><span style=' background-color:#999999;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liWhiteTagClose = "</li>";
            string liGreenTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:YellowGreen;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liGreenTagClose = "</li>";
            string liBlueTagOpen = "<li style='margin-top: 10px;'><span style=' background-color:LightBlue;padding: 2px 8px; border: 1px solid #000; display: inline; float: left;margin-top: 5px;'></span>&nbsp;";
            string liBlueTagClose = "</li>";
            lblLegendsText.Visible = true;
            lblLegendsText.Text = ulTagOpen + liWhiteTagOpen + "Query has not been revised" + liWhiteTagClose + liGreenTagOpen + "query has been resolved" + liGreenTagClose + liOrangeTagOpen + "query has been closed" + liOrangeTagClose + liYellowTagOpen + "query has been assigned to the data validator" + liYellowTagClose + liBlueTagOpen + "query has been assigned to the Field Monitor " + liGreenTagClose + ulTagClose;


            DataTable ques_table = new DataTable();
            string q = "SELECT [List_Name],[DB_Value],[Label] ,[Questions_Variable] FROM [Data_Verification] ";

            try
            {
                data_verification = GetData(q);
            }
            catch (Exception ex)
            {
                throw;
            }
            
            
            
            string q1 = "SELECT [question_variable] ,[description] FROM [data_description] ";
            try
            {
                data_description = GetData(q1);
            }
            catch (Exception ex)
            {
                throw;
            }
           
           

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StaticHeaderRow()", true);
            performance();

            try
            {
                report_id = Request.QueryString["key"];
                report_ID = report_id;
                DataTable dt = new DataTable();
                string query = "SELECT key_ FROM Summarized_Report WHERE Record_Id='" + report_id + "'";
                try
                {
                    dt = GetData(query);
                }
                catch (Exception ex)
                {
                    throw;
                }
             
               
                gotkey = dt.Rows[0]["key_"].ToString();
                HH_Key = gotkey;
                string Pa = Request.QueryString["pid"];
                //write_to_server(null, "Action:pageload, Key:" + gotkey + ",report_id :" + report_id);
                if (Pa.Contains(","))
                {
                    while (Pa.Contains(' '))
                    {
                        Pa = Pa.Remove(Pa.IndexOf(' '), 1);
                    }

                    String[] mul = Pa.Split(',');

                    for (int i = 0; i < mul.Length; i++)
                    {
                        if (i == 0)
                        {
                            gotpid = mul[0].Remove(mul[0].IndexOf('Q'), 1);
                        }
                        else
                        {
                            string Openpage = "function Open() { window.location.href('~Monitoring.aspx?key='" + gotkey + "&pid=" + mul[i] + ", '_blank'); }";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Openpage", Openpage, true);
                        }
                    }

                }
                else
                {
                    gotpid = Pa.Remove(Pa.IndexOf('Q'), 1);
                }
                string q11 = "SELECT [Section1] ,[Section2] ,[Section3],[Section5],[Section7],[Section8],[Section9] ,[Section10] FROM [dbo].[SummaryR] where Record_Id = " + report_id;
                try
                {
                    ques_table = GetData(q11);
                }
                catch (Exception ex)
                {
                    throw;
                }
              
               
                // set_buttons(ques_table, P);
                if (ques_table.Rows.Count > 0)
                {
                    //write_to_server(null, "Action:SettingButtons, Key:" + HH_Key);

                    set_buttons(ques_table, Pa);

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Response.Redirect("surveyProgress.aspx?id=1004_Mistakes_Summarized_Overall&name=Mistakes%20Summarized%20Report");
            }


            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else if ((string)(Session["Role"]) != "DeskMonitor" && (string)(Session["Role"]) != "DataValidator" && (string)(Session["Role"]) != "Supervisor")
            {
                Response.Redirect("surveyProgress.aspx?id=1004_Mistakes_Summarized_Overall&name=Mistakes%20Summarized%20Report");
            }

            else
            {
                sessionUserId = Session["UserID"] as string;
                sessionUserRole = Session["Role"] as string;
                Session["lblHeadingName"] = "<b>" + Session["reg"] + "</b>-Monitoring <br> Role : " + sessionUserRole;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "StaticHeaderRow()", true);


                if (!IsPostBack)
                {
                    active1 = "active";
                    active2 = "";

                    //Div_Hide.Style.Add("display", "none");
                    //GetDownloadData();
                    // SQL_Query();
                    LoadDataDrop(gotkey, gotpid);

                }
            }
          
        }
        

        private void LoadDataDrop(String key, String P)
        {
            proc_name = "";
 DataTable final_table = new DataTable();
            strQuery = @" SELECT 'Section'+[Section]+'_'+[Question_Name]+'_' +[Error_Name] as proc_name, Assigned ,Comments_Monitor ,Comments_Validator ,Comments_Supervisor , (case when Logical_Check IN ('0','1') then (select Monitor_Name from DeskMonitors where Monitor_ID in (select deskmonitor_id from Supervisor where Supervisor_ID in (select G3_Supervisor_1_code from Main where KEY_ ='" + key + @"'))) when Telephone IN ('0','1') then ( select Operator_Name from Operators where Operator_ID in (select Operator_ID from DeskMonitors where Monitor_ID in  (select deskmonitor_id from Supervisor where Supervisor_ID in (select G3_Supervisor_1_code from Main where KEY_ ='" + key + @"'))) group by Operator_Name )  when Field IN ('0','1') then ( select G3_Supervisor_1 from Main where KEY_ = '" + key + @"') Else NULL END) [Closed Or Reviewed By] FROM [dbo].[Summarized_Report] where Report_Name = '" + P + "' AND Key_ = '" + key + "'";
            //strQuery = "SELECT 'Section'+[Section]+'_'+[Question_Name]+'_' +[Error_Name] as proc_name, Assigned ,Comments_Monitor ,Comments_Validator ,Comments_Supervisor , (case when Logical_Check IN ('0','1') then 'Monitor' when Telephone IN ('0','1') then 'Validator' when Field IN ('0','1') then 'Supervisor' Else NULL END) [Closed Or Reviewed By] FROM [dbo].[Summarized_Report] where Report_Name = '" + P + "' AND Key_ = '" + key + "'";
            DataTable tb = new DataTable();
            
            try
            {
                tb = GetData(strQuery);
            }
            catch (Exception ex)
            {
                throw;
            }
           
               
            proc_name = tb.Rows[0]["proc_name"].ToString();
            string Assigned = tb.Rows[0]["Assigned"].ToString();
            if(proc_name=="")
            {
                //write_to_server(null, "Action:ProcNameInvalidReloading, ProcName:" + proc_name + ", ReportName :" + P);

                Response.Redirect(Request.RawUrl);
                
            }
            //write_to_server(null, "Action:fetchingProcedure&Assigned, ProcName:" + proc_name+", assigned :"+Assigned);


            table_heading.InnerHtml = proc_name; //"Q" + P + ": [" + proc_name + "]";
            DataSet table = new DataSet();
            DataTable temp_table = tb.Copy();
            temp_table.Columns.RemoveAt(0);
            temp_table.Columns.RemoveAt(0);

            try {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                using (var cmd = new SqlCommand(proc_name, con))
                using (var da = new SqlDataAdapter(cmd))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Key_", key));
                    //write_to_server(cmd, "");

                    da.Fill(table);

                    table.Tables[0].Columns.Remove("Comments_Monitors");
                    table.Tables[0].Columns.Remove("Comments_Validator");
                    table.Tables[0].Columns.Remove("Comments_Field"); //Closed Or Reviewed By
                    //table.Tables[0].Merge(temp_table, true);
                    table.Tables[0].Columns.Remove("Closed Or Reviewed By");
                    final_table = merging_tables(table.Tables[0], temp_table);
                    error_data = final_table.Copy();
                    GridView1.DataSource = table.Tables[0];

                    //Console.WriteLine(table);
                    try
                    {
                        hid.InnerHtml = "HouseHold ID : " + final_table.Rows[0]["household_id"].ToString();
                        add.InnerHtml = "Address : " + final_table.Rows[0]["address_of_household"].ToString();
                        clus.InnerHtml = "Cluster Code : " + final_table.Rows[0]["cluster_code"].ToString();

                        sup.InnerHtml = "Field Monitor Name : " + final_table.Rows[0]["g3_supervisor_1"].ToString();
                        sub_date.InnerHtml = "Submission Date : " + final_table.Rows[0]["SubmissionDate"].ToString();
                        i16.InnerHtml = "i6_12 : " + final_table.Rows[0]["i6_12"].ToString();
                        area_type.InnerHtml = "Area Type : " + final_table.Rows[0]["area_type"].ToString();
                        // intr.InnerHtml = "Enum Code : " + table.Tables[0].Rows[0]["G2_interviewer_1_code"].ToString();
                        // write_to_server(null, "Action:D, Key:" + HH_Key);

                    }
                    catch (Exception)
                    {
                        //write_to_server(null, "Action:ReportingSuccess, Key:" + HH_Key + ",ProcName:" + proc_name);

                        next_click.Visible = true;
                        next_click.Enabled = true;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error Has been Corrected...')", true);
                        update_flag(HH_Key, false, P);
                        roster_details.Visible = false;
                        assign_heading.InnerText = "Correction has been made already, Correct other errors..";
                        content_correction.Visible = false;
                        Button1191.Visible = true;
                        already_corrected.Visible = true;
                        correction_div.Visible = false;
                        asg_comment_box.Visible = false;
                        res.Visible = false;
                        asg_comment_lb.Visible = false;
                        dv.Visible = false;
                        sp.Visible = false;
                        strQuery = "SELECT [Data_Validator],[Supervisor], [Flag], [SQL_Query_Description]  FROM [dbo].[Monitoring_Queries] where SQL_Query = '" + proc_name + "'";
                       
                        try
                        {
                            tb = GetData(strQuery);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                      
                        Data_validator = tb.Rows[0]["Data_Validator"].ToString();
                        table_heading.Attributes.Remove("title");
                        table_heading.Attributes.Add("title", tb.Rows[0]["SQL_Query_Description"].ToString());
                        Supervisor = tb.Rows[0]["Supervisor"].ToString();
                        string mandatory = tb.Rows[0]["Flag"].ToString();
                        if (mandatory.Contains("1"))
                        {
                            mandatory_flag.InnerText = "( Correction Mandatory )    [  Corrected Already ]";
                        }
                        else
                        {
                            mandatory_flag.InnerText = "( REVISION AND NOT NECESSARILY CORRECTION )    [ Corrected Already ]";
                        }
                        return;


                    }
                    //write_to_server(null, "Action:DisplayData, Key:" + HH_Key + ",ProcName" + proc_name);

                    GridView1.DataBind();

                }


                //Closed Or Reviewed By

                //TODO: 
                strQuery = "SELECT [Data_Validator],[Supervisor], [Flag], [SQL_Query_Description] FROM [dbo].[Monitoring_Queries] where SQL_Query = '" + proc_name + "'";
               
                try
                {
                    tb = GetData(strQuery);
                }
                catch (Exception ex)
                {
                    throw;
                }
              
                table_heading.Attributes.Remove("title");
                table_heading.Attributes.Add("title", tb.Rows[0]["SQL_Query_Description"].ToString());
                string mandatory_ = tb.Rows[0]["Flag"].ToString();
                if (mandatory_.Contains("1"))
                {
                    mandatory_flag.InnerText = "( Correction Mandatory )";
                }
                else
                {
                    mandatory_flag.InnerText = "( REVISION AND NOT NECESSARILY CORRECTION )";
                }
                //write_to_server(null, "Action:CheckingResolvedORNOT, Key:" + HH_Key + ",ProcName" + proc_name);

                if (resovled_or_not(table))
                {
                    //write_to_server(null, "Action:ReporitngSuccess, Key:" + HH_Key + ",ProcName" + proc_name);

                    assign_heading.Visible = false;
                    dv.Visible = false;
                    sp.Visible = false;
                    if (P.Equals("1.1") || P.Equals("49") || P.Equals("6")) update_flag(HH_Key, false, P); //specific for 1.1 as it shows unresolved but solved from other query
                    correction_div.Visible = false;
                    already_corrected.Visible = true;
                    next_click.Visible = true;
                    next_click.Enabled = true;
                    res.Visible = false;
                    asg_comment_box.Visible = false;
                    asg_comment_lb.Visible = false;
                    if (mandatory_.Contains("1"))
                    {
                        mandatory_flag.InnerText = "( Correction Mandatory )    [  Corrected Already ]";
                    }
                    else
                    {
                        mandatory_flag.InnerText = "( REVISION AND NOT NECESSARILY CORRECTION )    [  Corrected Already ]";
                    }
                }
                else
                {
                    // //write_to_server(null, "Action:CheckingResolvedORNOT, Key:" + HH_Key + ",ProcName" + proc_name);

                    Data_validator = tb.Rows[0]["Data_Validator"].ToString();
                    Supervisor = tb.Rows[0]["Supervisor"].ToString();

                    //write_to_server(null, "Action:EnablingAssignmentTabOptions, Key:" + HH_Key + ",ProcName" + proc_name + ",DataValidator:" + Data_validator + ",Supervisor:" + Supervisor);

                    switch (sessionUserRole)
                    {
                        case "DeskMonitor":
                            if (Assigned.Contains("DeskMonitor"))
                            {
                                assign_heading.InnerText = "Assign to : ";
                                assign_heading.Visible = true;
                                if (Data_validator.Contains('1'))
                                {
                                    dv.Visible = true;
                                }
                                if (Supervisor.Contains('1'))
                                {
                                    sp.Visible = true;
                                }
                            }
                            else
                            {
                                assign_heading.InnerText = "Assigned to : " + Assigned;
                                next_click.Visible = true;
                                next_click.Enabled = true;
                                assign_heading.Visible = true;
                                correction_div.Visible = false;
                                asg_comment_lb.Visible = false;
                                asg_comment_box.Visible = false;
                            }
                            break;
                        case "DataValidator":
                            if (Assigned.Contains("DataValidator"))
                            {
                                if (Supervisor.Contains('1'))
                                {
                                    assign_heading.InnerText = "Assign to : ";
                                    assign_heading.Visible = true;
                                    sp.Visible = true;
                                }
                            }
                            else
                            {
                                assign_heading.InnerText = "Assigned to : " + Assigned;
                                assign_heading.Visible = true;
                                correction_div.Visible = false;
                                asg_comment_lb.Visible = false;
                                asg_comment_box.Visible = false;
                            }
                            break;

                        case "Supervisor":
                            if (Assigned.Contains("Supervisor"))
                            {
                                assign_heading.Visible = false;
                                dv.Visible = false;
                                sp.Visible = false;
                                res.Visible = false;
                            }
                            else
                            {
                                assign_heading.InnerText = "Assigned to : " + Assigned;
                                assign_heading.Visible = true;
                                correction_div.Visible = false;
                                asg_comment_lb.Visible = false;
                                asg_comment_box.Visible = false;
                            }

                            break;
                        default:
                            break;
                    }

                }

                if (table.Tables[0].Rows.Count > 0)
                {
                    int i = 0;

                    foreach (DataColumn dc in final_table.Columns)
                    {

                        if (dc.ToString().Contains("Corrected"))
                        {
                            correction_field[i] = dc.ToString();
                            correction_field[i] = correction_field[i].Remove(correction_field[i].IndexOf('C'), 10);
                            i++;

                        }
                    }
                    DataRow dr = final_table.Rows[0];
                    int key_id = 0;
                    DataTable data = final_table.Copy();
                    final_data = final_table.Copy();
                    //data.Columns["Key_"].Unique = true;
                    //data.Rows.Clear();
                    int pos = 0;
                    string[] keys = new string[data.Rows.Count];

                    foreach (DataRow item in final_table.Rows)
                    {
                        keys[pos] = item[1].ToString();
                        if (item["Corrected_" + correction_field[0]].ToString().Length > 1)
                        {

                            key_id = pos;

                        }
                        pos++;


                    }

                    DropDownList key_drop = new DropDownList();
                    String key_dd = "key_drop";
                    key_drop = get_dropdown_box(key_dd);
                    key_drop.DataSource = keys;
                    key_drop.DataBind();
                    list_view(correction_field, 1, keys[0]);
                    key_drop.SelectedIndex = key_id;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error Has been Corrected...')", true);
                    roster_details.Visible = false;
                    assign_heading.InnerText = "Correction has been made already, Correct other errors..";
                    asg_comment_box.Visible = false;
                    asg_comment_lb.Visible = false;
                    already_corrected.Visible = true;
                }

                // set_buttons(ques_table);
            }
            catch (Exception ex) {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Query taking longer need indexing ')", true);
            }
            
        }
        private DataTable merging_tables(DataTable main, DataTable temp_table)
        {
            DataColumn dc1 = temp_table.Columns["Comments_Monitor"];
            DataColumn dc2 = temp_table.Columns["Comments_Validator"];
            DataColumn dc3 = temp_table.Columns["Comments_Supervisor"];
            DataColumn dc4 = temp_table.Columns["Closed Or Reviewed By"];

            main.Columns.Add(dc1.ColumnName, dc1.DataType);
            main.Columns.Add(dc2.ColumnName, dc2.DataType);
            main.Columns.Add(dc3.ColumnName, dc2.DataType);
            main.Columns.Add(dc4.ColumnName, dc2.DataType);
            //if (temp_table.Rows.Count == main.Rows.Count)
            //{
                int counter = temp_table.Rows.Count;
            if (temp_table.Rows.Count > main.Rows.Count)
                counter = main.Rows.Count;

                for (int i = 0; i < counter; i++)
                {
                    main.Rows[i]["Comments_Monitor"] = temp_table.Rows[i]["Comments_Monitor"];
                    main.Rows[i]["Comments_Validator"] = temp_table.Rows[i]["Comments_Validator"];
                    main.Rows[i]["Comments_Supervisor"] = temp_table.Rows[i]["Comments_Supervisor"];
                    main.Rows[i]["Closed Or Reviewed By"] = temp_table.Rows[i]["Closed Or Reviewed By"];
                }
        //}
            return main;
        }
        private void list_view(String[] correction_field, int index, string keys)
        {
            string hh_key = HH_Key;
            int j = 1;

            String list_query = "";
            foreach (var item in correction_field)
            {
                if (item == "" || item == null) ;

                else
                {
                    list_query = "select List_Name as DB_Value, DB_Value as Label from Data_Verification where Questions_Variable = '" + item + "'";
                    if (item == "C3_9" || item == "C10_9" || item == "C6_9")
                        list_query = list_query.Replace("_9", "_9_1017");
                    if (item == "A31_3__SUN_O")
                        list_query = list_query.Replace("A31_3__SUN_O", "A31_3_SUN_O");
                    if (item == "C1_9" || item == "C2_9" || item == "C8_9") //C1_9_59
                        list_query = list_query.Replace("_9", "_9_59");
                    if (item.Contains("C23M") || item.Contains("C23O")) //C1_9_59
                        list_query = "select List_Name as DB_Value, DB_Value as Label from Data_Verification where Questions_Variable = 'C23Mg_10_1017'";
                    if (item.Contains("B29")) //C1_9_59
                        list_query = "select List_Name as DB_Value, DB_Value as Label from Data_Verification where Questions_Variable = 'B29_8'";

                    
                    DataTable dt = new DataTable();
                    
                    try
                    {
                        dt = GetData(list_query);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                   
                    set_label("f" + j, item);
                    string dec = decode_Values(item, "");
                    set_label("l" + j, dec);


                    if (dt.Rows[0]["Label"].Equals("text"))
                    {
                        correction_type[j] = 1;
                        set_text(j, "text", dt.Rows[0]["Label"].ToString());
                    }
                    else if (dt.Rows[0]["DB_Value"].Equals("integer"))
                    {
                        if (item.Contains("A9_1"))
                        {
                            dt = fetch_roster_details(hh_key, "All", "15");
                            dt.Columns["MemberID"].ColumnName = "DB_Value";
                            dt.Columns["Name"].ColumnName = "Label";

                            dt.Rows.Add(new Object[]{
                                            "99",
                                            "Not in roaster"
                                       });
                            correction_type[j] = 2;
                            set_dropdown(dt, j);

                        }
                        else if (item.Contains("A11_1"))
                        {
                            dt = fetch_roster_details(hh_key, "Male", "15");
                            dt.Columns["MemberID"].ColumnName = "DB_Value";
                            dt.Columns["Name"].ColumnName = "Label";
                            dt.Rows.Add(new Object[]{
                                            "99",
                                            "Not in roaster"
                                       });
                            dt.Rows.Add(new Object[]{
                                            "98",
                                            "Not alive"
                                       });
                            correction_type[j] = 2;
                            set_dropdown(dt, j);
                        }
                        else if (item.Contains("A10_1"))
                        {
                            dt = fetch_roster_details(hh_key, "Female", "15");
                            correction_type[j] = 2;
                            dt.Columns["MemberID"].ColumnName = "DB_Value";
                            dt.Columns["Name"].ColumnName = "Label";
                            dt.Rows.Add(new Object[]{
                                            "99",
                                            "Not in roaster"
                                       });
                            dt.Rows.Add(new Object[]{
                                            "98",
                                            "Not alive"
                                       });

                            set_dropdown(dt, j);
                        }
                        else
                        {
                            correction_type[j] = 1;
                            set_text(j, "integer", dt.Rows[0]["Label"].ToString());
                        }
                    }
                    else
                    {
                        correction_type[j] = 2;
                        set_dropdown(dt, j);
                    }
                    j++;
                }
            }
        }
        private bool resovled_or_not(DataSet table)
        {
            int pos = 0;
            string key_correct = "";
            foreach (DataColumn dc in table.Tables[0].Columns)
            {

                if (dc.ToString().Contains("Corrected"))
                {
                    key_correct = dc.ToString();
                    foreach (DataRow item in table.Tables[0].Rows)
                    {
                        if (item[key_correct].ToString().Length > 1)
                        {
                            if (item["Closed Or Reviewed By"].ToString().Length == 0)
                            {
                                return false;
                            }
                        }
                        pos++;
                    }

                }
            }
            return true;
        }
        private DataTable fetch_roster_details(string hh_key, string gender, string age)
        {
            DataTable table = new DataTable();
            using (var cmd = new SqlCommand("HH_Members_By_ID", con))
            using (var da = new SqlDataAdapter(cmd))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@HH_Key", hh_key));
                cmd.Parameters.Add(new SqlParameter("@Gender", gender));
                cmd.Parameters.Add(new SqlParameter("@Age", age));
                da.Fill(table);
                //write_to_server(cmd, "");

                return table;
            }
        }
        private void set_text(int i, string val, string tool)
        {
            TextBox tb = new TextBox();
            tb = get_text_box("TextBox" + i);
            if (val.Equals("text"))
            {
                tb.Text = "Insert Text Value";
            }
            else
            {
                tb.Text = "Insert Value"; //= "Insert " + tool + " Value";
                tb.ToolTip = tool;
                tb.Attributes.Add("Type", "Integer");
            }
            tb.Visible = true;
        }
        private void set_dropdown(DataTable dt, int i)
        {
            DropDownList tb = new DropDownList();
            String id = "DropDownList";
            tb = get_dropdown_box(id + i);

            try
            {
                //  dt = GetData("select Label, DB_Value  from (select l.List_Name, l.DB_Value, l.label, q.Questions_Variable from Data_LNL as l inner join Data_QnV as q on l.List_Name = q.List_Name) as T where T.Questions_Variable = '" + var + "'");

                tb.DataSource = dt;
                tb.DataTextField = "Label";
                tb.DataValueField = "DB_Value";
                tb.DataBind();
                tb.Items.Insert(0, new ListItem("---Select-----", "NA"));

                tb.Visible = true;
            }
            catch (Exception)
            {

            }
        }
        protected void bla_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false; // hides the first column
            e.Row.Cells[2].Visible = false; // hides the second column
            e.Row.Cells[3].Visible = false; // hides the second column
            e.Row.Cells[4].Visible = false; // hides the second column
            e.Row.Cells[5].Visible = false; // hides the second column
            e.Row.Cells[6].Visible = false; // hides the second column
            e.Row.Cells[7].Visible = false; // hides the second column

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        string tool = decode_Values(GridView1.HeaderRow.Cells[i].Text, error_data.Rows[e.Row.RowIndex][GridView1.HeaderRow.Cells[i].Text.ToString()].ToString()); // final_table.Rows[0]["household_id"].ToString();
                        e.Row.Cells[i].ToolTip = tool;
                    }
                }
            }
            catch (Exception ex) { 

            }
           

        }

        private DataTable GetData(string query)
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query,conn))
                {

                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 0;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
            catch
            {
                throw;
            }
            
           
        }

        protected void performance()
        {
            progress.Visible = true;
            string query = "select [Total Assigned],[Reviewed & Resolved] + [Reviewed & Closed] as [Resolved & Corrected] ,[Reviewed & Reassigned] from [Summary_Progress_Monitor] where Monitor_Name = ( SELECT [Username] FROM [dbo].[users] where UserId =  '" + Session["UserID"] + "')";
            DataTable dt = new DataTable();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            using (var cmd = new SqlCommand(query, conn))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                cmd.Connection = conn;


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //write_to_server(cmd, "");
                da.Fill(dt);
            }
      
            
           
            try
            {
                assigned.InnerText = dt.Rows[0]["Total Assigned"].ToString();
                resolved.InnerText = dt.Rows[0]["Resolved & Corrected"].ToString();
                reassigned.InnerText = dt.Rows[0]["Reviewed & Reassigned"].ToString();
            }
            catch (Exception)
            {
                
             
            }

           



        }
        string sender_text;
        Button clicked;
        //protected void Update_values_r(object sender, EventArgs e)
        //{
        //     sender_text = (sender as Button).Text;
        //     clicked = sender as Button;
        //    Thread t1 = new Thread(Update_values);
        //}


        protected void Update_values(object sender, EventArgs e)
        {
            //logFile.logHH((string)(Session["username"]), "Action:ClickedResolved, Key:" + HH_Key + ",ProcName" + proc_name);

            //write_to_server(null, "Action:ClickedResolved, Key:" + HH_Key + ",ProcName" + proc_name);
            string sender_text = (sender as Button).Text;
            Button clicked = sender as Button;
            //clicked.Enabled = false;
            if (comment_box.Text.Length > 15)
            {
               // loading_bar.Visible = true;
                String[] values = new String[15];
                String[] variables = new String[14];

                DropDownList dp = new DropDownList();
                TextBox tb = new TextBox();
                Label lb = new Label();
                int index = 0;
                DropDownList keys_dd = new DropDownList();
                keys_dd = get_dropdown_box("key_drop");
                string key = keys_dd.SelectedItem.Text;

                for (int i = 1; i < 15; i++)
                {
                    lb = get_label_box("f" + i);
                    if (lb.Visible)
                    {
                        variables[index] = lb.Text;
                        tb = get_text_box("TextBox" + i);
                        dp = get_dropdown_box("DropDownList" + i);
                        if (dp.Visible)
                        {
                            values[index] = dp.SelectedValue.ToString();
                            index++;
                        }
                        else if (tb.Visible)
                        {
                            values[index] = tb.Text;
                            index++;
                        }
                    }
                }
                tb = get_text_box("comment_box");
                values[14] = tb.Text;
                if (sender_text.Equals("Yes"))
                {
                   // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Next...')", true);
                    //write_to_server(null, "Action:ReportingForcefullyClosure, Key:" + HH_Key + ",ProcName" + proc_name);

                    // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error has been closed')", true);
                    updating_values(values, key, false, sessionUserRole, "0",gotpid);
                    //write_to_server(query_command, "");
                    update_assigment_comment("0", values[14]);
                    content_correction.Disabled = true;
                    comment_box.Enabled = false;
                    next_click.Enabled = true;
                    next_click.Visible = true;
                        Response.Redirect(Request.RawUrl);

                }
            
               
                // copying table to TEMP_Tables
                 using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                 using (var command = new SqlCommand("TMP_TABLES", conn))
                 {
                     if (conn.State != ConnectionState.Open)
                     {
                         conn.Open();
                     }
                     command.Parameters.AddWithValue("@Key_", HH_Key);
                     command.CommandType = CommandType.StoredProcedure;
                     command.CommandTimeout = 0;
                     SqlDataReader dr = command.ExecuteReader();
                 }
               
           
               
               
             
                //updating temp tables

                updating_values(values, key, true, sessionUserRole, "",gotpid);
                //write_to_server(null, "Action:CheckingUpdatedValuesonTemp, Key:" + HH_Key + ",ProcName" + proc_name);
               


              //  //write_to_server(query_command);

                if (verify_update(HH_Key, key, gotpid))
                {
                    //write_to_server(null, "Action:CheckingUpdatedValuesonActual, Key:" + HH_Key + ",ProcName" + proc_name);

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error is Fixed Updating the Data')", true);
                    updating_values(values, key, false, sessionUserRole, "1",gotpid);
                    //write_to_server(query_command,"");

                    update_assigment_comment("1", values[14]);
                    //if (clicked.Text.Equals("Resolved/Closed & Next"))
                    //{
                    //    //   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Next...')", true);
                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect","alert('Error is Corrected Moving to Next'); window.location='" + Request.ApplicationPath + "window.location.href='Monitoring.aspx?Key=11449&pid=Q47';", true);
                    //}
                    //else
                    //{
                    content_correction.Disabled = true;
                    comment_box.Enabled = false;
                    next_click.Enabled = true;
                    next_click.Visible = true;

                        Response.Redirect(Request.RawUrl);
                    //}
                }
                else
                {
                    DataTable dt = new DataTable();
                    //if (con.State != ConnectionState.Closed) { con.Close(); }

                    // copying table to TEMP_Tables
                    //con.Open();
                    string check_flag = "SELECT [Flag] FROM [dbo].[Monitoring_Queries] where SQL_Query ='" + proc_name + "'";
                   

                    try
                    {
                        dt = GetData(check_flag);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        con.Close();
                    }
            
                    //con.Close();
                    string flag = dt.Rows[0]["Flag"].ToString();
                    if (flag.Contains('0'))
                    {
                        //write_to_server(null, "Action:ReportingClosure, Key:" + HH_Key + ",ProcName" + proc_name);

                       // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error has been closed')", true);
                        updating_values(values, key, false, sessionUserRole, "0",gotpid);
                        //write_to_server(query_command,"");
                        update_assigment_comment("0", values[14]);
                        content_correction.Disabled = true;
                        comment_box.Enabled = false;
                        next_click.Enabled = true;
                        next_click.Visible = true;
                         Response.Redirect(Request.RawUrl);
                        
                    }
                    else
                    {

                        
                        //TODO: adding pop up here for 
                      //  Boolean r = confrim     ('Are you sure that you want to SMT lock/unlock this account?');

                       // //write_to_server(null, "Action:ReportingNoResolution, Key:" + HH_Key + ",ProcName" + proc_name);
                       // ScriptManager.RegisterStartupScript(this, this.GetType(), "confirmation", "document.getElementById('confirmation_box').click()", true);

                    //    confirmation_box.ServerClick();
                       ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error has not been corrected..')", true);
                       // Response.Redirect(Request.RawUrl);
                        //resolve_click.Text = "Close this query";
                        //resolve_click.BackColor = Color.IndianRed;
                        close_force.Visible = true;
                        return;
                    }
                }
            }
            else
            {
                //write_to_server(null, "Action:ReportingCommentsLength, Key:" + HH_Key + ",ProcName" + proc_name);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide proper Comment (min 15 character)...')", true);
            }

        }
        public void test()
        {
            Console.WriteLine("This is test0");
        }
        private static bool verify_update(string key, string record, string gotpid)
        {
            string[] key_level = record.Split('-');
            DataTable response = new DataTable();
            string poc = "TMP_" + proc_name;

            if (con.State != ConnectionState.Closed) { con.Close(); }
            //Running update procedure
            con.Open();
            using (var cmd_update = new SqlCommand(poc, con))
            using (var da = new SqlDataAdapter(cmd_update))
            {
                cmd_update.CommandType = CommandType.StoredProcedure;
                //string poc = "TMP_Section1_A9_1_SpousesNOT_matching";
                cmd_update.Parameters.AddWithValue("@Key_", key);
                da.Fill(response);
                if (key_level.Length > 5)
                {
                    if (response.Rows.Count >= 1)
                    {
                        bool value = row_exists(response, key_level[key_level.Length - 1],key, gotpid);
                        con.Close();
                        return value;
                    }
                }

                if (response.Rows.Count > 0)
                {
                    con.Close();
                    return false;
                }
            }
            con.Close();
            //Key updated here
            update_flag(key, false, gotpid);
            return true;
        }
        private static bool row_exists(DataTable tmp_response, string row, string key, string gotpid)
        {
            int index = Convert.ToInt32(row);

            foreach (DataColumn dc in tmp_response.Columns)
            {
                if (dc.ToString().Contains("Corrected"))
                {
                    for (int i = 0; i < tmp_response.Rows.Count; i++)
                    {
                        if (tmp_response.Rows[i][dc].ToString().Length > 0)
                        {

                        }
                        else
                        {
                         //   update_flag(tmp_response.Rows[i]["Key_"].ToString(), true);
                        }
                        if (tmp_response.Rows[i]["KEY_"].ToString() == key + "-" + row )
                            if(tmp_response.Rows[i][dc].ToString().Length > 0)
                                return false;
                    }
                    try
                    {
                        if (tmp_response.Rows[index - 1][dc].ToString().Length > 0)  //.Rows[0]["household_id"].ToString();
                            return false;
                    }
                    catch (Exception)
                    {
                        
               
                    }
                   

                }
            }
            return true;
        }

        public static void update_flag(string key, bool individual, string gotpid)
        {
           // string report_name = gotpid.Remove(gotpid.IndexOf('Q'), 1);
            string intro = "Introduce Corrected Value here";
            using (SqlCommand command = con.CreateCommand())
            {
                if (individual)
                {
                    command.CommandText = "update Summarized_Report set Field = '', Logical_Check = '', Telephone = '' where Key_+'-'+A1_1 = '" + key + "' and Report_Name = '" + gotpid + "' and Field = 'Introduce Corrected Value here' and Logical_Check = 'Introduce Corrected Value here' and Telephone = 'Introduce Corrected Value here'";

                }
                else
                {
                    command.CommandText = "update Summarized_Report set Field = '', Logical_Check = '', Telephone = '' where Key_ = '" + key + "' and Report_Name = '" + gotpid + "' and Field = '" + intro + "' and Logical_Check = '" + intro + "' and Telephone = '" + intro + "'";
                }
               // command.Parameters.AddWithValue("@key", gotpid);
              //  command.Parameters.AddWithValue("@report_name", report_ID);
                if (con.State != ConnectionState.Closed) { con.Close(); }
                //Running update procedure
                con.Open();
                command.ExecuteNonQuery();
                //write_to_server(null,"");

                // con.Close();

            }
        }
        private static void updating_values(string[] values, string key, bool temp_data, string session, string status, string gotpid)
        {
            string poc = proc_name; //Proc_name ---Update  --- //Sec2_A16_2_Check_age  //sec_8_B29_Source_of_income
            string update = "SQL_Excel_Update";
            string time = "";
            if (temp_data)
            {
                poc = "TMP_" + proc_name;
                update = "TMP_SQL_Excel_Update";
            }
            if (con.State != ConnectionState.Closed) { con.Close(); }
            //Running update procedure
            con.Open();
            /// string update = "TMP_SQL_Excel_Update";
            string query = "";
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            using (var cmd_update = new SqlCommand(update, conn))
            {
                conn.Open();
                cmd_update.CommandType = CommandType.StoredProcedure;
                //string poc = "TMP_Section1_A9_1_SpousesNOT_matching";
                cmd_update.Parameters.AddWithValue("@QueryName", poc); //poc Sec2_A16_2_Check_age
                cmd_update.Parameters.AddWithValue("@KEY_", key);

                if (session.Contains("DeskMonitor"))
                {
                    cmd_update.Parameters.AddWithValue("@Logical_Check", status);
                    cmd_update.Parameters.AddWithValue("@Telephone", DBNull.Value);
                    cmd_update.Parameters.AddWithValue("@Field", DBNull.Value);
                    cmd_update.Parameters.AddWithValue("@Comments_Logical", values[14]);
                    cmd_update.Parameters.AddWithValue("@Comments_Telephone", DBNull.Value);
                    cmd_update.Parameters.AddWithValue("@Comments_Field", DBNull.Value);
                    time = "logical_time";
                }
                else
                {
                    cmd_update.Parameters.AddWithValue("@Logical_Check", DBNull.Value);
                    if (session.Contains("DataValidator"))
                    {
                        cmd_update.Parameters.AddWithValue("@Telephone", status);
                        cmd_update.Parameters.AddWithValue("@Field", DBNull.Value);
                        cmd_update.Parameters.AddWithValue("@Comments_Telephone", values[14]);
                        cmd_update.Parameters.AddWithValue("@Comments_Logical", DBNull.Value);
                        cmd_update.Parameters.AddWithValue("@Comments_Field", DBNull.Value);
                        time = "telephone_time";

                    }
                    else
                    {
                        cmd_update.Parameters.AddWithValue("@Field", status);
                        cmd_update.Parameters.AddWithValue("@Telephone", DBNull.Value);
                        cmd_update.Parameters.AddWithValue("@Comments_Field", values[14]);
                        cmd_update.Parameters.AddWithValue("@Comments_Telephone", DBNull.Value);
                        cmd_update.Parameters.AddWithValue("@Comments_Logical", DBNull.Value);
                        time = "field_time";


                    }
                }
                if (values[0] == null || values[0].Contains("NA") || values[0].Contains("Insert") || values[0].Length==0)
                    cmd_update.Parameters.AddWithValue("@Corrected1", DBNull.Value);
                else
                {
                    cmd_update.Parameters.AddWithValue("@Corrected1", values[0]);
                }

                if (values[1] == null || values[1].Contains("NA") || values[1].Contains("Insert") || values[1].Length==0)
                    cmd_update.Parameters.AddWithValue("@Corrected2", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected2", values[1]); }

                if (values[2] == null || values[2].Contains("NA") || values[2].Contains("Insert") || values[2].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected3", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected3", values[2]); }

                if (values[3] == null || values[3].Contains("NA") || values[3].Contains("Insert") || values[3].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected4", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected4", values[3]); }

                if (values[4] == null || values[4].Contains("NA") || values[4].Contains("Insert") || values[4].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected5", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected5", values[4]); }

                if (values[5] == null || values[5].Contains("NA") || values[5].Contains("Insert") || values[5].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected6", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected6", values[5]); }

                if (values[6] == null || values[6].Contains("NA") || values[6].Contains("Insert") || values[6].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected7", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected7", values[6]); }

                if (values[7] == null || values[7].Contains("NA") || values[7].Contains("Insert") || values[7].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected8", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected8", values[7]); }

                if (values[8] == null || values[8].Contains("NA") || values[8].Contains("Insert") || values[8].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected9", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected9", values[8]); }

                if (values[9] == null || values[9].Contains("NA") || values[9].Contains("Insert") || values[9].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected10", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected10", values[9]); }

                if (values[10] == null || values[10].Contains("NA") || values[10].Contains("Insert") || values[10].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected11", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected11", values[10]); }

                if (values[11] == null || values[11].Contains("NA") || values[11].Contains("Insert") || values[11].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected12", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected12", values[11]); }

                if (values[12] == null || values[12].Contains("NA") || values[12].Contains("Insert") || values[12].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected13", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected13", values[12]); }

                if (values[13] == null || values[13].Contains("NA") || values[13].Contains("Insert") || values[13].Length == 0)
                    cmd_update.Parameters.AddWithValue("@Corrected14", DBNull.Value);
                else
                { cmd_update.Parameters.AddWithValue("@Corrected14", values[13]); }

                Int32 rowsAffected = cmd_update.ExecuteNonQuery();
                query = cmd_update.CommandText;
                query_command = cmd_update;
               
            }

            //Select A9_1 from TEMP_PART_1_A1 where KEY_ = 'uuid:ca810f31-f2a7-4af1-b5f1-0eeccc229fb1-2'
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            using (SqlCommand command = conn.CreateCommand())
            {
               command.CommandText = "update Summarized_Report set "+time+" = '"+DateTime.Now.ToString()+"' where Key_ = '" + key + "' and Report_Name = '" + gotpid + "'";
                conn.Open();
                command.ExecuteNonQuery();
               
            }
        }




        private Button get_button(String ID)
        {
            var container = Master.FindControl("ContentPlaceHolder1");
            Button tb = container.FindControl(ID) as Button;
            return tb;
        }

        private TextBox get_text_box(String ID)
        {
            var container = Master.FindControl("ContentPlaceHolder1");
            TextBox tb = container.FindControl(ID) as TextBox;
            return tb;
        }
        private DropDownList get_dropdown_box(String ID)
        {
            var container = Master.FindControl("ContentPlaceHolder1");
            DropDownList tb = container.FindControl(ID) as DropDownList;
            return tb;
        }
        private Label get_label_box(String ID)
        {
            var container = Master.FindControl("ContentPlaceHolder1");
            Label tb = container.FindControl(ID) as Label;
            return tb;
        }
        private void set_label(string id, string val)
        {
            Label tb = new Label();
            //String id = "f";
            tb = get_label_box(id);
            tb.Text = val;
            tb.Visible = true;
        }




        protected void key_drop_load(object sender, EventArgs e)
        {
            string tool = "";
            foreach (GridViewRow row1 in GridView1.Rows)
            {
                row1.BackColor = Color.White;
            }
            try
            {
                string searchValue = key_drop.SelectedItem.Text;

                var query = from GridViewRow row in GridView1.Rows
                            where row.Cells[1].Text == searchValue
                            select row;
                GridViewRow result = query.FirstOrDefault();

                result.BackColor = Color.Yellow;
                for (int i = 8; i < 30; i++)
                {
                    if (GridView1.HeaderRow.Cells[i].Text.ToString().Contains("Corrected"))
                        break;
                    else
                    {
                        tool = tool + GridView1.HeaderRow.Cells[i].Text.ToString() + " : [" + result.Cells[i].Text + "]    ";

                    }
                     if (GridView1.HeaderRow.Cells[i].Text.ToString().Equals("A2_1"))
                     {
                         update_name_lable(result.Cells[14].Text.ToString());
                     }
                }

                key1.ToolTip = tool;
                //var query_name = from GridViewRow row in GridView1.Rows
                //            where row.Cells[0].Text == searchValue
                //            select row;
                //GridViewRow result1 = query_name.FirstOrDefault();
                ////DataRow[] results = final_data.Select("key_ = '" + searchValue + "'");
                //if (result1.Cells[7].ToString().Length > 0)
                //    update_name_lable(result1.Cells[7].ToString());
            }
            catch (Exception)
            {

            }


        }
        private void set_buttons(DataTable dt, string p)
        {
            int count = 0;
            DataRow dr = dt.Rows[0];
            Button ques_button = new Button();
            for (int i = 1; i < 11; i++)
            {
                if (i == 4 || i == 6) ;
                else
                {
                    string sections = string_cleaner(dr["Section" + i].ToString());
                    if (sections.Length < 1) ;
                    else
                    {
                        if (sections.Contains(','))
                        {

                            string[] ques = sections.Split(',');
                            for (int j = 0; j < ques.Length; j++)
                            {
                                Color ar = color_button(ques[j]);
                                ques_button = create_buttons(ques[j]);
                                ques_button.ForeColor = Color.Black;
                                ques_button.BackColor = ar;
                                if (ques[j].Contains(p) && p.Length == ques[j].Length)
                                {
                                    ques_button.BorderColor = Color.Red;
                                    ques_button.BorderWidth = 1;

                                }
                                ques_button.Style.Add("margin", "1px");
                                ques_button.Style.Add("width", "100px");
                                Q_buttons.Controls.Add(ques_button);
                            }
                        }
                        else
                        {
                            Color ar = color_button(sections);
                            ques_button = create_buttons(sections);
                            ques_button.ForeColor = Color.Black;
                            ques_button.BackColor = ar;
                            if (sections.Contains(p) && p.Length == sections.Length)
                            {
                                ques_button.BorderColor = Color.Red;
                                ques_button.BorderWidth = 1;
                            }
                            ques_button.Style.Add("margin", "1px");
                            ques_button.Style.Add("width", "100px");
                            Q_buttons.Controls.Add(ques_button);
                        }
                    }
                }
            }
        }

        private Color color_button(string ques)
        {
            Color ar = new Color();
            ques = ques.Remove(ques.IndexOf('Q'), 1);
            string color = "";
            if (con.State != ConnectionState.Closed) { con.Close(); }
            //Running update procedure
            con.Open();
            using (var cmd_update = new SqlCommand("exec [CheckColor_latest] @key_='" + HH_Key + "', @report_name='" + ques + "'"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd_update.Connection = con;
                    sda.SelectCommand = cmd_update;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        color = dt.Rows[0]["Color"].ToString();
                    }
                }
            }
            con.Close();
            if (color == "0")
            {
                ar = System.Drawing.Color.Orange;
            }
            else if (color == "1")
            {
                ar = System.Drawing.Color.GreenYellow;
            }
            else if (color == "2")
            {
                ar = System.Drawing.Color.Yellow;
            }
            else if (color == "3")
            {
                ar = System.Drawing.Color.LightBlue;
            }
            return ar;
        }

        private Button create_buttons(string value)
        {
            Button db = new Button();
            db.Text = value;
            db.OnClientClick = "window.location.href='Monitoring.aspx?key=" + report_ID + "&pid=" + value + "'; return false;";
            db.CssClass = "btn btn-secondary";
            return db;
        }
        private string string_cleaner(string P)
        {
            while (P.Contains(' '))
            {
                P = P.Remove(P.IndexOf(' '), 1);
            }
            return P;
        }
        protected void forceResolvedClosed(object sender, EventArgs e)
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "update Summarized_Report set  Telephone='1',Comments_Validator='Confirmed and resolved' where Record_Id='" + report_ID + "'";
               // con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //write_to_server(cmd, "");
            }
            Response.Redirect("surveyProgress.aspx?id=1004_Mistakes_Summarized_Overall&name=Mistakes%20Summarized%20Report");
        }

        protected void dv_Click(object sender, EventArgs e)
        {
            DropDownList keys_dd = new DropDownList();
            keys_dd = get_dropdown_box("key_drop");
            string key = keys_dd.SelectedItem.Text;

            if (asg_comment_box.Text.Length > 15)
            {
                string[] val = new string[15];
                val[14] = asg_comment_box.Text;
                update_assigment_comment("DataValidator", asg_comment_box.Text);
                // update_assigment_flag("", "2");
                // updating_values(val, key, false, sessionUserRole, "2");
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Assigned -> Validator ')", true); //alert('Assigned -> Validator '"+HH_Key+"_"+proc_name+"'
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide comment for DataValidator.. (min 15 character)')", true); //alert('Assigned -> Validator '"+HH_Key+"_"+proc_name+"'
            }

        }

        protected void sp_Click(object sender, EventArgs e)
        {
            DropDownList keys_dd = new DropDownList();
            keys_dd = get_dropdown_box("key_drop");
            string key = keys_dd.SelectedItem.Text;

            if (asg_comment_box.Text.Length > 15)
            {
                string[] val = new string[15];
                val[14] = asg_comment_box.Text;
                update_assigment_comment("Supervisor", asg_comment_box.Text);
                //  update_assigment_flag("", "2");
                //  updating_values(val, key, false, sessionUserRole, "2");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Assigned -> Field Monitor ')", true); //" + HH_Key + "_" + proc_name + "
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please provide comment for Field Monitor... (min 15 character)')", true); //alert('Assigned -> Validator '"+HH_Key+"_"+proc_name+"'
            }


        }
        private string decode_Values(string label, string values)
        {


            if (values.Length < 1)
            {
                if (label == "C3_9" || label == "C10_9" || label == "C6_9" || label == "C1_9" || label == "C2_9" || label == "C8_9" || label == "C0_9") //C1
                    label = label.Replace("_9", "_9_59");
                DataRow[] results = data_description.Select("question_variable = '" + label + "'");
                if (results.Length > 0)

                    return results[0]["description"].ToString();
            }
            DataRow[] resultss = data_verification.Select("Questions_Variable = '" + label + "' AND List_Name = '" + values.Replace("'","") + "'");
            if (resultss.Length > 0)
            {
                return "[" + label + "] : " + resultss[0]["Db_Value"].ToString();
            }
            else
                return label;
        }

        public void update_name_lable(string name)
        {
            //write_to_server(null, "Action:ChangingName, Key:" + HH_Key + ",Name" + name);

            for (int i = 1; i < 15; i++)
            {
                Label tb = new Label();
                tb = get_label_box("l" + i);
                if (tb.Visible)
                {
                    Regex regex = new Regex(@"\$\{.*\}");
                    Match g = regex.Match(tb.Text);
                    bool match = regex.IsMatch(tb.Text);
                    if (match)
                    {
                        tb.Text = tb.Text.Replace(g.Groups[0].Value, "${" + name + "}");
                    }
                }
            }

        }

        public void write_to_server(SqlCommand cmd, string command)
        {

            if (cmd != null)
            {
                string query = cmd.CommandText;

                foreach (SqlParameter p in cmd.Parameters)
                {
                    if (p.Value.ToString().Length >= 1)
                        query = query + "," + p.ParameterName + " : " + p.Value.ToString();
                }
                string now = DateTime.Now.ToString("d MMM yyyy");
                using (StreamWriter w = new StreamWriter(Server.MapPath("~/" + now + ".txt"), true))
                {
                    w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Session["UserID"] + "," + query); // Write the text
                    w.Close();
                }
            }
            else
            {
                string now = DateTime.Now.ToString("d MMM yyyy");
                using (StreamWriter w = new StreamWriter(Server.MapPath("~/" + now + ".txt"), true))
                {
                    w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + Session["UserID"] + "," + command); // Write the text
                    w.Close();
                }
            }
        }
        private void update_assigment_comment(string assigned_to, string comment)
        {
            key_drop = get_dropdown_box("key_drop");
            string key_selected = key_drop.SelectedValue.ToString();
            if (key_selected.Split('-').Length == 5)
            {
                key_selected = key_selected + "-NULL";
            }

            string role_var = "";
            string comment_var = "";
            string status = "2";
            string time = "";

            if (Session["Role"].ToString().Contains("DeskMonitor"))
            {
                role_var = "Logical_Check";
                comment_var = "Comments_Monitor";
                time = "logical_time";
            }
            else if (Session["Role"].ToString().Contains("DataValidator"))
            {
                role_var = "Telephone";
                comment_var = "Comments_Validator";
                time = "telephone_time";

            }
            else
            {
                role_var = "Field";
                comment_var = "Comments_Supervisor";
                time = "field_time";

            }
            if (assigned_to.Length > 3)
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Summarized_Report SET Assigned = @assigned, "+time+" = @now  Where Report_Name = @rn and key_ = @key_";
                    cmd.Parameters.AddWithValue("@assigned", assigned_to);
                    cmd.Parameters.AddWithValue("@now", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@rn", gotpid);
                    cmd.Parameters.AddWithValue("@key_", HH_Key);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ////write_to_server(cmd,"");
                }
            }
            else
            {
                status = assigned_to;
            }
            using (SqlCommand command = con.CreateCommand())
            {
                if (proc_name.Contains("Section8"))
                {
                    command.CommandText = "UPDATE Summarized_Report SET  " + role_var + " = @status , " + comment_var + " = @comment , " + time + " = @now  Where Report_Name = @rn and key_ = @key";

                }
                else
                {
                    command.CommandText = "UPDATE Summarized_Report SET  " + role_var + " = @status , " + comment_var + " = @comment , " + time + " = @now  Where Report_Name = @rn and key_ +'-'+ A1_1 = @key";

                }
                command.Parameters.AddWithValue("@rn", gotpid);
                if (proc_name.Contains("Section8"))
                {
                    command.Parameters.AddWithValue("@Key", HH_Key);

                }
                else
                {
                    command.Parameters.AddWithValue("@Key", key_selected);

                }
                command.Parameters.AddWithValue("@now", DateTime.Now);
                command.Parameters.AddWithValue("@comment", comment);
                command.Parameters.AddWithValue("@status", status);
                if (con.State != ConnectionState.Open)
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                //write_to_server(command,"");


            }
            if (assigned_to.Length > 3)
            {
                using (SqlCommand cmd_ = con.CreateCommand())
                {
                    string var = "Introduce Corrected Value here";
                    cmd_.CommandText = "UPDATE Summarized_Report SET " + role_var + " = @status Where Report_Name = @rn and key_ = @key and " + role_var + " = '" + var + "'";
                    cmd_.Parameters.AddWithValue("@Key", HH_Key);
                    cmd_.Parameters.AddWithValue("@rn", gotpid);
                    cmd_.Parameters.AddWithValue("@status", status);
                    con.Open();
                    cmd_.ExecuteNonQuery();
                    con.Close();
                    //write_to_server(cmd_,"");

                }
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {

            string url = "";
            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            using (var cmd = new SqlCommand("GetNextURL", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("monitor", Session["username"]);
                cmd.CommandType = CommandType.StoredProcedure;
                da.Fill(table);

                 url = "Monitoring.aspx?" + table.Rows[0]["URL"].ToString();
                //Tables[0]["URL"]
               
            }
            //write_to_server(null, "Action:NextButtonClicked, url:" + url);


            Response.Redirect(url);
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            close_force.Visible = false;
        }

    }
}