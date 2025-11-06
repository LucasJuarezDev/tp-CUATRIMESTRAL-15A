<%@ Page Title="Detalle del Producto" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="DetalleCatalogo.aspx.cs" Inherits="Dominio.DetalleCatalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <asp:FormView ID="fvProducto" runat="server" ItemType="Clases.Producto">
            <ItemTemplate>
                <div class="row">
                    <!-- IMAGEN -->
                    <div class="col-lg-5">
                        <div class="position-sticky" style="top: 2rem;">
                            <img src='<%# string.IsNullOrEmpty(Item.ImagenUrl) 
                                        ? "https://via.placeholder.com/600x600/cccccc/666666?text=" + Server.UrlEncode(Item.Nombre.Length > 20 ? Item.Nombre.Substring(0,20)+"..." : Item.Nombre)
                                        : Item.ImagenUrl %>'
                                 class="img-fluid rounded shadow-sm"
                                 alt='<%# Item.Nombre %>'
                                 onerror="this.src='https://via.placeholder.com/600x600/cccccc/666666?text=Sin+Imagen'">
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