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
    public partial class PlotOnMap : System.Web.UI.Page
    {
         protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            else
            {
                Session["lblHeadingName"] = "<b>CLS</b>MAP";
                if (!IsPostBack)
                {

                }
            }
        }
    }
}

        