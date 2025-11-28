<%@ Page Title="Mi Cuenta" Language="C#" MasterPageFile="~/MasterPageCliente.Master" 
    AutoEventWireup="true" CodeBehind="MiCuenta.aspx.cs" Inherits="Dominio.MiCuenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-control.is-invalid { border-color: #dc3545; }
        .form-control.is-valid { border-color: #198754; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-9">
                <div class="card shadow border-0">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">
                            <i class="bi bi-person-circle me-2"></i> Mi Cuenta
                        </h4>
                    </div>
                    <div class="card-body p-5">

                        <!-- MENSAJE -->
                        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" role="alert">
                            <asp:Label ID="lblMensaje" runat="server" />
                        </asp:Panel>

                        <div class="row g-4">
                            <!-- DATOS PERSONALES -->
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Nombre *</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtNombre" 
                                    ErrorMessage="El nombre es obligatorio" CssClass="text-danger small" Display="Dynamic" runat="server" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Apellido *</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtApellido" 
                                    ErrorMessage="El apellido es obligatorio" CssClass="text-danger small" Display="Dynamic" runat="server" />
                            </div>

                            <div class="col-md-6">
                                <label class="form-label fw-bold">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej: 1123456789" />
                            </div>

                            <div class="col-md-6">
                                <label class="form-label fw-bold">Email</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                <small class="text-muted">Se usará para recuperar contraseña</small>
                            </div>

                            <!-- EMPRESA -->
                            <div class="col-12">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkEmpresa" runat="server" CssClass="form-check-input" AutoPostBack="true" 
                                        OnCheckedChanged="chkEmpresa_CheckedChanged" />
                                    <label class="form-check-label fw-bold">Pertenezco a una empresa</label>
                                </div>
                            </div>

                            <div class="col-12" id="divRazonSocial" runat="server" visible="false">
                                <label class="form-label fw-bold">Razón Social</label>
                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" placeholder="Nombre de la empresa" />
                            </div>

                            <!-- CAMBIO DE CONTRASEÑA -->
                            <div class="col-12 mt-5">
                                <h5 class="text-primary"><i class="bi bi-shield-lock me-2"></i>Cambiar Contraseña</h5>
                                <hr />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Contraseña actual</label>
                                <asp:TextBox ID="txtPassActual" runat="server" TextMode="Password" CssClass="form-control" />
                            </div>
                            <div class="col-md-6"></div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Nueva contraseña</label>
                                <asp:TextBox ID="txtPassNueva" runat="server" TextMode="Password" CssClass="form-control" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Repetir nueva contraseña</label>
                                <asp:TextBox ID="txtPassNueva2" runat="server" TextMode="Password" CssClass="form-control" />
                            </div>
                        </div>

                        <hr class="my-5" />
                        <div class="d-flex justify-content-end gap-3">
                            <a href="Catalogo.aspx" class="btn btn-outline-secondary px-4">Cancelar</a>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" 
                                CssClass="btn btn-success px-5" OnClick="btnGuardar_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>