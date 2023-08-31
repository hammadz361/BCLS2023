<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CLS_Dashboard.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CLS-Monitoring</title>
   <%-- <link rel="stylesheet" type="text/css" href="../assets/css/Login.css" />

    <link href="../assets/css/font-awesome.min.css" rel="stylesheet" type="text/css" />--%>
    <meta name="viewport" content="width=device-width, initial-scale: 1.0, user-screen:0" />
    <%--<script src="../assets/Scripts/javascriptlogin.js"></script>--%>
    <%--<script class="cssdeck" src="//cdnjs.cloudflare.com/ajax/libs/jquery/1.8.0/jquery.min.js"></script>--%>
    
    
    
     <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    <link rel="stylesheet" href="../assets/css/font-awesome.min.css" />
    
    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
    <!--     Fonts and icons     -->
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Roboto+Slab:400,700|Material+Icons" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/latest/css/font-awesome.min.css" />
    <!-- CSS Files -->
    <link href="../assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../assets/css/material-dashboard.css" rel="stylesheet" />
    <link href="css/demo-documentation.css" rel="stylesheet" />

      <style media="screen">
        .page-header {
            height: 100vh;
           
          
        }

        .page-header .description {
            color: #ffffff;
        }

        .header-filter .container {
            padding-top: 10vh;
             
        }
    </style>

    <style type="text/css">
        body{
            margin:0px;
        }
       
        input[type=text], input[type=password]
        {
            width:100%;
          height:30px;
          border:none;
          /*padding:5px 7px 5px 15px;*/
          background:#fff;
          color:#666;
          border:2px solid #0AC986;
          border-radius:4px;
          -moz-border-radius:4px;
          -webkit-border-radius:4px;
          margin-top:5px;
        }



        input[type=submit]
        {
          background:#0AC986;
          
          width:100%;
          font-size:16px;
          height:50px;
          color:#fff;
          text-decoration:none;
          border:none;
          border-radius:4px;
          -moz-border-radius:4px;
          -webkit-border-radius:4px;
          margin-left:30px;
        }

         label {
        margin-top:5px;
        font-size: 14px;
        line-height: 1.42857;
        color: white;
        font-weight: 400;
   
}

        .login-form{

          width:350px;
          padding:40px;
          background:rgba(235,235,205,0.7);
          border-radius:4px;
          -moz-border-radius:4px;
          -webkit-border-radius:4px;
          margin:50px auto;
          
}

        .form-control{
          width:100%;
          height:50px;
          border:none;
          padding:5px 7px 5px 15px;
          background:#fff;
          color:#666;
          border:2px solid #E0D68F;
          border-radius:4px;
          -moz-border-radius:4px;
          -webkit-border-radius:4px;
}
        .form-control:focus, .form-control:focus + .fa{
          border-color:#10CE88;
          color:#10CE88;
}

        table
        {
           
            margin-left: 10px;
            padding:5px 7px 5px 15px;
          
        }
        table th
        {
            background-color: #F7F7F7;
            color: #333;
            font-weight: bold;
            
           
           
        }
        table th, table td
        {
            padding: 20px 5px 5px 5px;
            border-color: #ccc;
            color:#AAA173;
              
            
            
             
        }
    </style>
</head>
<body class="components-page">
    <form id="form1" runat="server">
       
        <div class="page-header" style="background-image: url('../data4/images/ChildLabor.jpg');">
         <div class="container" style="margin-top:0px" >
             <div class="row" >
                 <div class="col-md-8 col-md-offset-2 text-center">
                      <h1 class="title ">Welcome</h1>
                     <h3 class="title ">Child Labour Survey</h3>
                     
                     <asp:Login ID="Login1" class="login-form" runat="server" OnAuthenticate="ValidateUser" TitleText=""></asp:Login>
                  </div>
                 </div>
             </div>
        </div>
       
    </form>
</body>
</html>
