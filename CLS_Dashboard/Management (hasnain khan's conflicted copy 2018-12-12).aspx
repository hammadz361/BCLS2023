<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Management.aspx.cs" Inherits="CLS_Dashboard.Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

         

            <div class="content">
                <div class="container-fluid" style="margin-top: 20px">
                    <div class="card card-nav-tabs">
                        <div class="card-header" data-background-color="purple">
                            <div class="nav-tabs-navigation">
                                <div class="nav-tabs-wrapper">
                                    <span class="nav-tabs-title">Users:</span>
                                    <ul class="nav nav-tabs" data-tabs="tabs">
                                        <li class=<%=active1%> >
                                            <a href="#Users" data-toggle="tab">
                                                <i class="material-icons">person_add</i>
                                                Users 
						<div class="ripple-container"></div>
                                            </a>
                                        </li>

                        <span class="nav-tabs-title" style="margin-left:10px; margin-right:0px; font:bold">Allow Roles:</span>
                                        <li class=<%=active2%>>
                                            <a href="#profile" data-toggle="tab">
                                                <i class="material-icons">cloud_download</i>
                                                Download
						<div class="ripple-container"></div>
                                            </a>
                                        </li>
                                        <li class="">
                                            <a href="#maps" data-toggle="tab">
                                                <i class="material-icons">place</i>
                                                MAP
						<div class="ripple-container"></div>
                                            </a>
                                        </li>
                                       
                                    </ul>
                                </div>
                            </div>
                        </div>

                        <div class="card-content">
                            <div class="tab-content" style="margin-left:0px">

                                <div class="tab-pane <%=active1%>" id="Users">

                                    <script type="text/javascript">

                                        function MessageUsers() {
                                            
                                            var y = document.getElementById("DivUser");
                                            if (y.style.display === "none") {
                                                y.style.display = "block";

                                            } else {
                                                y.style.display = "none";
                                            }
                                        }

                                        function MessageUsersWarning() {
                                            
                                            var y = document.getElementById("DivUserError");
                                            if (y.style.display === "none") {
                                                y.style.display = "block";

                                            } else {
                                                y.style.display = "none";
                                            }
                                        }
                                                        </script>


                                                        <div class="alert alert-success" id="DivUser" style="display: none;">
                                                            <div class="container-fluid">
                                                                <div class="alert-icon">
                                                                    <i class="material-icons">check</i>
                                                                </div>
                                                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                                    <span aria-hidden="true"><i class="material-icons">clear</i></span>
                                                                </button>
                                                                <b>Success Alert:</b> You have successfully created the User. 
                                                            </div>
                                                            <br />
                                                        </div>



                                     <div class="alert alert-warning" id="DivUserError" style="display: none;">
                                                            <div class="container-fluid">
                                                                <div class="alert-icon">
                                                                    <i class="material-icons">error_outline</i>
                                                                </div>
                                                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                                    <span aria-hidden="true"><i class="material-icons">clear</i></span>
                                                                </button>
                                                                <b>Warning Alert:</b> One of the require fields is missing. 
                                                            </div>
                                                            <br />
                                                        </div>

                                  
                                        <div class="container-fluid" style="float:none">
                                            
                                                <div class="col-md-12" >
                                                    <div class="card">
                                                        <div class="card-header" data-background-color="purple">
                                                            <h4 class="title">Create User Profile</h4>
                                                            <p class="category">Fill complete profile</p>
                                                        </div>

                                                        <div class="card-content">
                                                           
                                                                <div class="row">

                                                                    <div class="col-md-3">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Username</label>
                                                                            <asp:TextBox ID="txtUser" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-3">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Password</label>
                                                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>

                                                                    <div class="col-md-6">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Role</label>
                                                                            <asp:DropDownList  ID="ddRoles" class="dropdown" runat="server">
                                                                                <asp:ListItem>Admin</asp:ListItem>
                                                                                <asp:ListItem>User</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div class="col-md-12">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Email address</label>
                                                                            <asp:TextBox ID="txtEmail" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>



                                                                <asp:Button ID="btnSubmit" class="btn btn-primary pull-right" runat="server" OnClick="btnSubmit_Click"  Text="Create Profile" />

                                                                <div class="clearfix"></div>
                                                         
                                                        </div>
                                                    </div>
                                                </div>


                                             <div class="col-md-12" >
                                                    <div class="card">
                                                        <div class="card-header" data-background-color="purple">
                                                            <h4 class="title">Create Monitor-Supervisor User Profile</h4>
                                                            <p class="category">Fill complete profile</p>
                                                        </div>

                                                        <div class="card-content">
                                                           
                                                                <div class="row">

                                                                   
                                                                  <div id="div_dduser" runat="server" class="col-md-3">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Name</label>
                                                                            <asp:TextBox ID="txtUserMonitorSupervisor" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>

                                                                      <div id="div_ddSupervisors" runat="server"  class="col-md-5" style="display:none; width:25%">
                                                                      <div class="form-group label-floating">
                                                                            <label class="control-label">Supervisor Name</label>
                                                                            <asp:DropDownList  ID="ddSupervisors" class="dropdown" runat="server">                                                                                
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                        </div>

                                                                    <div class="col-md-3">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Type</label>
                                                                            <asp:DropDownList  ID="ddRolesMonitorSupervisor" class="dropdown" runat="server" AutoPostBack = "true" OnSelectedIndexChanged="ddRolesMonitorSupervisor_SelectedIndexChanged">
                                                                                <asp:ListItem>DeskMonitors</asp:ListItem>
                                                                                <asp:ListItem>Supervisor</asp:ListItem>
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                    </div>
                                                                    
                                                                     <div id="div_Monitors" runat="server"  class="col-md-5" style="display:none; width:25%">
                                                                      <div class="form-group label-floating">
                                                                            <label class="control-label">Associate with Monitor</label>
                                                                            <asp:DropDownList  ID="ddRolesMonitors" runat="server" class="dropdown" AutoPostBack = "true">                                                                                
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                        </div>
                                                                  

                                                                   
                                                                </div>

                                                                <div class="row">
                                                                    <div class="col-md-3">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Email address</label>
                                                                            <asp:TextBox ID="txtEmailMonitorSupervisor" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>

                                                                     <div class="col-md-3">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Email CC ( More 1 ; separated)</label>
                                                                            <asp:TextBox ID="txtEmailCCMonitorSupervisor" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>

                                                                     <div class="col-md-5">
                                                                        <div class="form-group label-floating">
                                                                            <label class="control-label">Email BCC ( More 1 , separated)</label>
                                                                            <asp:TextBox ID="txtEmailBCCMonitorSupervisor" runat="server" class="form-control"></asp:TextBox>

                                                                        </div>
                                                                    </div>
                                                                </div>



                                                                <asp:Button ID="btnSubmit2" class="btn btn-primary pull-right" runat="server" OnClick="btnSubmit2_MonitorSupervisor_Click"  Text="Create Profile" />

                                                                <div class="clearfix"></div>
                                                         
                                                        </div>
                                                    </div>
                                                </div>
                                            
                                        </div>
                                 

                                     <div class="container-fluid" style="float:none">
                                            
                                                <div class="col-md-12">
                                                    <div class="card">
                                                        <div class="card-header" data-background-color="purple">
                                                            <h4 class="title">Update User Profile</h4>
                                                            <p class="category">Fill complete profile</p>
                                                        </div>

                                                        <div class="card-content">
                                                           
                                                                <div class="row">

                                                                    <asp:GridView ID="GridView3" DataKeyNames="UserId" OnRowCancelingEdit="GridView3_RowCancelingEdit" OnRowDeleting="GridView3_RowDeleting"  
                                                                        OnRowEditing="GridView3_RowEditing" OnRowUpdating="GridView3_RowUpdating" CssClass="table table-hover" runat="server"
                                                                    AutoGenerateColumns="true" Width="80%" HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" AutoGenerateEditButton="true" AutoGenerateDeleteButton="true">



                                                                        </asp:GridView>



                                                                

                                                                <div class="clearfix"></div>
                                                         
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        
                                    </div>
                             </div>
                            



                                <div class="tab-pane <%=active2%>" id="profile">

                                    <script type="text/javascript">

                                        function UpdateMessage() {

                                            var x = document.getElementById("myDIV");
                                            if (x.style.display === "none") {
                                                x.style.display = "block";

                                            } else {
                                                x.style.display = "none";
                                            }
                                        }
                                    </script>


                                    <div class="alert alert-success" id="myDIV" style="display: none;">
                                        <div class="container-fluid">
                                            <div class="alert-icon">
                                                <i class="material-icons">check</i>
                                            </div>
                                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                <span aria-hidden="true"><i class="material-icons">clear</i></span>
                                            </button>
                                            <b>Success Alert:</b> You have successfully Updated the record. 
                                        </div>
                                        <br />
                                    </div>



                                    <div class="card-content table-responsive">

                                        <asp:GridView ID="GridView1" CssClass="table table-hover" runat="server"
                                            AutoGenerateColumns="false" Width="80%" HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Username">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddUsername" runat="server" Width="50%">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Role">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlRoles" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="50%">
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Role">
                                                    <ItemTemplate>
                                                        <asp:Button Width="50%" ID="btnUpdate" Text="Update" runat="server" CommandArgument='<%# Eval("UserId") %>'
                                                            OnClick="UpdateRole" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>




                                    <div class="card-content table-responsive">

                                        <asp:GridView ID="GridView2" CssClass="table table-hover" runat="server"
                                            AutoGenerateColumns="false" Width="80%" HorizontalAlign="Center" RowStyle-HorizontalAlign="Center" Visible="false">
                                            <Columns>
                                                <asp:BoundField DataField="Detail" HeaderText="Description" ItemStyle-Width="250px" />

                                                <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-Width="50px" />

                                                <asp:TemplateField HeaderText="Allow/ Disallow" ItemStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="30px">
                                                    <ItemTemplate>

                                                        <asp:CheckBox ID="chkRow" runat="server" />

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                    </div>

                                </div>
                                <div class="tab-pane" id="maps">
                                    MAPS to be display
                                </div>
                               
                            </div>
                        </div>

                    </div>
                </div>
            </div>



        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
