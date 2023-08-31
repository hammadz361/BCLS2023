<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Monitoring.aspx.cs" Inherits="CLS_Dashboard.Monitoring" %>

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
                    })
                }
                $(document).ready(function () {
                    $("#table_heading").tooltip();
                });


              
                function loadgif() {
                    debugger
        document.getElementById('loading_bar').style.visibility = 'visible';
        document.getElementById("next_click").disabled = true;
        return true;
    }

function confirm()

{

    return confirm("Are you sure you want to submit ?");
}
            </script>
               <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <style>
             .custombutton{
                 margin:0px;
                 width:100px;
                 padding:11px;
             }
             @media only screen and (max-width: 700px) {
     .container-fluid {
        float: none;
        width : 100%;
    }
     #to_be {
         width : 400px;
     }
     .font_size
     {
         font-size : smaller;
     }
     #ContentPlaceHolder1_correction_div
     {
         border-style:none;
     }
}
                </style>
            <div class="container-fluid" style="margin-top: 20px; width: 80%">

                <div class="col-md-12">
<%--                    <script type="text/javascript" language="javascript">
                        function ConfirmOnDelete() {
                            if (confirm("Are you sure?") == true)
                                return true;
                            else
                                return false;
                        }
    </script>--%>
                    <div>
                    
                        <h3 id="table_heading" class="font_size" runat="server" data-toggle="tooltip" title="heading wala text" style="text-align: center; font-family: Arial, Helvetica, sans-serif; font-size: x-large; font-weight: bold;">Procedure Name
                        </h3>
                        <h5 id="mandatory_flag" runat="server" style="text-align: center; font-family: Arial, Helvetica, sans-serif;">Procedure Name
                        </h5>
                           <asp:Label ID="lblLegendsText" runat="server" Text="Enumerator" Visible="false"></asp:Label>
                               



                        <div runat="server" id="roster_details" style="padding:10px">
                            <div class="row" style="font-family: Arial, Helvetica, sans-serif;text-align:center; font-size: medium; color: #800080">
                                <div id="hid" runat="server" class="col-md-3">.col-md-3</div>
                                <div id="add" runat="server" class="col-md-3">.col-md-3</div>
                                <div id="clus" runat="server" class="col-md-3">.col-md-3</div>
                                <div id="sup" runat="server" class="col-md-3">.col-md-3</div>
                            </div>
                            <div class="row col-xs-12" style="font-family: Arial, Helvetica, sans-serif;text-align:center; font-size: medium; color: #800080">
                                <div id="area_type" runat="server" class="col-md-3 col-xs-12">.col-md-3</div>
                                <div id="i16" runat="server" class="col-md-3 col-xs-12">.col-md-3</div>
                                <div id="sub_date" runat="server" class="col-md-4 col-xs-12">.col-md-3</div>
                            </div>

                        </div>
                    </div>
                     <table class="table table-responsive" runat="server" id="progress" visible="false"> 
                                <tr style="text-align:center">
                                    <td><b> Assigned </b></td>
                                     <td><b> Resolved OR Corrected</b></td>
                                     <td><b> Re-Assinged</b></td>

                                </tr>
                                <tr style="background-color:GreenYellow;text-align:center">
                                    <td runat="server" id="assigned"> </td>
                                     <td runat="server" id="resolved"> </td>
                                     <td runat="server" id="reassigned"> </td>

                                </tr>
                            </table>
                    <br />
                    <div id="tab_" runat="server" class="col-md-12">
                        <ul class="nav nav-pills">
                            <li class="active">
                                <a href="#2" data-toggle="tab">Resolve/Close</a>
                            </li>
                            <li runat="server" id="res"><a href="#asng" data-toggle="tab">Assigned To</a>
                            </li>
                        </ul>
                           
                        <div class="tab-content ">
                              <asp:Button runat="server" Visible="false" ID="Button1191" Text="Force Closed" OnClick="forceResolvedClosed" ToolTip="Force Closed" />
                           <div class="tab-pane" id="asng" style="border: double; padding: 5px">
                                <div class="btn-group" style="align-content: center">
                                    <asp:Label ID="asg_comment_lb" runat="server" Text="Comments" Font-Bold="True" Width="120px"></asp:Label>
                                    <asp:TextBox ID="asg_comment_box" runat="server" TextMode="MultiLine" Height="50px" width="360px" />
                                    <br />
                                    <h5 runat="server" visible="false" id="assign_heading" style="display: inline">Assign</h5>
                                    <asp:Button runat="server" Visible="false" ID="dv" Text="Data Validator" OnClick="dv_Click" ToolTip="Assign to Data Validator" />
                                    <asp:Button runat="server" Visible="false" ID="sp" Text="Field Monitor" OnClick="sp_Click" ToolTip="Assign to Field Monitor" />
                                </div>
                            </div>
                           <div class="tab-pane active" id="2">
                                <div runat="server" id="correction_div" lass="form-control" style="border: double; padding: 5px">
                                   
                                    <asp:Label ID="key1" runat="server" Text="Key :" Font-Bold="True" Width="120px"></asp:Label>
                                    <asp:DropDownList ID="key_drop" Visible="true" runat="server" class="btn btn-info dropdown-toggle" width="360px"  AutoPostBack="True" OnLoad="key_drop_load">
                                    </asp:DropDownList>
                                    <br />
                                    <div style="display: block;">
                                        <asp:Label ID="f1" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l1" runat="server" Style="border: dashed; padding: 5px" Visible="false" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox1" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList1" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server" />
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f2" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l2" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox2" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList2" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f3" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l3" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox3" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList3" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f4" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l4" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox4" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList4" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f5" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l5" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox5" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList5" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f6" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l6" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox6" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList6" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f7" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l7" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox7" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList7" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f8" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l8" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox8" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList8" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f9" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l9" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox9" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList9" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f10" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l10" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox10" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList10" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f11" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l11" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox11" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList11" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f12" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l12" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox12" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList12" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f13" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l13" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox13" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList13" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="f14" runat="server" Visible="False" Text="Label" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="l14" runat="server" Visible="false" Style="border: dashed; padding: 5px" Width="150px"></asp:Label>
                                        <asp:TextBox ID="TextBox14" runat="server" Visible="false" Text="Correct Field" />
                                        <asp:DropDownList ID="DropDownList14" Visible="false" class="btn btn-default dropdown-toggle" width="360px" runat="server">
                                        </asp:DropDownList>

                                    </div>
                                    <div style="display: block;">
                                        <asp:Label ID="comment_lbl" runat="server" Text="Comments" Width="120px" Font-Bold="True"></asp:Label>
                                        <asp:TextBox ID="comment_box" runat="server" TextMode="MultiLine" Height="50px" width="360px" />
                                        <br />
                                        <asp:Button Text="Resolved/Closed" ID="resolve_click" OnClick="Update_values" OnClientClick="return loadgif();" CssClass="btn btn-info" runat="server" ToolTip="Click to Resolve/Close the Query" />
                                      <%--  <asp:Button Text="Proceed anyway" ID="force_resolve" OnClick="proceed_correction" OnClientClick="return loadgif();" CssClass="btn btn-info" runat="server" ToolTip="Click to Resolve/Close the Query" />
                                      --%> 
                                         <img ID="loading_bar" style="visibility:hidden" src="../assets/img/loader.gif" />
                                        
                                    </div>
                                    <div runat="server" id="close_force" visible="false">
                                        <label style="font-size:medium;">Error has not been corrected.Are you sure to close this query forcefully?</label>
                                        <asp:Button ID="close_" runat="server" Text="Yes"  OnClick="Update_values" OnClientClick="return loadgif();" CssClass="btn btn-danger"/>
                                        <asp:Button ID="cancel" runat="server" Text="Cancel" OnClick="cancel_Click" CssClass="btn btn-default"/>
                                    </div>
                                </div>
                                <div id="already_corrected" runat="server" visible="false" class="col-sm-6 col-md-10">
                                    <h3>Already Corrected</h3>
                                </div>

                            </div>
                                         
                        </div>
                        <div style="text-align:left">
                         <asp:Button Text="Next" ID="next_click" Enabled="false" OnClick="btnConfirm_Click"  Visible="false" CssClass="btn btn-info" runat="server" ToolTip="Move to Next Query" />
                    </div>
                    </div>
                    
                   <div style="text-align: left">
                        <div class="col-lg-1 col-xs-12">
                            <h3>Other Errors</h3>
                            <div runat="server" class="btn-group-primary" style="margin:0.02px" id="Q_buttons">
                                <asp:Button Text="Full Report" CssClass="btn btn-secondary custombutton"  OnClientClick="window.location.href='surveyProgress.aspx?id=1004_Mistakes_Summarized_Overall&name=Mistakes%20Summarized%20Report'" runat="server" />
                            </div>
                        </div>
                        <div class="card-header col-lg-11 col-xs-12" data-background-color="blue" style=" height: 550px; margin-top:10px">
                            <div class="col-md-12" runat="server" id="content_correction">
                                <div id="to_be" class="card-content table-responsive" style="overflow-x: auto;overflow-y: auto; padding: 2%;height: 550px">

                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" OnRowCreated="bla_RowCreated"
                                        BackColor="White" BorderColor="#11111C" BorderStyle="None" BorderWidth="1px"  PagerStyle-CssClass="GridPager"
                                        CellPadding="5" HorizontalAlign="Center" CssClass="table table-hover">
                                         <HeaderStyle CssClass="GridHeader" />

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


