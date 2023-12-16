<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cluster_progress_report.aspx.cs" Inherits="Supervisor_asp.cluster_progress_report" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
</head>
<body>
    
    <form id="form1" runat="server">
        
   <div>
            <style type="text/css">
                .table table tbody tr td a,

                .table table tbody tr td span {
                    position: relative;
                    float: left;
                    padding: 6px 12px;
                    margin-left: -1px;
                    line-height: 1.42857143;
                    color: #337ab7;
                    text-decoration: none;
                    background-color: #fff;
                    border: 1px solid #ddd;
                }

                .table table > tbody > tr > td > span {
                    z-index: 3;
                    color: #fff;
                    cursor: default;
                    background-color: #337ab7;
                    border-color: #337ab7;
                }

                .table table > tbody > tr > td:first-child > a,
                .table table > tbody > tr > td:first-child > span {
                    margin-left: 0;
                    border-top-left-radius: 4px;
                    border-bottom-left-radius: 4px;
                }

                .table table > tbody > tr > td:last-child > a,
                .table table > tbody > tr > td:last-child > span {
                    border-top-right-radius: 4px;
                    border-bottom-right-radius: 4px;
                }


                .table table > tbody > tr > td > a:hover,
                .table table > tbody > tr > td > span:hover,
                .table table > tbody > tr > td > a:focus,
                .table table > tbody > tr > td > span:focus {
                    z-index: 2;
                    color: #23527c;
                    background-color: #eee;
                    border-color: #ddd;
                }
            </style>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true"></asp:ScriptManager>
       <asp:UpdatePanel ID="myUpdatePanel" runat="server">
    <ContentTemplate>
            <asp:GridView ID="GridView1"
                CssClass="table table-striped table-bordered table-hover" PageSize="50"
                runat="server" 

                AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" EnableModelValidation="True">
            </asp:GridView>
         </ContentTemplate>
    
</asp:UpdatePanel>

        </div>
    </form>
</body>
</html>