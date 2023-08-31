<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="CLS_Dashboard.WebForm2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="text-align:center">
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server">Do you want to close this query..</asp:Label>
    
    </div>
        <div style="text-align:center">
            <table style="text-align:center">
                <tr>
                    <td><asp:Button ID="yes" Text="Yes" runat="server"/></td>
                    <td><asp:Button ID="no" Text="No" runat="server"/></td>
                </tr>
            </table>
        
            
        
    
        </div>
    </form>

</body>
</html>
