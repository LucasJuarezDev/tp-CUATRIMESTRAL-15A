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

        <!-- Grid de Productos: 3 por fila -->
        <div class="row g-4">
            <!-- Producto 1 -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                    <img src="https://via.placeholder.com/400x300.png?text=Refrigerador+LG" class="card-img-top" alt="Refrigerador LG" style="height: 220px; object-fit: cover;">
                    <div class="card-body d-flex flex-column p-4">
                        <h6 class="card-title fw-bold mb-2">Refrigerador LG Inverter 18 pies</h6>
                        <p class="text-muted small mb-3">No Frost | 500 L | A+++</p>

                        <div class="mb-3">
                            <del class="text-muted small">$35.000</del>
                            <div class="h5 text-success fw-bold mb-1">$28.500</div>
                            <small class="text-success">Precio s/imp. nac. $23.231</small>
                        </div>

                        <asp:Button ID="btnComprar1" runat="server" Text="COMPRAR" 
                                    CssClass="btn btn-success w-100 mt-auto" 
                                     CommandArgument="1" />
                    </div>
                </div>
            </div>

            <!-- Producto 2 -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                    <img src="https://via.placeholder.com/400x300.png?text=Lavadora+Samsung" class="card-img-top" alt="Lavadora Samsung" style="height: 220px; object-fit: cover;">
                    <div class="card-body d-flex flex-column p-4">
                        <h6 class="card-title fw-bold mb-2">Lavadora Samsung 10kg Eco Bubble</h6>
                        <p class="text-muted small mb-3">Carga superior | 12 ciclos</p>

                        <div class="mb-3">
                            <del class="text-muted small">$22.000</del>
                            <div class="h5 text-success fw-bold mb-1">$18.900</div>
                            <small class="text-success">Precio s/imp. nac. $15.496</small>
                        </div>

                        <asp:Button ID="btnComprar2" runat="server" Text="COMPRAR" 
                                    CssClass="btn btn-success w-100 mt-auto" 
                                     CommandArgument="2" />
                    </div>
                </div>
            </div>

            <!-- Producto 3 -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                    <img src="https://via.placeholder.com/400x300.png?text=Microondas+Whirlpool" class="card-img-top" alt="Microondas Whirlpool" style="height: 220px; object-fit: cover;">
                    <div class="card-body d-flex flex-column p-4">
                        <h6 class="card-title fw-bold mb-2">Microondas Whirlpool 25L Grill</h6>
                        <p class="text-muted small mb-3">900W | 10 niveles de potencia</p>

                        <div class="mb-3">
                            <del class="text-muted small">$8.500</del>
                            <div class="h5 text-success fw-bold mb-1">$6.990</div>
                            <small class="text-success">Precio s/imp. nac. $5.732</small>
                        </div>

                        <asp:Button ID="btnComprar3" runat="server" Text="COMPRAR" 
                                    CssClass="btn btn-success w-100 mt-auto" 
                                     CommandArgument="3" />
                    </div>
                </div>
            </div>

            <!-- Producto 4 -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                    <img src="https://via.placeholder.com/400x300.png?text=TV+Smart+55" class="card-img-top" alt="TV Smart 55" style="height: 220px; object-fit: cover;">
                    <div class="card-body d-flex flex-column p-4">
                        <h6 class="card-title fw-bold mb-2">TV Smart LG 55" 4K UHD</h6>
                        <p class="text-muted small mb-3">WebOS | HDR10 | Magic Remote</p>

                        <div class="mb-3">
                            <del class="text-muted small">$45.000</del>
                            <div class="h5 text-success fw-bold mb-1">$39.800</div>
                            <small class="text-success">Precio s/imp. nac. $32.787</small>
                        </div>

                        <asp:Button ID="btnComprar4" runat="server" Text="COMPRAR" 
                                    CssClass="btn btn-success w-100 mt-auto" 
                                     CommandArgument="4" />
                    </div>
                </div>
            </div>

            <!-- Producto 5 -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                    <img src="https://via.placeholder.com/400x300.png?text=Licuadora+Philips" class="card-img-top" alt="Licuadora Philips" style="height: 220px; object-fit: cover;">
                    <div class="card-body d-flex flex-column p-4">
                        <h6 class="card-title fw-bold mb-2">Licuadora Philips PowerPro 1000W</h6>
                        <p class="text-muted small mb-3">Vaso 1.5L | 6 cuchillas</p>

                        <div class="mb-3">
                            <del class="text-muted small">$3.200</del>
                            <div class="h5 text-success fw-bold mb-1">$2.490</div>
                            <small class="text-success">Precio s/imp. nac. $2.041</small>
                        </div>

                        <asp:Button ID="btnComprar5" runat="server" Text="COMPRAR" 
                                    CssClass="btn btn-success w-100 mt-auto" 
                                     CommandArgument="5" />
                    </div>
                </div>
            </div>

            <!-- Producto 6 -->
            <div class="col-lg-4 col-md-6">
                <div class="card h-100 border-0 shadow-sm position-relative overflow-hidden">
                    <img src="https://via.placeholder.com/400x300.png?text=Ventilador+Rowenta" class="card-img-top" alt="Ventilador Rowenta" style="height: 220px; object-fit: cover;">
                    <div class="card-body d-flex flex-column p-4">
                        <h6 class="card-title fw-bold mb-2">Ventilador Rowenta Turbo Silence</h6>
                        <p class="text-muted small mb-3">3 velocidades | 45dB máx.</p>

                        <div class="mb-3">
                            <del class="text-muted small">$4.800</del>
                            <div class="h5 text-success fw-bold mb-1">$3.790</div>
                            <small class="text-success">Precio s/imp. nac. $3.106</small>
                        </div>

                        <asp:Button ID="btnComprar6" runat="server" Text="COMPRAR" 
                                    CssClass="btn btn-success w-100 mt-auto" 
                                     CommandArgument="6" />
                    </div>
                </div>
            </div>
        </div>
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