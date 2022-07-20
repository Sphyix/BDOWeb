<%@ Page Title="Aggiungi Drop" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddDrops.aspx.cs" Inherits="BDOWeb.AddDrops" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dberror" visible="false" class="alert" runat="server">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
            <asp:Label ID="errorLabel" Text="" CssClass="labeled" runat="server"></asp:Label>
        </div>
        <div id="success" visible="false" class="success" runat="server">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
            <asp:Label ID="successLabel" Text="Drop Aggiunto con Successo" CssClass="labeled" runat="server"></asp:Label>
        </div>
    <fieldset>
        <br />
        <asp:Label Text="Username" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="username" CssClass="inputText" AutoCompleteType="DisplayName" runat="server"></asp:TextBox><br />

        <asp:Label Text="Screenshot Link" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="screenshotlink" CssClass="inputText" AutoCompleteType="DisplayName" runat="server"></asp:TextBox><br />

        <asp:Label Text="Neidan" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="neidan" AutoCompleteType="Disabled" TextMode="Number" runat="server"></asp:TextBox><br />

        <asp:Label Text="Golden Coins" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="goldcoins" AutoCompleteType="Disabled" TextMode="Number" runat="server"></asp:TextBox><br />

        <asp:Label Text="Drop Rari" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="otherdrops" AutoCompleteType="Disabled" TextMode="Number" runat="server"></asp:TextBox><br />
        <div class="checkbox">
            <asp:CheckBox ID="isBackup" runat="server" />
        </div>
        &nbsp
        <asp:Label CssClass="labeled" Text="Backup Prossima Settimana" runat="server"></asp:Label><br />
        <asp:Button CssClass="confirmButton" OnClick="SendDropsBtn_Click" ID="sendDrops" Text="Invia" runat="server" />
    </fieldset>
</asp:Content>
