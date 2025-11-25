<%@ Page Title="Catálogo" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="Catalogo.aspx.cs" Inherits="Dominio.Catalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">

        <!-- Filtros y Ordenamiento -->
        <div class="row mb-4 align-items-center">
            <div class="col-md-6">
                <div class="d-flex align-items-center">
                    <label class="me-2 fw-semibold text-dark">Ordenar por:</label>
                    <asp:DropDownList ID="ddlOrdenar" runat="server" CssClass="form-select w-auto" AutoPostBack="true" >
                        <asp:ListItem Value="precio_desc" Selected="True">Mayor precio</asp:ListItem>
                        <asp:ListItem Value="precio_asc">Menor precio</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 text-md-end">
                <button type="button" class="btn btn-outline-success" data-bs-toggle="modal" data-bs-target="#filtroModal">
                    <i class="bi bi-funnel"></i> Filtrar
                </button>
            </div>
        </div>

        <div class="row g-4">
            <asp:Repeater ID="rptProductos" runat="server">
                <ItemTemplate>
                    <div class="col-lg-4 col-md-6">
                        <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                            <!-- IMAGEN -->
                            <img src='<%# ResolveUrl(Eval("ImagenPrincipal").ToString()) %>'
                                 class="card-img-top"
                                 alt='<%# Eval("Nombre") %>'
                                 style="height: 220px; object-fit: cover;"
                                 onerror="this.onerror=null; this.src='https://via.placeholder.com/400x300/cccccc/666666?text=Sin+Imagen';">

                            <div class="card-body d-flex flex-column p-4">
                                <!-- NOMBRE -->
                                <h6 class="card-title fw-bold mb-2">
                                    <%# Eval("Nombre") %>
                                </h6>

                                <!-- DESCRIPCIÓN CORTA -->
                                <p class="text-muted small mb-3">
                                    <%# Truncate(Eval("DescripcionCorta"), 60) %>
                                </p>

                                <!-- PRECIO -->
                                <div class="mb-3">
                                    <div class="h5 text-success fw-bold mb-1">
                                        $<%# Eval("Precio") %>
                                    </div>
                                    <small class="text-success">
                                        Precio s/imp. nac. $<%# Math.Round(Convert.ToDecimal(Eval("Precio")) * 0.82m, 0) %>
                                    </small>
                                </div>

                                <!-- BOTÓN COMPRAR -->
                                <asp:Button ID="btnComprar" runat="server" 
                                            Text="QUIERO VER MAS"
                                            CssClass="btn btn-success w-100 mt-auto"
                                            CommandArgument='<%# Eval("Id") %>'
                                            OnClick="btnVerDetalle_Click" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

    <!-- Modal de Filtros -->
    <div class="modal fade" id="filtroModal" tabindex="-1" aria-labelledby="filtroModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="filtroModalLabel">Filtrar Productos</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label class="form-label">Categoría</label>
                        <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Todas las categorías</asp:ListItem>
                            <asp:ListItem>Refrigeradores</asp:ListItem>
                            <asp:ListItem>Lavadoras</asp:ListItem>
                            <asp:ListItem>Televisores</asp:ListItem>
                            <asp:ListItem>Pequeños electrodomésticos</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Marca</label>
                        <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Todas las marcas</asp:ListItem>
                            <asp:ListItem>LG</asp:ListItem>
                            <asp:ListItem>Samsung</asp:ListItem>
                            <asp:ListItem>Whirlpool</asp:ListItem>
                            <asp:ListItem>Philips</asp:ListItem>
                            <asp:ListItem>Rowenta</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="row">
                        <div class="col-6">
                            <label class="form-label">Precio desde</label>
                            <asp:TextBox ID="txtPrecioDesde" runat="server" CssClass="form-control" TextMode="Number" placeholder="0"></asp:TextBox>
                        </div>
                        <div class="col-6">
                            <label class="form-label">Precio hasta</label>
                            <asp:TextBox ID="txtPrecioHasta" runat="server" CssClass="form-control" TextMode="Number" placeholder="100000"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnAplicarFiltro" runat="server" Text="Aplicar Filtros" CssClass="btn btn-success" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>