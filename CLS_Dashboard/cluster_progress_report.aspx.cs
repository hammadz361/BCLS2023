using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Supervisor_asp
{
    public partial class cluster_progress_report : System.Web.UI.Page
    {

        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);

        public DataSet ds;

        protected HtmlForm form1;

        protected ScriptManager ScriptManager1;

        protected UpdatePanel myUpdatePanel;

        protected GridView GridView1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                BindDataList();
            }
        }

        protected void BindDataList()
        {
            string text = "";
            string a = base.Request.QueryString["type"];
            string value = base.Request.QueryString["code"];
            ds = new DataSet();
            text = ((!(a == "s")) ? ((!(a == "e")) ? "" : "ENUMERATOR_WISE_CLUSTER_PROGRESS") : "SUPERVISOR_WISE_CLUSTER_PROGRESS_REPORT");
            if (text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, base.GetType(), "alertMessage", "alert('Check url parameter not found')", true);
            }
            else
            {
                using (SqlCommand sqlCommand = new SqlCommand(text, con))
                {
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        try
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            if (a == "s")
                            {
                                sqlCommand.Parameters.Add(new SqlParameter("@Supervisor_code", Convert.ToInt32(value)));
                            }
                            if (a == "e")
                            {
                                sqlCommand.Parameters.Add(new SqlParameter("@cluster", value));
                            }
                            sqlDataAdapter.Fill(ds);
                            Session["ds"] = ds;
                            GridView1.DataSource = ds;
                            GridView1.DataBind();
                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, base.GetType(), "alertMessage", "alert('" + ex.Message + "')", true);
                        }
                    }
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            DataSet dataSource = (DataSet)Session["ds"];
            GridView1.DataSource = dataSource;
            GridView1.DataBind();
        }
    }
}