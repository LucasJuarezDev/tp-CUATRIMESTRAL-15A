<%@ Page Title="Detalle del Producto" Language="C#" MasterPageFile="~/MasterPageCliente.Master" 
    AutoEventWireup="true" CodeBehind="DetalleCatalogo.aspx.cs" Inherits="Dominio.DetalleCatalogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container py-5">
        <asp:FormView ID="fvProducto" runat="server" ItemType="Clases.Producto">
            <ItemTemplate>
                <div class="row g-5">
                    <!-- IMÁGENES -->
                    <div class="col-lg-6">
                        <asp:PlaceHolder ID="phCarrusel" runat="server" Visible='<%# (Item.Imagenes?.Count ?? 0) > 1 %>'>
                            <div id="carouselProducto" class="carousel slide shadow-lg rounded overflow-hidden" data-bs-ride="carousel">
                                <div class="carousel-indicators">
                                    <asp:Repeater ID="rptIndicators" runat="server" DataSource='<%# Item.Imagenes %>'>
                                        <ItemTemplate>
                                            <button type="button" data-bs-target="#carouselProducto" 
                                                    data-bs-slide-to="<%# Container.ItemIndex %>" 
                                                    class='<%# Container.ItemIndex == 0 ? "active" : "" %>'
                                                    aria-current='<%# Container.ItemIndex == 0 ? "true" : "false" %>'
                                                    aria-label="Slide <%# Container.ItemIndex + 1 %>"></button>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>

                                <div class="carousel-inner">
                                    <asp:Repeater ID="rptImagenes" runat="server" DataSource='<%# Item.Imagenes %>'>
                                        <ItemTemplate>
                                            <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                                                <img src='<%# ResolveUrl(Eval("UrlImagen").ToString()) %>'
                                                     class="d-block w-100"
                                                     style="max-height: 650px; object-fit: contain; background: #000;"
                                                     alt="Imagen <%# Container.ItemIndex + 1 %>"
                                                     onerror="this.src='<%= ResolveUrl("~/img/productos/no_img.png") %>'">
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>

                                <button class="carousel-control-prev" type="button" data-bs-target="#carouselProducto" data-bs-slide="prev">
                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Anterior</span>
                                </button>
                                <button class="carousel-control-next" type="button" data-bs-target="#carouselProducto" data-bs-slide="next">
                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Siguiente</span>
                                </button>
                            </div>
                        </asp:PlaceHolder>

                        <!-- SI SOLO TIENE 1 IMAGEN O NINGUNA -->
                        <asp:PlaceHolder ID="phUnaImagen" runat="server" Visible='<%# (Item.Imagenes?.Count ?? 0) <= 1 %>'>
                            <div class="text-center bg-black rounded shadow-lg p-4">
                                <img src='<%# (Item.Imagenes?.Count ?? 0) > 0 ? ResolveUrl(Item.Imagenes[0].UrlImagen) : ResolveUrl("~/img/productos/no_img.png") %>'
                                     class="img-fluid rounded"
                                     style="max-height: 650px; object-fit: contain;"
                                     alt="Imagen del producto"
                                     onerror="this.src='<%= ResolveUrl("~/img/productos/no_img.png") %>'">
                            </div>
                        </asp:PlaceHolder>

                        <!-- MINIATURAS (solo si tiene más de 1 imagen) -->
                        <div class="row mt-4 g-2" id="divMiniaturas" runat="server" visible='<%# (Item.Imagenes?.Count ?? 0) > 1 %>'>
                            <asp:Repeater ID="rptMiniaturas" runat="server" DataSource='<%# Item.Imagenes %>'>
                                <ItemTemplate>
                                    <div class="col-3">
                                        <img src='<%# ResolveUrl(Eval("UrlImagen").ToString()) %>'
                                             class="img-thumbnail cursor-pointer border border-3"
                                             style="height: 100px; object-fit: cover;"
                                             onclick="document.querySelector('#carouselProducto .carousel-item:nth-child(<%# Container.ItemIndex + 1 %>)').scrollIntoView({behavior: 'smooth'});"
                                             onerror="this.src='<%= ResolveUrl("~/img/productos/no_img.png") %>'">
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <!-- DETALLES -->
                    <div class="col-lg-6">
                        <a href="Catalogo.aspx" class="text-decoration-none text-muted small mb-3 d-inline-block">
                            <i class="bi bi-arrow-left"></i> Volver al catálogo
                        </a>

                        <h1 class="display-5 fw-bold mb-3"><%# Item.Nombre %></h1>

                        <div class="mb-3">
                            <span class="badge bg-primary fs-6 me-2"><%# Item.Marca?.Nombre ?? "Sin marca" %></span>
                            <span class="badge bg-secondary fs-6"><%# Item.Categoria?.Nombre ?? "Sin categoría" %></span>
                        </div>

                        <div class="h2 text-success fw-bold mb-4">$<%# Item.Precio.ToString("N0") %></div>

                        <p class="lead text-muted mb-4"><%# Item.DescripcionCorta %></p>

                        <div class="border-start border-primary border-4 ps-4 mb-5">
                            <h5 class="fw-bold text-primary mb-3">
                                <i class="bi bi-card-checklist"></i> Características del producto
                            </h5>
                            <div class="lh-lg text-dark"><%# Item.DescripcionExtendida.Replace("\n", "<br>") %></div>
                        </div>

                        <div class="alert alert-success border-0 shadow-sm mb-4" role="alert">
                            <%# Item.Stock > Item.StockMinimo 
                                ? "<i class='bi bi-check2-circle'></i> <strong>¡Stock disponible!</strong>" 
                                : "<i class='bi bi-exclamation-triangle text-warning'></i> <strong>¡Últimas unidades disponibles!</strong>" %>
                        </div>

                        <div class="d-grid d-md-flex gap-3">
                            <asp:Button ID="btnAgregarCarrito" runat="server"
                                        Text="Agregar al carrito"
                                        CssClass="btn btn-success btn-lg px-5 fw-bold"
                                        CommandArgument='<%# Item.Id %>'
                                        OnClick="btnAgregarCarrito_Click" />
                            <a href="Catalogo.aspx" class="btn btn-outline-dark btn-lg px-5">
                                Seguir comprando
                            </a>
                        </div>
                    </div>
                </div>
            </ItemTemplate>

            <EmptyDataTemplate>
                <div class="text-center py-5">
                    <img src="<%= ResolveUrl("~/img/productos/no_img.png") %>" class="mb-4" style="height:200px;" />
                    <h3 class="text-muted">Producto no encontrado</h3>
                    <a href="Catalogo.aspx" class="btn btn-primary mt-3">Volver al catálogo</a>
                </div>
            </EmptyDataTemplate>
        </asp:FormView>
    </div>
</asp:Content>