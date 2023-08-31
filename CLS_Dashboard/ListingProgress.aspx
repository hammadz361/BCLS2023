<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false"  CodeBehind="ListingProgress.aspx.cs" Inherits="CLS_Dashboard.Listing_Progress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <!-- Static header row of gridview -->
         <%--   <script src="../assets/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>--%>
            <script src="../assets/Scripts/ScrollableGridPlugin.js" type="text/javascript"></script>
            <link href="../assets/css/bootstrap.min.css" rel="stylesheet" />
            <!-- Static header row of gridview -->


            <script type="text/javascript">
                function StaticHeaderRow() {

                    $(document).ready(function () {
                        $('#<%=GridView1.ClientID %>').Scrollable();
                    }
    )



                }


            </script>
                         <style>
                 body.modal-open div.modal-backdrop { 
       z-index: 0; 
}
             </style> 



           <div class="container-fluid" style="margin-top: 20px">

                    <div class="col-md-12">

                    <div class="row" id="tickers" runat="server" visible="false">
                        <div class="col-lg-2 col-md-6 col-sm-6">
                            <div class="card card-stats">
                                <div class="card-header" data-background-color="orange">
                                    <asp:ImageButton ID="ImageButton1" src="../assets/img/SIndh _District.gif" Text="CSV" Style="width: 52px; height: 52px" runat="server" />
                                </div>
                                <div class="card-content">

                                    <p class="category">Districts</p>
                                    <h3 class="title">
                                        <asp:Label ID="lbl_Indicator_District" runat="server" Text=""></asp:Label>
                                        <small>active</small>
                                    </h3>

                                </div>

                                <div class="card-footer">
                                    <div class="stats">
                                        <i class="material-icons text-danger">warning</i>
                                        <a href="#pablo">
                                            <asp:Label ID="LastSubmissionDate" runat="server" Text=""></asp:Label></a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-6 col-sm-6">
                            <div class="card card-stats">
                                <div class="card-header" data-background-color="green">
                                    <asp:ImageButton ID="ImageButton3" src="../assets/img/Eumerators-Supervisors-3.gif" Text="CSV" Style="width: 52px; height: 52px" runat="server" />
                                </div>
                                <div class="card-content">
                                    <p class="category">Listers</p>
                                    <h3 class="title">
                                        <asp:Label ID="lbl_Indicator_Enumerator" runat="server" Text=""></asp:Label>
                                        
                               
                                    </h3>
                                </div>
                                <div class="card-footer">
                                    <div class="stats">
                                        <i class="material-icons">date_range</i> Engaged to date
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-6 col-sm-6">
                            <div class="card card-stats">
                                <div class="card-header" data-background-color="red">
                                    <asp:ImageButton ID="ImageButton4" src="../assets/img/Household.gif" Style="width: 52px; height: 52px" runat="server" />
                                </div>
                                <div class="card-content">
                                    <p class="category">Households</p>
                                    <h3 class="title">
                                        <asp:Label ID="lbl_Indicator_HH" runat="server" Text=""></asp:Label></h3>
                                </div>
                                <div class="card-footer">
                                    <div class="stats">
                                        <i class="material-icons">local_offer</i> visited by Listers
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-6 col-sm-6">
                            <div class="card card-stats" data-background-color="red">
                                <div class="card-header" data-background-color="blue">
                                    <asp:ImageButton ID="ImageButton2" src="../assets/img/children2.gif" Style="width: 52px; height: 52px" runat="server" />
                                </div>
                                <div class="card-content">
                                    <p class="category">Children</p>
                                    <h3 class="title">
                                        <asp:Label ID="lbl_Indicator_Childern" runat="server" Text=""></asp:Label></h3>
                                </div>
                                <div class="card-footer">
                                    <div class="stats">
                                        <i class="material-icons">update</i> interviewed
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="col-lg-2 col-md-6 col-sm-6">
                            <div class="card card-stats" data-background-color="red">
                                <div class="card-header" data-background-color="blue">
                                    <asp:ImageButton ID="ImageButton5" src="../assets/img/children2.gif" Style="width: 52px; height: 52px" runat="server" />
                                </div>
                                <div class="card-content">
                                    <p class="category">Average No of Boys</p>
                                    <h3 class="title">
                                        <asp:Label ID="Boys" runat="server" Text=""></asp:Label></h3>
                                </div>
                                <div class="card-footer">
                                    <div class="stats">
                                        <i class="material-icons">update</i> interviewed
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="col-lg-2 col-md-6 col-sm-6">
                            <div class="card card-stats" data-background-color="red">
                                <div class="card-header" data-background-color="blue">
                                    <asp:ImageButton ID="ImageButton6" src="../assets/img/children2.gif" Style="width: 52px; height: 52px" runat="server" />
                                </div>
                                <div class="card-content">
                                    <p class="category">Average No of Girls</p>
                                    <h3 class="title">
                                        <asp:Label ID="Girls" runat="server" Text=""></asp:Label></h3>
                                </div>
                                <div class="card-footer">
                                    <div class="stats">
                                        <i class="material-icons">update</i> interviewed
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                   

              
                   
                  

                       

              
                

                    <div class="col-md-12">

                        <div class="card" style="text-align: center">
                            <div class="card-header" data-background-color="blue">
                                <ul class="nav nav-tabs" data-background-color="blue" data-tabs="tabs">

                                    <li class="">
                                        <a href="#settings" data-toggle="tab">
                                            <i class="material-icons">cloud</i>
                                            Listing Progress Report Types:
						                <div class="ripple-container"></div>
                                        </a>
                                    </li>

                                </ul>
                            </div>
                            <br />



                            <table class="table table-hover" style="text-align: left; margin-left: 5%; width: 90%">
                                <tr>

                                    <td colspan="7">
                                        <asp:Label ID="Last_Updates" runat="server" Text=""></asp:Label></td>

                                </tr>
                               
                                <tr>

                                    <td>
                                        <asp:Label ID="lblDistricts" runat="server" CssClass="btn btn-primary" Text="Districts" Visible="False"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTypes" runat="server" CssClass="btn btn-primary" Text="Types" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMonitors" runat="server" CssClass="btn btn-primary" Text="Monitors" Visible="false"></asp:Label></td>

                                    <td>
                                        <asp:Label ID="lblSurveyTeam" runat="server" CssClass="btn btn-info" Text="Survey-Team" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblClusterCode" runat="server" CssClass="btn btn-success" Text="Cluster-Code" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblEnumerator" runat="server" CssClass="btn btn-warning" Text="Lister Name" Visible="false"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblLegendsHeading" runat="server" CssClass="btn-label" Text="Legends" Visible="false"></asp:Label></td>
                                     <td>
                                        <asp:Label ID="lblcriteria" runat="server" CssClass="btn btn-primary" Text="Criteria"  Visible="false" ></asp:Label></td>
                                </tr>



                                <tr>

                                    <td>
                                        <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstTypes" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstTypes_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                    <td>
                                        <asp:ListBox ID="lstSurveyTeam" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstSurveyTeam_SelectedIndexChanged" Visible="False"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstClusterCodes" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstClusterCodes_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:ListBox ID="lstEnumerator" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstEnumerator_SelectedIndexChanged" Visible="False"></asp:ListBox></td>
                                    <td>
                                        <asp:Label ID="lblLegendsText" runat="server" Text="Enumerator" Visible="false"></asp:Label></td>
                                     <td>
                                        <asp:ListBox ID="criteria_list" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="criteria_list_SelectedIndexChanged" Visible="False"></asp:ListBox></td>

                                </tr>
                                 <%--<tr id="criteria_row" runat="server" visible="false" >
                                     <td>
                                        <asp:Button ID="c1" runat="server" CssClass="btn btn-primary" Text="Criteria 1" OnClick="c1_Click" ></asp:Button></td>
                                    <td>
                                        <asp:Button ID="c2" runat="server" CssClass="btn btn-primary" Text="Criteria 2" OnClick="c2_Click" ></asp:Button></td>
                                    <td>
                                        <asp:Button ID="c3" runat="server" CssClass="btn btn-primary" Text="Criteria 3" OnClick="c3_Click"></asp:Button></td>

                                    <td>
                                        <asp:Button ID="c4" runat="server" CssClass="btn btn-info" Text="Criteria 4" OnClick="c4_Click" ></asp:Button></td>
                                    <td>
                                        <asp:Button ID="c5" runat="server" CssClass="btn btn-success" Text="Criteria 5" OnClick="c5_Click"></asp:Button></td>
                                </tr>--%>
                            </table>

                            <div>


                                  <asp:Label ID="footnote" runat="server" Visible="false" Text=""></asp:Label>



                            </div>

                            <div class="col-md-12">
                               <div class="card">
                                    <div class="card-header" data-background-color="blue">
                                        <h4 class="title">
                                            <asp:Label ID="lblTableHeading" runat="server" Text=""></asp:Label></h4>
                                        <p class="category">CLS -  Child Labour Survey</p>
                                    </div>
                                    <div class="card-content table-responsive" style="width: 100%;overflow-x: auto">


                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover" AutoPostBack="True" AllowPaging="true" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging"  PagerStyle-CssClass="GridPager" ShowFooter="True" OnRowDataBound="GridView1_RowDataBound" PageSize="50">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" ForeColor="White" />

                                            <EmptyDataTemplate>
                                                No data found for this report.  
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                             
                                    </div>
                                    <br />
                                    <div class="card-content table-responsive" style="width: 100%;overflow-x: auto">

                                           <asp:GridView ID="GridView2" runat="server" CssClass="table table-hover" AutoPostBack="True" Visible="false" AllowPaging="true" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging"  PagerStyle-CssClass="GridPager" ShowFooter="True" OnRowDataBound="GridView1_RowDataBound" PageSize="50">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" ForeColor="White" />

                                            <EmptyDataTemplate>
                                                No data found for this report.  
                                            </EmptyDataTemplate>
                                        </asp:GridView>


                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                  <div class="form-row">

                            <script type='text/javascript'>


                                function openModal() {
                                    //  alert("i am here");
                                    $('#myModal').modal('show')

                                }



                                var mymodal = document.getElementById('myModal');
                                window.onclick = function (event) {
                                    if (event.target == mymodal) {
                                        mymodal.style.display = "none";
                                    }
                                }
                            </script>


                            <!-- Modal -->
                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" >
                                <div class="modal-dialog">

                                    <!-- Modal content-->
                                    <div class="card-content table-responsive" style="width: 100%; height:500px; overflow-x: auto" >
                                        <div class="modal-header" style="background-color: #13B093; color: white">
                                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                                              <h4 class="title">
                                            <asp:Label ID="modal_heading" runat="server" Text=""></asp:Label></h4>
                                            <asp:GridView id="modal_content" runat="server"  PageSize="50" CssClass="table">
                                            <HeaderStyle CssClass="GridHeader" />
                                            <FooterStyle CssClass="GridFooter" ForeColor="White" />

                                            <EmptyDataTemplate>
                                                No data found for this report.  
                                            </EmptyDataTemplate>
                                            </asp:GridView>

                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


