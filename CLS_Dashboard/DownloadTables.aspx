<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="DownloadTables.aspx.cs" Inherits="CLS_Dashboard.DownloadTables" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


      
     

    <div class="container-fluid" style="margin-top: 20px">

        <div class="col-md-12">

            <div class="card" style="text-align: center">
                <div class="card-header" data-background-color="blue">
                    <ul class="nav nav-tabs" data-background-color="blue" data-tabs="tabs">

                        <li class="">
                            <a href="#settings" data-toggle="tab">
                                <i class="material-icons">cloud_download</i>
                                Download File Types:
						<div class="ripple-container"></div>
                            </a>
                        </li>

                    </ul>
                </div>
                <br />

                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header" data-background-color="blue">
                            <h4 class="title">
                                <asp:Label ID="lblTableHeading" runat="server" Text=""></asp:Label></h4>
                            <p class="category">CLS -  Child Labour Survey</p>
                        </div>

                        <script type="text/javascript">
                            function toggle_visibility() {
                                var e = document.getElementById("Div_Download");
                                if (e.style.display == 'block')
                                    e.style.display = 'none';
                                else
                                    e.style.display = 'block';
                            }
                        </script>

                        <%--<div class="alert alert-success" id="Div_Download" runat="server">
                            <div class="container-fluid">
                                <div class="alert-icon">
                                    <i class="material-icons">check</i>
                                </div>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true"><i class="material-icons">clear</i></span>
                                </button>
                                <b>Success Alert:</b> Yuhuuu! You've got your $11.99 album from The Weeknd
                            </div>
                        </div>--%>


                        <div class="card-content table-responsive">

                            <asp:GridView ID="GridView1" CssClass="table table-hover" runat="server"
                                AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand" Width="80%" HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                <Columns>
                                    <asp:BoundField DataField="Detail" HeaderText="Description" ItemStyle-Width="250px" />

                                    <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="50px" />

                                    <asp:TemplateField HeaderText="Click here to File Download" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <%--<asp:LinkButton Text="Word" runat="server" CommandName="Word" CommandArgument="<%# Container.DataItemIndex %>" />
                                            <asp:LinkButton Text="Excel" runat="server" CommandName="Excel" CommandArgument="<%# Container.DataItemIndex %>" />
                                            <asp:LinkButton Text="PDF" runat="server" CommandName="PDF" CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                            <asp:ImageButton runat="server" ID="ImageButton1" src="../assets/img/CSV.png" Text="CSV" style="width: 32px; height: 32px"  CommandName="CSV" CommandArgument="<%# Container.DataItemIndex %>"/>
                                           <asp:Button runat="server" ID="request_btn" Text="Request" Visible="false"  CommandName="request" CommandArgument="<%# Container.DataItemIndex %>"/>
                                             <%--<asp:LinkButton Text="CSV" runat="server"  />--%>
                                            </br>
                                        <%--<img src="../assets/img/word.png " style="width: 32px; height: 32px" /> 
                                            <img src="../assets/img/Excel.png" style="width: 25px; height: 25px" />
                                            <img src="../assets/img/adobe.png" style="width: 25px; height: 25px" />--%>
                                            <%--<img src="../assets/img/CSV.png" style="width: 64px; height: 64px" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>   

</asp:Content>
