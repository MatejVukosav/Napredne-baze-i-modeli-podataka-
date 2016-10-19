<%@ Page Title="Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="NBMP_1.projekt.Search" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Search.css" runat="server" />



    <h2><%: Title %></h2>
    <h4>Search text from database</h4>

    <input
        id="search_input"
        type="text"
        name="search_query"
        size="50"
        runat="server" />

    <asp:Button
        ID="search_query_btn"
        Text="Search"
        OnClick="search_query_btn_Click"
        runat="server" />

    <div id="container">
        <div id="left">
            <asp:RadioButtonList ID="RadioButtonOperators" runat="server">
                <asp:ListItem Selected="True" Value="and">AND</asp:ListItem>
                <asp:ListItem Value="or">OR</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div id="right">
            <asp:RadioButtonList ID="RadioButtonSearchMethod" runat="server">
                <asp:ListItem Selected="True" Value="morphology_&_semantic">morphology & semantic</asp:ListItem>
                <asp:ListItem Value="fuzzy_string_matching">fuzzy string matching</asp:ListItem>
            </asp:RadioButtonList>

        </div>
    </div>

    <div id="search_query_string">
        <h4>Query string </h4>
        <textarea id="search_query_textarea" rows="10" cols="50" runat="server">

        </textarea>
    </div>

    <asp:Label runat="server"> Number od documents retrieved:</asp:Label>
    <asp:Label ID="num_of_search_items" runat="server">0</asp:Label>

    <div style="display: table; vertical-align: middle">
        <asp:Label ID="search_result" runat="server">
        </asp:Label>
    </div>



</asp:Content>


