<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SurveyProgress.aspx.cs" Inherits="CLS_Dashboard.SurveyProgress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

             <!-- Static header row of gridview -->
     <script src="../assets/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
     <script src="../assets/Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
    <!-- Static header row of gridview -->
            

            <script type="text/javascript">
                function StaticHeaderRow() {
                    
                    $(document).ready(function () {
                        $('#<%=GridView1.ClientID %>').Scrollable();
                    }
    )
                   
               
                
                }

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



                            <table class="table table-hover" style="text-align: left; margin-left: 5%; width: 90%">
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
                                        <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstTypes" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstTypes_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstMonitors" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstMonitors_SelectedIndexChanged" Visible="False"></asp:ListBox></td>

                                    <td>
                                        <asp:ListBox ID="lstSurveyTeam" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstSurveyTeam_SelectedIndexChanged" Visible="False"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstClusterCodes" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstClusterCodes_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstEnumerator" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstEnumerator_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:Label ID="lblLegendsText" runat="server" Text="Enumerator" Visible="false"></asp:Label></td>
                                </tr>

                            </table>

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


                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover" AllowPaging="true" OnPageIndexChanging="GridView1_PageIndexChanging" PagerStyle-CssClass="GridPager" ShowFooter="True" OnRowDataBound="GridView1_RowDataBound" PageSize="50">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" ForeColor="White" />

                                       <emptydatatemplate>
        No data found for this report.  
    </emptydatatemplate></asp:GridView>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


