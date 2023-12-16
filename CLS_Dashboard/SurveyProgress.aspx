
<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SurveyProgress.aspx.cs" Inherits="CLS_Dashboard.SurveyProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

             <!-- Static header row of gridview -->
     <script src="../assets/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
     <script src="../assets/Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
    <!-- Static header row of gridview -->
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
             <!-- Styling of page when width is lower than 700px -->
            <style>
                @media only screen and (max-width: 700px) {
                    .container-fluid {
                       
                        width: 100%;
                        padding-left: 0px;
                        padding-right: 0px;
                    }
                
                .btn
                {
                       padding: 12px 0px;
                }
                }
            </style>

            <script type="text/javascript">
               <%-- function StaticHeaderRow() {
                    debugger
                    
                    $(document).ready(function () {
                        $('#<%=GridView1.ClientID %>').Scrollable();
                    }
    )
                    $(document).ready(function () {
                        $('#<%=GridView2.ClientID %>').Scrollable();
                   }
    )
               
                
                }--%>

                </script>

            <div class="content">

                <div class="container-fluid" style="margin-top: 20px">

                    <div class="col-md-12">

                        <div class="card" style="text-align: center">
                            <div class="card-header" data-background-color="blue">
                                <ul class="nav nav-tabs" data-background-color="blue" data-tabs="tabs">

                                    <li class="">
                                        <a href="#settings" data-toggle="tab">
                                            <i class="material-icons">cloud</i>
                                            Survey Progress Report Types:
						<div class="ripple-container"></div>
                                        </a>
                                    </li>

                                </ul>
                            </div>
                            <br />




                            <table class="table-responsive table-hover" style="text-align: left; margin-left: 5%; width: 90%">
                                <tr>

                                    <td colspan="7">
                                        <asp:Label ID="Last_Updates" runat="server" Text=""></asp:Label></td>
                                    
                                   
                               </tr>
                                <tr>

                                    <td>
                                        <asp:Label ID="lblDistricts" runat="server" CssClass="btn btn-primary" Text="Districts"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTypes" runat="server" CssClass="btn btn-primary" Text="Types" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMonitors" runat="server" CssClass="btn btn-primary" Text="Monitors" Visible="false"></asp:Label></td>

                                    <td>
                                        <asp:Label ID="lblSurveyTeam" runat="server" CssClass="btn btn-info" Text="Survey-Team" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblClusterCode" runat="server" CssClass="btn btn-success" Text="Cluster-Code" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblEnumerator" runat="server" CssClass="btn btn-warning" Text="Enumerator" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblLegendsHeading" runat="server" CssClass="btn-label" Text="Legends" Visible="false"></asp:Label></td>
                                </tr>



                                <tr>

                                    <td>
                                        <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple" style="height: 150px !important;" OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstTypes" runat="server" AutoPostBack="true" SelectionMode="Multiple" style="height: 150px !important;" OnSelectedIndexChanged="lstTypes_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstMonitors" runat="server" AutoPostBack="true" SelectionMode="Multiple" style="height: 150px !important;" OnSelectedIndexChanged="lstMonitors_SelectedIndexChanged" Visible="False"></asp:ListBox></td>

                                    <td>
                                        <asp:ListBox ID="lstSurveyTeam" runat="server" AutoPostBack="true" SelectionMode="Multiple" style="height: 150px !important;" OnSelectedIndexChanged="lstSurveyTeam_SelectedIndexChanged" Visible="False"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstClusterCodes" runat="server" AutoPostBack="true" SelectionMode="Multiple"  style="height: 150px !important;" OnSelectedIndexChanged="lstClusterCodes_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstEnumerator" runat="server" AutoPostBack="true" SelectionMode="Multiple" style="height: 150px !important;" OnSelectedIndexChanged="lstEnumerator_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:Label ID="lblLegendsText" runat="server" Text="Enumerator" Visible="false"></asp:Label></td>
                                </tr>

                            </table>
                            <% if (Request["id"] == "1004_Mistakes_Summarized_Overall") { %>
                           
                             <table class="table table-hover" style="text-align: left; margin-left: 5%; width: 90%" >
                                 <tr>
                                    
                                    <td>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell BackColor="Yellow" Height="80px" Width="80px" HorizontalAlign="Center" >
                                                    <asp:Label ID="Label1" runat="server"  Text="QX"></asp:Label></asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="Label3" runat="server" Text="Refers that atleat one query is assigned to a either DataValidator/Field Monitor and rest are closed/resolved"></asp:Label></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                    <td>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell BackColor="GreenYellow" Height="80px" Width="80px" HorizontalAlign="Center" >
                                                    <asp:Label ID="Label2" runat="server"  Text="QX"></asp:Label></asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="Label4" runat="server" Text="Refers that all queries of the sections has been resolved"></asp:Label></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                    <td>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell BackColor="Orange" Height="80px" Width="80px" HorizontalAlign="Center" >
                                                    <asp:Label ID="Label5" runat="server"  Text="QX, QXL"></asp:Label></asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="Label6" runat="server" Text="Refers that atleast one query has been closed and rest of the quries are resolved"></asp:Label></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>

                                </tr>
                                  <tr>
                                    
                                    <td>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell BorderWidth="1px" Height="80px" Width="80px" HorizontalAlign="Center" >
                                                    <asp:Label ID="Label7" runat="server"  Text="QX"></asp:Label></asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="Label8" runat="server" Text="Refers that all of the queries are as it is, untouched"></asp:Label></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                    <td>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell BackColor="Yellow" BorderStyle="Solid" BorderColor="LightBlue" Height="80px" Width="80px" HorizontalAlign="Center" >
                                                    <asp:Label ID="Label9" runat="server"  Text="QX"></asp:Label></asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="Label10" runat="server"  Text="Refers that atleast one query is Assigned to Field Monitor"></asp:Label></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>
                                    <td>
                                        <asp:Table runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell BackColor="Yellow" BorderStyle="Solid" BorderColor="DarkBlue" Height="80px" Width="80px" HorizontalAlign="Center" >
                                                    <asp:Label ID="Label11" runat="server"  Text="QX, QXL"></asp:Label></asp:TableCell>
                                                <asp:TableCell>
                                                    <asp:Label ID="Label12" runat="server" Text="Refers that atleast one Assigned to DataValidator"></asp:Label></asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </td>

                                </tr>
                                 </table>
                              <table class="table table-hover" runat="server" id="progress" visible="false"> 
                                <tr style="background-color:flavour">
                                    <td><b> Assigned </b></td>
                                     <td><b> Resolved OR Corrected</b></td>
                                     <td><b> Re-Assinged</b></td>

                                </tr>
                                <tr style="background-color:GreenYellow">
                                    <td runat="server" id="assigned"> </td>
                                     <td runat="server" id="resolved"> </td>
                                     <td runat="server" id="reassigned"> </td>

                                </tr>
                            </table>
                            <% } %>
                            <div>


                                <br />
                                <br />



                            </div>

                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-header" data-background-color="blue">
                                        <h4 class="title">
                                            <asp:Label ID="lblTableHeading" runat="server" Text=""></asp:Label></h4>
                                        <p class="category">CLS -  Child Labour Survey</p>
                                    </div>
                                    <div class="card-content table-responsive" style="width: 100%;overflow-x: auto">
                                        
                                        <asp:CheckBox OnCheckedChanged="chksummaryOrderAsc_CheckedChanged" ID="chksummaryOrderAsc" runat="server" AutoPostBack="True" Text="Old Data First" Visible ="false"/>
                                    
                                        <asp:Label ID="Data_Source" Visible="false" runat="server" Text=""></asp:Label>
                                                <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" PagerStyle-CssClass="GridPager" ShowFooter="True" OnRowDataBound="GridView1_RowDataBound" PageSize="20" PageIndex="1">
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <FooterStyle CssClass="GridFooter" ForeColor="White" />

                                               <emptydatatemplate>
                                                    No data found for this report.  
                                                </emptydatatemplate>

                                                </asp:GridView>
                                        
                                        <asp:GridView ID="GridView2" runat="server" CssClass="table table-hover" OnPageIndexChanging="GridView2_PageIndexChanging" PagerStyle-CssClass="GridPager" ShowFooter="True" OnRowDataBound="GridView1_RowDataBound" AutoGenerateColumns="False" PageSize="20">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <Columns>
                                                <asp:BoundField DataField="HouseHold Key" HeaderText="HouseHold Key" />
                                                <asp:BoundField DataField="Record_Id" HeaderText="ID" Visible="false" />
                                                <asp:BoundField DataField="Cluster Code" HeaderText="Cluster Code" />
                                                <asp:BoundField DataField="Household ID" HeaderText="Household ID" />
                                                <asp:BoundField DataField="Name of District" HeaderText="Name of District" />
                                                <asp:BoundField DataField="Name Of Enumerator" HeaderText="Name Of Enumerator" />
                                                <asp:BoundField DataField="Enumerator Code" HeaderText="Enumerator Code" />
                                                <asp:BoundField DataField="Name of Supervisor"  HeaderText="Name of Supervisor"/>
                                                <asp:BoundField DataField="Supervisor Code" HeaderText="Supervisor Code"/>
                                               <%-- <asp:BoundField DataField="Monitor_Name" HeaderText="Monitor_Name"/>
                                               --%> <asp:BoundField DataField="Tehsil" HeaderText="Tehsil"/>
                                                <asp:HyperLinkField DataNavigateUrlFields=" Record_Id, Section1"  HeaderText="Section1" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}"  DataTextField="Section1" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section2" HeaderText="Section2" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section2" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section3" HeaderText="Section3" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section3" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section5" HeaderText="Section5" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section5" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section7" HeaderText="Section7" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section7" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section8" HeaderText="Section8" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section8" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section9" HeaderText="Section9" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section9" />
                                                <asp:HyperLinkField DataNavigateUrlFields="Record_Id , Section10" HeaderText="Section10" DataNavigateUrlFormatString="Monitoring.aspx?key={0}&amp;pid={1}" DataTextField="Section10" />
                                                
                                            </Columns>
                                            <FooterStyle CssClass="GridFooter" ForeColor="White" />

                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


