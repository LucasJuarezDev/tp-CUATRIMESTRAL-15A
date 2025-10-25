<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageLogin.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Dominio.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-center align-items-center" style="height: calc(100vh - 60px);">
    <div class="card shadow p-4" style="width: 22rem;">
        <div class="card-body">
            <h3 class="card-title text-center mb-4">Iniciar Sesión</h3>

            <div class="mb-3">
                <label for="txtUsuario" class="form-label">Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Ingrese su usuario"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtPassword" class="form-label">Contraseña</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña"></asp:TextBox>
            </div>

            <div class="d-grid mt-4">
                <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-primary" Text="Ingresar" />
            </div>
        </div>
    </div>
</div>

</asp:Content>

