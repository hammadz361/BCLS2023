<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SearchListing.aspx.cs" Inherits="CLS_Dashboard.SearchListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

         

            <div class="content">
                <div class="container-fluid" style="margin-top: 20px">
                    <div class="card card-nav-tabs">
                        <div class="card-header" data-background-color="purple">
                            <div class="nav-tabs-navigation">
                                <div class="nav-tabs-wrapper">
                                    <span class="nav-tabs-title">Listing Data Search</span>
                                    <ul class="nav nav-tabs" data-tabs="tabs">
                                        <li >
                                            <a href="#Users" data-toggle="tab">
                                                  
						<div class="ripple-container"></div>
                                            </a>
                                        </li>

                                     
                                       
                                    </ul>
                                </div>
                            </div>
                        </div>
                                 <div class="card-content table-responsive" style="width: 100%;overflow-x: auto;float:none; padding-left:0px; margin-left:25px">
                                     <div class="row">
                                                                    <div class="col-md-6">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Household ID:</label>
                                                                            <asp:TextBox ID="HH_ID" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                         <div class="form-group label-floating">
                                                                            <label class="control-label">And Cluster Code:</label>
                                                                             <asp:TextBox ID="Cluster_Code" runat="server" class="form-control"></asp:TextBox>
                                                                        </div>
                                                                        <div class="form-group label-floating">
                                                                            <asp:Button ID="Button1" OnClick="Button1_Click" runat="server" Text="Search" />
                                                                        </div>
                                                                    </div>
                                         <div class="col-md-12">
                                             <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" >
                                                 <Columns>
                                                <asp:BoundField DataField="Key_" HeaderText="HouseHold Key" />
                                                <asp:BoundField DataField="Cluster" HeaderText="Cluster Code" />
                                                <asp:BoundField DataField="SR_NO_HOUSEHOLD" HeaderText="Household ID" />
                                                <asp:BoundField DataField="District" HeaderText="Name of District" />
                                                <asp:BoundField DataField="NOTABLE_PERSON" HeaderText="Notable Person" />
                                                <asp:BoundField DataField="NOTABLE_PERSON_CELL" HeaderText="Notable Person Phone" />
                                                <asp:BoundField DataField="LOCALITY_ADDRESS"  HeaderText="Locality Address"/>
                                                <asp:BoundField DataField="Lister" HeaderText="Lister Name"/>
                                                <asp:BoundField DataField="LOCALITY" HeaderText="LOCALITY"/>
                                                <asp:BoundField DataField="Address" HeaderText="HH Address"/>
                                                <asp:BoundField DataField="HHMEMBERS_COUNT" HeaderText="HH Members"/>
                                                <asp:BoundField DataField="HH_HEAD_NAME" HeaderText="HH Head"/>
                                                <asp:BoundField DataField="Children" HeaderText="Children"/>
                                                <asp:BoundField DataField="Boys" HeaderText="Boys"/>
                                                <asp:BoundField DataField="Girls" HeaderText="Girls"/>
                                                <asp:BoundField DataField="CHILD_SCHOOL_COUNT" HeaderText="School Going Children"/>
                                                <asp:BoundField DataField="CHILD_PRESENT_STATUS" HeaderText="Children Present"/>
                                                <asp:HyperLinkField DataNavigateUrlFields="RECORDGPS_FIRST_LNG,RECORDGPS_FIRST_LAT" DataNavigateUrlFormatString="PlotOnMap.aspx?long={0}&lat={1}"  Text="Plot on Map" />
                                                
                                            </Columns>
                                             </asp:GridView>  
                                             <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
                                                                    </div>
                                         
                                                                  
                                                             

                                                                    
                                                                </div>

                                </div> 
                                
                               
                            </div>
                        </div>
                        

                    </div>
                </div>
            </div>



        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>