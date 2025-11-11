<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" 
    CodeBehind="alterarEmpleado.aspx.cs" Inherits="Dominio.alterarEmpleado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Editar Empleado</title>
    <style>
        .form-label { font-weight: 600; }
        .text-muted small { font-size: 0.85rem; }
        .card { max-width: 600px; margin: 2rem auto; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="card border-0 shadow-sm">
            <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                <h5 class="mb-0">
                    <i class="bi bi-person-fill me-2"></i> Editar Empleado
                </h5>
                <a href="gestionEmpleados.aspx" class="btn btn-sm btn-outline-light">
                    <i class="bi bi-arrow-left"></i> Volver
                </a>
            </div>
            <div class="card-body p-4">
                <!-- Mensaje -->
                <asp:Label ID="lblMensaje" runat="server" CssClass="alert d-block text-center" Visible="false"></asp:Label>

                <div class="row g-3">
                    <!-- NOMBRE -->
                    <div class="col-md-6">
                        <label class="form-label">Nombre <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Juan" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                            ControlToValidate="txtNombre"
                            ErrorMessage="El nombre es obligatorio."
                            CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <!-- APELLIDO -->
                    <div class="col-md-6">
                        <label class="form-label">Apellido <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" placeholder="Pérez" />
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                            ControlToValidate="txtApellido"
                            ErrorMessage="El apellido es obligatorio."
                            CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <!-- TELÉFONO -->
                    <div class="col-md-6">
                        <label class="form-label">Teléfono</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="11 1234-5678" />
                        <small class="text-muted">Opcional</small>
                    </div>

                    <!-- SUELDO -->
                    <div class="col-md-6">
                        <label class="form-label">Sueldo <span class="text-danger">*</span></label>
                        <div class="input-group">
                            <span class="input-group-text">$</span>
                            <asp:TextBox ID="txtSueldo" runat="server" CssClass="form-control" 
                                         placeholder="70000" TextMode="Number" />
                        </div>
                        <asp:RequiredFieldValidator ID="rfvSueldo" runat="server"
                            ControlToValidate="txtSueldo"
                            ErrorMessage="El sueldo es obligatorio."
                            CssClass="text-danger small" Display="Dynamic" />
                        <asp:RangeValidator ID="rvSueldo" runat="server"
                            ControlToValidate="txtSueldo"
                            MinimumValue="1" MaximumValue="99999999"
                            Type="Integer"
                            ErrorMessage="Sueldo debe ser mayor a 0."
                            CssClass="text-danger small" Display="Dynamic" />
                    </div>
                </div>

                <!-- BOTONES -->
                <div class="d-grid gap-2 d-md-flex justify-content-md-end mt-4">
                    <a href="gestionEmpleados.aspx" class="btn btn-secondary me-md-2">
                        <i class="bi bi-x-circle"></i> Cancelar
                    </a>
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios"
                                CssClass="btn btn-success" OnClick="btnGuardar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>