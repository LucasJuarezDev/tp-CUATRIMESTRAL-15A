<%@ Page Title="Mis Compras" Language="C#" MasterPageFile="~/MasterPageCliente.Master"
    AutoEventWireup="true" CodeBehind="MisCompras.aspx.cs" Inherits="Dominio.MisCompras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tabla-compras th {
            background-color: #0d6efd;
            color: white;
            padding: 12px;
        }

        .tabla-compras td {
            padding: 15px;
            vertical-align: middle;
        }

        .btn-detalle {
            padding: 6px 10px;
            background-color: #0d6efd;
            color: white;
            border-radius: 6px;
            text-decoration: none;
            font-size: 14px;
        }
        
        .btn-detalle:hover {
            background-color: #084298;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="btnCargarDetalle" runat="server" 
            OnClick="btnCargarDetalle_Click"
            Style="display:none" />
    <asp:HiddenField ID="hfIdVentaDetalle" runat="server" />

    <div class="container mt-4">
        <h3 class="mb-4">
            <i class="bi bi-box-seam"></i> Mis Compras
        </h3>

        <asp:GridView ID="gvCompras" runat="server" AutoGenerateColumns="False"
            CssClass="table tabla-compras text-center" GridLines="None">

            <Columns>

                <asp:BoundField DataField="Id" HeaderText="Compra" />

                <asp:BoundField DataField="FechaVenta" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                <asp:BoundField DataField="TipoPago.Nombre" HeaderText="Pago" />

                <asp:TemplateField HeaderText="Estado Pago">
                    <ItemTemplate>
                        <span class="badge <%# GetBadgeClassPago(Eval("EstadoPago.Id")) %>">
                            <%# Eval("EstadoPago.Nombre") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Pedido">
                    <ItemTemplate>
                        <span class="badge <%# GetBadgeClassPreparacion(Eval("EstadoPreparacion.Id")) %>">
                            <%# Eval("EstadoPreparacion.Nombre") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Envío">
                    <ItemTemplate>
                        <span class="badge <%# GetBadgeClassEnvio(Eval("EstadoEnvio.Id")) %>">
                            <%# Eval("EstadoEnvio.Nombre") %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <button type="button" class="btn-detalle" onclick="cargarDetalleCompra('<%# Eval("Id") %>')">Ver detalle</button>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

        <!-- MODAL FACTURA X -->
    <div class="modal fade" id="modalDetalleCompra" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content bg-dark text-white">
                <div class="modal-header bg-primary border-0">
                    <h4 class="modal-title"><i class="bi bi-receipt"></i> Detalle de Compra Nº <asp:Label ID="lblIdCompraModal" runat="server" /></h4>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <div class="row g-4">
                        <div class="col-md-6">
                            <h5 class="text-warning"><i class="bi bi-person"></i> Tus datos</h5>
                            <p><strong>Nombre:</strong> <asp:Label ID="lblNombreModal" runat="server" /><br>
                               <strong>Email:</strong> <asp:Label ID="lblEmailModal" runat="server" /><br>
                               <strong>Teléfono:</strong> <asp:Label ID="lblTelefonoModal" runat="server" /><br>
                               <strong>Razón Social:</strong> <asp:Label ID="lblRazonSocialModal" runat="server" /></p>
                        </div>
                        <div class="col-md-6 text-md-end">
                            <h5 class="text-info"><i class="bi bi-calendar"></i> Información de la compra</h5>
                            <p><strong>Fecha:</strong> <asp:Label ID="lblFechaModal" runat="server" /><br>
                               <strong>Tipo de Pago:</strong> <asp:Label ID="lblTipoPagoModal" runat="server" /><br>
                               <strong>Total:</strong> $ <asp:Label ID="lblTotalModal" runat="server" Font-Bold="true" /></p>
                        </div>
                    </div>

                    <hr class="border-secondary">

                    <h5 class="text-success mb-3"><i class="bi bi-cart"></i> Productos</h5>
                    <div class="table-responsive">
                        <asp:GridView ID="gvProductosModal" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-dark table-striped table-hover">
                            <Columns>
                                <asp:BoundField DataField="Producto.Nombre" HeaderText="Producto" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cant." ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                            </Columns>
                        </asp:GridView>
                    </div>

                    <hr class="border-secondary">

                    <h5 class="text-warning"><i class="bi bi-file-earmark-image"></i> Comprobante</h5>
                    <div class="text-center">
                        <asp:PlaceHolder ID="phComprobanteModal" runat="server">
                            <asp:Image ID="imgComprobanteModal" runat="server" CssClass="img-fluid rounded shadow-lg" Style="max-height:400px;" />
                            <div class="mt-2">
                                <asp:HyperLink ID="lnkComprobanteModal" runat="server" CssClass="btn btn-primary btn-sm" Target="_blank" Text="Ver completo" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phSinComprobanteModal" runat="server" Visible="false">
                            <div class="alert alert-secondary">
                                <i class="bi bi-exclamation-triangle"></i> No hay comprobante cargado
                            </div>
                        </asp:PlaceHolder>
                    </div>

                    <hr class="border-secondary">

                    <h5 class="text-primary"><i class="bi bi-info-circle"></i> Estados</h5>
                    <div class="row g-3">
                        <div class="col-md-4">
                            <div class="p-3 bg-secondary rounded text-center">
                                <h6>Pago</h6>
                                <span class="badge bg-light text-dark fs-6"><asp:Label ID="lblEstadoPagoModal" runat="server" /></span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3 bg-secondary rounded text-center">
                                <h6>Preparación</h6>
                                <span class="badge bg-light text-dark fs-6"><asp:Label ID="lblEstadoPreparacionModal" runat="server" /></span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3 bg-secondary rounded text-center">
                                <h6>Envío</h6>
                                <span class="badge bg-light text-dark fs-6"><asp:Label ID="lblEstadoEnvioModal" runat="server" /></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer border-0">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    </div>
    <script>
        function cargarDetalleCompra(idVenta) {
            document.getElementById('<%= hfIdVentaDetalle.ClientID %>').value = idVenta;

            // Importante: usar UniqueID, NO el ID literal
            __doPostBack('<%= btnCargarDetalle.UniqueID %>', '');
        }
    </script>


</asp:Content>



