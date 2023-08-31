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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;

namespace CLS_Dashboard
{
    public partial class DownloadTables : System.Web.UI.Page
    {
        public static string Database_Name = ConfigurationManager.AppSettings["Database_Name"];
        public static string code = "";
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCLSCon"].ConnectionString);
        public static string strQuery = "";

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

                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Detail"), new DataColumn("Code") });
                    if (Assign.Contains("A") || Assign.Contains("Allow")) { dt.Rows.Add(1, "Roster Details", "A"); }
                    if (Assign.Contains("A1") || Assign.Contains("Allow")) { dt.Rows.Add(2, "Family tree", "A1"); }
                    if (Assign.Contains("B") || Assign.Contains("Allow")) { dt.Rows.Add(3, "Household Asset details", "B"); }
                    if (Assign.Contains("Main") || Assign.Contains("Allow")) { dt.Rows.Add(4, "Adult Questionnaire", "Main"); }
                    if (Assign.Contains("C") || Assign.Contains("Allow")) { dt.Rows.Add(5, "Child questionnaire", "C"); }
                    if (Assign.Contains("Monitoring") || Assign.Contains("Allow")) { dt.Rows.Add(6, "Monitoring Data", "Monitoring"); }
                    if (Assign.Contains("Observer") || Assign.Contains("Allow")) { dt.Rows.Add(7, "Observers data", "Observer"); }
                    if (Assign.Contains("IndustriesTextReview") || Assign.Contains("Allow")) { dt.Rows.Add(8, "Industries TextReview", "IndustriesTextReview"); }
                    if (Assign.Contains("ProfessionTextReview") || Assign.Contains("Allow")) { dt.Rows.Add(9, "Occupation TextReview", "ProfessionTextReview"); }
                    if (Assign.Contains("ToolsTextReview") || Assign.Contains("Allow")) { dt.Rows.Add(10, "Tools TextReview", "ToolsTextReview"); }
                    if (Assign.Contains("SummaryReport") || Assign.Contains("Allow")) { dt.Rows.Add(11, "Summarized Report", "SummaryReport"); }
                    if (Assign.Contains("MistakesCount") || Assign.Contains("Allow")) { dt.Rows.Add(12, "Mistake Count Report", "MistakesCount"); }
                    if (Assign.Contains("MistakeOutlier") || Assign.Contains("Allow")) { dt.Rows.Add(13, "Mistake Outlier Report", "MistakeOutlier"); }
                    if (Assign.Contains("OthersFieldSummary") || Assign.Contains("Allow")) { dt.Rows.Add(14, "Others Field Wise Report", "OthersFieldSummary"); }
                    if (Assign.Contains("OthersSummary") || Assign.Contains("Allow")) { dt.Rows.Add(15, "Others Enumerators Report", "OthersSummary"); }
                    if (Assign.Contains("HHSummary") || Assign.Contains("Allow")) { dt.Rows.Add(16, "Household Size Report", "HHSummary"); }
                    if (Assign.Contains("OthersCodes") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Other Coding", "OthersCodes"); }
                    if (Assign.Contains("AcceptedCodesInd") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Accepted Codes Industries", "AcceptedCodesInd"); }
                    if (Assign.Contains("AcceptedCodesOcc") || Assign.Contains("Allow")) { dt.Rows.Add(17, "Accepted Codes Occupations", "AcceptedCodesOcc"); }

                    //dt.re
                    GridView1.DataSource = dt;
                    GridView1.DataBind();



                    //if (Assign.Contains("A")) { GridView1.Rows[1].Visible = true; }

                }
            }
        }



        private void SQL_Query(string code)
        {
            if (code == "A")
            {
                strQuery = "SELECT * from  PART_1_A";
            }
            else if (code == "A1")
            {
                strQuery = "SELECT * from  PART_1_A1";
            }
            else if (code == "B")
            { strQuery = "SELECT * from  PART_1_B"; }
            else if (code == "C")
            { strQuery = "SELECT * from  C"; }
            else if (code == "Main")
            {
                strQuery = "SELECT * from  Main";
                //  strQuery = "
            }
            else if (code == "Monitoring")
            {
                strQuery = "Select * FROM Monitoring";
            }
            else if (code == "Observer")
            {
                strQuery = "select * from observer inner join observerext on observer._URI=observerext._URI";
            }
            else if (code == "IndustriesTextReview")
            {
                strQuery = "select * from  Industries_Text_Review";
            }
            else if (code == "ProfessionTextReview")
            {
                strQuery = "select * from  Occupation_Text_Review";
            }
            else if (code == "ToolsTextReview")
            {
                strQuery = "select * from  Tools_Text_Review";
            }
            else if (code == "SummaryReport")
            {
                strQuery = "select * from  SummaryR";
            }
            else if (code == "HHSummary")
            {
                strQuery = "EXEC [Report_HH_Size]";
            }
            else if (code == "OthersFieldSummary")
            {
                strQuery = "select * from  Others_Percentage_FieldWise";
            }
            else if (code == "OthersSummary")
            {
                strQuery = "select * from  Others_Question_percentage";
            }
            else if (code == "MistakesCount")
            {
                strQuery = "select * from  Enumerator_Mistakes_Count";
            }
            else if (code == "MistakeOutlier")
            {
                strQuery = "exec  [Report_Outlier]";
            }
            else if (code == "OthersCodes")
            {
                strQuery = "exec  [Report_Others]";
            }
            else if (code == "AcceptedCodesInd")
            {
                strQuery = "SELECT * FROM [All_Accepted_Industries]";
            }
            else if (code == "AcceptedCodesOcc")
            {
                strQuery = "SELECT * FROM [All_Accepted_Occupations]";
            }







        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Word")
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

            //    ExportToWord(sender, e);

            //}

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

            //else if (e.CommandName == "PDF")
            //{
            //    //Determine the RowIndex of the Row whose LinkButton was clicked.
            //    int rowIndex = Convert.ToInt32(e.CommandArgument);

            //    //Reference the GridView Row.
            //    GridViewRow row = GridView1.Rows[rowIndex];

            //    //Fetch value of Name.
            //    //string name = (row.FindControl("txtName") as TextBox).Text;

            //    //Fetch value of Code
            //     code = row.Cells[1].Text;

            //    SQL_Query(code);

            //    ExportToPDF(sender, e);

            //}

            if (e.CommandName == "CSV")
            {
                //Determine the RowIndex of the Row whose LinkButton was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GridView1.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Code
                string code = row.Cells[1].Text;

                SQL_Query(code);

                ExportToCSV(code, sender, e);

            }


        }



        private DataTable GetData(SqlCommand cmd)
        {
            DataTable dt = new DataTable();


            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    string myString = dt.Rows[i][k].ToString();
                    myString = myString.Replace(System.Environment.NewLine, " ");
                    myString= Regex.Replace(myString, @"<(.|\n)*?>", " ");
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



    }
}