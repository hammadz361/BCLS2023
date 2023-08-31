<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Change_Password.aspx.cs" Inherits="CLS_Dashboard.Change_Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>


            <div class="content">
                <div class="container-fluid" style="margin-left: -50%; padding-bottom: 20%; padding-right: 20%">

                


                    <div class="container-fluid">

                            <div class="alert alert-success" id="DivUser" style="display: none;">
                        
                            <div class="alert-icon">
                                <i class="material-icons">check</i>
                            </div>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true"><i class="material-icons">clear</i></span>
                            </button>
                            <b>Success Alert:</b> <%=message%> 
                        
                        <br />
                    </div>



                    <div class="alert alert-warning" id="DivUserError" style="display: none;">
                        
                            <div class="alert-icon">
                                <i class="material-icons">error_outline</i>
                            </div>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true"><i class="material-icons">clear</i></span>
                            </button>
                            <b>Warning Alert:</b> <%=message%>  
                     
                        <br />
                    </div>

                        <div class="card">
                            <div class="card-header" data-background-color="purple">
                                <h4 class="title">Password Update</h4>
                                <p class="category">Fill complete fields</p>
                            </div>

                            <div class="card-content" style="margin-left: 5%">

                                <div class="row">

                                    <div class="col-md-5">
                                        <div class="form-group label-floating">
                                            <label class="control-label">Old Password</label>
                                            <asp:TextBox ID="txtOldPass" TextMode="Password" runat="server" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>




                                </div>
                                <div class="row">
                                    <div class="col-md-5">
                                        <div class="form-group label-floating">
                                            <label class="control-label">New Password</label>
                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>

                                <div class="row">
                                    <div class="col-md-5">
                                        <div class="form-group label-floating">
                                            <label class="control-label">Confirm Password</label>
                                            <asp:TextBox ID="txtConfPassword" TextMode="Password" runat="server" class="form-control"></asp:TextBox>

                                        </div>
                                    </div>

                                </div>





                                <asp:Button ID="btnSubmit" class="btn btn-primary pull-right" runat="server" OnClick="btnSubmit_Click" Text="Update" />

                                <div class="clearfix"></div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>

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

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
