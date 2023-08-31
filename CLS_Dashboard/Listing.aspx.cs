using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;

namespace CLS_Dashboard
{
    public partial class Listing : System.Web.UI.Page
    {
        public static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

        public string constr;
        public int columnCodeGlobal = 0;



        public enum MessageType { Success, Error, Info, Warning };



        public void connection()
        {

            con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

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

            if (!IsPostBack)
            {
                try
                {
                    //lblUserName.Text = (string)(Session["Assign"]);
                    load_Filters_Data();
                    logFile.log("Listing Data", Session["username"].ToString());
                    rep_bind();


                }
                catch (Exception ex)
                {
                    ShowMessage("Error: Unable to Load filters Data .. ", MessageType.Error);
                }

            }



        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.rep_bind();
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblParentKey.Text = GridView1.SelectedRow.Cells[0].Text;
            // keys.Text = GridView1.SelectedRow.Cells[0].Text;
            int index = GridView1.SelectedRow.RowIndex;
            string ClusterCode = GridView1.SelectedRow.Cells[3].Text;
            string ListerName = GridView1.SelectedRow.Cells[5].Text;
            string message = "<br> <b>Row</b> : " + index + "<br>  <b>Cluster Code:</b> " + ClusterCode + "<br>  <b>Lister Name:</b> " + ListerName;
            lblDelete.Text = message;
            // message_box.Text = message;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "Confirm();", true);

        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            lblParentKey.Text = GridView1.SelectedRow.Cells[0].Text;
            //int index = GridView1.SelectedRow.RowIndex;
            //string ClusterCode = GridView1.SelectedRow.Cells[3].Text;
            //string ListerName = GridView1.SelectedRow.Cells[5].Text;
            //string message = "<b>Deleted!!</b><br> <b>Row</b> : " + index + "<br>  <b>Cluster Code:</b> " + ClusterCode + "<br>  <b>Lister Name:</b> " + ListerName;
            //lblmsg.Text = message;
            try
            {


                using (SqlCommand cmd = new SqlCommand("DELETE FROM Listing_HH WHERE PARENT_KEY = @PARENT_KEY", con))
                {

                    connection();
                    cmd.Connection = con;

                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@PARENT_KEY", lblParentKey.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ShowMessage("Duplicate Record deleted successfully", MessageType.Success);
                    rep_bind();

                }


            }
            catch (Exception ex)
            {
                ShowMessage("Error: Unable to delete Record .. ", MessageType.Error);
            }

            // ShowMessage(message, MessageType.Success);
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

        private void load_Other_Filter_Selected(string Column, string condition, int columnCode)
        {
            logFile.log("Filter------"+condition, Session["username"].ToString());
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

                    cmd.CommandTimeout = 0;
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

                    cmd.CommandTimeout = 0;
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

                    cmd.CommandTimeout = 0;
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

                    cmd.CommandTimeout = 0;
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

                    cmd.CommandTimeout = 0;
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

            rep_bind();
        }

        protected void lstClusterCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 1;

            load_Other_Filter_Selected(" LOCALITY ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 2);


            load_Other_Filter_Selected(" LISTER_NAME ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 3);


            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 4);


            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " CLUSTER_CODE = '" + lstClusterCodes.SelectedItem + "' AND  ", 5);

            rep_bind();
        }

        protected void lstLocality_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 2;

