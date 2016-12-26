<%@ Page Title="Analisys" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Analisys.aspx.cs" Inherits="NBMP_1.projekt.Analisys" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Search.css" runat="server" />
    <link rel="stylesheet" type="text/css" href="Table.css" runat="server" />
    <h2><%: Title %>.</h2>
    <h3>Analisys of given text</h3>



    <!doctype html>
    <html lang="en">
    <head>
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>jQuery UI Datepicker - Default functionality</title>
        <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
        <link rel="stylesheet" href="/resources/demos/style.css">
        <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


        <script>
            $(function () {
                $("#date_from").datepicker();
                $("#date_to").datepicker();



            });
        </script>
    </head>
    <body>
        <table style="">
            <tr>
                <th></th>
                <td>
                    <label>Search by date</label>
                </td>
            </tr>
            <tr>
                <th>Date from:</th>
                <td>
                    <input type="text" id="date_from" runat="server">
                </td>
            </tr>
            <tr>
                <th>Date to:</th>
                <td>
                    <input type="text" id="date_to" runat="server">
                </td>
            </tr>


            <tr>
                <th>Choose search option:</th>

                <td>
                    <asp:RadioButtonList ID="RadioButtonChooseQueriesSearch" runat="server">
                        <asp:ListItem Selected="True" Value="days">by days</asp:ListItem>
                        <asp:ListItem Value="hours">by hours</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>

            <tr>
                <th>
                    <asp:Button
                        ID="search_queries"
                        Text="Search"
                        OnClick="search_queries_btn_Click"
                        runat="server" />

                </th>
                <td></td>
            </tr>
        </table>

    </body>
    </html>

    <asp:Table ID="tablePrikaz" runat="server">
    </asp:Table>


</asp:Content>
