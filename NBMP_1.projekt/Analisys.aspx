<%@ Page Title="Analisys" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Analisys.aspx.cs" Inherits="NBMP_1.projekt.Analisys" %>

<asp:content id="BodyContent" contentplaceholderid="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Search.css" runat="server"/> 
    <h2><%: Title %>.</h2>
    <h3>Analisys of given text</h3>
    

    <div class="well">
  <div id="datetimepicker1" class="input-append date">
    <input data-format="dd/MM/yyyy hh:mm:ss" type="text"></input>
    <span class="add-on">
    <i data-time-icon="icon-time" data-date-icon="icon-calendar">
    </i>
    </span>
    </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker1').datetimepicker({
                language: 'pt-BR'
            });
        });
    </script>

    <!DOCTYPE HTML>
<html>
  <head>
    <link href="http://netdna.bootstrapcdn.com/twitter-bootstrap/2.2.2/css/bootstrap-combined.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" media="screen"
     href="http://tarruda.github.com/bootstrap-datetimepicker/assets/css/bootstrap-datetimepicker.min.css">
  </head>
  <body>
    <div id="datetimepicker" class="input-append date">
      <input type="text"></input>
      <span class="add-on">
        <i data-time-icon="icon-time" data-date-icon="icon-calendar"></i>
      </span>
    </div>
    <script type="text/javascript"
     src="http://cdnjs.cloudflare.com/ajax/libs/jquery/1.8.3/jquery.min.js">
    </script> 
    <script type="text/javascript"
     src="http://netdna.bootstrapcdn.com/twitter-bootstrap/2.2.2/js/bootstrap.min.js">
    </script>
    <script type="text/javascript"
     src="http://tarruda.github.com/bootstrap-datetimepicker/assets/js/bootstrap-datetimepicker.min.js">
    </script>
    <script type="text/javascript"
     src="http://tarruda.github.com/bootstrap-datetimepicker/assets/js/bootstrap-datetimepicker.pt-BR.js">
    </script>
    <script type="text/javascript">
      $('#datetimepicker').datetimepicker({
        format: 'dd/MM/yyyy hh:mm:ss',
        language: 'en'
      });
    </script>
  </body>
<html>

</asp:content>