            load_Other_Filter_Selected(" LISTER_NAME ", " LOCALITY = '" + lstLocality.SelectedItem + "' AND  ", 3);

            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " LOCALITY = '" + lstLocality.SelectedItem + "' AND  ", 4);

            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " LOCALITY = '" + lstLocality.SelectedItem + "' AND  ", 5);

            rep_bind();
        }



        protected void lstLister_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 3;

            load_Other_Filter_Selected(" COUNT(SR_NO_HOUSEHOLD) AS Total_HH ", " LISTER_NAME = '" + lstLister_name.SelectedItem + "' AND  ", 4);


            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " LISTER_NAME = '" + lstLister_name.SelectedItem + "' AND  ", 5);


            rep_bind();
        }

        protected void lstTotal_hh_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 4;

            load_Other_Filter_Selected(" SUM(CAST(CHILDREN AS int)) AS Total_children ", " SR_NO_HOUSEHOLD = '" + lstTotal_hh.SelectedItem + "' AND  ", 5);

            rep_bind();
        }

        protected void lstTotal_children_SelectedIndexChanged(object sender, EventArgs e)
        {
            columnCodeGlobal = 5;

            rep_bind();
        }
        private void rep_bind()
        {
            connection();
            string query = "SELECT  [PARENT_KEY],  CONVERT(VARCHAR(10), _SUBMISSION_DATE, 105) as SubmissionDate,    DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,COUNT(SR_NO_HOUSEHOLD) AS Total_HH,SUM(CAST(CHILDREN AS int)) AS Total_children FROM            Listing_HH ";


            string condition = string.Empty;

            if (columnCodeGlobal == 0)
            {

                foreach (ListItem item in lstDistricts.Items)
                {
                    condition += item.Selected ? string.Format("'{0}',", item.Value) : string.Empty;
                }

                if (!string.IsNullOrEmpty(condition) && !condition.Contains("All"))
                {
                    condition = string.Format(" WHERE district IN ({0})", condition.Substring(0, condition.Length - 1));
                }

                else
                {

                    condition = "";
                }

            }

            else if (columnCodeGlobal == 1)
            {

                foreach (ListItem item in lstClusterCodes.Items)
                {
                    condition += item.Selected ? string.Format("'{0}',", item.Value) : string.Empty;
                }

                if (!string.IsNullOrEmpty(condition) && !condition.Contains("All"))
                {
                    condition = string.Format(" WHERE CLUSTER_CODE IN ({0})", condition.Substring(0, condition.Length - 1));
                }

                else
                {

                    condition = "";
                }

            }

            else if (columnCodeGlobal == 2)
            {

                foreach (ListItem item in lstLocality.Items)
                {
                    condition += item.Selected ? string.Format("'{0}',", item.Value) : string.Empty;
                }

                if (!string.IsNullOrEmpty(condition) && !condition.Contains("All"))
                {
                    condition = string.Format(" WHERE LOCALITY IN ({0})", condition.Substring(0, condition.Length - 1));
                }

                else
                {

                    condition = "";
                }

            }

            else if (columnCodeGlobal == 3)
            {

                foreach (ListItem item in lstLister_name.Items)
                {
                    condition += item.Selected ? string.Format("'{0}',", item.Value) : string.Empty;
                }

                if (!string.IsNullOrEmpty(condition) && !condition.Contains("All"))
                {
                    condition = string.Format(" WHERE LISTER_NAME IN ({0})", condition.Substring(0, condition.Length - 1));
                }

                else
                {

                    condition = "";
                }

            }

            else if (columnCodeGlobal == 4)
            {

                foreach (ListItem item in lstTotal_hh.Items)
                {
                    condition += item.Selected ? string.Format("'{0}',", item.Value) : string.Empty;
                }

                if (!string.IsNullOrEmpty(condition) && !condition.Contains("All"))
                {
                    condition = string.Format(" GROUP BY  [PARENT_KEY],_SUBMISSION_DATE, DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key having COUNT(SR_NO_HOUSEHOLD) IN ({0})", condition.Substring(0, condition.Length - 1));

                    condition = condition + " order by CLUSTER_CODE  asc";
                }

                else
                {

                    condition = " GROUP BY  [PARENT_KEY],_SUBMISSION_DATE, DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by CLUSTER_CODE  asc";
                }

            }

            else if (columnCodeGlobal == 5)
            {

                foreach (ListItem item in lstTotal_children.Items)
                {
                    condition += item.Selected ? string.Format("'{0}',", item.Value) : string.Empty;
                }



                if (!string.IsNullOrEmpty(condition) && !condition.Contains("All"))
                {
                    condition = string.Format(" GROUP BY [PARENT_KEY],_SUBMISSION_DATE,DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key having SUM(CAST(CHILDREN AS int)) IN ({0})", condition.Substring(0, condition.Length - 1));

                    condition = condition + " order by CLUSTER_CODE  asc";
                }

                else
                {

                    condition = " GROUP BY [PARENT_KEY],_SUBMISSION_DATE,DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by CLUSTER_CODE  asc";
                }

            }



            using (SqlConnection conn = new SqlConnection(constr))
            {
                if (columnCodeGlobal == 4)
                {


                }

                else if (columnCodeGlobal == 5)
                {


                }

                else
                {
                    condition = condition + " GROUP BY [PARENT_KEY],_SUBMISSION_DATE,DISTRICT, CLUSTER_CODE, LOCALITY, LISTER_NAME,Listing_HH.parent_key order by CLUSTER_CODE  asc";
                }

                using (SqlCommand cmd = new SqlCommand(query + condition))
                {

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {

                        cmd.Connection = con;

                        cmd.CommandTimeout = 0;
                        using (DataSet ds = new DataSet())
                        {
                            da.Fill(ds);

                            GridView1.DataSource = ds;
                            GridView1.SortedDescendingCellStyle.BackColor = System.Drawing.Color.LightCoral;
                            GridView1.DataBind();
                        }
                    }
                }
            }

            //SqlDataAdapter da = new SqlDataAdapter(query, con);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            //GridView1.DataSource = ds;
            //GridView1.DataBind();
        }

        public List<string> DupStrings = new List<string>();
        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView1.HeaderRow.Cells[0].Visible = false;
                e.Row.Cells[0].Visible = false;

                if (DupStrings.Contains(e.Row.Cells[3].Text))
                {
                    if (e.Row.RowIndex > 0)
                    {
                        var prevRow = GridView1.Rows[e.Row.RowIndex - 1];
                        prevRow.BackColor = System.Drawing.Color.LightCoral;
                        prevRow.ForeColor = System.Drawing.Color.White;
                    }

                    e.Row.BackColor = System.Drawing.Color.LightCoral;
                    e.Row.ForeColor = System.Drawing.Color.White;

                    if (Session["Role"].ToString() == "listadmin")
                    {
                        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
                        e.Row.Attributes["style"] = "cursor:pointer";
                        if (e.Row.RowIndex > 0)
                        {
                            var prevRow = GridView1.Rows[e.Row.RowIndex - 1];
                            prevRow.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + prevRow.RowIndex);
                            prevRow.Attributes["style"] = "cursor:pointer";
                        }

                    }
                }
                else
                {

                    e.Row.Attributes["onmouseover"] = "onMouseOver('" + (e.Row.RowIndex + 1) + "')";
                    e.Row.Attributes["onmouseout"] = "onMouseOut('" + (e.Row.RowIndex + 1) + "')";
                    // Add text to list
                    DupStrings.Add(e.Row.Cells[3].Text);
                }
            }
        }




    }
}
