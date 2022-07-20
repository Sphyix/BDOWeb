<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Loggin.aspx.cs" Inherits="BDOWeb.Loggin" %>
<%@ MasterType VirtualPath="Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div id="dberror" visible="false" class="alert" runat="server">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
            <asp:Label ID="errorLabel" Text="" CssClass="labeled" runat="server"></asp:Label>
        </div>
        <div id="success" visible="false" class="success" runat="server">
            <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
            <asp:Label ID="successLabel" Text="" CssClass="labeled" runat="server"></asp:Label>
        </div>
    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>Accedi qua sotto</h4>
                    <hr />
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label labeled">Username</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Search" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label labeled">Password</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="LogIn" Text="Log in" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
