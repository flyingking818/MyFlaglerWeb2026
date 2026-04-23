<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="MyFlaglerWeb2026.PersonApp.Summary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All Profiles</h2>
    <asp:Label ID="lblSearch" runat="server" Text="Search by Name or Email: " />
    <asp:TextBox ID="txtSearch" runat="server" Width="200px" />
    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    <br />
    <br />

    <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="true" BorderWidth="1px" BorderColor="Gray" CellPadding="5" />

</asp:Content>

