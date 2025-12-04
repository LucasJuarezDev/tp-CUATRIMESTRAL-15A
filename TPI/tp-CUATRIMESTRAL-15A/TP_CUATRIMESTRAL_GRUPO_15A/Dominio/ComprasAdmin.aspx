<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" 
    CodeBehind="ComprasAdmin.aspx.cs" Inherits="Dominio.ComprasAdmin" Title="Gestión de Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        .badge-pago-1 { background-color: #ffc107; color: black; } /* Pendiente */
        .badge-pago-2 { background-color: #28a745; color: white; } /* Aprobado */
        .badge-pago-3 { background-color: #dc3545; color: white; } /* Rechazado */
        .badge-pago-4 { background-color: #6c757d; color: white; } /* Pendiente comprobante */

        .badge-prep-1 { background-color: #6c757d; } /* No iniciado */
        .badge-prep-2 { background-color: #17a2b8; } /* En preparación */
        .badge-prep-3 { background-color: #007bff; } /* Listo para envío */
        .badge-prep-4 { background-color: #dc3545; } /* Cancelado */

        .badge-envio-1 { background-color: #6c757d; }
        .badge-envio-2 { background-color: #fd7e14; }
        .badge-envio-3 { background-color: #28a745; }
        .badge-envio-4, .badge-envio-5 { background-color: #dc3545; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="container py-5">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h2><i class="bi bi-truck me-2"></i>Gestión de Pedidos</h2>
        <div class="d-flex align-items-center gap-3">
            <span class="text-muted">Total: <strong><asp:Label ID="lblTotal" runat="server" /></strong></span>
            <div class="d-flex align-items-center gap-2">
                <label class="text-muted small mb-0">Mostrar:</label>
                <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                    <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
                    <asp:ListItem Value="25">25</asp:ListItem>
                    <asp:ListItem Value="50">50</asp:ListItem>
                    <asp:ListItem Value="100">100</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>

    <div class="card shadow">
        <div class="card-body">
            <asp:GridView ID="gvPedidos" runat="server" AutoGenerateColumns="false"
                CssClass="table table-hover table-striped mb-0" GridLines="None"
                AllowPaging="true" PageSize="10"
                OnPageIndexChanging="gvPedidos_PageIndexChanging"
                OnRowCommand="gvPedidos_RowCommand"
                PagerStyle-CssClass="pager"
                PagerSettings-Mode="NumericFirstLast"
                PagerSettings-FirstPageText="«"
                PagerSettings-LastPageText="»"
                PagerSettings-PageButtonCount="5">
                
                <PagerStyle CssClass="pagination justify-content-center mt-3" HorizontalAlign="Center" />
                
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Compra" />
                    <asp:TemplateField HeaderText="Cliente">
                        <ItemTemplate>
                            <strong><%# Eval("Cliente.Usuario.Email") %></strong><br />
                            <small class="text-muted"><%# Eval("Cliente.Nombre") %> <%# Eval("Cliente.Apellido") %></small>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FechaVenta" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                    <asp:BoundField DataField="MontoTotal" HeaderText="Total" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" />
                    <asp:TemplateField HeaderText="Pago">
                        <ItemTemplate>
                            <span class="badge <%# "badge-pago-" + Eval("EstadoPago.Id") %> px-3 py-2">
                                <%# Eval("TipoPago.Nombre") %> - <%# Eval("EstadoPago.Nombre") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Preparación">
                        <ItemTemplate>
                            <span class="badge <%# "badge-prep-" + Eval("EstadoPreparacion.Id") %> px-3 py-2">
                                <%# Eval("EstadoPreparacion.Nombre") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Envío">
                        <ItemTemplate>
                            <span class="badge <%# "badge-envio-" + Eval("EstadoEnvio.Id") %> px-3 py-2">
                                <%# Eval("EstadoEnvio.Nombre") %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seguimiento">
                        <ItemTemplate>
                            <button type="button" class="btn btn-sm btn-outline-primary" data-bs-toggle="modal" data-bs-target="#modalSeguimiento"
                                onclick="cargarModal('<%# Eval("Id") %>','<%# Eval("EstadoPago.Id") %>','<%# Eval("EstadoPreparacion.Id") %>','<%# Eval("EstadoEnvio.Id") %>')">
                                <i class="bi bi-pencil-square"></i> Cambiar
                            </button>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:Button ID="btnFactura" runat="server" Text="Factura" CssClass="btn btn-sm btn-success"
                                CommandName="GenerarFactura" CommandArgument='<%# Eval("Id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- Paginación visual (opcional, queda lindo) -->
            <div class="text-center mt-3" id="divPaginacion" runat="server" visible='<%# gvPedidos.PageCount > 1 %>'>
                <small class="text-muted">
                    Página <strong><asp:Label ID="lblPaginaActual" runat="server" /></strong> de <strong><asp:Label ID="lblTotalPaginas" runat="server" /></strong>
                </small>
            </div>
        </div>
    </div>
</div>

    <!-- MODAL DE SEGUIMIENTO -->
    <div class="modal fade" id="modalSeguimiento" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title"><i class="bi bi-truck"></i> Actualizar Estado del Pedido</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hfIdVenta" runat="server" />

                    <div class="mb-3">
                        <label class="form-label fw-bold text-primary">Estado del Pago</label>
                        <asp:DropDownList ID="ddlEstadoPago" runat="server" CssClass="form-select" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold text-info">Estado de Preparación</label>
                        <asp:DropDownList ID="ddlEstadoPreparacion" runat="server" CssClass="form-select" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-bold text-success">Estado de Envío</label>
                        <asp:DropDownList ID="ddlEstadoEnvio" runat="server" CssClass="form-select" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarEstados" runat="server" Text="Guardar Cambios" 
                        CssClass="btn btn-success" OnClick="btnGuardarEstados_Click" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function cargarModal(idVenta, pago, prep, envio) {
            document.getElementById('<%= hfIdVenta.ClientID %>').value = idVenta;
            document.getElementById('<%= ddlEstadoPago.ClientID %>').value = pago;
            document.getElementById('<%= ddlEstadoPreparacion.ClientID %>').value = prep;
            document.getElementById('<%= ddlEstadoEnvio.ClientID %>').value = envio;
        }
    </script>
</asp:Content>
