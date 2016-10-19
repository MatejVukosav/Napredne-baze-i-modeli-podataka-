<%@ Page Title="Add text" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="NBMP_1.projekt.Add" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Search.css" runat="server" />
    <link rel="stylesheet" type="text/css" href="Table.css" runat="server" />

    <h2><%: Title %></h2>
    <h3>Add text to PosgreSQL database</h3>


    <table style="">
        <tr>
            <th>Title</th>
            <td>
                <textarea id="add_title"
                    name="add_title" runat="server"></textarea>
            </td>
        </tr>
        <tr>
            <th>Keywords</th>
            <td>
                <textarea id="add_keywords"
                    name="add_keywords" runat="server"></textarea>

            </td>
        </tr>
        <tr>
            <th>Summary</th>
            <td>
                <textarea id="add_summary" name="add_summary" rows="4" runat="server"></textarea>

            </td>
        </tr>
        <tr>
            <th>Body</th>
            <td>
                <textarea id="add_body" name="add_body" rows="6" runat="server"></textarea>

            </td>
        </tr>
        <tr>
            <th></th>
            <td>
                <asp:Button
                    ID="add_text_btn"
                    Text="Add text to a database"
                    OnClick="add_text_btn_Click"
                    runat="server" />

            </td>
        </tr>
    </table>






</asp:Content>
