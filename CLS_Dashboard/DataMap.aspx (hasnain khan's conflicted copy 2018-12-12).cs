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
    public partial class DataMap : System.Web.UI.Page
    {
      
        
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

        protected void GetLongLatFromDataBase()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 500 [recordgps_first_Latitude] as Latitude1 ,[recordgps_first_Longitude] as Longitude1 ,[recordgps_second_Latitude] as Latitude2,[recordgps_second_Longitude] as Longitude2,[recordgps_third_Latitude] as Latitude3,[recordgps_third_Longitude] as Longitude3, district, cluster_code, household_ID, G3_Supervisor_1 as Supervisor, G2_Interviewer_1 as Enumerator, today_Visit1 FROM[dbo].[Main]"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.Fill(dt);
                        BuildScript(dt);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else
            {
                Session["lblHeadingName"] = "<b>CLS</b>-DATA-MAP";
                
                

                if (!IsPostBack)
                {

                    LoadFiltersData();
                    GetLongLatFromDataBase();
                   

                    


                    // create a line of JavaScript for marker on map for this record
//                    try
//                    {
//                        string Locations = string.Empty;


//                        // construct the final script
//                        js.Text = @"<script  type='text/javascript'>
//                        window.onload = function() 
//                        {
//                            if (GBrowserIsCompatible()) {
//                            var map = new GMap2(document.getElementById('map_canvas'));
//                            map.setCenter(new GLatLng(" + 24.75123 + "," + 67.92105 + ")" + ", 7);" + Locations + @"
//                            map.setUIToDefault();
//                            }
//                        }
//                        </script> ";
//                    }
//                    catch (Exception)
//                    {

//                    }
                }
            }
        }

        private void LoadFiltersData()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT district FROM [dbo].[Main]"))
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

                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT G3_Supervisor_1 as Supervisor FROM [dbo].[Main] WHERE G3_Supervisor_1 != 'NULL'"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                                lstSupervisor.Items.Add(dt.Rows[i].Field<string>(0));
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void BuildScript(DataTable dt)
        {
            String Locations = "";
            double longitude1 = 0;
            double latitude1 = 0;
            double avglongitude1 = 0;
            double avglatitude1 = 0;
            string district = "";
            string Cluster_Code = "";
            string household_ID = "";
            string Supervisor = "";
            string Enumerator = "";
            DateTime VisitDate = new DateTime();
            int totalPts = 0;
            int markerNo = 1;

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        latitude1 = dt.Rows[i].Field<double>(0);
                        longitude1 = dt.Rows[i].Field<double>(1);
                        district = dt.Rows[i].Field<string>(6);
                        Cluster_Code = dt.Rows[i].Field<string>(7);
                        household_ID = dt.Rows[i].Field<string>(8);
                        Supervisor = dt.Rows[i].Field<string>(9);
                        Enumerator = dt.Rows[i].Field<string>(10);
                        VisitDate = dt.Rows[i].Field<DateTime>(11);
                    }
                    catch (Exception)
                    {
                        latitude1 = 0;
                        longitude1 = 0;
                    }

                    // if first long, lat values are NULL, check the next values
                    try
                    {
                        if (latitude1 == 0 && longitude1 == 0)
                        {
                            latitude1 = dt.Rows[i].Field<double>(2);
                            longitude1 = dt.Rows[i].Field<double>(3);
                        }
                    }
                    catch (Exception)
                    {
                        latitude1 = 0;
                        longitude1 = 0;
                    }

                    // if second long, lat values are NULL, check the next values
                    try
                    {
                        if (latitude1 == 0 && longitude1 == 0)
                        {
                            latitude1 = dt.Rows[i].Field<double>(4);
                            longitude1 = dt.Rows[i].Field<double>(5);
                        }
                    }
                    catch (Exception)
                    {
                        latitude1 = 0;
                        longitude1 = 0;
                    }

                    avglatitude1 += latitude1;
                    avglongitude1 += longitude1;

                    if (latitude1 != 0 && longitude1 != 0)
                        totalPts++;

                    // create a line of JavaScript for marker on map for this record
                    Locations += Environment.NewLine + "var marker" + markerNo.ToString() + " = new GMarker(new GLatLng(" + latitude1 + "," + longitude1 + "), { title:'" + "District: " + district + ",  CC_ID: " + Cluster_Code + ",  HH_ID :" + household_ID + ", Supervisor: " + Supervisor + ", Enumerator: " + Enumerator + ", Visit_Date: " + VisitDate.ToString("dd-MM-yyyy,HH:mm:ss") + "'})";
                    Locations += Environment.NewLine + " map.addOverlay(marker" + markerNo.ToString() + ");";

                    Locations += "GEvent.addListener(marker" + markerNo.ToString() + ", 'click', function() {";
                    Locations += "marker" + markerNo.ToString() + ".openInfoWindow('" + latitude1 + "," + longitude1 + "');";
                    Locations += "});";
                    markerNo++;
                }

                avglongitude1 = avglongitude1 / totalPts;
                avglatitude1 = avglatitude1 / totalPts;

                // construct the final script
                js.Text = @"<script  type='text/javascript'>
                        function LoadMap()
                        {
                            if (GBrowserIsCompatible()) {
                            var map = new GMap2(document.getElementById('map_canvas'));
                            map.setCenter(new GLatLng(" + avglatitude1 + "," + avglongitude1 + ")" + ", 7);" + Locations + @"
                            map.setUIToDefault();
                            }
                        }
                        window.onload = function () {LoadMap();}

                        var prm = Sys.WebForms.PageRequestManager.getInstance();    
                        prm.add_initializeRequest(InitializeRequest);
                        prm.add_endRequest(EndRequest);

                        function InitializeRequest(sender, args) {      
                        }

                        // fires after the partial update of UpdatePanel
                        function EndRequest(sender, args) {
                            LoadMap();

                        }


                        </script>
            <script src='http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=AIzaSyAAKcEw_ct_k7UMmXMigNZqJfw5vIRX0fM' type='text/javascript'>
                </script>";
            }
            catch (Exception)
            {

            }
        }

        private bool IsDistrictAllFilter(string[] district_filter)
        {
            foreach (string district in district_filter)
            {
                if (district == "All")
                    return true;
            }
            return false;
        }

        protected string PrepareFilteredString(string inputString, string[] filterstring, string filterName)
        {
            try
            {
                inputString += filterName + " = ";

                for (int i = 0; i < filterstring.Length; i++)
                {
                    if (i == filterstring.Length - 1)
                        inputString += "'" + filterstring[i] + "'";
                    else
                        inputString += "'" + filterstring[i] + "' OR " + filterName + " = ";
                }
            }
            catch (Exception)
            {

            }

            return inputString;
        }

        protected void Filter_Selected(object sender, EventArgs e)
        {
            try
            {
                string cmdString = "SELECT TOP 500 [recordgps_first_Latitude] as Latitude1 ,[recordgps_first_Longitude] as Longitude1 ,[recordgps_second_Latitude] as Latitude2,[recordgps_second_Longitude] as Longitude2,[recordgps_third_Latitude] as Latitude3,[recordgps_third_Longitude] as Longitude3, district, cluster_code, household_ID, G3_Supervisor_1 as Supervisor, G2_Interviewer_1 as Enumerator, today_Visit1  FROM[dbo].[Main]";
                bool districtAllFilter = true;

                int i = 0;
                int selectedItem = this.lstDistricts.SelectedIndex;
                int[] selectedDistrictIndexes = lstDistricts.GetSelectedIndices();
                string[] selectedDistrictValues = new string[selectedDistrictIndexes.Length];

                foreach (int index in selectedDistrictIndexes)
                    selectedDistrictValues[i++] = lstDistricts.Items[index].ToString();

                selectedItem = this.lstSupervisor.SelectedIndex;
                int[] selectedSupervisorIndexes = lstSupervisor.GetSelectedIndices();
                string[] selectedSupervisorValues = new string[selectedSupervisorIndexes.Length];

                i = 0;
                foreach (int index in selectedSupervisorIndexes)
                    selectedSupervisorValues[i++] = lstSupervisor.Items[index].ToString();

                if (selectedDistrictValues.Length == 0)
                    districtAllFilter = true;
                else
                    districtAllFilter = IsDistrictAllFilter(selectedDistrictValues);

                if (selectedDistrictValues.Length > 0 && !districtAllFilter && lstDistricts.SelectedItem.Text != "All")
                    cmdString = PrepareFilteredString(cmdString + " WHERE (", selectedDistrictValues, "district") + ")";

                if (selectedSupervisorValues.Length > 0 && lstSupervisor.SelectedItem.Text != "All")
                {
                    if (districtAllFilter)
                        cmdString = PrepareFilteredString(cmdString + " WHERE (", selectedSupervisorValues, "G3_Supervisor_1") + ")";
                    else
                        cmdString = PrepareFilteredString(cmdString + " AND (", selectedSupervisorValues, "G3_Supervisor_1") + ")";
                }

                using (SqlCommand cmd = new SqlCommand(cmdString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                         sda.SelectCommand = cmd;

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            BuildScript(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}