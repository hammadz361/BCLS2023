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

namespace CLS_Dashboard
{
    public partial class DataMap : System.Web.UI.Page
    {
        static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        //static SqlConnection consp = new SqlConnection(ConfigurationManager.ConnectionStrings["SPCon"].ConnectionString);

        //keyboard shortcut Ctrl + M, P to expand or Ctrl + M, O to collapse.

        //new work

        public class Location
        {
            public string Name { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string District { get; set; }
            public string ClusterCode { get; set; }
            public string Household_ID { get; set; }
            public string Supervisor { get; set; }
            public string VisitDate { get; set; }
            public string Household_Status { get; set; }
            public string RecordImage { get; set; }
            public string DataSource { get; set; } // Add the DataSource property
        }

        // Fetch locations from the database
        public List<Location> Locations { get; set; }


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

                    // Fetch locations from the database


                    Locations = FetchEnumeratorData();

                    //GetDatafromMain();
                    LoadFiltersData();

                    // Bind the merged data to the map
                    BindMap();
                  
                }
            }
        }

        private void LoadFiltersData()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("select DISTINCT district FROM [dbo].[Main]"))
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

                            }


                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT G3_Supervisor_1 as Supervisor FROM [dbo].[Main] WHERE G3_Supervisor_1 != 'NULL'"))
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
                                lstSupervisor.Items.Add(dt.Rows[i].Field<string>(0));
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT cluster_code FROM [dbo].[Main] WHERE cluster_code is not NULL"))
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
                                lstClusterCodes.Items.Add(dt.Rows[i].Field<string>(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Error Message: {0}\\n\\n", ex.Message);

                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert(\"" + "14." + message + "\");", true);
            }

            finally
            {
                if (con.State != ConnectionState.Closed) { con.Close(); }
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
        private void load_Filter_Supervisor(string Filter_Data)
        {
            lstSupervisor.Items.Clear();
            lstSupervisor.Items.Add("All");
            lstSupervisor.SelectedIndex = 0;

            string query = "SELECT DISTINCT G3_Supervisor_1 FROM [Main] " + Filter_Data + ")";
            if (Filter_Data == "")
                query = query.Replace(")", "");
            if (Filter_Data.Contains("[District]")) { Filter_Data = Filter_Data.Replace("[District]", "district"); }

            using (SqlCommand cmd = new SqlCommand(query))
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
                            lstSupervisor.Items.Add(dt.Rows[i].Field<string>(0));


                    }
                }
            }
        }
        private void load_Filter_ClusterCodes(string Filter_Data)
        {
            lstClusterCodes.Items.Clear();
            lstClusterCodes.Items.Add("All");
            lstClusterCodes.SelectedIndex = 0;
            string query = "  SELECT  DISTINCT CAST(cluster_code AS numeric) AS CCode FROM [Main] " + Filter_Data;
            if (Filter_Data != "")
                query = query + ")" + " order by CAST(cluster_code AS numeric)";


            if (Filter_Data.Contains("[District]")) { Filter_Data = Filter_Data.Replace("[District]", "district"); }
            if (Filter_Data.Contains("[Supervisor]")) { Filter_Data = Filter_Data.Replace("[Supervisor]", "G3_Supervisor_1"); }

            using (SqlCommand cmd = new SqlCommand(query))
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
                            lstClusterCodes.Items.Add(dt.Rows[i].ItemArray[0].ToString());


                    }
                }
            }
        }
    
        private void loadMapData()
        {
           
            // Initialize the Locations list if it is null
            if (Locations == null)
            {
                Locations = new List<Location>();
            }
            else
            {
                // Clear the existing location data
                Locations.Clear();
            }

            if (Chk_Main.Checked)
            {
                // Fetch monitor data from the database
                List<Location> enumeratorData = FetchEnumeratorData();

                // Merge previous data and monitor data
                Locations.AddRange(enumeratorData);
            }

            if (Chk_Monitors.Checked)
            {
                // Fetch monitor data from the database
                List<Location> monitorData = FetchMonitorData();

                // Merge previous data and monitor data
                Locations.AddRange(monitorData);
            }

            if (Chk_Observers.Checked)
            {
                // Fetch monitor data from the database
                List<Location> observerData = FetchObserverData();

                // Merge previous data and monitor data
                Locations.AddRange(observerData);
            }

            if (Chk_Supervisors.Checked)
            {
                // Fetch monitor data from the database
                List<Location> supervisorData = FetchSupervisorData();

                // Merge previous data and monitor data
                Locations.AddRange(supervisorData);
            }

            // Bind the merged data to the map
            BindMap();
        }

        private List<Location> FetchEnumeratorData()
        {
            List<Location> enumeratorData = new List<Location>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            {
                string query = "SELECT TOP " + txtTopSQL.Text + " CAST([recordgps_first_Latitude] AS FLOAT) AS Latitude1, CAST([recordgps_first_Longitude] AS FLOAT) AS Longitude1, [recordgps_second_Latitude] AS Latitude2, [recordgps_second_Longitude] AS Longitude2, [recordgps_third_Latitude] AS Latitude3, [recordgps_third_Longitude] AS Longitude3, district, cluster_code, household_ID, G3_Supervisor_1 AS Supervisor, G2_Interviewer_1 AS Enumerator, today_Visit1 FROM [dbo].[Main] WHERE recordgps_first_Latitude IS NOT NULL";

                // Check if any districts are selected and not empty
                if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                {
                    query += " AND district = @district";
                }

                // Check if any supervisors are selected and not empty
                if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                {
                    query += " AND G3_Supervisor_1 = @supervisor";
                }

                // Check if any cluster codes are selected and not empty
                if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                {
                    query += " AND cluster_code = @cluster";
                }

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    // Set parameter values based on the selected items
                    if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@district", lstDistricts.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@supervisor", lstSupervisor.SelectedValue);
                    }

                    if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@cluster", lstClusterCodes.SelectedValue);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double latitude;
                            double longitude;

                            if (reader["Latitude1"] != DBNull.Value && double.TryParse(reader["Latitude1"].ToString(), out latitude) && reader["Longitude1"] != DBNull.Value && double.TryParse(reader["Longitude1"].ToString(), out longitude))
                            {
                                string name = reader["Enumerator"].ToString();
                                latitude = Convert.ToDouble(reader["Latitude1"]);
                                longitude = Convert.ToDouble(reader["Longitude1"]);
                                string district = reader["district"].ToString();
                                string cluster_code = reader["cluster_code"].ToString();
                                string household_id = reader["household_ID"].ToString();
                                string supervisor = reader["Supervisor"].ToString();
                                string visit_date = reader["today_Visit1"].ToString();

                                enumeratorData.Add(new Location { Name = name, Latitude = latitude, Longitude = longitude, District = district, ClusterCode = cluster_code, Household_ID = household_id, Supervisor = supervisor, VisitDate = visit_date, DataSource = "Enumerator" });
                            }
                        }
                    }
                }
            }

            if (enumeratorData.Count == 0)
            {
                ShowToastNotification("No data found Enumerator");
            }

            return enumeratorData;
        }


        private List<Location> FetchMonitorData()
        {


            List<Location> monitorData = new List<Location>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            {
                String query = "select top " + txtTopSQL.Text + " RECORDGPS_INTERVIEW_LAT as Latitude1 ,[RECORDGPS_INTERVIEW_LNG] as Longitude1,district,[MONITOR],  cluster_code, household_ID, record_image FROM[dbo].Monitoring WHERE 1 = 1";

                // Check if any districts are selected and not empty
                if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                {
                    query += " AND district = @district";
                }

                //---------------No supervisor in monitor

                //// Check if any supervisors are selected and not empty
                //if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                //{
                //    query += " AND G3_Supervisor_1 = @supervisor";
                //}

                // Check if any cluster codes are selected and not empty
                if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                {
                    query += " AND cluster_code = @cluster";
                }

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    // Set parameter values based on the selected items
                    if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@district", lstDistricts.SelectedValue);
                    }

                    //---------------No supervisor in monitor

                    //if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                    //{
                    //    command.Parameters.AddWithValue("@supervisor", lstSupervisor.SelectedValue);
                    //}

                    if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@cluster", lstClusterCodes.SelectedValue);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double latitude;
                            double longitude;

                            if (reader["Latitude1"] != DBNull.Value && double.TryParse(reader["Latitude1"].ToString(), out latitude) && reader["Longitude1"] != DBNull.Value && double.TryParse(reader["Longitude1"].ToString(), out longitude))
                            {
                                string name = reader["MONITOR"].ToString();
                                latitude = Convert.ToDouble(reader["Latitude1"]);
                                longitude = Convert.ToDouble(reader["Longitude1"]);
                                string district = reader["district"].ToString();
                                string cluster_code = reader["cluster_code"].ToString();
                                string household_id = reader["household_ID"].ToString();
                                string record_image = "";
                                if (reader["record_image"] != DBNull.Value)
                                {
                                    //record_image = Server.MapPath("~/replace/assets/" + reader["record_image"].ToString());
                                    record_image = "assets/" + reader["record_image"].ToString().Replace("\\", "/");
                                }
                                else {
                                    record_image = "assets/img/No-Image.jpg";
                                }
                                

                                monitorData.Add(new Location { Name = name, Latitude = latitude, Longitude = longitude, District = district, ClusterCode = cluster_code, Household_ID = household_id, RecordImage = record_image, DataSource = "Monitor" });
                            }

                        }
                    }
                }
            }

            if (monitorData.Count == 0)
            {
                ShowToastNotification("No data found Monitor");
            }

            return monitorData;
        }

        private List<Location> FetchObserverData()
        {


            List<Location> observerData = new List<Location>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString))
            {
                String query = "SELECT distinct top " + txtTopSQL.Text + " RECORDGPS_INTERVIEW_LAT as Latitude1 ,[RECORDGPS_INTERVIEW_LNG] as Longitude1, district, Observer, cluster_code, household_ID FROM [dbo].Observer1 x inner join Observer x1 on x._URI=x1._URI  where RECORDGPS_INTERVIEW_LAT is not null";

                // Check if any districts are selected and not empty
                if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                {
                    query += " AND district = @district";
                }

                //---------------No supervisor in observer

                //if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                //{
                //    command.Parameters.AddWithValue("@supervisor", lstSupervisor.SelectedValue);
                //}

                // Check if any cluster codes are selected and not empty
                if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                {
                    query += " AND cluster_code = @cluster";
                }

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    // Set parameter values based on the selected items
                    if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@district", lstDistricts.SelectedValue);
                    }

                    //---------------No supervisor in observer

                    //if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                    //{
                    //    command.Parameters.AddWithValue("@supervisor", lstSupervisor.SelectedValue);
                    //}

                    if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@cluster", lstClusterCodes.SelectedValue);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {   
                            // Create a new location object and populate its properties
                            double latitude;
                            double longitude;

                            if (reader["Latitude1"] != DBNull.Value && double.TryParse(reader["Latitude1"].ToString(), out latitude) && reader["Longitude1"] != DBNull.Value && double.TryParse(reader["Longitude1"].ToString(), out longitude))
                            {
                                string name = reader["Observer"].ToString();
                                latitude = Convert.ToDouble(reader["Latitude1"]);
                                longitude = Convert.ToDouble(reader["Longitude1"]);
                                string district = reader["district"].ToString();
                                string cluster_code = reader["cluster_code"].ToString();
                                string household_id = reader["household_ID"].ToString();


                                observerData.Add(new Location { Name = name, Latitude = latitude, Longitude = longitude, District = district, ClusterCode = cluster_code, Household_ID = household_id, DataSource = "Observer" });
                            }

                        }
                    }
                }
            }
            if (observerData.Count == 0)
            {
                ShowToastNotification("No data found Observer");
            }

            return observerData;
        }

        private List<Location> FetchSupervisorData()
        {


            List<Location> supervisorData = new List<Location>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SPCon"].ConnectionString))
            {
                String query = "select distinct top " + txtTopSQL.Text + "  Location, Cluster,HH_id, HH_status, Timestamp from HH_App where Timestamp <> 'NULL'";

                // Check if any districts are selected and not empty
                //if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                //{
                //    query += " AND district = @district";
                //}

                //---------------No supervisor in monitor
                //if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                //{
                //    command.Parameters.AddWithValue("@supervisor", lstSupervisor.SelectedValue);
                //}

                // Check if any cluster codes are selected and not empty
                if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                {
                    query += " AND Cluster = @cluster";
                }

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    //-------No supervisor and district

                    // Set parameter values based on the selected items
                    //if (!string.IsNullOrEmpty(lstDistricts.SelectedValue) && lstDistricts.SelectedValue != "All")
                    //{
                    //    command.Parameters.AddWithValue("@district", lstDistricts.SelectedValue);
                    //}

                    //if (!string.IsNullOrEmpty(lstSupervisor.SelectedValue) && lstSupervisor.SelectedValue != "All")
                    //{
                    //    command.Parameters.AddWithValue("@supervisor", lstSupervisor.SelectedValue);
                    //}

                    if (!string.IsNullOrEmpty(lstClusterCodes.SelectedValue) && lstClusterCodes.SelectedValue != "All")
                    {
                        command.Parameters.AddWithValue("@cluster", lstClusterCodes.SelectedValue);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new location object and populate its properties

                            if (reader["Location"] != DBNull.Value)
                            {

                                string latlong = reader["Location"].ToString(); ;
                               
                                // Split the data based on the delimiter (hyphen in this case)
                                string[] parts = latlong.Split('-');

                                // Extract the latitude and longitude values from the splitted parts
                                double latitude = Convert.ToDouble(parts[1]);
                                double longitude = Convert.ToDouble(parts[0]);
                                string cluster_code = reader["Cluster"].ToString();
                                string household_id = reader["HH_id"].ToString();
                                string status = reader["HH_status"].ToString();
                                string timestamp = reader["Timestamp"].ToString();


                                supervisorData.Add(new Location { Latitude = latitude, Longitude = longitude, ClusterCode = cluster_code, Household_ID = household_id, VisitDate = timestamp, DataSource = "Supervisor" });
                            }

                        }
                    }
                }
            }

            if (supervisorData.Count == 0)
            {
                ShowToastNotification("No data found Supervisor");
            }
            return supervisorData;
        }

        private void BindMap()
        {
            // Convert the list of locations to a JSON string
            string locationsJson = ConvertLocationsToJson(Locations);

            // Register a startup script to initialize the map and display the pins
            string script = @"
        <script>
            function initializeMap() {
var clicked = false;
                  var locationsJson = '" + locationsJson + @"';
                var locations = JSON.parse(locationsJson);

                var mapOptions = {
                    center: getCenter(locations), // Set the center of the map
                    zoom: 10 // Set the initial zoom level
                };

                var map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

                // Loop through the locations and create a marker for each
                for (var i = 0; i < locations.length; i++) {
                    var location = locations[i];
                    
                    // Set the pin color based on the data source
                    var PointColor = 'http://maps.google.com/mapfiles/ms/icons/green-dot.png';

                        switch(location.DataSource) {
                          case 'Enumerator':
                           PointColor = 'http://maps.google.com/mapfiles/ms/icons/green-dot.png';
                            break;
                          case 'Monitor':
                             PointColor = 'http://maps.google.com/mapfiles/ms/icons/yellow-dot.png';
                            break;
                            case 'Observer':
                             PointColor = 'http://maps.google.com/mapfiles/ms/icons/orange-dot.png';
                            break;
                            case 'Supervisor':
                             PointColor = 'http://maps.google.com/mapfiles/ms/icons/purple-dot.png';
                            break;
                          default:
                            PointColor = 'http://maps.google.com/mapfiles/ms/icons/green-dot.png';
                        }

                    var marker = new google.maps.Marker({
                        position: new google.maps.LatLng(location.Latitude, location.Longitude),
                        map: map,
                        title: location.Name,
                        icon: PointColor
                    });

                    // Create an InfoWindow for the marker

                    var infoWindow = createInfoWindow(location);

                    // Show the InfoWindow when marker is clicked
                    attachInfoWindowToMarker(map, marker, infoWindow);

                }
            }

            function getCenter(locations) {
                // Calculate the center of the locations using their average latitude and longitude
                var totalLat = 0;
                var totalLng = 0;

                for (var i = 0; i < locations.length; i++) {
                    var location = locations[i];
                    totalLat += location.Latitude;
                    totalLng += location.Longitude;
                }

                var avgLat = totalLat / locations.length;
                var avgLng = totalLng / locations.length;

                return new google.maps.LatLng(avgLat, avgLng);
            }

            function loadGoogleMaps() {
             
                var script = document.createElement('script');
                script.type = 'text/javascript';
                script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyBZRLQRqsxZUjHaZ1H6PSTMK3wAV2lJrtY&callback=initializeMap';
                document.body.appendChild(script);
            }

            function createInfoWindow(location) {
                  var contentString = '';

//if(location.RecordImage){
//alert(location.RecordImage)
//}

switch(location.DataSource) {
                          case 'Enumerator':
                           contentString = '<h3>' + location.Name + '</h3>' +
                                            '<p>Latitude: ' + location.Latitude + '</p>' +
                                            '<p>Longitude: ' + location.Longitude + '</p>' +
                                            '<p>District: ' + location.District + '</p>' +
                                            '<p>ClusterCode: ' + location.ClusterCode + '</p>' +
                                            '<p>Household_ID: ' + location.Household_ID + '</p>' +
                                            '<p>Supervisor: ' + location.Supervisor + '</p>' +
                                            '<p>VisitDate: ' + location.VisitDate + '</p>';
                            break;
                          case 'Monitor':
                           contentString = '<h3>' + location.Name + '</h3>' +
                                            '<p>Latitude: ' + location.Latitude + '</p>' +
                                            '<p>Longitude: ' + location.Longitude + '</p>' +
                                            '<p>District: ' + location.District + '</p>' +
                                            '<p>ClusterCode: ' + location.ClusterCode + '</p>' +
                                            '<p>Household_ID: ' + location.Household_ID + '</p>' +
                                            '<img src=""' + location.RecordImage + '"" style=""width: 100px; height: 100px;"">' +
                                            '<br /><input type=""button"" class=""btn btn-primary"" value=""View Image"" onclick=""viewImage(\'' + location.RecordImage + '\');"">';
                            break;
                            case 'Observer':
                           contentString = '<h3>' + location.Name + '</h3>' +
                                            '<p>Latitude: ' + location.Latitude + '</p>' +
                                            '<p>Longitude: ' + location.Longitude + '</p>' +
                                            '<p>District: ' + location.District + '</p>' +
                                            '<p>ClusterCode: ' + location.ClusterCode + '</p>' +
                                            '<p>Household_ID: ' + location.Household_ID + '</p>';
                                            
                            break;
                            case 'Supervisor':
                           contentString = '<h3>' + location.Household_ID + '</h3>' +
                                            '<p>Latitude: ' + location.Latitude + '</p>' +
                                            '<p>Longitude: ' + location.Longitude + '</p>' +
                                            '<p>District: ' + location.District + '</p>' +
                                            '<p>ClusterCode: ' + location.ClusterCode + '</p>' +
                                            '<p>VisitDate: ' + location.VisitDate + '</p>';
                            break;
                          default:
                            contentString = '<h3>No Selection Found</h3>';
                        }

                return new google.maps.InfoWindow({
                    content: contentString
                });
            }

          function attachInfoWindowToMarker(map, marker, infowindow) {


                google.maps.event.addListener(marker, 'mouseover', function() {
                    if (!clicked) {
                        infowindow.open(map, marker);
                    }
                });

                google.maps.event.addListener(marker, 'mouseout', function() {
                    if (!clicked) {
                        infowindow.close();
                    }
                });

                google.maps.event.addListener(marker, 'click', function() {
                    clicked = true;
                    infowindow.open(map, marker);
                });
                google.maps.event.addListener(infowindow,'closeclick',function() {
                        clicked = false;
                    })
            };

               function viewImage(imagePath) {
                 var modalImage = document.getElementById('modalImage');
                    modalImage.src = imagePath;
                $('#imageModal').modal('show');

                  }


            loadGoogleMaps();

            
        </script>
    ";

            // Register the startup script
            ClientScript.RegisterStartupScript(GetType(), "MapScript", script, false);
        }

        private string ConvertLocationsToJson(List<Location> locations)
        {
            // Convert the list of locations to a JSON string using a JSON serializer
            // You can use a library like Newtonsoft.Json or the built-in System.Text.Json

            // Example using Newtonsoft.Json:
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(locations);

            return json;
        }

        protected void Filter_Selected(object sender, EventArgs e) {
            loadMapData();
        }
        protected void Chk_Main_CheckedChanged(object sender, EventArgs e)
        {
            loadMapData();
        }
        protected void Chk_Monitors_CheckedChanged(object sender, EventArgs e)
        {
            loadMapData();
        }
        protected void Chk_Observers_CheckedChanged(object sender, EventArgs e)
        {
            loadMapData();
        }
        protected void Chk_Supervisors_CheckedChanged(object sender, EventArgs e)
        {
            loadMapData();
        }

        protected void txtTopSQL_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(txtTopSQL.Text, out value))
            {
                if (value < 100)
                {
                    ShowToastNotification("Number cannot be less than 100");
                    txtTopSQL.Text = "100";
                }
                else if (value > 200)
                {
                    ShowToastNotification("Number cannot be greater than 200");
                    txtTopSQL.Text = "200";
                }
            }
            else
            {
                txtTopSQL.Text = "100";
            }
        }

        private void ShowToastNotification(string message)
        {
            string script = @"<script>
                         alert('" + message + @"');
                      </script>";

            ScriptManager.RegisterStartupScript(this, typeof(Page), "ToastNotification", script, false);
        }


    }
}