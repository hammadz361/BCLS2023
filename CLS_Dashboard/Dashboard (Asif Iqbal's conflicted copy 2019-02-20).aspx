<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CLS_Dashboard.Dashboard1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js"  type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#ContentPlaceHolder1_lstDistricts').multiselect({
                includeSelectAllOption: true
            });
            $('#btnSelected').click(function () {
                var selected = $("#ContentPlaceHolder1_lstDistricts option:selected");
                var message = "";
                selected.each(function () {
                    message += $(this).val() + ",";
                });
                window.location.href='Dashboard.aspx?Filter=1&District=' + message;
            });
        });
    </script>
   
 
    
    <%--  <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
            <script type="text/javascript">
                function ShowProgress() {
                    setTimeout(function () {
                        var modal = $('<div />');
                        modal.addClass("modal");
                        $('Content1').append(modal);
                        var loading = $(".loading");
                        loading.show();
                        var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                        var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                        loading.css({ top: top, left: left });
                    }, 200);
                }
                window.onload = ShowProgress;
                 
            </script>--%>

    <%-- <div class="loading">
                    Loading. Please wait.<br />
                    <br />
                    <img src="../assets/img/loader.gif" alt="" />
                </div>--%>



    <div class="content">


        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-3 col-md-6 col-sm-6">
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
                <div class="col-lg-3 col-md-6 col-sm-6">
                    <div class="card card-stats">
                        <div class="card-header" data-background-color="green">
                            <asp:ImageButton ID="ImageButton3" src="../assets/img/Eumerators-Supervisors-3.gif" Text="CSV" Style="width: 52px; height: 52px" runat="server" />
                        </div>
                        <div class="card-content">
                            <p class="category">Enumerator / Supervisor</p>
                            <h3 class="title">
                                <asp:Label ID="lbl_Indicator_Enumerator" runat="server" Text=""></asp:Label>
                                /
                                <asp:Label ID="lbl_Indicator_Supervisor" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                        <div class="card-footer">
                            <div class="stats">
                                <i class="material-icons">date_range</i> Engaged to date
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6 col-sm-6">
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
                                <i class="material-icons">local_offer</i> visited by enumerators
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6 col-sm-6">
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
            </div>

            <br />

            <div class="card" style="padding: 10px 8px 13px 8px; width: 100%; margin-left: 0px;">
                <div class="card-header" data-background-color="green">
                    <h4 class="title">CLS -  OVERALL SURVEY Progress</h4>

                </div>
            </div>
           
            <div>
                <table>
                    <tr>

                        <div id="container1"></div>
                         <div class="col-md-4">
                        <div class="card" id="container11"></div>
                    </div>
                         <div class="col-md-4">
                        <div class="card" id="container4"></div>
                    </div>

                    </tr>
                </table>

                   <div class="row" style="width: 100%; margin-left: 0px;">
                    <br />


                <div class="card" style="text-align: center; padding: 10px 8px 13px 8px; width: 97%; margin-left: 14px; margin-bottom: 0px;">
                         <table class="table table-hover" style="text-align: left; margin-left: 5%; width: 90%">
                            <tr>

                                <td>
                                    <asp:Label ID="lblDistricts" runat="server" CssClass="btn btn-primary" Text="Districts"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblTypes" runat="server" CssClass="btn btn-primary" Text="Types" Visible="false"></asp:Label></td>

                            </tr>


                            <tr>

                                <td>
                                    <asp:ListBox ID="lstDistricts" runat="server"  SelectionMode="Multiple"></asp:ListBox>

    <input type="button" id="btnSelected" value="Apply Filter" />

                                </td>

                            </tr>

                        </table>
                    </div>


                <div class="row" style="width: 100%; margin-left: 0px;">
                    <br />

                    <div class="card" style="padding: 10px 8px 13px 8px; width: 100%; margin-left: 0px;">
                        <div class="card-header" data-background-color="blue">
                            <h4 class="title">CLS -  CLUSTER Level</h4>

                        </div>
                    </div>
                     
                                        
                    <div class="col-md-4">

                        <div class="card" id="container2"></div>
                    </div>

                    <div class="col-md-4">
                        <div class="card" id="container3"></div>
                    </div>

                  </div>

                      

                    <br />

                    <div class="card" style="padding: 10px 8px 13px 8px; width: 100%; margin-left: 0px;">
                        <div class="card-header" data-background-color="purple">
                            <h4 class="title">CLS -  HOUSEHOLD level</h4>

                        </div>
                    </div>

                       

                    <div class="col-md-4">
                        <div class="card" id="container5"></div>
                    </div>
                    <div class="col-md-4">
                        <div class="card" id="container6"></div>
                    </div>


                    <br />

                    <div class="card" style="padding: 10px 8px 13px 8px; width: 100%; margin-left: 0px;">
                        <div class="card-header" data-background-color="orange">
                            <h4 class="title">CLS -  SUPERVISOR Level</h4>

                        </div>
                    </div>

                    <div class="col-md-4">

                        <div class="card" id="container7" style="margin-top: 0px;"></div>
                    </div>

                    <div class="col-md-4">
                        <div class="card" id="container8" style="margin-top: 0px;"></div>
                    </div>


                    <br />

                    <div class="card" style="padding: 10px 8px 13px 8px; width: 100%; margin-left: 0px;">
                        <div class="card-header" data-background-color="red">
                            <h4 class="title">CLS -  CHILDREN Level</h4>

                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="card" id="container9"></div>
                    </div>

                    <div class="col-md-4">
                        <div class="card" id="container10"></div>
                    </div>

                
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
             var title1 = '<%= chartTitle1 %>';

             $(function () {

                 $.ajax({

                     url: "Dashboard.aspx/getChartData1",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (response) {

                         var TotalFormSubmitted = eval(response.d[0]);
                         var PerDay = eval(response.d[1]);
                         var RequiredDays = eval(response.d[2]);
                         var Line_C = eval(response.d[3]);

                         Highcharts.chart('container1', {
                             chart: {

                                 margin: [89, 89, 150, 89]
                             },
                             title: {
                                 text: title1
                             },

                             subtitle: {
                                 text: 'Source: Interviewers submissions'
                             },

                             yAxis: {
                                 title: {
                                     text: 'Household Survey'
                                 }
                             },
                             xAxis: {
                                 categories: PerDay,

                                 //labels: { rotation: 35, x: -20 },

                                 tickInterval: 5, // 5 day,


                             },
                             legend: {
                                 layout: 'vertical',
                                 align: 'center',
                                 verticalAlign: 'bottom'
                             },
                             tooltip: {
                                 pointFormat: '{series.name}: <b>{point.y}</b>', headerFormat: '<b>{point.key}</b><br>'
                             },
                             plotOptions: {
                                 line: {
                                     dataLabels: {
                                         enabled: true,
                                         formatter: function () {

                                             return this.point.category;


                                         }
                                     },
                                 },
                             },
                             series: [{
                                 name: 'Actual Form Submitted',
                                 color: '#16cc22',
                                 data: TotalFormSubmitted
                             }, {
                                 name: 'Additional projected fieldwork days',
                                 data: RequiredDays,
                                 dashStyle: 'dot',
                                 //dataLabels: {
                                 //    enabled: true,
                                 //    borderRadius: 5,
                                 //    backgroundColor: 'rgba(252, 255, 197, 0.7)',
                                 //    borderWidth: 1,
                                 //    borderColor: '#AAA',
                                 //    y: -6,
                                 //    borderColor: 'red',
                                 //    borderWidth: 2,
                                 //    padding: 5,
                                 //    style: {
                                 //        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                 //    },
                                 //    connectorColor: 'silver'
                                 //}

                             },
                             {
                                 name: 'Originally Projected Completion',
                                 color: 'orange',
                                 data: Line_C
                             }]

                         });

                     }

                 });
             }
             );


    </script>

    <script type="text/javascript">
        function UpdateChart2() {
            var title14 = '<%= chartTitle2 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData23",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {


                        Highcharts.chart('container2', {
                            chart: {
                                type: 'column',
                                options3d: {
                                    enabled: true,
                                    alpha: 20,
                                    beta: 20,

                                    viewDistance: 150,
                                    depth: 20,

                                }
                            },
                            title: {
                                text: title14
                            },
                            subtitle: {
                              text: 'Source: Supervisors app'  
                            },

                            xAxis: {
                                labels: { rotation: 10, y: 30, staggerLines: 1 },
                                categories: ['Cluster Initiated', 'Cluster finalised', 'Cluster to be Revisited'],


                            },
                            yAxis: {

                                gridLineColor: 'transparent',
                                title: {
                                    text: 'Percentage Progress',
                                    align: 'center'
                                },
                                labels: {
                                    overflow: 'justify',
                                    skew3d: true,


                                }
                            },
                            tooltip: {
                                valueSuffix: ' Percent'
                            },
                            plotOptions: {

                                column: {
                                    depth: 50,
                                    dataLabels: {

                                        enabled: true,
                                        borderRadius: 5,
                                        backgroundColor: 'rgba(220, 218, 218, 0.49)',
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                                        formatter: function () {
                                            // numberFormat takes your label's value and the decimal places to show
                                            return Highcharts.numberFormat(this.y, 2) + '%';
                                        },
                                    }
                                }

                            },
                            legend: {
                                layout: 'horizontal',
                                align: 'center',
                                verticalAlign: 'bottom',

                                shadow: false
                            },
                            credits: {
                                enabled: false
                            },
                            series: [ <%= chart_SurveyProgress_ClusterLevel %> ]

                        });

                    }

                });
            }
            );
        }

    </script>


    <script type="text/javascript">
        function UpdateChart3() {
            var title2 = '<%= chartTitle3 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData3",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        Highcharts.chart('container3', {
                            chart: {
                                type: 'bar'
                            },
                            title: {
                                text: title2
                            },
                            subtitle: {
                                text:'Source: Supervisors app'
                            },
                            xAxis: {
                                categories: [ <%= chart_district %> ],
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: 'Clusters count',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                valueSuffix: ' Clusters'
                            },
                            plotOptions: {
                                bar: {
                                    grouping: false,
                                    shadow: false,
                                    borderWidth: 0,
                                    dataLabels: {
                                        enabled: true,
                                        borderRadius: 5,
                                        backgroundColor: 'rgba(220, 218, 218, 0.49)',
                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'center',
                                verticalAlign: 'bottom',


                            },
                            credits: {
                                enabled: false
                            },
                            tooltip: {
                                shared: true
                            },
                            series: [ <%= chart_Completed_RemainingHHs %>]

                        });

                    }

                });
            }
            );
        }

    </script>

    <script type="text/javascript">

             var title1333 = '<%= chartTitle12 %>';



             $(function () {

                 $.ajax({

                     url: "Dashboard.aspx/getChartData4",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     error: function (xhr, errorType, exception) {
                         var err = eval("(" + xhr.responseText + ")");
                         //Display Error Message.
                         alert(err.Message);
                     },
                     success: function (response) {
                         var district = eval(response.d)[0];
                         var chartOverallPercent = eval(response.d[1]); //Data 

                         var total = 0;

                         //var str = 'Hello, World, etc';
                         var str_array = district.split(',');

                         for (var i = 0; i < str_array.length; i++) {
                             // Trim the excess whitespace.
                             str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                             // Add additional code here, such as:
                             //alert(str_array[i]);
                         }





                         Highcharts.chart('container4', {
                             colors: ['#90ed7d', 'red'],
                             chart: {
                                 renderTo: 'container4',
                                 type: 'pie',
                                 options3d: {
                                     enabled: true,
                                     alpha: 45,
                                     beta: 0
                                 }
                             },
                             xAxis: {
                                 categories: [district],
                             },
                             title: {
                                 text: title1333
                             },
                             subtitle: {
                                 text: 'Source:  Interviewers submissions'
                             },

                             plotOptions: {
                                 pie: {
                                     point: {
                                         events: {
                                             legendItemClick: function () {
                                                 if (total > 1) total = 0;
                                                 return this.name;
                                             }
                                         }
                                     },

                                     showInLegend: true
                                 },
                                 series: {

                                     depth: 35,
                                     dataLabels: {
                                         enabled: true,
                                         formatter: function () {
                                             if (total > 1) total = 0;
                                             return '</b>' + str_array[total++] + ':' + this.y;

                                         },

                                         allowOverlap: false,
                                         distance: -30,
                                         color: 'white'
                                     },


                                     showInLegend: true,



                                 }

                             },
                             legend: {
                                 enable: true,
                                 labelFormatter: function () {

                                     if (total > 1) total = 0;
                                     return this.name[0] = str_array[total++];



                                 },

                             },

                             series: [{
                                 name: 'Percent',

                                 data: chartOverallPercent,

                             }]

                         });

                     },


                 });
             }
             );


    </script>
   <!-- <script type="text/javascript">

     



        $(function () {

            $.ajax({

                url: "Dashboard.aspx/getChartData4",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                error: function (xhr, errorType, exception) {
                    var err = eval("(" + xhr.responseText + ")");
                    //Display Error Message.
                    alert(err.Message);
                },
                success: function (response) {
                    var district = eval(response.d)[0];
                    var chartOverallPercent = eval(response.d[1]); //Data 
                    
                    var total = 0;

                    //var str = 'Hello, World, etc';
                    var str_array = district.split(',');

                    for (var i = 0; i < str_array.length; i++) {
                        // Trim the excess whitespace.
                        str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                        // Add additional code here, such as:
                        //alert(str_array[i]);
                    }





                    Highcharts.chart('container4', {
                        colors: ['#8533ff', '#a366ff', '#c299ff', '#e0ccff'],
                        chart: {
                            renderTo: 'container4',
                            type: 'pie',
                            options3d: {
                                enabled: true,
                                alpha: 45,
                                beta: 0
                            }
                        },
                        xAxis: {
                            categories: district,
                            title: {
                                text: null
                            }
                        },
                        title: {
                            text: titlehousehold
                        },

                        plotOptions: {
                            pie: {
                                point: {
                                    events: {
                                        legendItemClick: function () {
                                            if (total > str_array.length) total = 0;
                                            return this.name;
                                        }
                                    }
                                },

                                showInLegend: true
                            },
                            series: {

                                depth: 35,
                                dataLabels: {
                                    enabled: true,
                                    formatter: function () {
                                        if (total > str_array.length - 1) total = 0;
                                        return '</b>' + str_array[total++] + ':' + this.y;

                                    },

                                    allowOverlap: false,
                                    distance: -30,
                                    color: 'white'
                                },


                                showInLegend: true,



                            }

                        },
                        legend: {
                            enable: true,
                            labelFormatter: function () {

                                if (total > str_array.length) total = 0;
                                return this.name[0] = str_array[total++];



                            },

                        },

                        series: [{
                            name: 'Percent',

                            data: chartOverallPercent,

                        }]

                    });

                },


            });
        }
              );


    </script>
       -->
    <script type="text/javascript">
        function UpdateChart5() {
            var title5 = '<%= chartTitle5 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData5",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var District = eval(response.d[0]);



                        Highcharts.chart('container5', {
                            chart: {
                                type: 'column'

                            },
                            title: {
                                text: title5
                            },
                            subtitle: {
                                text: 'Source: Interviewers submissions'
                            },
                            xAxis: {
                                categories: District,
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: ' ',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                shared: true
                            },
                            plotOptions: {
                                column: {
                                    stacking: 'normal',
                                    dataLabels: {
                                        enabled: true,
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'center',
                                verticalAlign: 'bottom',
                                shadow: false


                            },
                            credits: {
                                enabled: false
                            },
                            series: [ <%= chart_Enumeration_Progress %>]

                        });

                    }

                });
            });

        }
    </script>

    <script type="text/javascript">
        function UpdateChart6() {
            var title11 = '<%= chartTitle6 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData6",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var District = eval(response.d[0]);


                        Highcharts.chart('container6', {
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: title11
                            },
                            subtitle: {
                                text: 'Source: Supervisors app'
                            },
                            xAxis: {
                                categories: District,
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: ' ',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                shared: true
                            },
                            plotOptions: {
                                column: {
                                    stacking: 'normal',
                                    dataLabels: {
                                        enabled: true,
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'center',
                                verticalAlign: 'bottom',
                                shadow: false


                            },
                            credits: {
                                enabled: false
                            },


                            series: [ <%= chart_HH_Visited_Interview_not_Completed %>]

                        });

                    }

                });
            }
            );
        }

    </script>

    <script type="text/javascript">
        function UpdateChart7() {
            var title9 = '<%= chartTitle7 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData7",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {


                        var Team_Supervisor = eval(response.d[0]);
                        var Clusters_Revisit = eval(response.d[1]);
                        var Clusters_In_Progress = eval(response.d[2]);
                        var Clusters_Completed = eval(response.d[3]);
                        var Clusters_Assigned = eval(response.d[4]);



                        Highcharts.chart('container7', {
                            chart: {
                                margin: [89, 20, 100, 60],
                            },

                            title: {
                                text: title9
                            },
                            subtitle: {
                                text: 'Source: Supervisors app'
                            },
                            xAxis: {
                                categories: Team_Supervisor,
                                labels: {
                                    step: 1,
                                    formatter: function () {
                                        return this.value.replace(/ /g, '<br />');
                                    },
                                    style: {
                                        fontSize: '8px',
                                        //fontFamily: 'proxima-nova,helvetica,arial,sans-seri',
                                        whiteSpace: 'nowrap',
                                        //paddingLeft: '10px',
                                        //paddingRight: '10px',
                                        //paddingTop: '10px',
                                        //paddingBottom: '10px',
                                        padding: '50px',


                                    }
                                },

                            },




                            yAxis: {
                                min: 0,
                                title: {
                                    text: 'Clusters',
                                    align: 'high'
                                },
                            },
                            stackLabels: {
                                enabled: true,
                                style: {
                                    fontWeight: 'bold',
                                    fontSize: '6px',
                                    color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                                }
                            },
                            legend: {
                                column: {
                                    pointPadding: 0.4,
                                    borderWidth: 0
                                },

                                align: 'center',
                                x: 0,
                                verticalAlign: 'bottom',
                                y: 0,
                                floating: true,
                                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                                borderColor: '#CCC',
                                borderWidth: 1,
                                shadow: false
                            },
                            tooltip: {
                                headerFormat: '<b>{point.x}</b><br/>',
                                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
                            },
                            plotOptions: {
                                column: {
                                    stacking: 'normal',
                                    dataLabels: {

                                        enabled: true,
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                                    }
                                }
                            },
                            series: [{
                                type: 'spline',
                                dataLabels: {
                                    enabled: true,
                                    color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'black'
                                },
                                name: 'Clusters_Assigned',
                                data: Clusters_Assigned,
                                color: '#24E07C',
                                marker: {
                                    lineWidth: 2,
                                    lineColor: Highcharts.getOptions().colors[2],
                                    fillColor: 'white'
                                }

                            }, {
                                type: 'column',
                                name: 'Clusters_To_Be_Visited',
                                color: '#800000',
                                data: Clusters_Revisit

                                },
                                {
                                type: 'column',
                                name: 'Cluster Initiated',
                                color: '#FFCC00',
                                data: Clusters_In_Progress

                                },                     
                                {
                                type: 'column',
                                name: 'Cluster Finalised',
                                color: '#FF9900',
                                data: Clusters_Completed

                            }]
                        });

                    }

                });
            }
            );

        }
        function UpdateChart8() {

            var title = '<%= chartTitle8 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData8",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        var Team_Supervisor = eval(response.d[0]); //Data 
                        var Completed_Percentage = eval(response.d[1]);
                        var Progress_Percentage = eval(response.d[2]);
                        var Revisit = eval(response.d[3]);


                        Highcharts.chart('container8', {
                            chart: {
                                type: 'column',
                                marginBottom: 100,
                            },
                            title: {
                                text: title
                            },
                            subtitle: {
                                text: 'Source: Interviewers submissions'
                            },
                            xAxis: {
                                categories: Team_Supervisor,
                                title: {
                                    text: null
                                },
                                labels: {
                                    step: 1,
                                    formatter: function () {
                                        return this.value.replace(/ /g, '<br />');
                                    },
                                    style: {
                                        fontSize: '8px',
                                        //fontFamily: 'proxima-nova,helvetica,arial,sans-seri',
                                        whiteSpace: 'nowrap',
                                        //paddingLeft: '10px',
                                        //paddingRight: '10px',
                                        //paddingTop: '10px',
                                        //paddingBottom: '10px',
                                        //margin: '100px',


                                    }
                                },
                            },
                            yAxis: {
                                min: 0,
                                max: 100,
                                title: {
                                    text: 'Percentage',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                valueSuffix: '%'
                            },
                            plotOptions: {
                                column: {
                                    stacking: 'normal',
                                    dataLabels: {
                                        enabled: true,
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'center',
                                verticalAlign: 'bottom',

                                floating: true,
                                borderWidth: 1,
                                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                                shadow: true
                            },
                            credits: {
                                enabled: false
                            },
                            series: [{
                                name: 'Cluster finalised Percentage',
                                color: 'green',
                                data: Completed_Percentage
                            },
                                {
                                name: 'Cluster initiated Percentage',
                                color: 'orange',
                                data: Progress_Percentage
                                },
                                {
                                name: 'Cluster To be Visited',
                                color: '#f44336',
                                data: Revisit
                                }

                                
                            ]
                        });

                    }

                });
            }
            );
        }
    </script>


    <script type="text/javascript">
        function UpdateChart9() {
            var title55 = '<%= chartTitle9 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData9",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        var District = eval(response.d[0]);



                        Highcharts.chart('container9', {
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: title55
                            },
                            subtitle: {
                                text: 'Source:  Interviewers submissions'
                            },
                            xAxis: {
                                categories: District,
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: 'Population',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                valueSuffix: 'Population'
                            },
                            plotOptions: {
                                column: {
                                    dataLabels: {
                                        enabled: true,
                                        inside: Boolean,
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',

                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'center',
                                verticalAlign: 'bottom',


                            },
                            credits: {
                                enabled: false
                            },
                            series: [ <%= chart_Children_Statistics %>]

                        });

                    }

                });
            }
            );

        }
    </script>


    <script type="text/javascript">
        function UpdateChart10() {
            var title15 = '<%= chartTitle10 %>';

            $(function () {

                $.ajax({

                    url: "Dashboard.aspx/getChartData10",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        var District = eval(response.d[0]);


                        Highcharts.chart('container10', {
                            chart: {
                                type: 'column'
                            },
                            title: {
                                text: title15
                            },
                            subtitle: {
                                text: 'Source:  Interviewers submissions<br>Please note: The graphs presented below intend to show the number of working children and do not provide estimations of child labour'
                            },
                            xAxis: {
                                categories: District,
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: 'Population',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                valueSuffix: 'Population'
                            },
                            plotOptions: {
                                column: {
                                    dataLabels: {
                                        enabled: true,
                                        inside: Boolean,
                                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',

                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'center',
                                verticalAlign: 'bottom',


                            },
                            credits: {
                                enabled: false
                            },
                            series: [ <%= chart_Total_Working_Childern %>]

                        });

                    }

                });
            }
            );
        }

    </script>

     <script type="text/javascript">

             var title13 = '<%= chartTitle11 %>';



             $(function () {

                 $.ajax({

                     url: "Dashboard.aspx/getChartData11",
                     type: "POST",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     error: function (xhr, errorType, exception) {
                         var err = eval("(" + xhr.responseText + ")");
                         //Display Error Message.
                         alert(err.Message);
                     },
                     success: function (response) {
                         var district = eval(response.d)[0];
                         var chartOverallPercent = eval(response.d[1]); //Data 

                         var total = 0;

                         //var str = 'Hello, World, etc';
                         var str_array = district.split(',');

                         for (var i = 0; i < str_array.length; i++) {
                             // Trim the excess whitespace.
                             str_array[i] = str_array[i].replace(/^\s*/, "").replace(/\s*$/, "");
                             // Add additional code here, such as:
                             //alert(str_array[i]);
                         }





                         Highcharts.chart('container11', {
                             colors: ['#90ed7d', 'red'],
                             chart: {
                                 renderTo: 'container11',
                                 type: 'pie',
                                 options3d: {
                                     enabled: true,
                                     alpha: 45,
                                     beta: 0
                                 }
                             },
                             xAxis: {
                                 categories: [district],
                             },
                             title: {
                                 text: title13
                             },
                             subtitle: {
                                 text: 'Source: Supervisor App' 
                             },

                             plotOptions: {
                                 pie: {
                                     point: {
                                         events: {
                                             legendItemClick: function () {
                                                 if (total > 1) total = 0;
                                                 return this.name;
                                             }
                                         }
                                     },

                                     showInLegend: true
                                 },
                                 series: {

                                     depth: 35,
                                     dataLabels: {
                                         enabled: true,
                                         formatter: function () {
                                             if (total > 1) total = 0;
                                             return '</b>' + str_array[total++] + ':' + this.y;

                                         },

                                         allowOverlap: false,
                                         distance: -30,
                                         color: 'white'
                                     },


                                     showInLegend: true,



                                 }

                             },
                             legend: {
                                 enable: true,
                                 labelFormatter: function () {

                                     if (total > 1) total = 0;
                                     return this.name[0] = str_array[total++];



                                 },

                             },

                             series: [{
                                 name: 'Percent',

                                 data: chartOverallPercent,

                             }]

                         });

                     },


                 });
             }
             );

         
    </script>
    <script>
        UpdateChart2();
        UpdateChart3();
        UpdateChart5();
        UpdateChart6();
        UpdateChart7();
        UpdateChart8();
        UpdateChart9();
        UpdateChart10();
        function UpdateCharts() {
        UpdateChart2();
        UpdateChart3();
        UpdateChart5();
        UpdateChart6();
        UpdateChart7();
        UpdateChart8();
        UpdateChart9();
        UpdateChart10();
        }


        (function()
{
  if( window.localStorage )
  {
    if( !localStorage.getItem('firstLoad') )
    {
      localStorage['firstLoad'] = true;
     setTimeout(function(){window.location.href = window.location.href + '?r=1'},2000)
    }  
    else
      localStorage.removeItem('firstLoad');
  }
})();


    </script>
</asp:Content>