<%@ Page Title="Registrarse" Language="C#" MasterPageFile="~/MasterPageCliente.Master" 
    AutoEventWireup="true" CodeBehind="RegistrarCliente.aspx.cs" Inherits="Dominio.RegistrarUsuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-6 col-md-8">
                <div class="card border-0 shadow-sm rounded-3" style="background-color: #495057;">
                    <div class="card-body p-5 text-white">

                        <h2 class="text-center fw-bold mb-4">Crear cuenta</h2>

                        <!-- Resumen de errores -->
                        <asp:ValidationSummary ID="vsErrores" runat="server" 
                            CssClass="alert alert-danger rounded p-3 mb-4" 
                            HeaderText="Por favor, corrija los siguientes errores:" 
                            DisplayMode="BulletList" />

                        <div class="row g-3">
                            <!-- Nombre -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Nombre</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Juan" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>

                            <!-- Apellido -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Apellido</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Pérez" />
                                <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                    ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>

                            <!-- Teléfono -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" 
                                    placeholder="+54 11 1234-5678" TextMode="Phone" />
                            </div>

                            <!-- Email -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                    placeholder="juan@example.com" TextMode="Email" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                    ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                    ControlToValidate="txtEmail" 
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="El email no tiene un formato válido"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RegularExpressionValidator>
                            </div>

                            <!-- Nombre de usuario -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Nombre de usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="juanperez" />
                                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server"
                                    ControlToValidate="txtUsuario" ErrorMessage="El nombre de usuario es obligatorio"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>

                            <!-- Contraseña -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" 
                                    TextMode="Password" placeholder="••••••••" />
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                    ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RequiredFieldValidator>
                            </div>

                            <!-- Repetir contraseña -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Repetir contraseña</label>
                                <asp:TextBox ID="txtRepetirPassword" runat="server" CssClass="form-control" 
                                    TextMode="Password" placeholder="••••••••" />
                                <asp:RequiredFieldValidator ID="rfvRepetir" runat="server"
                                    ControlToValidate="txtRepetirPassword" ErrorMessage="Debe repetir la contraseña"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvPassword" runat="server"
                                    ControlToValidate="txtRepetirPassword" 
                                    ControlToCompare="txtPassword"
                                    ErrorMessage="Las contraseñas no coinciden"
                                    ForeColor="#ff6b6b" Display="Dynamic">*</asp:CompareValidator>
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
                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" 
                                    placeholder="Ej: Pérez SRL" />
                                <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server"
                                    ControlToValidate="txtRazonSocial" 
                                    ErrorMessage="La Razón Social es obligatoria si es empresa"
                                    ForeColor="#ff6b6b" Display="Dynamic" Enabled="false">*</asp:RequiredFieldValidator>
                            </div>

                            <!-- Botón Registrarse -->
                            <div class="col-12 mt-4">
                                <asp:Button ID="btnRegistrarse" runat="server" Text="REGISTRARSE"
                                    CssClass="btn btn-success w-100 fw-bold py-2"
                                    OnClick="btnRegistrarse_Click" />
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

    <!-- Script para mostrar/ocultar Razón Social y activar validador -->
    <script type="text/javascript">
        function toggleRazonSocial() {
            const select = document.getElementById('<%= ddlEsEmpresa.ClientID %>');
            const div = document.getElementById('divRazonSocial');
            const validator = document.getElementById('<%= rfvRazonSocial.ClientID %>');

            if (select.value === "1") {
                div.style.display = "block";
                if (validator) validator.enabled = true;
                setTimeout(() => div.querySelector('input').focus(), 100);
            } else {
                div.style.display = "none";
                if (validator) validator.enabled = false;
            }
        }

        document.addEventListener("DOMContentLoaded", function () {
            toggleRazonSocial();
        });
    </script>
</asp:Content>