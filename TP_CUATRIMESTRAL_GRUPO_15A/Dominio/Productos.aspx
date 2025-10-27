<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageAdmin.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Dominio.Productos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid mt-4">

        <!-- Breadcrumb -->
        <nav aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Resumen</a></li>
                <li class="breadcrumb-item active" aria-current="page">Productos</li>
            </ol>
        </nav>

        <!-- Card principal -->
        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h5 class="mb-0"><i class="bi bi-box-seam"></i> Lista de Productos</h5>
                <a href="#" class="btn btn-success">Crear Nuevo</a>
            </div>

            <div class="card-body">

                <!-- Mostrar registros y búsqueda -->
                <div class="d-flex justify-content-between mb-3">
                    <div>
                        <label>Mostrar
                            <select class="form-select form-select-sm d-inline w-auto">
                                <option>10</option>
                                <option>25</option>
                                <option>50</option>
                                <option>100</option>
                            </select> registros
                        </label>
                    </div>
                    <div>
                        <input type="text" class="form-control form-control-sm" placeholder="Buscar...">
                    </div>
                </div>

                <!-- Tabla -->
                <div class="table-responsive">
                    <table class="table table-bordered table-hover table-striped align-middle">
                        <thead class="table-light">
                            <tr>
                                <th>Nombre</th>
                                <th>Descripción</th>
                                <th>Marca</th>
                                <th>Categoría</th>
                                <th>Precio</th>
                                <th>Stock</th>
                                <th>Editar</th>
                                <th>Eliminar</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Consola de PS4 Pro 1TB Negro</td>
                                <td>Tipo: PS4, Procesador: AMD, Entradas USB: 3, HDMI: 1, WiFi</td>
                                <td>SONYTE</td>
                                <td>Tecnología</td>
                                <td>150</td>
                                <td>49</td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                </td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                            <tr>
                                <td>HP Laptop 15-EF1019LA</td>
                                <td>Procesador: AMD Ryzen 5, Tarjeta gráfica: Radeon, Pantalla: 15.6"</td>
                                <td>HPTE</td>
                                <td>Tecnología</td>
                                <td>1200</td>
                                <td>27</td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                </td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                            <tr>
                                <td>Televisor 50" 4K Ultra HD Smart Android</td>
                                <td>Resolución: 4K Ultra HD, Tecnología: LED, Conexión: Bluetooth</td>
                                <td>HYUNDAITE</td>
                                <td>Tecnología</td>
                                <td>3200</td>
                                <td>70</td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-primary btn-sm"><i class="bi bi-pencil-fill"></i></a>
                                </td>
                                <td class="text-center">
                                    <a href="#" class="btn btn-danger btn-sm"><i class="bi bi-trash-fill"></i></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!-- Paginacion -->
                <div class="d-flex justify-content-between mt-2">
                    <div>Mostrando 1 a 3 de 3 registros</div>
                    <nav>
                        <ul class="pagination pagination-sm mb-0">
                            <li class="page-item disabled"><a class="page-link" href="#">Anterior</a></li>
                            <li class="page-item active"><a class="page-link" href="#">1</a></li>
                            <li class="page-item"><a class="page-link" href="#">Siguiente</a></li>
                        </ul>
                    </nav>
                </div>

            </div>
        </div>
    </div>

</asp:Content>


