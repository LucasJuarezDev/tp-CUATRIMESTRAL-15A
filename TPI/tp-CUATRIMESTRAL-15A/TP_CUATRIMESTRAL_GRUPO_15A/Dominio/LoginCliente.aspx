<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="LoginCliente.aspx.cs" Inherits="Dominio.LoginCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Contenedor centrado -->
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-5 col-md-7 col-sm-9">

                <!-- Tarjeta con fondo del navbar -->
                <div class="card border-0 shadow-sm rounded-3" style="background-color: #495057;">
                    <div class="card-body p-5 text-white">

                        <!-- Icono de usuario -->
                        <div class="text-center mb-4">
                            <i class="bi bi-person-circle display-1 text-white"></i>
                        </div>

                        <!-- Título -->
                        <h2 class="text-center fw-bold mb-4">Iniciar Sesión</h2>

                        <!-- Formulario -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold">Nombre de usuario o email</label>
                            <asp:TextBox ID="txtUsuario" runat="server" 
                                         CssClass="form-control form-control-lg" 
                                         placeholder="juanperez o juan@example.com" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label fw-semibold">Contraseña</label>
                            <asp:TextBox ID="txtPassword" runat="server" 
                                         CssClass="form-control form-control-lg" 
                                         TextMode="Password" 
                                         placeholder="••••••••" />
                        </div>

                        <!-- Recordarme -->
                        <div class="form-check mb-3">
                            <asp:CheckBox ID="chkRecordarme" runat="server" CssClass="form-check-input" />
                            <label class="form-check-label fw-medium">Recordarme</label>
                        </div>

                        <!-- Botón Iniciar Sesión -->
                        <asp:Button ID="btnLogin" runat="server" 
                                    Text="INICIAR SESIÓN" 
                                    CssClass="btn btn-success w-100 fw-bold py-2" 
                                     />

                        <!-- Enlaces -->
                        <div class="text-center mt-3">
                            <a href="cambiarContrasena.aspx" class="text-white text-decoration-underline small">
                                ¿Olvidaste tu contraseña?
                            </a>
                        </div>

                        <div class="text-center mt-2">
                            <small class="text-white-50">
                                ¿No tienes cuenta? 
                                <a href="RegistrarUsuario.aspx" class="text-white text-decoration-underline">
                                    Registrarse
                                </a>
                            </small>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>