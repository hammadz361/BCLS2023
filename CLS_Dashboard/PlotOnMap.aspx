<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PlotOnMap.aspx.cs" Inherits="CLS_Dashboard.PlotOnMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="content">



        <div class="container-fluid" style="margin-top: 20px">
            <div class="card" style="text-align: center">
                <div class="card-header" data-background-color="green">
                    <ul class="nav nav-tabs" data-background-color="green" data-tabs="tabs">

                        <li class="">
                            <a href="#settings" data-toggle="tab">
                                <i class="material-icons">location_on</i>
                                Map Plotting:
						<div class="ripple-container"></div>
                            </a>
                        </li>

                    </ul>
                </div>
                <br />

                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header" data-background-color="red">
                            <h4 class="title">
                                <asp:Label ID="lblTableHeading" runat="server" Text=""></asp:Label></h4>
                            <p class="category">CLS -  MAP</p>
                        </div>
                        <div class="card-content table-responsive">

                            <asp:Panel ID="Panel1" runat="server">



                                
                                <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.4/jquery.min.js"></script>
                                <script type="text/javascript" src="../assets/Scripts/graham_scan.js"></script>
                                <script type="text/javascript" src="../assets/Scripts/convex_hull.js"></script>



                                <div id="map_canvas" style="width: 100%; height: 728px; margin-bottom: 2px;">
                                </div>
                               <script>
      function initMap() {
        var uluru = {lat: <%=Request["lat"]%>, lng: <%=Request["long"]%>};
        var map = new google.maps.Map(document.getElementById('map_canvas'), {
          zoom: 15,
          center: uluru
        });
        var marker = new google.maps.Marker({
          position: uluru,
          map: map
        });
      }
    </script>
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBZRLQRqsxZUjHaZ1H6PSTMK3wAV2lJrtY&callback=initMap"
                                    async defer></script>

                                
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



</asp:Content>
