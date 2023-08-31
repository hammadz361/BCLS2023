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
using System.Text.RegularExpressions;

namespace CLS_Dashboard
{
    public partial class Map : System.Web.UI.Page
    {
        public SqlConnection con;
        public string constr;
        public int columnCodeGlobal = 0;
        public string MapQuery = "";
        protected string ArrayStore = "";
        public static string strQuery = "";



        public enum MessageType { Success, Error, Info, Warning };



        public void connection()
        {
            constr = ConfigurationManager.ConnectionStrings["SCLSCon"].ToString();
            con = new SqlConnection(constr);
            if (con.State != ConnectionState.Closed) { con.Close(); }



            try
            {


                con.Open();


            }
            catch (Exception ex)
            {
                ShowMessage("Error: Unable to Open database Connection .. ", MessageType.Error);
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
                if ("" != Session["Role"].ToString())
                {
                    if (!IsPostBack)
                    {
                        try
                        {
                           // lblUserName.Text = "Loggin as " + Session["id"].ToString();
                            load_Filters_Data();
                            MapQuery = "select distinct top 500  RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where RECORDGPS_FIRST_LAT is not null AND RECORDGPS_FIRST_LNG is not null group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "initialize();", true);
                            if ("listmonitor" == Session["Role"].ToString() )
                            {
                                download.Visible = true;
                                download_tab.Visible = true;
                                load_content("LISTMONITOR");
                            }
                            else if("listadmin" == Session["Role"].ToString())
                            {
                                download.Visible = true;
                                download_tab.Visible = true;
                                load_content("LISTADMIN");
                            }

                        }
                        catch (Exception ex)
                        {
                            ShowMessage("Error: Unable to Load filters Data .. ", MessageType.Error);
                        }

                    }


                }

                else
                {
                    Response.Redirect("Login.aspx");
                }

           
        }
        public void load_content(string ROLE)
        {
            string query= "";
            if (ROLE == "LISTADMIN")
            {
                query = @"SELECT [username], [cluster_code]
                          ,[status]
	                        FROM [download_log] ";
            }
            else if (ROLE == "LISTMONITOR")
            {
                query = @"SELECT [username], [cluster_code]
                          ,[status]
	                        FROM [download_log] where username ='" + Session["username"]+"'";
            }
            DataTable dt = this.GetData(query);
            download_tab.DataSource = dt;
            download_tab.DataBind();
        }

        private DataTable GetData(string query)
        {
         
            using (SqlCommand cmd = new SqlCommand(query, con))
            {

                connection();
                cmd.Connection = con;

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {



                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }


        }


        protected string getMarkerObjects()
        {
            const string indent = "    ";
            if(MapQuery.Length<1)
            {
                MapQuery = "select distinct top 500  RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where RECORDGPS_FIRST_LAT is not null AND RECORDGPS_FIRST_LNG is not null group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                           
            }
            DataTable dt = this.GetData(MapQuery);
            //dt.Columns.AddRange(new[] { new DataColumn("Name", typeof(string)), new DataColumn("Latitude", typeof(double)), new DataColumn("Longitude", typeof(double)), new DataColumn("Description", typeof(string)) });
            //dt.Rows.Add(new Object[] { "p1", 32.2217, -110.9264, "p1" });
            //dt.Rows.Add(new Object[] { "p2", 31.9556, -110.3067, "p2" });
            string ret = string.Empty;
            foreach (DataRow r in dt.Rows)
            {
                ret += ((ret.Equals(string.Empty)) ? "" : indent + "," + "\r\n") +
                    indent + "{" + "\r\n" +

                    indent + indent + "\"lat\": '" + r["RECORDGPS_FIRST_LAT"] + "'," + "\r\n" +
                    indent + indent + "\"lng\": '" + r["RECORDGPS_FIRST_LNG"] + "'," + "\r\n" +
                    indent + indent + "\"description\": '" + "Cluster Code: " + r["CLUSTER_CODE"] + "'" + "\r\n" +

                    indent + "}" + "\r\n";
            }

            ArrayStore = ret;
            return ret;
        }

        protected void download_Click(object sender, EventArgs e)
        {
            
            //int limit = (int)limitlstClusterCodes.SelectedValue.ToString();
            if (Session["Role"].ToString().Contains("listmonitor"))
                {
                    if (lstClusterCodes.SelectedValue.ToString() == "All")
                    {
                        MapQuery = "select distinct top 500   RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where RECORDGPS_FIRST_LAT is not null AND RECORDGPS_FIRST_LNG is not null group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Select a Cluster Code to download')", true);

                    }
                    else
                    {
                        string sql_double = @" select count(cluster_code) from download_log
                        where cluster_code ='" + lstClusterCodes.SelectedValue.ToString() + "' and username = '"+Session["username"]+"'";
                        DataTable dtt = GetData(sql_double);
                        if (dtt.Rows[0].ItemArray[0].ToString() != "0")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Already in Queue for Download')", true);

                        }
                        else
                        {
                            string selectSql = @"select count(distinct(X.cluster_code))+1 as clusters, Y.DISTRICT, X.username as username from download_log X
                        right join Listing_HH Y on X.cluster_code=Y.CLUSTER_CODE
                        where Y.DISTRICT = (select distinct(DISTRICT) from Listing_HH where CLUSTER_CODE ='" + lstClusterCodes.SelectedValue.ToString() + "')group by Y.DISTRICT, X.username";

                            // SqlCommand cmdd = new SqlCommand("select Assign from users where Role = 'listmonitor'");
                            DataTable dt = GetData(selectSql);
                            DataRow[] results = dt.Select("username = '"+Session["username"]+"' AND clusters = '6'");
                            if (results.Length > 0)
                            {
                                MapQuery = "select distinct top 500   RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where RECORDGPS_FIRST_LAT is not null AND RECORDGPS_FIRST_LNG is not null group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Download limit has reached for " + dt.Rows[0].ItemArray[1].ToString() + "')", true);

                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Download queued')", true);
                                string updateSql =
                                            @"INSERT INTO [dbo].[download_log]
                                               ([username]
                                               ,[cluster_code]
                                               ,[status]
                                               ,[requested_date])
                                           Values
                                              ( @username, @cluster, @status, @requested)";

                                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                                {
                                    connection.Open();
                                    var command = new SqlCommand(updateSql, connection);
                                    var args = command.Parameters;

                                    args.Add("@username", Session["username"]);
                                    args.Add("@cluster", lstClusterCodes.SelectedValue.ToString());
                                    args.Add("@status", "0");
                                    args.Add("@requested", DateTime.Now.ToString());

                                    command.ExecuteNonQuery();

                                }
                                load_content("LISTMONITOR");
                            }

                        }
                    }
                   
              }
                else if (Session["Role"].ToString().Contains("listadmin"))
                {
                    
                    MapQuery = "select distinct top 500  RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where CLUSTER_CODE = '" + searchTerm.Value.Trim() + "' group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                    strQuery = @"SELECT [PARENT_KEY]
                      ,[_CREATION_DATE]
                      ,[_SUBMISSION_DATE]
                      ,[DISTRICT]
                      ,[NOTABLE_PERSON_CELL]
                      ,[TODAY_VISIT1]
                      ,[EBCODE]
                      ,[DEVICE_ID]
                      ,[CLUSTER_CODE]
                      ,[LOCALITY_ADDRESS]
                      ,[NOTABLE_PERSON]
                      ,[LISTER_NAME]
                      ,[LISTER_CODE]
                      ,[LOCALITY]
                      ,[AREA_TYPE]
                      ,[TEHSIL]
                      ,[ENDTIME_VISIT]
                      ,[STARTTIME_VISIT1]
                      ,[DEVICE_PHONE_NUMBER]
                      ,[SIM_ID]
                      ,[KEY_]
                      ,[SR_NO_HOUSEHOLD]
                      ,[STRUCTURE_TYPE]
                      ,[STRUCTURE_NO]
                      ,[REMARKS]
                      ,[ADDRESS_OF_HOUSEHOLD]
                      ,[CHILD_PRESENT_STATUS]
                      ,[HHMEMBERS_COUNT]
                      ,[HH_HEAD_NAME]
,[HH_FATHER_NAME]
                      ,[CHILD_SCHOOL_COUNT]
                      ,[CHILDREN]
                      ,[BOYS_COUNT]
                      ,[GIRLSL_COUNT]
                      ,[HHSELECTED]
                      ,[SELECTED_DATE]
                  FROM [Listing_HH]  where CLUSTER_CODE = '" + lstClusterCodes.SelectedValue.ToString() + "'";
                    ExportToCSV("List_Monitoring", sender, e);
                }
            }
        public int convert(string intString)
        {
            int i = 0;
            if (!Int32.TryParse(intString, out i))
            {
                i = -1;
            }
            return i;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (searchTerm.Value.Trim() == "")
                {
                    load_Filters_Data();

                    MapQuery = "select distinct top 500   RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where RECORDGPS_FIRST_LAT is not null AND RECORDGPS_FIRST_LNG is not null group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "initialize();", true);


                    ShowMessage("Error: Please input record .. ", MessageType.Error);
                    return;
                }

                MapQuery = "select distinct top 500  RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where CLUSTER_CODE = '" + searchTerm.Value.Trim() + "' group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "initialize();", true);

                if (getMarkerObjects().Length < 1)
                {
                    ShowMessage("Error: Unable to Search Record .. ", MessageType.Error);
                    return;
                }


            }
            catch (Exception ex)
            {
                ShowMessage("Error: Unable to Search Record .. ", MessageType.Error);
            }
        }


        protected void ShowMessage(string Message, MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "ShowMessage('" + Message + "','" + type + "');", true);
        }
        private void load_Filters_Data()
        {

            //For 1st Filter Districts

            load_Filter_Districts();
            lstDistricts.SelectedIndex = 0;

            //For 2st Filter ClusterCode

            load_Filter_ClusterCode();
            lstClusterCodes.SelectedIndex = 0;

            //For 3rd Filter Lister_name
            load_Filter_Lister_name();
            lstLister_name.SelectedIndex = 0;

            //For 4th Filter Locality

            load_Filter_Locality();
            lstLocality.SelectedIndex = 0;

            //For Total_hh Filter
            load_Filter_Total_hh();
            lstTotal_hh.SelectedIndex = 0;

            //For Total_children Filter
            load_Filter_Total_children();
            lstTotal_children.SelectedIndex = 0;




        }

        private void load_Filter_Districts()
        {
            lstDistricts.Items.Clear();
            lstDistricts.Items.Add("All");
            connection();

            using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT district FROM Listing_HH where district is not null  GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by 1", con))
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

        private void load_Other_Filter_Selected(string Column, string condition, int columnCode)
        {
            string distinct = "distinct";

            if (columnCode == 1)
            {
                lstClusterCodes.Items.Clear();
                lstClusterCodes.Items.Add("All");
            }

            if (columnCode == 2)
            {
                lstLocality.Items.Clear();
                lstLocality.Items.Add("All");
            }

            if (columnCode == 3)
            {
                lstLister_name.Items.Clear();
                lstLister_name.Items.Add("All");
            }

            if (columnCode == 4)
            {
                distinct = "";
                lstTotal_hh.Items.Clear();
                lstTotal_hh.Items.Add("All");
            }

            if (columnCode == 5)
            {

                distinct = "";
                lstTotal_children.Items.Clear();
                lstTotal_children.Items.Add("All");
            }

            if (condition.Contains("All"))
            {
                condition = "";

            }




            connection();

            string query = "SELECT " + distinct + " " + Column + " FROM Listing_HH where " + condition + "  " + Column.Replace("COUNT(SR_NO_HOUSEHOLD) AS Total_HH", "SR_NO_HOUSEHOLD").Replace("SUM(CAST(CHILDREN AS int)) AS Total_children", "CHILDREN") + " is not null ";

            query = query + " GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by " + Column.Replace("COUNT(SR_NO_HOUSEHOLD) AS Total_HH", "Total_HH").Replace("SUM(CAST(CHILDREN AS int)) AS", "") + "  asc";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)

                            if (columnCode == 1)
                                lstClusterCodes.Items.Add(dt.Rows[i].Field<string>(0));

                            else if (columnCode == 2)
                                lstLocality.Items.Add(dt.Rows[i].Field<string>(0));

                            else if (columnCode == 3)
                                lstLister_name.Items.Add(dt.Rows[i].Field<string>(0));

                            else if (columnCode == 4)
                                lstTotal_hh.Items.Add(dt.Rows[i].Field<int>(0).ToString());

                            else if (columnCode == 5)
                                lstTotal_children.Items.Add(dt.Rows[i].Field<int>(0).ToString());


                    }
                }
            }

        }


        private void load_Filter_ClusterCode()
        {

            lstClusterCodes.Items.Clear();
            lstClusterCodes.Items.Add("All");
            connection();

            using (SqlCommand cmd = new SqlCommand("SELECT CLUSTER_CODE FROM Listing_HH where CLUSTER_CODE is not null GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by 1", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstClusterCodes.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }

        }

        private void load_Filter_Locality()
        {

            lstLocality.Items.Clear();
            lstLocality.Items.Add("All");
            connection();

            using (SqlCommand cmd = new SqlCommand("SELECT Locality FROM Listing_HH where Locality is not null GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by 1", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstLocality.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }

        }

        private void load_Filter_Lister_name()
        {

            lstLister_name.Items.Clear();
            lstLister_name.Items.Add("All");
            connection();

            using (SqlCommand cmd = new SqlCommand("SELECT distinct Lister_name FROM Listing_HH where Lister_name is not null GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by 1", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstLister_name.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }

        }

        private void load_Filter_Total_hh()
        {

            lstTotal_hh.Items.Clear();
            lstTotal_hh.Items.Add("All");
            connection();

            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(SR_NO_HOUSEHOLD) AS Total_HH FROM Listing_HH where SR_NO_HOUSEHOLD is not null GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by 1", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstTotal_hh.Items.Add(dt.Rows[i].Field<int>(0).ToString());


                    }
                }
            }

        }

        private void load_Filter_Total_children()
        {

            lstTotal_children.Items.Clear();
            lstTotal_children.Items.Add("All");
            connection();

            using (SqlCommand cmd = new SqlCommand("SELECT SUM(CAST(CHILDREN AS int)) AS Total_children FROM Listing_HH where CHILDREN is not null GROUP BY DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by 1", con))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;

                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                            lstTotal_children.Items.Add(dt.Rows[i].Field<int>(0).ToString());


                    }
                }
            }

        }


        protected void lstDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 0;

            load_Other_Filter_Selected(" CLUSTER_CODE ", " district = '" + lstDistricts.SelectedItem + "' AND  ", 1);


            load_Other_Filter_Selected(" LOCALITY ", "district = '" + lstDistricts.SelectedItem + "' AND", 2);


            load_Other_Filter_Selected(" LISTER_NAME ", " district = '" + lstDistricts.SelectedItem + "' AND  ", 3);


            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " district = '" + lstDistricts.SelectedItem + "' AND  ", 4);


            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " district = '" + lstDistricts.SelectedItem + "' AND  ", 5);

            // rep_bind();
        }

        protected void lstClusterCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 1;

            load_Other_Filter_Selected(" LOCALITY ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 2);


            load_Other_Filter_Selected(" LISTER_NAME ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 3);


            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 4);


            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 5);

            if (lstClusterCodes.SelectedItem.Value == "All")
            {
                MapQuery = "select distinct top 500  RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where RECORDGPS_FIRST_LAT is not null AND RECORDGPS_FIRST_LNG is not null group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";
            }
            else
            {
                MapQuery = "select distinct top 500  RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE from Listing_HH where CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' group by RECORDGPS_FIRST_LAT, RECORDGPS_FIRST_LNG, CLUSTER_CODE";

            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "initialize();", true);

            if (getMarkerObjects().Length < 1)
            {
                ShowMessage("Error: Unable to Search Record .. ", MessageType.Error);
                return;
            }


        }

        protected void lstLocality_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 2;

            load_Other_Filter_Selected(" LISTER_NAME ", " LOCALITY = '" + lstLocality.SelectedItem + "' AND  ", 3);

            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " LOCALITY = '" + lstLocality.SelectedItem + "' AND  ", 4);

            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " LOCALITY = '" + lstLocality.SelectedItem + "' AND  ", 5);

            // rep_bind();
        }



        protected void lstLister_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 3;

            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " LISTER_NAME = '" + lstLister_name.SelectedItem + "' AND  ", 4);


            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " LISTER_NAME = '" + lstLister_name.SelectedItem + "' AND  ", 5);


            // rep_bind();
        }

        protected void lstTotal_hh_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 4;

            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " SR_NO_HOUSEHOLD = '" + lstTotal_hh.SelectedItem + "' AND  ", 5);

            //rep_bind();
        }

        protected void lstTotal_children_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 5;

            //rep_bind();
        }

        protected void ExportToCSV(string filename, object sender, EventArgs e)
        {
            //Get the data from database into datatable

           // SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(strQuery);

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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
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
                    sb.Append(myString + ',');
                }
                //append new line
                sb.Append("\r\n");
            }
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void download_tab_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "download")
            {
                if (e.CommandArgument.ToString().Length > 0)
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = download_tab.Rows[rowIndex];

                    //Fetch value of Name.
                    //string name = (row.FindControl("txtName") as TextBox).Text;

                    //Fetch value of Code
                    string code = row.Cells[1].Text;
                    //string role = Session["Role"].ToString();

                    strQuery = @"SELECT [PARENT_KEY]
                      ,[_CREATION_DATE]
                      ,[_SUBMISSION_DATE]
                      ,[DISTRICT]
                      ,[NOTABLE_PERSON_CELL]
                      ,[TODAY_VISIT1]
                      ,[EBCODE]
                      ,[DEVICE_ID]
                      ,[CLUSTER_CODE]
                      ,[LOCALITY_ADDRESS]
                      ,[NOTABLE_PERSON]
                      ,[LISTER_NAME]
                      ,[LISTER_CODE]
                      ,[LOCALITY]
                      ,[AREA_TYPE]
                      ,[TEHSIL]
                      ,[ENDTIME_VISIT]
                      ,[STARTTIME_VISIT1]
                      ,[DEVICE_PHONE_NUMBER]
                      ,[SIM_ID]
                      ,[KEY_]
                      ,[SR_NO_HOUSEHOLD]
                      ,[STRUCTURE_TYPE]
                      ,[STRUCTURE_NO]
                      ,[REMARKS]
                      ,[ADDRESS_OF_HOUSEHOLD]
                      ,[CHILD_PRESENT_STATUS]
                      ,[HHMEMBERS_COUNT]
                      ,[HH_HEAD_NAME]
                      ,[CHILD_SCHOOL_COUNT]
                      ,[CHILDREN]
                      ,[BOYS_COUNT]
                      ,[GIRLSL_COUNT]
                      ,[HHSELECTED]
                      ,[SELECTED_DATE]
                  FROM [Listing_HH]  where CLUSTER_CODE = '" + code + "'";
                  ExportToCSV("List_Monitoring_00", sender, e);
                    
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Downloading Cluster" + code + "')", true);
                }
            }
            else if (e.CommandName == "approve")
            {
                if (e.CommandArgument.ToString().Length > 0)
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = download_tab.Rows[rowIndex];

                    //Fetch value of Code
                    string code = row.Cells[1].Text;
                    string user = row.Cells[0].Text;
                    //string role = Session["Role"].ToString();
                    string updateSql =
                                             @"UPDATE [download_log]
                                               SET 
                                                   [status] = '1'
                                                  ,[approved_date] = @updated
                                             WHERE cluster_code = @cluster and username = @user";

                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
                    {
                        connection.Open();
                        var command = new SqlCommand(updateSql, connection);
                        var args = command.Parameters;
                        args.Add("@updated", DateTime.Now.ToString());
                        args.Add("@cluster", code);
                        args.Add("@user", user);

                        command.ExecuteNonQuery();

                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Approved Cluster " + code + "')", true);
                }
            }


        }

        protected void download_tab_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if(Session["Role"].ToString()=="listmonitor")
            {
                e.Row.Cells[3].Visible = false;
            }
            if (Session["Role"].ToString() == "listadmin")
            {
                e.Row.Cells[2].Visible = false;
            }
        }
       



    }
}
