<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="BDOWeb.AccessDenied" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert" runat="server">
        <asp:Label ID="errorLabel" Text="Non hai i permessi per accedere alla pagina." CssClass="labeled" runat="server"></asp:Label>
    </div>
</asp:Content>
