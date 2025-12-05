<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" 
    CodeBehind="ComprasAdmin.aspx.cs" Inherits="Dominio.ComprasAdmin" Title="Gestión de Pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
    <style>
        .badge-pago-1 { background-color: #ffc107; color: black; }
        .badge-pago-2 { background-color: #28a745; color: white; }
        .badge-pago-3 { background-color: #dc3545; color: white; }
        .badge-pago-4 { background-color: #6c757d; color: white; }

        .badge-prep-1 { background-color: #6c757d; }
        .badge-prep-2 { background-color: #17a2b8; }
        .badge-prep-3 { background-color: #007bff; }
        .badge-prep-4 { background-color: #dc3545; }

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

                    <asp:TemplateField HeaderText="Comprobante">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnVerComprobante" Text="Detalle" CssClass="btn btn-sm btn-dark"
                                CommandName="VerComprobante" CommandArgument='<%# Eval("Id") %>' />
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

            <div class="text-center mt-3" id="divPaginacion" runat="server" visible='<%# gvPedidos.PageCount > 1 %>'>
                <small class="text-muted">
                    Página <strong><asp:Label ID="lblPaginaActual" runat="server" /></strong> de <strong><asp:Label ID="lblTotalPaginas" runat="server" /></strong>
                </small>
            </div>

            <!--PREVIEW DEL COMPROBANTE-->
            <div id="divComprobante" runat="server"
                 class="mt-4 p-4 bg-light border rounded text-center position-relative"
                 visible="false" style="max-width:480px; margin:auto;">

                <asp:Button ID="btnCerrarComprobante" runat="server"
                 CssClass="btn-close position-absolute top-0 end-0 m-2"
                 OnClick="btnCerrarComprobante_Click"
                 ToolTip="Cerrar"
                 Text="" />

                <h5 class="fw-bold mb-3 text-dark">Comprobante de Pago</h5>

                <asp:Image ID="imgComprobante" runat="server"
                           CssClass="img-fluid rounded shadow"
                           Style="max-width:450px; max-height:450px;" />

                <div class="mt-3">
                    <asp:HyperLink ID="lnkDetalle" runat="server"
                                   CssClass="btn btn-sm btn-primary"
                                   Text="Detalle" />
                </div>
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
                    <label class="form-label fw-bold text-primary">Estado del Pago</label>
                    <asp:DropDownList ID="ddlEstadoPago" runat="server" CssClass="form-select" />

                    <label class="form-label fw-bold text-info mt-3">Estado de Preparación</label>
                    <asp:DropDownList ID="ddlEstadoPreparacion" runat="server" CssClass="form-select" />

                    <label class="form-label fw-bold text-success mt-3">Estado de Envío</label>
                    <asp:DropDownList ID="ddlEstadoEnvio" runat="server" CssClass="form-select" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarEstados" runat="server" Text="Guardar Cambios" CssClass="btn btn-success"
                        OnClick="btnGuardarEstados_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>


