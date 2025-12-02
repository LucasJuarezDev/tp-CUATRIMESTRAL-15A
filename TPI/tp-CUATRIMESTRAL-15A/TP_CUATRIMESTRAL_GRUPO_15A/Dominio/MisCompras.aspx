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
                        <a href="DetalleCompra.aspx?id=<%# Eval("Id") %>" class="btn-detalle">Ver detalle</a>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>



