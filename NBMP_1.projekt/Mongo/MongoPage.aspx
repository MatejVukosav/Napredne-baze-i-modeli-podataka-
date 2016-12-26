<%@ Page Title="Mongo Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MongoPage.aspx.cs" Inherits="NBMP_1.projekt.Mongo.MongoPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="Search.css" runat="server" />
    <link rel="stylesheet" type="text/css" href="Table.css" runat="server" />

    <h2>News</h2>

    <asp:ListView ID="productsList" runat="server" OnItemDataBound="productsList_ItemDataBound">

        <LayoutTemplate>
            <ul class="productList">
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </ul>

            <tr>
                <td colspan="3">
                    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="productsList" PageSize="5">
                        <Fields>
                            <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="false" ShowPreviousPageButton="true"
                                ShowNextPageButton="false" />
                            <asp:NumericPagerField ButtonType="Link" />
                            <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="false" ShowPreviousPageButton="false" />
                        </Fields>
                    </asp:DataPager>
                </td>
            </tr>

        </LayoutTemplate>

        <ItemTemplate>
            <li>
                <img src='<%# Eval("image") %>' width="200" height="200" /><br />
                <h3><%#Eval("title") %> </h3>

                <em>'<%#Eval("text") %>' </em>

                <asp:Label ID="commentLabel" runat="server" Text="Comments:" />

                <asp:ListView ID="CommentsListView" runat="server">
                    <ItemTemplate>
                        <asp:Label runat="server"><%#Eval("created")%></asp:Label>
                        <asp:Label runat="server"><%#Eval("text")%></asp:Label>

                    </ItemTemplate>
                </asp:ListView>
                <textbox runat="server" id="txtNewComment"></textbox>
                <asp:Button ID="addNewCommentBtn" Text="Add new comment" OnClick="btnAddNewComment_click" runat="server" />

            </li>
        </ItemTemplate>


        <EmptyDataTemplate>
            <div>
                Sorry - no news found
            </div>
        </EmptyDataTemplate>

    </asp:ListView>

</asp:Content>
