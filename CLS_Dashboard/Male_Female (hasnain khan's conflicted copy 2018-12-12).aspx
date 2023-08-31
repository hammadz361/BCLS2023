<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Male_Female.aspx.cs" Inherits="CLS_Dashboard.Male_Female" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <div class="content">


        <div class="container-fluid">


            <div class="card" style="padding: 0px 8px 13px 8px; width: 100%; margin-left: 0px;">
                <div class="card-header" data-background-color="blue">
                    <h4 class="title">CLS -  OVERALL MALE FEMALE AGE GENDER SURVEY Progress</h4>

                </div>
            </div>

            <div class="card" style="text-align: center; padding: 10px 8px 13px 8px; width: 97%; margin-left: 14px; margin-bottom: 0px;">
                 <table class="table table-hover" style="text-align: left; margin-left: 5%; width: 90%">
                                <tr>

                                    <td>
                                        <asp:Label ID="lblDistricts" runat="server" CssClass="btn btn-primary" Text="Districts"></asp:Label></td>
                                   
                                   
                                    <td>
                                        <asp:Label ID="lblSurveyTeam" runat="server" CssClass="btn btn-info" Text="Survey-Team" ></asp:Label></td>
                                     <td>
                                        <asp:Label ID="lblEnumerator" runat="server" CssClass="btn btn-warning" Text="Enumerator" ></asp:Label></td>
                                    </tr>



                                <tr>

                                    <td>
                                        <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstDistricts_SelectedIndexChanged"></asp:ListBox></td>
                                      
                                    
                                    <td>
                                        <asp:ListBox ID="lstSurveyTeam" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstSurveyTeam_SelectedIndexChanged" ></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstEnumerator" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="lstEnumerator_SelectedIndexChanged" ></asp:ListBox></td>
                                     </tr>

                            </table>
            </div>
             <div class="col-md-12" style="height:600px">

                <div class="card" style="height:500px" id="container_MemberInGroupof5"></div>
            </div>
            <div class="col-md-12" style="height:600px">

                <div class="card" style="height:500px" id="container_MemberUnder20"></div>
            </div>

            


        </div>

   </div>

            


    <script type="text/javascript">
        function UpdateChartAge() {
            var title1 = '<%= chartTitle1 %>';

            $(function () {

                $.ajax({

                    url: "Male_Female.aspx/getChartData_Dashboard_MemberUnder20",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (xhr, errorType, exception) {
                        var err = eval("(" + xhr.responseText + ")");
                        //Display Error Message.
                        alert(err.Message);
                    },
                    success: function (response) {

                        Highcharts.setOptions({ lang: { noData: "No data found" } })

                        var categories = eval(response.d[0]);
                        var Male = eval(response.d[1]);
                        var Female = eval(response.d[2]);

                        

                      
                        
                        
                        

                        var charts = new Highcharts.chart('container_MemberUnder20', {
                            chart: {
                                type: 'bar'
                            },
                            title: {
                                text: title1
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: [{
                                categories: categories,
                                reversed: false,
                                labels: {
                                    step: 1
                                }
                            }, { // mirror axis on right side
                                opposite: true,
                                reversed: false,
                                categories: categories,
                                linkedTo: 0,
                                labels: {
                                    step: 1
                                }
                            }],
                            yAxis: {
                                title: {
                                    text: null
                                },
                                labels: {
                                    formatter: function () {
                                        return Math.abs(this.value) + '%';
                                    }
                                }
                            },

                            plotOptions: {
                                series: {
                                    stacking: 'normal'
                                }
                            },

                            tooltip: {
                                formatter: function () {
                                    return '<b>' + this.series.name + ', age ' + this.point.category + '</b><br/>' +
                                        'Population: ' + Highcharts.numberFormat(Math.abs(this.point.y), 0);
                                }
                            },

                            series: [{
                                name: 'Male',
                                data: Male
                            }, {
                                name: 'Female',
                                data: Female
                            }]
                        });

                    }

                });
            }
                  );
      
                     var title2 = '<%= chartTitle2 %>';

            $(function () {

                $.ajax({

                    url: "Male_Female.aspx/getChartData_Dashboard_MemberInGroupof5",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        var categories = eval(response.d[0]);
                        var Male = eval(response.d[1]);
                        var Female = eval(response.d[2]);

                       

                        Highcharts.chart('container_MemberInGroupof5', {
                            chart: {
                                type: 'bar'
                            },
                            title: {
                                text: title2
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: [{
                                categories: categories,
                                reversed: false,
                                labels: {
                                    step: 1
                                }
                            }, { // mirror axis on right side
                                opposite: true,
                                reversed: false,
                                categories: categories,
                                linkedTo: 0,
                                labels: {
                                    step: 1
                                }
                            }],
                            yAxis: {
                                title: {
                                    text: null
                                },
                                labels: {
                                    formatter: function () {
                                        return Math.abs(this.value) + '%';
                                    }
                                }
                            },

                            plotOptions: {
                                series: {
                                    stacking: 'normal'
                                }
                            },

                            tooltip: {
                                formatter: function () {
                                    return '<b>' + this.series.name + ', age ' + this.point.category + '</b><br/>' +
                                        'Population: ' + Highcharts.numberFormat(Math.abs(this.point.y), 0);
                                }
                            },

                            series: [{
                                name: 'Male',
                                data: Male
                            }, {
                                name: 'Female',
                                data: Female
                            }]
                        });

                    }

                });
            }
                  );
        }

    </script>
  </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
