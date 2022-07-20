<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="BDOWeb.Manage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView AllowPaging="false" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" ID="gvstatus" runat="server" Width="600px"
        OnPageIndexChanging="gvstatus_PageIndexChanging" OnRowCancelingEdit="gvstatus_RowCancelingEdit" OnRowDeleting="gvstatus_RowDeleting"
        OnRowEditing="gvstatus_RowEditing" OnRowUpdating="gvstatus_RowUpdating">
        <Columns>
            <asp:BoundField DataField="userid" HeaderText="User ID" />
            <asp:BoundField DataField="username" HeaderText="Username" />
            <asp:CommandField ShowEditButton="true" />
            <asp:CommandField ShowDeleteButton="true" />


        </Columns>
    </asp:GridView>
    <asp:Label Text="Username"  runat="server"></asp:Label><br />
    <asp:TextBox ID="addusername" runat="server"></asp:TextBox> <br />
    <asp:Button OnClick="BtnClick_AddUser" ID="sendDrops" Text="Aggiungi user" runat="server" />

</asp:Content>
