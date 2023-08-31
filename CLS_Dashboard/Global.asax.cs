using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace CLS_Dashboard
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            String action = "SCLS Dashboard"+
              "MESSAGE: " + ex.Message +
              "\nSOURCE: " + ex.Source +
              "\nFORM: " + Request.Form.ToString() +
              "\nQUERYSTRING: " + Request.QueryString.ToString() +
              "\nTARGETSITE: " + ex.TargetSite +
              "\nSTACKTRACE: " + ex.StackTrace+
              EventLogEntryType.Error;

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "logs_global");

            StreamWriter w = new StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath("~/logs_global/" + "_" + DateTime.Now.ToString("d MMM yyyy") + ".txt"), true);


            try
            {
               
                //Trace.Listeners.Add(twtl);
                //Trace.Write(DateTime.Now.ToString() + "---Action----- " + action + "----Procedure & values---- " + query + "---User :--- " + user);
                w.WriteLine(DateTime.Now.ToString() + "---Action----- " + action + "----User---- " ); // Write the text
                w.Flush();
                w.Close();

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}