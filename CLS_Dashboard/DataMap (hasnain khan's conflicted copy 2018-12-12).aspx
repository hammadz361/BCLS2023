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
           

                <table class="table table-hover" style="text-align: left; margin-left: 5%;">
                    <tr>

                        <td>
                            <asp:Label ID="labelDistrict" runat="server" Text="District : " CssClass="btn btn-primary" ></asp:Label></td>
                        <td>
                            <asp:Label ID="labelSupervisor" runat="server" Text="Supervisor : " CssClass="btn btn-info" ></asp:Label></td>


                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lstDistricts" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="Filter_Selected" Font-Size="Large" ForeColor="#006600">
                                <asp:ListItem Text="All" Value="All"> </asp:ListItem>
                            </asp:ListBox></td>


                        <td>
                            <asp:ListBox ID="lstSupervisor" runat="server" AutoPostBack="true" SelectionMode="Multiple" OnSelectedIndexChanged="Filter_Selected"  Font-Size="Large" ForeColor="#006600">
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

                    <asp:Literal ID="js" runat="server"></asp:Literal>
                    <%--Place for google to show your MAP--%>
                    <div id="map_canvas" style="width: 100%; height: 728px; margin-bottom: 2px;">
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



</asp:Content>
