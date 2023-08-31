<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Maps.aspx.cs" Inherits="CLS_Dashboard.Map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" style="margin-top: 20px; width: 80%">

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">

    <head>
        <title>CLS- Listing 2018</title>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
   <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

        <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />

     <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


        <%--Google Map--%>
        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBZRLQRqsxZUjHaZ1H6PSTMK3wAV2lJrtY"></script>

        <script type="text/javascript">

            var map = null;
            var markers = [];

            function initialize() {



                markers = [

            <%=this.getMarkerObjects()%>

                ];

                var mapOptions = {
                    center: new google.maps.LatLng(markers[0].lat, markers[0].lng),
                    zoom: 8,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                var infoWindow = new google.maps.InfoWindow();
                map = new google.maps.Map(document.getElementById("dvMap"), mapOptions);

                google.maps.event.addListenerOnce(map, 'bounds_changed', function () {

                    for (i = 0; i < markers.length; i++) {
                        var data = markers[i]
                        var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                        var marker = new google.maps.Marker({
                            position: myLatlng,
                            map: map,
                            title: data.title
                        });
                        (function (marker, data) {
                            google.maps.event.addListener(marker, "click", function (e) {
                                infoWindow.setContent(data.description);
                                infoWindow.open(map, marker);
                            });
                        })(marker, data);
                    }
                });
            }

        </script>
        <meta name="viewport" content="width=device-width, initial-scale=1.0">




        <%--    <script type="text/javascript">

        var map;

        // Cretes the map
        function initialize() {
           
            map = new google.maps.Map(document.getElementById('dvMap'), {
                zoom: 10,
                center: new google.maps.LatLng(27.45475, 68.189405),
                mapTypeId: google.maps.MapTypeId.ROADMAP
            });

            generateMarkers([<%=this.getMarkerObjects()%>]);
        }

        // This function takes an array argument containing a list of marker data
        function generateMarkers(markers) {
            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.description);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);


            }
        }
  </script>

    
   <script type="text/javascript">
       window.load = function() {
            initialize();          
            
       }
  </script> --%>

        <%-- <script>
        function initMap() {

           
            var markers = [ <%=ArrayStore%>];
            
            console.log('===>' + markers.length);

            var mapDiv = document.getElementById('dvMap');
            var map = new google.maps.Map(mapDiv, {
                zoom: 8,
                center: new google.maps.LatLng(markers[0].lat, markers[0].lng)
            });

           

            for (i = 0; i < markers.length; i++) {
                var data = markers[i]
                var myLatlng = new google.maps.LatLng(data.lat, data.lng);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    title: data.title
                });
                (function (marker, data) {
                    google.maps.event.addListener(marker, "click", function (e) {
                        infoWindow.setContent(data.description);
                        infoWindow.open(map, marker);
                    });
                })(marker, data);
            }

            // We add a DOM event here to show an alert if the DIV containing the
            // map is clicked.
            google.maps.event.addDomListener(mapDiv, 'click', function() {
                window.alert('Map was clicked!');
            });
        }
    </script>--%>


        <style>
            .custom-combobox {
                position: relative;
                display: inline-block;
            }

            .custom-combobox-toggle {
                position: absolute;
                top: 0;
                bottom: 0;
                margin-left: -1px;
                padding: 0;
            }

            .custom-combobox-input {
                margin: 0;
                padding-top: 2px;
                padding-bottom: 5px;
                padding-right: 5px;
                padding-left: 15px;
            }



            ::-webkit-scrollbar {
                width: 7px;
                height: 7px;
            }

            ::-webkit-scrollbar-track {
                background: #C8C8C8;
                border-radius: 6px;
                -webkit-border-radius: 6px;
                -moz-border-radius: 6px;
            }

            ::-webkit-scrollbar-thumb {
                background-color: #555;
                border-radius: 6px;
                -webkit-border-radius: 6px;
                -moz-border-radius: 6px;
            }

            ::-moz-selection {
                color: #fff;
                background: @verde;
            }

            ::selection {
                color: #fff;
                background: @verde;
            }


            .messagealert {
                width: 80%;
                position: fixed;
                top: 10px;
                z-index: 100000;
                padding: 0;
                font-size: 15px;
            }

            @media only screen and (max-width: 1600px) {
                .side_ {
                    float: none;
                    width: 80%;
                    margin-left: 180px;
                }
            }
        </style>




        <script>
            $(function () {
                $.widget("custom.combobox", {
                    _create: function () {
                        this.wrapper = $("<span>")
                            .addClass("custom-combobox")
                            .insertAfter(this.element);

                        this.element.hide();
                        this._createAutocomplete();
                        this._createShowAllButton();
                    },

                    _createAutocomplete: function () {
                        var selected = this.element.children(":selected"),
                            value = selected.val() ? selected.text() : "";

                        this.input = $("<input>")
                            .appendTo(this.wrapper)
                            .val(value)
                            .attr("title", "")
                            .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
                            .autocomplete({
                                delay: 0,
                                minLength: 0,
                                source: $.proxy(this, "_source")
                            })
                            .tooltip({
                                classes: {
                                    "ui-tooltip": "ui-state-highlight"
                                }
                            });

                        this._on(this.input, {
                            autocompleteselect: function (event, ui) {
                                ui.item.option.selected = true;
                                this._trigger("select", event, {
                                    item: ui.item.option
                                });
                            },

                            autocompletechange: "_removeIfInvalid"
                        });
                    },

                    _createShowAllButton: function () {
                        var input = this.input,
                            wasOpen = false

                        $("<a>")
                            .attr("tabIndex", -1)
                            .attr("title", "Show All Items")
                            .attr("height", "")
                            .tooltip()
                            .appendTo(this.wrapper)
                            .button({
                                icons: {
                                    primary: "ui-icon-triangle-1-s"
                                },
                                text: "false"
                            })
                            .removeClass("ui-corner-all")
                            .addClass("custom-combobox-toggle ui-corner-right")
                            .on("mousedown", function () {
                                wasOpen = input.autocomplete("widget").is(":visible");
                            })
                            .on("click", function () {
                                input.trigger("focus");

                                // Close if already visible
                                if (wasOpen) {
                                    return;
                                }

                                // Pass empty string as value to search for, displaying all results
                                input.autocomplete("search", "");
                            });
                    },

                    _source: function (request, response) {
                        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                        response(this.element.children("option").map(function () {
                            var text = $(this).text();
                            if (this.value && (!request.term || matcher.test(text)))
                                return {
                                    label: text,
                                    value: text,
                                    option: this
                                };
                        }));
                    },

                    _removeIfInvalid: function (event, ui) {

                        // Selected an item, nothing to do
                        if (ui.item) {
                            return;
                        }

                        // Search for a match (case-insensitive)
                        var value = this.input.val(),
                            valueLowerCase = value.toLowerCase(),
                            valid = false;
                        this.element.children("option").each(function () {
                            if ($(this).text().toLowerCase() === valueLowerCase) {
                                this.selected = valid = true;
                                return false;
                            }
                        });

                        // Found a match, nothing to do
                        if (valid) {
                            return;
                        }

                        // Remove invalid value
                        this.input
                            .val("")
                            .attr("title", value + " didn't match any item")
                            .tooltip("open");
                        this.element.val("");
                        this._delay(function () {
                            this.input.tooltip("close").attr("title", "");
                        }, 2500);
                        this.input.autocomplete("instance").term = "";
                    },

                    _destroy: function () {
                        this.wrapper.remove();
                        this.element.show();
                    }
                });

                $("#combobox").combobox();
                $("#toggle").on("click", function () {
                    $("#combobox").toggle();
                });
            });




        </script>

        <script type="text/javascript">
            function ShowMessage(message, messagetype) {
                var cssclass;
                switch (messagetype) {
                    case 'Success':
                        cssclass = 'alert-success'
                        break;
                    case 'Error':
                        cssclass = 'alert-danger'
                        break;
                    case 'Warning':
                        cssclass = 'alert-warning'
                        break;
                    default:
                        cssclass = 'alert-info'
                }
                $('#alert_container').append('<div id="alert_div" style="margin: 0 0.2% 0.2 0; -webkit-box-shadow: 3px 4px 6px #999;" class="alert fade in ' + cssclass + '"><a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a><strong>' + messagetype + '!</strong> <span>' + message + '</span></div>');
            }
        </script>

    </head>

    <body>



        <div class="container">


            <div class="side_">





                <div class="messagealert" id="alert_container">
                </div>


                <div class="form-row">

                                        <div class="form-group col-md-12">

                        <div class="card-content table-responsive" style="width: 100%;height:200px; overflow-x: auto">
                            <asp:GridView ID="download_tab" runat="server" Visible="true" AutoGenerateColumns="false" BackColor="White" BorderColor="#11111C" BorderStyle="None" BorderWidth="1px" PagerStyle-CssClass="GridPager"
                                CellPadding="5" HorizontalAlign="Center" CssClass="table table-hover" OnRowCommand="download_tab_RowCommand" OnRowCreated="download_tab_RowCreated">
                                <Columns>
                                    <asp:BoundField DataField="username" HeaderText="User Name" />
                                    <asp:BoundField DataField="cluster_code" HeaderText="Cluster Code" />
                                    <asp:TemplateField  HeaderText="Download" >
                                        <ItemTemplate>
                                            <asp:Button ID="download_btn" runat="server"  Enabled =<%# Eval("status").ToString() == "1" ? true : false %> Text=<%# Eval("status").ToString() == "1" ? "Download" : "Pending Request" %> CommandName="download" CommandArgument="<%# Container.DataItemIndex %>"/>
                                        </ItemTemplate>
                                     </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Approve">
                                        <ItemTemplate>
                                            <asp:Button ID="approve_btn" runat="server" Enabled =<%# Eval("status").ToString() == "0" ? true : false %>  Text=<%# Eval("status").ToString() == "1" ? "Approved" : "Approve" %>  CommandName="approve" CommandArgument="<%# Container.DataItemIndex %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <div class="form-group col-md-6" style="padding-left: 0px; margin-top: 10px">
                        <asp:Button ID="Button1" class="btn btn-info form-control col-md-6" runat="server" CssClass="btn btn-primary form-group col-md-12" Text="Search here" />
                        <input type="text" id="searchTerm" runat="server" class="form-control col-md-6" style="margin-top: 10px;"
                            placeholder="Search Cluster Code" />
                        <asp:Button ID="btnSearch" class="btn btn-info form-control col-md-6" runat="server" CssClass="btn btn-danger form-group col-md-12" Text="Search" OnClick="btnSearch_Click" />
                        <asp:Button ID="download" Visible="false" class="btn btn-info form-control col-md-6" runat="server" CssClass="btn btn-info form-group col-md-12" Text="Download" OnClick="download_Click" />

                    </div>

                    <div class="form-group col-md-6" style="padding-left: 0px; margin-top: 10px">

                        <asp:Label ID="lblClusterCode" runat="server" CssClass="btn btn-primary form-group col-md-12" Text="Cluster-Code"
                            Visible="true"></asp:Label>



                        <asp:ListBox ID="lstClusterCodes" runat="server" Style="padding-left: 0px; margin-top: 10px" AutoPostBack="true" SelectionMode="Multiple" CssClass="form-group col-md-12" OnSelectedIndexChanged="lstClusterCodes_SelectedIndexChanged" Visible="true"></asp:ListBox>



                    </div>

                    <br />

                </div>


                <div class="form-row">

                    <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:Label ID="lblDistricts" runat="server" CssClass="btn btn-primary" Style="width: 215px"
                            Text="Districts"></asp:Label>
                    </div>




                    <div class="form-group col-md-2" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:Label ID="lblLocality" runat="server" CssClass="btn btn-info" Style="width: 150px"
                            Text="Locality" Visible="true"></asp:Label>
                    </div>

                    <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:Label ID="lblLister_name" runat="server" CssClass="btn btn-success" Style="width: 215px"
                            Text="Lister Name" Visible="true"></asp:Label>
                    </div>

                    <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:Label ID="lblTotal_hh" runat="server" CssClass="btn btn-warning" Text="Total HH"
                            Visible="true"></asp:Label>
                    </div>

                    <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:Label ID="lblTotal_children" runat="server" CssClass="btn btn-warning" Text="Total Child"
                            Visible="true"></asp:Label>
                    </div>

                </div>


                <div class="form-row">
                    <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                            OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged"></asp:ListBox>
                    </div>


                    <div class="form-group col-md-2" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:ListBox ID="lstLocality" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                            OnSelectedIndexChanged="lstLocality_SelectedIndexChanged" Visible="true"></asp:ListBox>
                    </div>
                    <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:ListBox ID="lstLister_name" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                            OnSelectedIndexChanged="lstLister_name_SelectedIndexChanged" Visible="true"></asp:ListBox>
                    </div>
                    <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:ListBox ID="lstTotal_hh" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                            OnSelectedIndexChanged="lstTotal_hh_SelectedIndexChanged" Visible="true"></asp:ListBox>
                    </div>
                    <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px; display: none">
                        <asp:ListBox ID="lstTotal_children" runat="server" AutoPostBack="true" SelectionMode="Multiple"
                            OnSelectedIndexChanged="lstTotal_children_SelectedIndexChanged" Visible="true"></asp:ListBox>

                    </div>



                    <%--Google Map--%>
                    <div class="form-group col-md-12" style="padding-left: 0px; margin-top: 10px">

                        <div class="card">
                            <div class="card-body">
                                <h2 class="card-title text-center">Listing MAP</h2>
                                <p class="card-text text-center">Input Cluster Code in search box for specific cluster</p>
                                <div id="dvMap" style="width: 100%; height: 728px"></div>
                            </div>
                        </div>


                    </div>

                </div>

            </div>


        </div>

        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #13B093; color: white">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Confirm Delete</h4>

                    </div>
                    <div class="modal-body">
                        <p>
                            Are you sure you want to delete ?
                        <img src="cross.png" style="width: 50px; height: 50px" />
                        </p>
                        <p>
                            <b>ParentKey: </b>
                            <asp:Label ID="lblParentKey" Text="" runat="server" />
                            <asp:Label ID="lblDelete" Text="" runat="server" />
                        </p>

                    </div>
                    <div class="modal-footer">

                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>

                    </div>
                </div>

            </div>
        </div>




        <%--Modal Bootstrap--%>
        <script type='text/javascript'>
            function openModal() {

                $('#myModal').modal('show')

            }
        </script>





    </body>

    </html>



</asp:Content>
