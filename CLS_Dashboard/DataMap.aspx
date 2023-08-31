<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DataMap.aspx.cs" Inherits="CLS_Dashboard.DataMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content">

      

   

        <div class="container-fluid" style="margin-top: 20px">
            <div class="card" style="text-align: center">
                <div class="card-header" data-background-color="green">
                    <ul class="nav nav-tabs" data-background-color="green" data-tabs="tabs">

                        <li class="">
                            <a href="#settings" data-toggle="tab">
                                <i class="material-icons">location_on</i>
                                Survey Data Collected GPS COORDINATES:
					
                                <div class="ripple-container"></div>
                            </a>
                        </li>

                    </ul>
                </div>
                <br />

                <div class="check">
                    <label>
                        <input id="myCheck" class="check" onchange="DrawHidePolygon(this)" type="checkbox" hidden="hidden">
                        :Select TOP number to fetch records
                    </label>
                    <br />
                    <label>
                        <asp:TextBox ID="txtTopSQL" runat="server" OnTextChanged="txtTopSQL_TextChanged">100</asp:TextBox>
                    </label>
                </div>



                <asp:CheckBox ID="Chk_Main" runat="server" Checked="true" Text="Enumerators" AutoPostBack="true" OnCheckedChanged="Chk_Main_CheckedChanged" />
                <asp:CheckBox ID="Chk_Monitors" runat="server" Checked="false" Text="Monitors" AutoPostBack="true" OnCheckedChanged="Chk_Monitors_CheckedChanged" />
                <asp:CheckBox ID="Chk_Observers" runat="server" Checked="false" Text="Observers" AutoPostBack="true" OnCheckedChanged="Chk_Observers_CheckedChanged" />
                <asp:CheckBox ID="Chk_Supervisors" runat="server" Checked="false" Text="Supervisors" AutoPostBack="true" OnCheckedChanged="Chk_Supervisors_CheckedChanged" />
           

                <table class="table table-hover" style="text-align: left; margin-left: 5%;">
                    <tr>

                        <td>
                            <asp:Label ID="labelDistrict" runat="server" Text="District : " CssClass="btn btn-primary"></asp:Label></td>
                        <td>
                            <asp:Label ID="labelSupervisor" runat="server" Text="Supervisor : " CssClass="btn btn-info"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblClusterCode" runat="server" CssClass="btn btn-success" Text="Cluster-Code" Visible="true"></asp:Label></td>


                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lstDistricts" Style="height: 150px" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="Filter_Selected">
                                <asp:ListItem Text="All" Value="All"> </asp:ListItem>
                            </asp:ListBox></td>


                        <td>
                            <asp:ListBox ID="lstSupervisor" Style="height: 150px" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="Filter_Selected">
                                <asp:ListItem Text="All" Value="All"> </asp:ListItem>
                            </asp:ListBox></td>
                        <td>
                            <asp:ListBox ID="lstClusterCodes" Style="height: 150px" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="Filter_Selected" Visible="true">
                                <asp:ListItem Text="All" Value="All"> </asp:ListItem>
                            </asp:ListBox></td>

                    </tr>
                </table>
                <br />
                <br />

                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header" data-background-color="red">
                            <h4 class="title">
                                <asp:Label ID="lblTableHeading" runat="server" Text=""></asp:Label></h4>
                            <p class="category">CLS -  DATA MAP</p>
                        </div>
                        <div class="card-content table-responsive">

                            <asp:Panel ID="Panel1" runat="server">
                                <%--Place holder to fill with javascript by server side code--%>

                                <%--<asp:Literal ID="js" runat="server"></asp:Literal>--%>
                                <%--Place for google to show your MAP--%>


                                 <div id="toastContainer"></div>
                              
                                <script type="text/javascript">
                                  
                             
                 
                                   
                                    
                                </script>



                                <div>
                                    <h2>Google Map</h2>
                                    <div id="map-canvas" style="width: 100%; height: 728px; margin-bottom: 2px;">
                                    </div>


                                </div>
                                <br />
                            </asp:Panel>

                        </div>
                    </div>
                </div>

                <br />

                <br />

            </div>
        </div>
    </div>
     <style>
        .modal {
    position: fixed;
    top: 0;
    left: 0;
    background-color: black;
    z-index: 99;
     opacity: 1; 
    filter: alpha(opacity=100);
    -moz-opacity: 1;
    min-height: 100%;
    width: 100%;
}
        .modal-backdrop {
     position: relative;
        }
    </style>
      <div  id="imageModal" class="modal" tabindex="-1" role="dialog">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="modalTitle">Full Image</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
       <img id="modalImage" src="" alt="Full-size Image" style="width: 100%; height: auto;">
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

</asp:Content>
