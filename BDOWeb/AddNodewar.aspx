<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNodewar.aspx.cs" Inherits="BDOWeb.AddNodewar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dberror" visible="false" class="alert" runat="server">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
            <asp:Label ID="errorLabel" Text="" CssClass="labeled" runat="server"></asp:Label>
        </div>
        <div id="success" visible="false" class="success" runat="server">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
            <asp:Label ID="successLabel" Text="Nodewar aggiunta con successo." CssClass="labeled" runat="server"></asp:Label>
        </div>
    <fieldset>
        <br />
        <asp:Label Text="Tier della Node" CssClass="labeled" runat="server"></asp:Label><br />
        <asp:DropDownList ID="nodeTier" CssClass="inputText" runat="server">
            <asp:ListItem Enabled="true" Text="" Value="-1"></asp:ListItem>
            <asp:ListItem Text="Tier 1" Value="1"></asp:ListItem>
            <asp:ListItem Text="Tier 2" Value="2"></asp:ListItem>
            <asp:ListItem Text="Tier 3" Value="3"></asp:ListItem>
        </asp:DropDownList>

        <asp:GridView AllowPaging="false" AutoGenerateColumns="false" CellPadding="4" ID="gvstatus" runat="server" Width="70%">
            <Columns>
                <asp:BoundField DataField="userid" HeaderText="User ID" />
                <asp:BoundField DataField="username" HeaderText="Username" />
                <asp:TemplateField HeaderText="Ha partecipato?">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="partecipated" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        &nbsp
        <asp:Button CssClass="confirmButton" OnClick="AddNodeBtn_Click" ID="addNode" Text="Invia" runat="server" />
    </fieldset>
</asp:Content>
