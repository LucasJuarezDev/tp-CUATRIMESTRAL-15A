<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Dominio.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Panel de estadísticas -->
    <div class="row mb-4">
        <div class="col-md-4">
            <div class="card text-white bg-success mb-3">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="card-title">Cantidad Clientes:</h6>
                        <h3>0</h3>
                    </div>
                    <i class="bi bi-people-fill" style="font-size:2rem;"></i>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card text-dark bg-warning mb-3">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="card-title">Cantidad Ventas:</h6>
                        <h3>0</h3>
                    </div>
                    <i class="bi bi-bag-fill" style="font-size:2rem;"></i>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card text-white bg-secondary mb-3">
                <div class="card-body d-flex justify-content-between align-items-center">
                    <div>
                        <h6 class="card-title">Cantidad Productos:</h6>
                        <h3>0</h3>
                    </div>
                    <i class="bi bi-box-seam" style="font-size:2rem;"></i>
                </div>
            </div>
        </div>
    </div>

    <!-- Filtros y botones -->
    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-3 align-items-end">
                <div class="col-md-3">
                    <label for="fechaInicio" class="form-label">Fecha Inicio</label>
                    <input type="date" class="form-control" id="fechaInicio">
                </div>
                <div class="col-md-3">
                    <label for="fechaFin" class="form-label">Fecha Fin</label>
                    <input type="date" class="form-control" id="fechaFin">
                </div>
                <div class="col-md-3">
                    <label for="idTransaccion" class="form-label">ID Venta</label>
                    <input type="text" class="form-control" id="idTransaccion">
                </div>
                <div class="col-md-3 d-flex">
                    <button class="btn btn-primary me-2">Buscar</button>
                    <button class="btn btn-success">Exportar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- DataGridView -->
    <div class="card">
        <div class="card-body">
            <div class="d-flex justify-content-between mb-2">
                <div>
                    Mostrar 
                    <select class="form-select d-inline w-auto">
                        <option>10</option>
                        <option>25</option>
                        <option>50</option>
                    </select>
                    registros
                </div>
                <div>
                    Buscar: <input type="text" class="form-control d-inline w-auto">
                </div>
            </div>

            <div class="table-responsive">
                <table class="table table-bordered table-hover">
                    <thead class="table-light">
                        <tr>
                            <th>Fecha Venta</th>
                            <th>Cliente</th>
                            <th>Producto</th>
                            <th>Precio</th>
                            <th>Cantidad</th>
                            <th>Total</th>
                            <th>ID Transacción</th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>

</asp:Content>

