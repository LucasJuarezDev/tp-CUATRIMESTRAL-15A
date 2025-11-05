<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="RegistrarUsuario.aspx.cs" Inherits="Dominio.RegistrarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Contenedor centrado -->
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-6 col-md-8">

                <!-- Tarjeta con fondo del navbar -->
                <div class="card border-0 shadow-sm rounded-3" style="background-color: #495057;">
                    <div class="card-body p-5 text-white">

                        <!-- Título -->
                        <h2 class="text-center fw-bold mb-4">Crear cuenta</h2>

                        <!-- Formulario -->
                        <div class="row g-3">

                            <!-- Nombre -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Nombre</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Juan" />
                            </div>

                            <!-- Apellido -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Apellido</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Pérez" />
                            </div>

                            <!-- Teléfono -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="+54 11 1234-5678" TextMode="Phone" />
                            </div>

                            <!-- Email -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="juan@example.com" TextMode="Email" />
                            </div>

                            <!-- Nombre de usuario (nickname) -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Nombre de usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="juanperez" />
                            </div>

                            <!-- Contraseña -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••" />
                            </div>

                            <!-- Repetir contraseña -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Repetir contraseña</label>
                                <asp:TextBox ID="txtRepetirPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••••" />
                            </div>

                            <!-- ¿Es empresa? -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">¿Es empresa?</label>
                                <asp:DropDownList ID="ddlEsEmpresa" runat="server" CssClass="form-select"
                                    onchange="toggleRazonSocial()">
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                    <asp:ListItem Value="1">Sí</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <!-- Razón Social (oculto por defecto) -->
                            <div class="col-12" id="divRazonSocial" style="display: none;">
                                <label class="form-label fw-semibold">Razón Social</label>
                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" placeholder="Ej: Pérez SRL" />
                            </div>

                            <!-- Botón Registrarse -->
                            <div class="col-12 mt-4">
                                <asp:Button ID="btnRegistrarse" runat="server" Text="REGISTRARSE" 
                                            CssClass="btn btn-success w-100 fw-bold py-2" 
                                             />
                            </div>

                            <!-- Enlace a login -->
                            <div class="col-12 text-center mt-3">
                                <small>
                                    ¿Ya tienes cuenta? 
                                    <a href="LoginCliente.aspx" class="text-white text-decoration-underline">Iniciar sesión</a>
                                </small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Script para mostrar/ocultar Razón Social -->
    <script>
        function toggleRazonSocial() {
            const select = document.getElementById('<%= ddlEsEmpresa.ClientID %>');
            const div = document.getElementById('divRazonSocial');
            if (select.value === "1") {
                div.style.display = "block";
                setTimeout(() => div.querySelector('input').focus(), 100);
            } else {
                div.style.display = "none";
            }
        }

        // Ejecutar al cargar la página (por si viene preseleccionado)
        document.addEventListener("DOMContentLoaded", toggleRazonSocial);
    </script>

</asp:Content>