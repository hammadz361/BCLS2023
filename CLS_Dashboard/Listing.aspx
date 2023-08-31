<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Listing.aspx.cs" Inherits="CLS_Dashboard.Listing" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <title>CLS- Listing 2018</title>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0">

        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

        <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />





        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>



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


            .GridPager a, .GridPager span {
                display: block;
                height: 25px;
                width: 25px;
                font-weight: bold;
                text-align: center;
                text-decoration: none;
                margin: 5px;
                padding-bottom: 10px;
            }

            .GridPager a {
                background-color: #f5f5f5;
                color: #969696;
                border: 1px solid #969696;
            }

            .GridPager span {
                background-color: #a3f5e6;
                color: #000;
                border: 1px solid #13B093;
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

                <asp:UpdatePanel ID="updatepnl" runat="server">

                    <ContentTemplate>

                        <h1>Listing Search Directory 2018</h1>

                        <div class="messagealert" id="alert_container">
                        </div>

                        <script>
                            function doSearch() {
                                var input, filter, found, table, tr, td, i, j;
                                input = document.getElementById("searchTerm");
                                filter = input.value.toUpperCase();
                                table = document.getElementById("GridView1");
                                tr = table.getElementsByTagName("tr");
                                for (i = 1; i < tr.length; i++) {
                                    td = tr[i].getElementsByTagName("td");
                                    for (j = 0; j < td.length; j++) {
                                        if (td[j].innerHTML.toUpperCase().indexOf(filter) > -1) {
                                            found = true;
                                        }
                                    }
                                    if (found) {
                                        tr[i].style.display = "";
                                        found = false;
                                    } else {
                                        tr[i].style.display = "none";
                                    }
                                }
                            }
                        </script>



                        <div class="form-row">

                            <script type='text/javascript'>
                                $(document).ready(function () {

                                    $('#openBtn').click(function () {
                                        $('#myModal').modal({
                                            show: true
                                        })
                                    });

                                    $(document).on('show.bs.modal', '.modal', function (event) {
                                        var zIndex = 1040 + (10 * $('.modal:visible').length);
                                        $(this).css('z-index', zIndex);
                                        setTimeout(function () {
                                            $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
                                        }, 0);
                                    });


                                });

                                function Confirm() {

                                    $('#myModal').modal('show')

                                }
                                function openModal() {
                                    $('.modal').modal('hide');
                                    //  $('#Modalstart').modal('show')
                                    //  var modal4 = document.getElementById('myModal4');
                                    // var mymodal = document.getElementById('myModal');
                                    // modal4.style.display = "none";
                                    // mymodal.style.display = "none";
                                    //  alert("I am deleting");

                                }
                                function close() {
                                    var modal4 = document.getElementById('myModal4');
                                    var mymodal = document.getElementById('myModal');
                                    modal4.style.display = "none";
                                    mymodal.style.display = "none";
                                }
                                var modal4 = document.getElementById('myModal4');
                                var mymodal = document.getElementById('myModal');

                                window.onclick = function (event) {
                                    //alert(event.target)
                                    if (event.target == modal4 || event.target == mymodal) {
                                        modal4.style.display = "none";
                                        mymodal.style.display = "none";
                                    }
                                }
                            </script>


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
                                                <img src="assets/img/cross.png" style="width: 50px; height: 50px" />
                                            </p>
                                            <p>
                                                <b>ParentKey: </b>
                                                <asp:Label ID="lblParentKey" Text="" runat="server" />
                                                <asp:Label ID="lblDelete" Text="" runat="server" />
                                            </p>

                                        </div>
                                        <div class="modal-footer">

                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                            <a data-toggle="modal" href="#myModal4" class="btn btn-primary">Delete</a>
 <%--                                           <asp:Button ID="btnDelete" class="btn btn-danger" runat="server" OnClick="btnDelete_Click" UseSubmitBehavior="false" data-dismiss="modal" Text="Delete" />--%>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                      


                        <div class="form-row">


                            <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px">
                                <asp:Label ID="lblDistricts" runat="server" CssClass="btn btn-primary" Style="width: 215px" Text="Districts"></asp:Label>

                            </div>

                            <div class="form-group col-md-2" style="padding-left: 0px; margin-top: 10px">
                                <asp:Label ID="lblClusterCode" runat="server" CssClass="btn btn-primary" Text="Cluster-Code" Visible="true"></asp:Label>

                            </div>
                            <div class="form-group col-md-2" style="padding-left: 0px; margin-top: 10px">
                                <asp:Label ID="lblLocality" runat="server" CssClass="btn btn-info" Style="width: 150px" Text="Locality" Visible="true"></asp:Label>
                            </div>
                            <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px">
                                <asp:Label ID="lblLister_name" runat="server" CssClass="btn btn-success" Style="width: 215px" Text="Lister Name" Visible="true"></asp:Label>

                            </div>

                            <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px">
                                <asp:Label ID="lblTotal_hh" runat="server" CssClass="btn btn-warning" Text="Total HH" Visible="true"></asp:Label>

                            </div>

                            <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px">
                                <asp:Label ID="lblTotal_children" runat="server" CssClass="btn btn-warning" Text="Total Child" Visible="true"></asp:Label>

                            </div>



                        </div>


                        <div class="form-row">
                            <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px">
                                <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged"></asp:ListBox>
                            </div>
                            <div class="form-group col-md-2" style="padding-left: 0px; margin-top: 10px">
                                <asp:ListBox ID="lstClusterCodes" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstClusterCodes_SelectedIndexChanged" Visible="true"></asp:ListBox>
                            </div>
                            <div class="form-group col-md-2" style="padding-left: 0px; margin-top: 10px">
                                <asp:ListBox ID="lstLocality" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstLocality_SelectedIndexChanged" Visible="true"></asp:ListBox>
                            </div>
                            <div class="form-group col-md-3" style="padding-left: 0px; margin-top: 10px">
                                <asp:ListBox ID="lstLister_name" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstLister_name_SelectedIndexChanged" Visible="true"></asp:ListBox>
                            </div>
                            <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px">
                                <asp:ListBox ID="lstTotal_hh" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstTotal_hh_SelectedIndexChanged" Visible="true"></asp:ListBox>
                            </div>
                            <div class="form-group col-md-1" style="padding-left: 0px; margin-top: 10px">
                                <asp:ListBox ID="lstTotal_children" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstTotal_children_SelectedIndexChanged" Visible="true"></asp:ListBox>


                            </div>



                            <script type="text/javascript">
                                function onMouseOver(rowIndex) {
                                    var gv = document.getElementById("GridView1");
                                    var rowElement = gv.rows[rowIndex];
                                    rowElement.style.backgroundColor = "#c8e4b6";

                                }

                                function onMouseOut(rowIndex) {
                                    var gv = document.getElementById("GridView1");
                                    var rowElement = gv.rows[rowIndex];
                                    rowElement.style.backgroundColor = "#fff";

                                }
                            </script>

                            <div class="row">

                                <div class="col-md-12">
                                    <hr>
                                </div>
                            </div>


                            <div class="form-group col-md-12" style="padding-left: 0px; margin-top: 10px">

                                <div class="table-responsive">
                                    <asp:GridView ID="GridView1" AllowPaging="true" OnSelectedIndexChanged="OnSelectedIndexChanged" OnPageIndexChanging="OnPageIndexChanging" PageSize="20" OnRowDataBound="GridView1_RowDataBound" runat="server" BorderStyle="None"
                                        CssClass="table table-striped table-bordered table-hover">
                                        <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                        <HeaderStyle BackColor="#13B093" Font-Bold="True" ForeColor="White" />

                                    </asp:GridView>

                                </div>
                            </div>
                        </div>
                    </ContentTemplate>

                </asp:UpdatePanel>
            </div>
              <div class="modal fade" id="myModal4">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    
                                    
                                     <h3>Final Confirm Delete</h3>

                                        </div>
                                        <div class="modal-body" style="background-color:#ffffff">
                                            <p>
                                                You are about to delete this cluster data, Are you sure ?
                                               <img src="assets/img/cross.png" style="width: 50px; height: 50px" />
                                            </p>
                                           

                                        </div>
                                    <div class="modal-footer">
                                        <a href="#" data-dismiss="modal" onclick="close();"  class="btn">Close</a>
                                       <asp:Button ID="btnDelete" class="btn btn-danger" runat="server" OnClick="btnDelete_Click" UseSubmitBehavior="false" data-dismiss="modal" Text="I am Sure! Delete" />

                                    </div>
                                </div>
                            </div>
        </div>

        
                       
    </body>
    </html>
</asp:Content>

