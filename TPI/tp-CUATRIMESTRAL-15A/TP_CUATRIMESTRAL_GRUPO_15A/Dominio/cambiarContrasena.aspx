<%@ Page Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="cambiarContrasena.aspx.cs" Inherits="Dominio.cambiarContrasena" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-5 col-md-7 col-sm-9">
                <div class="card border-0 shadow-sm rounded-3" style="background-color: #495057;">
                    <div class="card-body p-5 text-white">

                        <!-- PASO 1: INGRESAR EMAIL -->
                        <div id="divEmail" runat="server">
                            <div class="text-center mb-4">
                                <i class="bi bi-envelope-check display-1 text-white"></i>
                            </div>
                            <h2 class="text-center fw-bold mb-3">Recuperar contraseña</h2>
                            <p class="text-center text-white-50 small mb-4">
                                Ingresa tu email y te enviaremos un código de verificación
                            </p>
                            <div class="mb-4">
                                <label class="form-label fw-semibold">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-lg" placeholder="tuemail@dominio.com" />
                            </div>
                            <asp:Button ID="btnEnviarCodigo" runat="server" Text="ENVIAR CÓDIGO" 
                                        CssClass="btn btn-primary w-100 fw-bold py-3" OnClick="btnEnviarCodigo_Click" />
                        </div>

                        <!-- PASO 2: VERIFICAR CÓDIGO Y NUEVA CONTRASEÑA -->
                        <div id="divCodigo" runat="server" visible="false">
                            <div class="text-center mb-4">
                                <i class="bi bi-shield-lock display-1 text-success"></i>
                            </div>
                            <h2 class="text-center fw-bold mb-3">Verificar y cambiar</h2>
                            <p class="text-center text-white-50 small mb-4">
                                Te enviamos un código a <strong><asp:Label ID="lblEmailEnviado" runat="server" /></strong>
                            </p>
                            <div class="mb-3">
                                <label class="form-label fw-semibold">Código de 6 dígitos</label>
                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control form-control-lg text-center" 
                                             MaxLength="6" placeholder="000000" />
                            </div>
                            <div class="mb-3">
                                <label class="form-label fw-semibold">Nueva contraseña</label>
                                <asp:TextBox ID="txtNuevaPassword" runat="server" TextMode="Password" 
                                             CssClass="form-control form-control-lg" placeholder="••••••••" />
                            </div>
                            <div class="mb-4">
                                <label class="form-label fw-semibold">Repetir contraseña</label>
                                <asp:TextBox ID="txtRepetirPassword" runat="server" TextMode="Password" 
                                             CssClass="form-control form-control-lg" placeholder="••••••••" />
                            </div>
                            <asp:Button ID="btnCambiarPassword" runat="server" Text="CAMBIAR CONTRASEÑA" 
                                        CssClass="btn btn-success w-100 fw-bold py-3" OnClick="btnCambiarPassword_Click" />
                            <div class="text-center mt-3">
                                <asp:LinkButton ID="lnkVolverEmail" runat="server" Text="← Cambiar email" 
                                                CssClass="text-white small" OnClick="lnkVolverEmail_Click" />
                            </div>
                        </div>

                        <!-- MENSAJE DE ÉXITO -->
                        <div id="divExito" runat="server" visible="false" class="text-center">
                            <i class="bi bi-check-circle-fill display-1 text-success mb-4"></i>
                            <h2 class="fw-bold">¡Contraseña cambiada!</h2>
                            <p class="text-white-50">Ya puedes iniciar sesión con tu nueva contraseña</p>
                            <a href="LoginCliente.aspx" class="btn btn-primary w-100 py-3 fw-bold">
                                IR AL LOGIN
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>