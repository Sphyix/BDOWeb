<%@ Page Title="Totali settimana" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WeekTotals.aspx.cs" Inherits="BDOWeb.WeekTotals" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dberror" class="alert" runat="server">
        <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
        <strong>Attenzione!</strong>
        <asp:Label ID="errorLabel" Text="" CssClass="labeled" runat="server"></asp:Label>
    </div>
    <fieldset>
        <br />
        <asp:Label Text="Data Inizio" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="startdate" CssClass="inputText" TextMode="Date" runat="server"></asp:TextBox><br />
        <asp:Label Text="Data Fine" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:TextBox ID="enddate" CssClass="inputText" TextMode="Date" runat="server"></asp:TextBox><br />
        <div class="checkbox">
            <asp:CheckBox ID="isBackup" runat="server" />
        </div>
        &nbsp
        <asp:Label CssClass="labeled" Text="Backup Prossima Settimana" runat="server"></asp:Label><br />
        <asp:Button CssClass="confirmButton" OnClick="SendDropsBtn_Click" ID="sendDrops" Text="Cerca" runat="server" />
    </fieldset>
    <asp:GridView ID="grdContent" runat="server" Width="100%" CssClass="blueTable" HeaderStyle-CssClass="thead" AutoGenerateColumns="false">
        <AlternatingRowStyle />
        <Columns>
            <asp:BoundField DataField="nome" HeaderText="Nome" />
            <asp:BoundField DataField="neidan" HeaderText="Neidan" HeaderImageUrl="https://www.bddatabase.net/items/new_icon/03_etc/10_free_tradeitem/00055052.png" />
            <asp:BoundField DataField="goldcoin" HeaderText="Golden Coins" HeaderImageUrl="https://www.bddatabase.net/items/new_icon/03_etc/10_free_tradeitem/00055054.png" />
            <asp:BoundField DataField="otherdrops" HeaderText="Drop Rari" HeaderImageUrl="https://www.bddatabase.net/items/new_icon/03_etc/10_free_tradeitem/00055053.png" />
            <asp:BoundField DataField="totale" HeaderText="Totale" />
        </Columns>
    </asp:GridView>
</asp:Content>
