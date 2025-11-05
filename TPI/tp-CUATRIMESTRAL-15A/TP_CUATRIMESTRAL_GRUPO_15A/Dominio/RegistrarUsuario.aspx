<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" 
    CodeBehind="RegistrarUsuario.aspx.cs" Inherits="Dominio.RegistrarUsuario1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-6 col-md-8">
                <div class="card border-0 shadow-sm rounded-3" style="background-color: #495057;">
                    <div class="card-body p-5 text-white">
                        <h2 class="text-center fw-bold mb-4">Crear cuenta</h2>

                        <!-- Mensaje de éxito/error -->
                        <asp:Label ID="lblMensaje" runat="server" CssClass="alert d-block text-center" Visible="false"></asp:Label>

                        <div class="row g-3">
                            <!-- Nickname -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Nickname</label>
                                <asp:TextBox ID="txtNickname" runat="server" CssClass="form-control" placeholder="juan123" />
                                <asp:RequiredFieldValidator ID="rfvNickname" runat="server" ControlToValidate="txtNickname"
                                    ErrorMessage="Nickname requerido" CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Email -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="juan@ejemplo.com" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                    ErrorMessage="Email requerido" CssClass="text-danger small" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ErrorMessage="Email inválido" CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Rol -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Rol</label>
                                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="-- Seleccionar rol --" />
                                    <asp:ListItem Value="2">Empleado</asp:ListItem>
                                    <asp:ListItem Value="3">Cliente</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvRol" runat="server" ControlToValidate="ddlRol"
                                    ErrorMessage="Selecciona un rol" CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Contraseña -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Contraseña</label>
                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Contraseña requerida" CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Repetir contraseña -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Repetir contraseña</label>
                                <asp:TextBox ID="txtRepetirPassword" runat="server" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="rfvRepetir" runat="server" ControlToValidate="txtRepetirPassword"
                                    ErrorMessage="Repite la contraseña" CssClass="text-danger small" Display="Dynamic" />
                                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtRepetirPassword"
                                    ControlToCompare="txtPassword" ErrorMessage="Las contraseñas no coinciden"
                                    CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Botón -->
                            <div class="col-12 mt-4">
                                <asp:Button ID="btnRegistrarse" runat="server" Text="CREAR USUARIO" CssClass="btn btn-success w-100 fw-bold py-2" OnClick="btnRegistrarse_Click" />
                            </div>

                            <div class="col-12 text-center mt-3">
                                <a href="Usuarios.aspx" class="text-white">← Volver a la lista</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>