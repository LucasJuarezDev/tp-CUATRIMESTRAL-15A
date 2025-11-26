<%@ Page Title="Detalle del Producto" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="DetalleCatalogo.aspx.cs" Inherits="Dominio.DetalleCatalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <asp:FormView ID="fvProducto" runat="server" ItemType="Clases.Producto">
            <ItemTemplate>
                <div class="row">
                    <!-- CARRUSEL DE IMÁGENES -->
                    <div class="col-lg-5">
                        <div id="carouselProducto" class="carousel slide" data-bs-ride="carousel">
                            <div class="carousel-inner">
                                <asp:Repeater ID="rptImagenes" runat="server">
                                    <ItemTemplate>
                                        <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                                            <img src='<%# Eval("UrlImagen") %>' 
                                                 class="d-block w-100 rounded shadow-sm" 
                                                 style="max-height:600px; object-fit:contain; background:#000"
                                                 alt="Imagen del producto"
                                                 onerror="this.src='https://via.placeholder.com/600x600/cccccc/666666?text=Sin+Imagen'">
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <!-- controles del carrusel -->
                        </div>

                        <!-- Miniaturas -->
                        <div class="row mt-3 g-2">
                            <asp:Repeater ID="rptMiniaturas" runat="server">
                                <ItemTemplate>
                                    <div class="col-3">
                                    <img src='<%# ResolveUrl(Eval("ImagenPrincipal").ToString()) %>'
                                         class="card-img-top"
                                         alt='<%# Eval("Nombre") %>'
                                         style="height: 220px; object-fit: cover;"
                                         onerror="this.onerror=null; this.src='https://via.placeholder.com/400x300/cccccc/666666?text=Sin+Imagen';">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <!-- DETALLES -->
                    <div class="col-lg-7">
                        <div class="ps-lg-4">
                            <a href="Catalogo.aspx" class="text-decoration-none text-muted mb-3 d-inline-block">
                                <i class="bi bi-arrow-left"></i> Volver al catálogo
                            </a>

                            <h1 class="display-6 fw-bold mb-2"><%# Item.Nombre %></h1>

                            <p class="text-muted mb-3">
                                <span class="badge bg-primary me-2"><%# Item.Marca?.Nombre ?? "Sin marca" %></span>
                                <span class="badge bg-secondary"><%# Item.Categoria?.Nombre ?? "Sin categoría" %></span>
                            </p>

                            <div class="mb-4">
                                <div class="h3 text-success fw-bold">$<%# Item.Precio.ToString("N0") %></div>
                            </div>

                            <p class="text-muted small mb-3"><%# Item.DescripcionCorta %></p>

                            <div class="mb-4">
                                <h5 class="fw-semibold d-flex align-items-center gap-2">
                                    <i class="bi bi-list-check text-success"></i> Características
                                </h5>
                                <p class="text-muted lh-lg"><%# Item.DescripcionExtendida %></p>
                            </div>

                            <div class="row mb-4 g-3">
                                <div class="col-sm-6">
                                    <%# Item.Stock > Item.StockMinimo 
                                        ? $"<div class='border rounded p-3 bg-light'><strong class='text-success'>Stock disponible</strong></div>"
                                        : $"<div class='border rounded p-3 bg-danger text-white'><strong>¡Últimas unidades!</strong></div>" %>
                                </div>
                            </div>

                            <div class="d-grid d-md-flex gap-2">
                                <asp:Button ID="btnAgregarCarrito" runat="server" 
                                            Text="Agregar al carrito" 
                                            CssClass="btn btn-success btn-lg px-5"
                                            CommandArgument='<%# Item.Id %>'
                                            OnClick="btnAgregarCarrito_Click" />
                                <a href="Catalogo.aspx" class="btn btn-outline-secondary btn-lg px-5">
                                    Seguir comprando
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
            <EmptyDataTemplate>
                <div class="alert alert-warning">Producto no encontrado.</div>
            </EmptyDataTemplate>
        </asp:FormView>
    </div>
</asp:Content>