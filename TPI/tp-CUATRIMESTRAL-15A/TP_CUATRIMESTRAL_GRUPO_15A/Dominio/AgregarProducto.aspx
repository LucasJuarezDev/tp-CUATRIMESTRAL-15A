<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageAdmin.Master" CodeBehind="AgregarProducto.aspx.cs" Inherits="Dominio.AgregarProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container py-5">
        <div class="row justify-content-center">
            <div class="col-lg-8">
                <div class="card shadow">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">
                            <i class="bi bi-plus-circle me-2" id="iconAgregar" runat="server"></i>
                            <i class="bi bi-pencil-square me-2" id="iconModificar" runat="server" visible="false"></i>
                            <asp:Label ID="lblTitulo" runat="server" Text="Agregar Nuevo Producto" />
                        </h4>
                    </div>
                    <div class="card-body">
                        <!-- Mensaje de éxito/error -->
                        <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert" role="alert">
                            <asp:Label ID="lblMensaje" runat="server" />
                        </asp:Panel>

                        <div class="row g-3">
                            <!-- Nombre -->
                            <div class="col-md-8">
                                <label class="form-label fw-semibold">Nombre del producto <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Galaxy S24 Ultra" />
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." 
                                    CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Precio -->
                            <div class="col-md-4">
                                <label class="form-label fw-semibold">Precio <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <span class="input-group-text">$</span>
                                    <asp:TextBox ID="txtPrecio" runat="server" 
                                                 CssClass="form-control" 
                                                 placeholder="Ej: 6200000,00" />
                                </div>
                                <asp:RequiredFieldValidator ID="rfvPrecio" runat="server"
                                    ControlToValidate="txtPrecio" ErrorMessage="El precio es obligatorio."
                                    CssClass="text-danger small" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revPrecio" runat="server"
                                    ControlToValidate="txtPrecio"
                                    ValidationExpression="^\d{1,10}(,\d{1,2})?$"
                                    ErrorMessage="Use formato: 1234,56"
                                    CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- Marca y Categoría -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Marca <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">-- Seleccionar marca --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvMarca" runat="server" 
                                    ControlToValidate="ddlMarca" ErrorMessage="Seleccione una marca." 
                                    CssClass="text-danger small" Display="Dynamic" InitialValue="" />
                            </div>

                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Categoría <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                                    <asp:ListItem Value="">-- Seleccionar categoría --</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCategoria" runat="server" 
                                    ControlToValidate="ddlCategoria" ErrorMessage="Seleccione una categoría." 
                                    CssClass="text-danger small" Display="Dynamic" InitialValue="" />
                            </div>

                            <!-- Stock y Stock Mínimo -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Stock inicial <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" min="0" placeholder="8" />
                                <asp:RequiredFieldValidator ID="rfvStock" runat="server" 
                                    ControlToValidate="txtStock" ErrorMessage="Ingrese el stock." 
                                    CssClass="text-danger small" Display="Dynamic" />
                            </div>

                            <!-- STOCK MÍNIMO (OBLIGATORIO) -->
                            <div class="col-md-6">
                                <label class="form-label fw-semibold">Stock mínimo <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtStockMinimo" runat="server" CssClass="form-control" 
                                             TextMode="Number" min="0" placeholder="3" />
                                <small class="text-muted">Obligatorio. Define cuándo mostrar "Últimas unidades".</small>
    
                                <asp:RequiredFieldValidator ID="rfvStockMinimo" runat="server" 
                                    ControlToValidate="txtStockMinimo" 
                                    ErrorMessage="El stock mínimo es obligatorio." 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" />
        
                                <asp:RangeValidator ID="rvStockMinimo" runat="server" 
                                    ControlToValidate="txtStockMinimo" 
                                    MinimumValue="0" MaximumValue="999999" 
                                    Type="Integer" 
                                    ErrorMessage="Ingrese un número válido (0 o más)." 
                                    CssClass="text-danger small" 
                                    Display="Dynamic" />
                            </div>

                            <!-- Descripción Corta -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Descripción corta (catálogo) <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDescripcionCorta" runat="server" CssClass="form-control" 
                                             MaxLength="250" placeholder="Ej: 512GB, 12GB RAM, 200MP" />
                                <small class="text-muted">Máximo 250 caracteres. Se muestra en el listado.</small>
                            </div>

                            <!-- DESCRIPCIÓN COMPLETA (OBLIGATORIA) -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">Descripción completa (detalle del producto) <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDescripcionExtendida" runat="server" 
                                             TextMode="MultiLine" Rows="6" CssClass="form-control" 
                                             placeholder="Escriba una descripción detallada del producto..." />
                                <small class="text-muted">Obligatorio. Se muestra en la página de detalle del producto.</small>
    
                                <asp:RequiredFieldValidator ID="rfvDescripcionExtendida" runat="server" 
                                    ControlToValidate="txtDescripcionExtendida" 
                                    ErrorMessage="La descripción completa es obligatoria." 
                                    CssClass="text-danger small d-block" 
                                    Display="Dynamic" />
                            </div>

                            <!-- Imagen URL -->
                            <div class="col-12">
                                <label class="form-label fw-semibold">URL de la imagen</label>
                                <asp:TextBox ID="txtImagenUrl" runat="server" CssClass="form-control" 
                                             placeholder="https://ejemplo.com/imagen.jpg" />
                                <small class="text-muted">Pega el enlace directo a la imagen (opcional).</small>
                            </div>
                        </div>

                        <!-- Botones -->
                        <hr class="my-4" />
                        <div class="d-flex gap-2 justify-content-end">
                            <a href="Productos.aspx" class="btn btn-outline-secondary px-4">
                                <i class="bi bi-x-circle"></i> Cancelar
                            </a>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Producto"
                                        CssClass="btn btn-success px-5" 
                                        OnClick="btnGuardar_Click"
                                        OnClientClick="return validarStock();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
<script>
    function validarStock() {
        const stock = parseInt(document.getElementById('<%= txtStock.ClientID %>').value) || 0;
        const stockMinimo = parseInt(document.getElementById('<%= txtStockMinimo.ClientID %>').value) || 0;

        if (stock <= stockMinimo) {
            alert('El stock inicial debe ser mayor al stock mínimo.');
            return false;
        }
        return true;
    }

    function mostrarExito() {
        Swal.fire({
            title: '¡Éxito!',
            text: 'El producto se creó correctamente.',
            icon: 'success',
            confirmButtonText: 'Aceptar',
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = 'Productos.aspx';
            }
        });
    }
</script>
</asp:Content>

