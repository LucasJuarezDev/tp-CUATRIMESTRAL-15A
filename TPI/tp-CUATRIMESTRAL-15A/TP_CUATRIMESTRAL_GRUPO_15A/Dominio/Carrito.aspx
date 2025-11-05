<%@ Page Title="Carrito" Language="C#" MasterPageFile="~/MasterPageCliente.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="Dominio.Carrito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Contenedor principal -->
    <div class="container py-5">
        <div class="row g-4">

            <!-- ================== CARRITO (izquierda) ================== -->
            <div class="col-lg-8">
                <div class="card text-white border-0 rounded-3 shadow-sm" style="background-color: #495057;">
                    <div class="card-body text-center py-5">

                        <!-- Icono del carrito -->
                        <i class="bi bi-cart3 display-1 text-white mb-3"></i>

                        <!-- Título principal -->
                        <h3 class="fw-bold mb-2">Aún no hay ítems en el carrito!</h3>

                        <!-- Subtítulo -->
                        <p class="text-white-50 small">
                            Agrega primero un producto para poder visualizar el carrito
                        </p>
                    </div>
                </div>
            </div>

            <!-- ================== RESUMEN DE PEDIDO (derecha) ================== -->
            <div class="col-lg-4">
                <div class="card text-white border-0 rounded-3 shadow-sm h-100" style="background-color: #495057;">
                    <div class="card-body d-flex flex-column">

                        <!-- Título del resumen -->
                        <h5 class="fw-bold mb-4">RESUMEN DE PEDIDO</h5>

                        <!-- Total -->
                        <div class="d-flex justify-content-between align-items-center mb-4">
                            <span class="fw-bold">TOTAL:</span>
                            <span class="h4 fw-bold">$ 0.00</span>
                        </div>

                        <!-- Botón Continuar -->
                        <button type="button" class="btn btn-secondary w-100 mt-auto" disabled>
                            CONTINUAR
                        </button>
                    </div>
                </div>
            </div>

        </div>
    </div>

</asp:Content>