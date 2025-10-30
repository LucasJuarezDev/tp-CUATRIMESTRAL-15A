<%@ Page Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="cambiarContrasena.aspx.cs" Inherits="Dominio.cambiarContrasena" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Contenedor centrado -->
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-5 col-md-7 col-sm-9">

                <!-- Tarjeta con fondo del navbar -->
                <div class="card border-0 shadow-sm rounded-3" style="background-color: #495057;">
                    <div class="card-body p-5 text-white">

                        <!-- Icono de candado -->
                        <div class="text-center mb-4">
                            <i class="bi bi-shield-check display-1 text-white"></i>
                        </div>

                        <!-- Título -->
                        <h2 class="text-center fw-bold mb-3">Restablecer contraseña</h2>

                        <!-- Descripción -->
                        <p class="text-center text-white-50 small mb-4">
                            Ingresa tu nueva contraseña. Asegúrate de que sea segura.
                        </p>

                        <!-- Nueva contraseña -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold">Nueva contraseña</label>
                            <asp:TextBox ID="txtNuevaPassword" runat="server" 
                                         CssClass="form-control form-control-lg" 
                                         TextMode="Password" 
                                         placeholder="••••••••" />
                        </div>

                        <!-- Repetir nueva contraseña -->
                        <div class="mb-4">
                            <label class="form-label fw-semibold">Repetir nueva contraseña</label>
                            <asp:TextBox ID="txtRepetirPassword" runat="server" 
                                         CssClass="form-control form-control-lg" 
                                         TextMode="Password" 
                                         placeholder="••••••••" />
                        </div>

                        <!-- Botón Cambiar -->
                        <asp:Button ID="btnCambiar" runat="server" 
                                    Text="CAMBIAR CONTRASEÑA" 
                                    CssClass="btn btn-success w-100 fw-bold py-2" 
                                     />

                        <!-- Enlace volver al login -->
                        <div class="text-center mt-3">
                            <a href="LoginCliente.aspx" class="text-white text-decoration-underline small">
                                ← Volver al inicio de sesión
                            </a>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
