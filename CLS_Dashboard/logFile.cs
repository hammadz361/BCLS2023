using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace CLS_Dashboard
{
    public class logFile
    {


        public static void log( string action , string user)
        {

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "logs");

            StreamWriter w = new StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath("~/Logs/" + user + "_" + DateTime.Now.ToString("d MMM yyyy") + ".txt"), true);

            
            try
            {
                string IP_Address = GetIPAddress();
 
                string now = DateTime.Now.ToString("d MMM yyyy");


                //Trace.Listeners.Add(twtl);
                //Trace.Write(DateTime.Now.ToString() + "---Action----- " + action + "----Procedure & values---- " + query + "---User :--- " + user);
                w.WriteLine(DateTime.Now.ToString() + "---Action----- " + action + "----User---- " + user + "---IP :--- " + IP_Address); // Write the text
                w.Flush();
                w.Close();

            }
            catch (Exception)
            {

                throw;
            }
            //throw new NotImplementedException();
        }

        public static void logHH(string user ,string action)
        {

            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "logs_hh");

            StreamWriter w = new StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath("~/logs_hh/" + user + "_" + DateTime.Now.ToString("d MMM yyyy") + ".txt"), true);


            try
            {


           
                w.WriteLine(DateTime.Now.ToString() + "----User---- " + user + "---Action----- " + action ); // Write the text
                w.Flush();
                w.Close();

            }
            catch (Exception)
            {

                throw;
            }
        }


        public static string logging(string path, string action, SqlCommand component, string user)
        {
            string logtext = "";
            try
            {
                string query = component.CommandText;

                foreach (SqlParameter p in component.Parameters)
                {
                    if (p.Value.ToString().Length >= 1)
                        query = query + "," + p.ParameterName + " : " + p.Value.ToString();
                }
                //    string now = DateTime.Now.ToString("d MMM yyyy");
                //using (StreamWriter w = new StreamWriter(path, true))
                //{
                //w.WriteLine(DateTime.Now.ToString() + "---Action----- " + action + "----Procedure & values---- " + query + "---User :--- " + user); // Write the text
                logtext = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "---Action----- " + action + "----Procedure & values---- " + query + "---User :--- " + user; // Write the text

                //w.Close();
                //}
            }
            catch (Exception)
            {

                throw;
            }
            //throw new NotImplementedException();

            return logtext;
        }
        internal static void opening()
        {
            //  w.Close();

        }
        public static string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        
    }
}