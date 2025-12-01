<%@ Page Title="Mis Compras" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="MisCompras.aspx.cs" Inherits="Dominio.MisCompras" %>

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

        .card {
            padding: 20px;
            border-radius: 12px;
            box-shadow: 0 0 10px rgba(0,0,0,0.12);
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h3 class="mb-4">
            <i class="bi bi-box-seam"></i> Mis Compras
        </h3>

        <div class="card">

            <table class="table tabla-compras">
                <thead>
                    <tr>
                        <th>Compra</th>
                        <th>Fecha</th>
                        <th>Pago</th>
                        <th>Estado Pago</th>
                        <th>Pedido</th>
                        <th>Envío</th>
                        <th>Acciones</th>
                    </tr>
                </thead>

                <tbody>

                    <!-- COMPRA 1 -->
                    <tr>
                        <td>#1021</td>
                        <td>10/11/2025</td>
                        <td>Transferencia</td>

                        <!-- Estado Pago -->
                        <td><span class="badge bg-success">Aprobado</span></td>

                        <!-- Pedido -->
                        <td><span class="badge bg-warning text-dark">Armando</span></td>

                        <!-- Envío -->
                        <td><span class="badge bg-success">En camino</span></td>

                        <td><a href="#" class="btn-detalle">Ver detalle</a></td>
                    </tr>

                    <!-- COMPRA 2 -->
                    <tr>
                        <td>#1020</td>
                        <td>05/11/2025</td>
                        <td>Tarjeta</td>

                        <!-- Estado Pago -->
                        <td><span class="badge bg-warning text-dark">En espera</span></td>

                        <!-- Pedido -->
                        <td><span class="badge bg-secondary">No iniciado</span></td>

                        <!-- Envio-->
                        <td><span class="badge bg-warning text-dark">Pendiente</span></td>

                        <td><a href="#" class="btn-detalle">Ver detalle</a></td>
                    </tr>

                    <!-- COMPRA 3 -->
                    <tr>
                        <td>#1019</td>
                        <td>01/11/2025</td>
                        <td>Efectivo</td>

                        <!-- Estado Pago -->
                        <td><span class="badge bg-danger">Rechazado</span></td>

                        <!-- Pedido-->
                        <td><span class="badge bg-danger">Cancelado</span></td>

                        <!-- Envio-->
                        <td><span class="badge bg-secondary">No iniciado</span></td>

                        <td><a href="#" class="btn-detalle">Ver detalle</a></td>
                    </tr>

                    <!-- COMPRA 4 -->
                    <tr>
                        <td>#1018</td>
                        <td>28/10/2025</td>
                        <td>Transferencia</td>

                        <!-- Estado Pago -->
                        <td><span class="badge bg-success">Aprobado</span></td>

                        <!-- Pedido-->
                        <td><span class="badge bg-success">Terminado</span></td>

                        <!-- Envio-->
                        <td><span class="badge bg-success">Entregado</span></td>

                        <td><a href="#" class="btn-detalle">Ver detalle</a></td>
                    </tr>

                    <!-- COMPRA 5  -->
                    <tr>
                        <td>#1017</td>
                        <td>25/10/2025</td>
                        <td>Tarjeta</td>

                        <!-- Estado Pago -->
                        <td><span class="badge bg-success">Aprobado</span></td>

                        <!-- Pedido -->
                        <td><span class="badge bg-success">Terminado</span></td>

                        <!-- Envio -->
                        <td><span class="badge bg-danger">Cancelado</span></td>

                        <td><a href="#" class="btn-detalle">Ver detalle</a></td>
                    </tr>

                </tbody>
            </table>

        </div>
    </div>

</asp:Content>



