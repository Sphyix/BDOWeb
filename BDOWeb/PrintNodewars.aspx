<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintNodewars.aspx.cs" Inherits="BDOWeb.PrintNodewars" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dberror" class="alert" runat="server">
        <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
        <asp:Label ID="errorLabel" Text="" CssClass="labeled" runat="server"></asp:Label>
    </div>
    <div id="success" class="success" runat="server">
        <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
        <asp:Label ID="Label1" Text="Drop inserito correttamente." CssClass="labeled" runat="server"></asp:Label>
    </div>

    <fieldset>
        <br />
        <%--gridview per le nodewar--%>
        <asp:GridView AllowPaging="false" AutoGenerateColumns="false" CellPadding="4" ID="gvstatus" runat="server" Width="90%"
            OnRowCancelingEdit="gvstatus_RowCancelingEdit" OnRowEditing="gvstatus_RowEditing" 
            OnRowUpdating="gvstatus_RowUpdating" OnRowDeleting="gvstatus_RowDeleting">
            <Columns>
                <asp:BoundField DataField="data" HeaderText="Data" />
                <asp:BoundField DataField="nodewarid" HeaderText="Nodewar ID" />
                <asp:BoundField DataField="nodewartier" HeaderText="Nodewar Tier" />
                <asp:BoundField DataField="money" HeaderText="Soldi" />
                <asp:TemplateField HeaderText="Seleziona">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="partecipated" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField  ShowEditButton="true" />
                <asp:CommandField ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
        &nbsp
        <asp:Button CssClass="confirmNodeButton" OnClick="ShowPartecipationsBtn_Click" ID="showResults" Text="Mostra risultati" runat="server" />
        <asp:Button CssClass="confirmNodeButton" OnClick="EditPartecipationsBtn_Click" ID="editPartecipations" Text="Modifica partecipazioni" runat="server" />

        <%--gridview per i risultati, utenti e soldi per utente--%>
        <asp:GridView AllowPaging="false" AutoGenerateColumns="false" Visible="false" CellPadding="4" ID="usersResult" runat="server" Width="80%">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="Nome" />
                <asp:BoundField DataField="partecipations" HeaderText="Numero Presenze" />
                <asp:BoundField DataField="money" HeaderText="Soldi" />
            </Columns>
        </asp:GridView>
        &nbsp
        <asp:Button CssClass="confirmButton" OnClick="RefreshPageBtn_Click" Visible="false" ID="goBack" Text="Torna alle Nodewars" runat="server" />
    </fieldset>

     <%--gridview per modificare partecipazioni alla node--%>
    <fieldset>
        <asp:GridView AllowPaging="false" AutoGenerateColumns="false" Visible="false" CellPadding="4" ID="gvpartecipationsEditor" runat="server" Width="80%">
            <Columns>
                <asp:BoundField DataField="username" HeaderText="Nome" />
                <asp:BoundField DataField="ispartecipate" HeaderText="Ha partecipato" >
                    <ItemStyle CssClass="hidden"/>
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="partecipated" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp
        <asp:Button CssClass="confirmNodeButton" OnClick="SaveAndRefreshBtn_Click" Visible="false" ID="goBackEdit" Text="Salva" runat="server" />
        <asp:Button CssClass="confirmNodeButton" OnClick="CancelAndRefreshBtn_Click" Visible="false" ID="gobackCancelEdit" Text="Annulla" runat="server" />
    </fieldset>
</asp:Content>
